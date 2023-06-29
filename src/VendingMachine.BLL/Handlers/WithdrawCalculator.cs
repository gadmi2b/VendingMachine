using System.Runtime.CompilerServices;
using VendingMachine.BLL.DTO;

[assembly: InternalsVisibleTo("VendingMachine.Tests")]
namespace VendingMachine.BLL.Handlers
{
    /// <summary>
    /// Provides all operations related to withdraw calculations processes
    /// </summary>
    internal class WithdrawCalculator
    {
        /// <summary>
        /// Calculates withdrowed coins and their quantites
        /// </summary>
        /// <param name="originalCoins"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Dictionary<CoinDTO, int> CalculateWithdrawedCoins(List<CoinDTO> originalCoins, int balance)
        {
            if (balance < 0)
                throw new ArgumentException();

            originalCoins = originalCoins.OrderByDescending(c => c.Nominal).ToList();

            Dictionary<CoinDTO, int> coinsQuantities = new Dictionary<CoinDTO, int>();
            foreach (var coin in originalCoins)
            {
                int quantity = balance/coin.Nominal;
                if (quantity > 0)
                {
                    coinsQuantities.Add(coin, quantity);
                    balance = balance - coin.Nominal * quantity;
                }
            }

            return coinsQuantities;
        }
        
        /// <summary>
        /// Calculates summary nominal of all coins and their quantites
        /// provided to method
        /// </summary>
        /// <param name="coinsQuantities"></param>
        /// <returns></returns>
        public int CalculateWithdrawedNominals(Dictionary<CoinDTO, int> coinsQuantities)
        {
            int nominals = 0;
            foreach (CoinDTO coin in coinsQuantities.Keys)
            {
                nominals = nominals + coin.Nominal * coinsQuantities.GetValueOrDefault(coin);
            }

            return nominals;
        }
    }
}
