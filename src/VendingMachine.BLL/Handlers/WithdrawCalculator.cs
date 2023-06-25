using System.Runtime.CompilerServices;
using VendingMachine.BLL.DTO;

[assembly: InternalsVisibleTo("VendingMachine.Tests")]
namespace VendingMachine.BLL.Handlers
{
    internal class WithdrawCalculator
    {
        List<CoinDTO> _originalCoins;

        public WithdrawCalculator(List<CoinDTO> originalCoins)
        {
            _originalCoins = originalCoins;
        }
        /// <summary>
        /// Calculates withdrowed coins and their quantites
        /// </summary>
        /// <param name="balance"></param>
        /// <returns></returns>
        public Dictionary<CoinDTO, int> CalculateWithdrawedCoins(int balance)
        {
            //_originalCoins.Sort((x, y) => (x.Nominal > y.Nominal);
            _originalCoins = _originalCoins.OrderByDescending(c => c.Nominal).ToList();

            Dictionary<CoinDTO, int> coinsQuantities = new Dictionary<CoinDTO, int>();
            foreach (var coin in _originalCoins)
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
