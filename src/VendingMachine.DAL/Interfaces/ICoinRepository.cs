using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface ICoinRepository
    {
        Task<List<Coin>> GetCoinsAsync();
        Task<Coin> GetCoinAsync(long coinId);
        Task<bool> ChangeIsJammedStateReturnState(long coinId);    
    }
}
