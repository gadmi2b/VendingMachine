namespace VendingMachine.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<int> GetUserBalanceAsync(long userId);

        Task AddBalanceAsync(long userId, int value);

        Task ReduceBalanceAsync(long userId, int value);
    }
}
