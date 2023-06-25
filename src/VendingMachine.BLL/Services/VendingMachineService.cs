using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Handlers;
using VendingMachine.BLL.Identity;
using VendingMachine.BLL.Infrastructure;
using VendingMachine.BLL.Interfaces;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Interfaces;

namespace VendingMachine.BLL.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        IDrinkRepository _drinkRepository;
        ICoinRepository _coinRepository;
        IUserRepository _userRepository;
        readonly IMapper _mapper;

        public VendingMachineService(IDrinkRepository drinkRepo,
                                     ICoinRepository coinRepo,
                                     IUserRepository userRepo,
                                     IMapper mapper)
        {
            _drinkRepository = drinkRepo;
            _coinRepository = coinRepo;
            _userRepository = userRepo;
            _mapper = mapper;
        }


        public IndexDTO GetIndexDTO()
        {
            IndexDTO indexDTO = new IndexDTO();
            indexDTO.drinks = _mapper.Map<List<DrinkDTO>>(_drinkRepository.GetDrinks());
            indexDTO.coins = _mapper.Map<List<CoinDTO>>(_coinRepository.GetCoins());
            indexDTO.CurrentBallance = _userRepository.GetUserBalance(UserIdentity.UserId);

            return indexDTO;
        }

        public MaintainDTO GetMaintainDTO()
        {
            MaintainDTO maintainDTO = new MaintainDTO();
            maintainDTO.drinks = _mapper.Map<List<DrinkDTO>>(_drinkRepository.GetDrinks());
            maintainDTO.coins = _mapper.Map<List<CoinDTO>>(_coinRepository.GetCoins());

            return maintainDTO;
        }
    
        public void AddCoinToBalance(long coinId)
        {
            try
            {
                Coin coin = _coinRepository.GetCoin(coinId);
                if (coin == null)
                    throw new VendingMachineException("Unable to add coin. " +
                                                      "Selected coin was not recognized.");

                if (coin.IsJammed)
                    throw new VendingMachineException("Unable to add coin. " +
                                                      "Selected coin is jammed.");

                _userRepository.AddBalance(UserIdentity.UserId, coin.Nominal);
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to add coin. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        public int GetUserBalance()
        {
            try
            {
                int? balance = _userRepository.GetUserBalance(UserIdentity.UserId);
                return balance == null ? 0 : (int)balance;
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to check balance. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        public int GetDrinkQuantity(long drinkId)
        {
            try
            {
                Drink drink = _drinkRepository.GetDrink(drinkId);
                if (drink == null)
                    throw new VendingMachineException("Unable to get information about a drink. " +
                                                      "Selected drink was not recognized.");
                return drink.Quantity;
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to check balance. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        public void PurchaseDrink(long drinkId)
        {
            try
            {
                Drink drink = _drinkRepository.GetDrink(drinkId);
                if (drink == null)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "Selected drink was not recognized.");

                if (drink.Quantity < 1)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "All this drinks are sold out. Please select another drink.");

                int? balance = _userRepository.GetUserBalance(UserIdentity.UserId);
                if (balance == null)
                    balance = 0;

                if (balance < drink.Cost)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "Lack of funds");

                drink.Quantity = drink.Quantity - 1;
                _drinkRepository.UpdateQuantity(drink);

                _userRepository.ReduceBalance(UserIdentity.UserId, drink.Cost);
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to purchase a drink. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        public Dictionary<CoinDTO, int> Withdraw()
        {
            try
            {
                int? balance = _userRepository.GetUserBalance(UserIdentity.UserId);
                if (balance == null || balance == 0)
                    throw new VendingMachineException("Nothing to withdraw.");

                List<CoinDTO> availableCoins = _mapper.Map<List<CoinDTO>>(_coinRepository.GetCoins());
                if (availableCoins == null || availableCoins.Count == 0)
                    throw new VendingMachineException("Unable to withdraw. " +
                                                      "Try again, and if the problem persists, " +
                                                      "see your system administrator.");

                WithdrawCalculator withdrawCalculator = new WithdrawCalculator(availableCoins);
                Dictionary<CoinDTO, int> coinsQuantities = withdrawCalculator.CalculateWithdrawedCoins((int)balance);
                if (coinsQuantities.Count() == 0)
                    throw new VendingMachineException("Unable to withdraw. " +
                                                      "There are no appropriate coins to provide operation. " +
                                                      "Please add coins to make withdraw available.");

                int withdrawedNominals = withdrawCalculator.CalculateWithdrawedNominals(coinsQuantities);
                _userRepository.ReduceBalance(UserIdentity.UserId, withdrawedNominals);

                return coinsQuantities;
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to withdraw. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }
    }
}
