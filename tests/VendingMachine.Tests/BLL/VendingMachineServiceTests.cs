using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Handlers;
using VendingMachine.Tests.BLL.TestData;

namespace VendingMachine.Tests.BLL
{
    public class VendingMachineServiceTests
    {
        [Theory]
        [ClassData(typeof(CalculateWithdrawedCoinsTestData))]
        public void CalculateWithdrawedCoins_ClassData_ReturnsCorrecredPairs(int balance, List<CoinDTO> initialCoins, Dictionary<CoinDTO, int> expected)
        {
            var withdrawCalculator = new WithdrawCalculator(initialCoins);

            var actual = withdrawCalculator.CalculateWithdrawedCoins(balance);

            Assert.Equal(expected, actual);
        }
    }
}