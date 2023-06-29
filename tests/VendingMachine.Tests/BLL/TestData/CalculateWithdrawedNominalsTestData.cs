using System.Collections;
using VendingMachine.BLL.DTO;

namespace VendingMachine.Tests.BLL.TestData
{
    internal class CalculateWithdrawedNominalsTestData : IEnumerable<object[]>
    {
        private List<CoinDTO> _initialCoins = new List<CoinDTO>
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
                new Dictionary<CoinDTO, int> {},
                0
            };
            yield return new object[]
            {
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[1], 2 },
                    { _initialCoins[0], 1 },
                },
                5
            };
            yield return new object[]
            {
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[2], 1 },
                    { _initialCoins[1], 2 },
                },
                9

            };
            yield return new object[]
            {
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[3], 1 },
                    { _initialCoins[1], 2 },
                    { _initialCoins[2], 1 },
                },
                19
            };
            yield return new object[]
            {
                new Dictionary<CoinDTO, int>
                {
                    { _initialCoins[1], 1 },
                    { _initialCoins[2], 1 },
                    { _initialCoins[0], 4 },
                    { _initialCoins[3], 1 },
                },
                21
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
