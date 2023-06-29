using Moq;
using AutoMapper;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Services;
using VendingMachine.DAL.Interfaces;
using VendingMachine.Presentation.Mapper;
using VendingMachine.DAL.Entities;

namespace VendingMachine.Tests.BLL
{
    public class VendingMachineServiceTests
    {
        IVendingMachineService _vendingMachineService { get; set; }
        Mock<IDrinkRepository> _drinkRepoMock;
        Mock<ICoinRepository> _coinRepoMock;
        Mock<IUserRepository> _userRepoMock;

        public VendingMachineServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _coinRepoMock = new Mock<ICoinRepository>();
            _drinkRepoMock = new Mock<IDrinkRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());

            _vendingMachineService = new VendingMachineService(_drinkRepoMock.Object,
                                                               _coinRepoMock.Object,
                                                               _userRepoMock.Object,
                                                               new Mapper(config));
        }

        [Fact]
        public void AddCoinToBalanceAsync_AddCoin_ReturnsNoExceptions()
        {

            _userRepoMock.Setup(repo => repo.AddBalanceAsync(1, 1)).Returns(Task.CompletedTask);
            _coinRepoMock.Setup(repo => repo.GetCoinAsync(1)).ReturnsAsync(new Coin { });

            var task = Record.ExceptionAsync(() => _vendingMachineService.AddCoinToBalanceAsync(1));

            Assert.Null(task.Exception);

        }
    }
}
