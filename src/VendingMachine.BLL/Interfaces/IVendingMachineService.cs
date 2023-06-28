using VendingMachine.BLL.DTO;

namespace VendingMachine.BLL.Interfaces
{
    public interface IVendingMachineService
    {
        Task<IndexDTO> GetIndexDTOAsync();
        Task<MaintainDTO> GetMaintainDTOAsync();
        Task AddCoinToBalanceAsync(long coinId);
        Task<int> GetUserBalanceAsync();
        Task PurchaseDrinkAsync(long drinkId);
        Task<int> GetDrinkQuantityAsync(long drinkId);
        Task<Dictionary<CoinDTO, int>> WithdrawAsync();
        Task<string> DeleteDrinkAsync(long drinkId);
        Task AddDrinkAsync(DrinkDTO drink);
        Task UpdateDrinkAsync(DrinkDTO drink);
        Task<bool> ChangeCoinIsJammedState(long coinId);
        Task<List<DrinkDTO>> GetDrinksAsync();
    }
}