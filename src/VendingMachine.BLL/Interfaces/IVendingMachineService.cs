using VendingMachine.BLL.DTO;

namespace VendingMachine.BLL.Interfaces
{
    public interface IVendingMachineService
    {
        IndexDTO GetIndexDTO();
        MaintainDTO GetMaintainDTO();

        void AddCoinToBalance(long coinId);

        int GetUserBalance();

        void PurchaseDrink(long drinkId);

        int GetDrinkQuantity(long drinkId);

        Dictionary<CoinDTO, int> Withdraw();
    }
}
