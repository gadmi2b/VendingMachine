using System.Collections;
using VendingMachine.BLL.DTO;

namespace VendingMachine.Tests.BLL.TestData
{
    public class CalculateWithdrawedCoinsTestData : IEnumerable<object[]>
    {
        public List<CoinDTO> _initialCoins = new List<CoinDTO>
                {
                    new CoinDTO { Nominal = 1 },
                    new CoinDTO { Nominal = 2 },
                    new CoinDTO { Nominal = 5 },
                    new CoinDTO { Nominal = 10 },
                };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                0,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    
                },
            };
            yield return new object[]
            {
                41,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 4 },
                    { _initialCoins[0], 1 },
                },
            };
            yield return new object[]
            {
                42,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 4 },
                    { _initialCoins[1], 1 },
                },
            };
            yield return new object[]
            {
                43,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 4 },
                    { _initialCoins[1], 1 },
                    { _initialCoins[0], 1 },
                },
            };
            yield return new object[]
            {
                47,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 4 },
                    { _initialCoins[2], 1 },
                    { _initialCoins[1], 1 },
                },
            };
            yield return new object[]
            {
                48,
                _initialCoins,
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 4 },
                    { _initialCoins[2], 1 },
                    { _initialCoins[1], 1 },
                    { _initialCoins[0], 1 },
                },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
