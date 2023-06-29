using System.Collections;
using VendingMachine.BLL.DTO;

namespace VendingMachine.Tests.BLL.TestData
{
    internal class CalculateWithdrawedCoinsTestData : IEnumerable<object[]>
    {
        public List<CoinDTO> InitialCoins = new List<CoinDTO>
                {
                    new CoinDTO { Nominal = 1 },
                    new CoinDTO { Nominal = 2 },
                    new CoinDTO { Nominal = 5 },
                    new CoinDTO { Nominal = 10 },
                };

        /// <summary>
        /// Balance , Initial coins, Expected coinsQuantities
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                0,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    
                },
            };
            yield return new object[]
            {
                41,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { InitialCoins[3], 4 },
                    { InitialCoins[0], 1 },
                },
            };
            yield return new object[]
            {
                42,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { InitialCoins[3], 4 },
                    { InitialCoins[1], 1 },
                },
            };
            yield return new object[]
            {
                43,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { InitialCoins[3], 4 },
                    { InitialCoins[1], 1 },
                    { InitialCoins[0], 1 },
                },
            };
            yield return new object[]
            {
                47,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { InitialCoins[3], 4 },
                    { InitialCoins[2], 1 },
                    { InitialCoins[1], 1 },
                },
            };
            yield return new object[]
            {
                48,
                InitialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { InitialCoins[3], 4 },
                    { InitialCoins[2], 1 },
                    { InitialCoins[1], 1 },
                    { InitialCoins[0], 1 },
                },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
