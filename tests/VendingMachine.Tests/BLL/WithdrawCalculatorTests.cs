using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Handlers;
using VendingMachine.Tests.BLL.TestData;

namespace VendingMachine.Tests.BLL
{
    public class WithdrawCalculatorTests
    {
        private List<CoinDTO> _initialCoins =>
            new List<CoinDTO>
            {
                new CoinDTO { Nominal = 1 },
                new CoinDTO { Nominal = 2 },
                new CoinDTO { Nominal = 5 },
                new CoinDTO { Nominal = 10 },
            };

        [Fact]
        public void CalculateWithdrawedCoins_BalanceLessZero_ThrowsArgumentException()
        {
            var withdrawCalculator = new WithdrawCalculator();

            var actual = () => withdrawCalculator.CalculateWithdrawedCoins(_initialCoins, -1);

            Assert.Throws<ArgumentException>(actual);
        }

        [Theory]
        [ClassData(typeof(CalculateWithdrawedCoinsTestData))]
        public void CalculateWithdrawedCoins_ClassData_ReturnsCorrectedPairs(int balance,
                                                                             List<CoinDTO> initialCoins,
                                                                             Dictionary<CoinDTO, int> expected)
        {
            var withdrawCalculator = new WithdrawCalculator();

            var actual = withdrawCalculator.CalculateWithdrawedCoins(initialCoins, balance);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(CalculateWithdrawedNominalsTestData))]
        public void CalculateWithdrawedNominals_ClassData_ReturnsCorrectSum(Dictionary<CoinDTO, int> coinsQuantities,
                                                                            int expected)
        {
            var withdrawCalculator = new WithdrawCalculator();

            var actual = withdrawCalculator.CalculateWithdrawedNominals(coinsQuantities);

            Assert.Equal(expected, actual);
        }

    }
}
