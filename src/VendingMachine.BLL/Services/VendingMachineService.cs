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
    /// <summary>
    /// Represents service for controlled interaction between
    /// presentation layer - business layer - data access layer
    /// </summary>
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

        /// <summary>
        /// Prepares IndexDTO object for further use in Index view 
        /// </summary>
        /// <returns>IndexDTO</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<IndexDTO> GetIndexDTOAsync()
        {
            try
            {
                IndexDTO indexDTO = new IndexDTO()
                {
                    Drinks = _mapper.Map<List<DrinkDTO>>(await _drinkRepository.GetDrinksAsync()),
                    Coins = _mapper.Map<List<CoinDTO>>(await _coinRepository.GetCoinsAsync()),
                    Ballance = await _userRepository.GetUserBalanceAsync(UserIdentity.UserId),
                };
                return indexDTO;

            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to get information. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        /// <summary>
        /// Prepares MaintainDTO object for further use in Maintain view
        /// </summary>
        /// <returns>MaintainDTO</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<MaintainDTO> GetMaintainDTOAsync()
        {
            try
            {
                MaintainDTO maintainDTO = new MaintainDTO
                {
                    Drinks = _mapper.Map<List<DrinkDTO>>(await _drinkRepository.GetDrinksAsync()),
                    Coins = _mapper.Map<List<CoinDTO>>(await _coinRepository.GetCoinsAsync()),
                };
                return maintainDTO;
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to get information. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }
        
        /// <summary>
        /// Add coin nominal to user balance by coinId
        /// </summary>
        /// <param name="coinId"></param>
        /// <returns></returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task AddCoinToBalanceAsync(long coinId)
        {
            try
            {
                Coin coin = await _coinRepository.GetCoinAsync(coinId);
                if (coin == null)
                    throw new VendingMachineException("Unable to add coin. " +
                                                      "Selected coin was not recognized.");

                if (coin.IsJammed)
                    throw new VendingMachineException("Unable to add coin. " +
                                                      "Selected coin is jammed.");

                await _userRepository.AddBalanceAsync(UserIdentity.UserId, coin.Nominal);
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

        /// <summary>
        /// Returns user balance
        /// </summary>
        /// <returns>user balance</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<int> GetUserBalanceAsync()
        {
            try
            {
                int? balance = await _userRepository.GetUserBalanceAsync(UserIdentity.UserId);
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

        /// <summary>
        /// Returns available quantity of drink by drinkId
        /// </summary>
        /// <param name="drinkId"></param>
        /// <returns>available quantity of drink</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<int> GetDrinkQuantityAsync(long drinkId)
        {
            try
            {
                Drink drink = await _drinkRepository.GetDrinkAsync(drinkId);
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

        /// <summary>
        /// Purchase a drink: reduce available quantity of drink for 1
        /// reduce users balance for drink cost
        /// </summary>
        /// <param name="drinkId"></param>
        /// <returns></returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task PurchaseDrinkAsync(long drinkId)
        {
            try
            {
                Drink drink = await _drinkRepository.GetDrinkAsync(drinkId);
                if (drink == null)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "Selected drink was not recognized.");

                if (drink.Quantity < 1)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "All this drinks are sold out. Please select another drink.");

                int? balance = await _userRepository.GetUserBalanceAsync(UserIdentity.UserId);
                if (balance == null)
                    balance = 0;

                if (balance < drink.Cost)
                    throw new VendingMachineException("Unable to purchase a drink. " +
                                                      "Lack of funds");

                drink.Quantity = drink.Quantity - 1;
                await _drinkRepository.UpdateQuantityAsync(drink);

                await _userRepository.ReduceBalanceAsync(UserIdentity.UserId, drink.Cost);
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

        /// <summary>
        /// Withdraw users balance in collection of coins and their quantites
        /// </summary>
        /// <returns>Collection of coins and their quantites</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<Dictionary<CoinDTO, int>> WithdrawAsync()
        {
            try
            {
                int? balance = await _userRepository.GetUserBalanceAsync(UserIdentity.UserId);
                if (balance == null || balance == 0)
                    throw new VendingMachineException("Nothing to withdraw.");

                List<CoinDTO> availableCoins = _mapper.Map<List<CoinDTO>>(await _coinRepository.GetCoinsAsync());
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
                await _userRepository.ReduceBalanceAsync(UserIdentity.UserId, withdrawedNominals);

                return coinsQuantities;
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to withdraw. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }
        
        /// <summary>
        /// Deletes drink from Database by drinkId
        /// </summary>
        /// <param name="drinkId"></param>
        /// <returns></returns>
        public async Task<string> DeleteDrinkAsync(long drinkId)
        {
            try
            {
                await _drinkRepository.DeleteDrinkAsync(drinkId);
                return string.Empty;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                return "Unable to delete drink. " +
                       "Try again, and if the problem persists, " +
                       "see your system administrator.";
            }
        }
        
        /// <summary>
        /// Add a new drink to database
        /// </summary>
        /// <param name="drink"></param>
        /// <returns></returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task AddDrinkAsync(DrinkDTO drink)
        {
            try
            {
                DrinkChecker drinkChecker = new DrinkChecker();
                drinkChecker.CheckDrinkBeforeAdd(drink);

                if (drink.Id != 0) { drink.Id = 0; }

                if (await _drinkRepository.IsExistWithSameNameAsync(drink.Name))
                    throw new VendingMachineException("Unable to add drink. " +
                                                      "A drink with same name is already exist");

                await _drinkRepository.AddDrinkAsync(_mapper.Map<Drink>(drink));
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to add a drink. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }
        
        /// <summary>
        /// Updates existing drink in the database
        /// </summary>
        /// <param name="drink"></param>
        /// <returns></returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task UpdateDrinkAsync(DrinkDTO drink)
        {
            try
            {
                DrinkChecker drinkChecker = new DrinkChecker();
                drinkChecker.CheckDrinkBeforeUpdate(drink);

                if (drink.Id <= 0)
                    throw new VendingMachineException("Unable to update drink. " +
                                                      "Unknown drink.");

                await _drinkRepository.UpdateDrinkAsync(_mapper.Map<Drink>(drink));
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to update a drink. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        /// <summary>
        /// Changes IsJammed state of coin by coinId
        /// returns actual IsJammed state
        /// </summary>
        /// <param name="coinId"></param>
        /// <returns>actual IsJammed state</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<bool> ChangeCoinIsJammedState(long coinId)
        {
            try
            {
                if (coinId <= 0)
                    throw new VendingMachineException("Unable to change isJammed state. " +
                                                      "Unknown coin.");

                return await _coinRepository.ChangeIsJammedStateReturnState(coinId);
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to change isJammed state of coin. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }

        /// <summary>
        /// Returns all drinks from Database
        /// </summary>
        /// <returns>all drinks from Database</returns>
        /// <exception cref="VendingMachineException"></exception>
        public async Task<List<DrinkDTO>> GetDrinksAsync()
        {
            try
            {
                return _mapper.Map<List<DrinkDTO>>(await _drinkRepository.GetDrinksAsync());
            }
            catch (Exception ex) when (ex is VendingMachineException)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                throw new VendingMachineException("Unable to get information about drinks. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.");
            }
        }
    }
}
