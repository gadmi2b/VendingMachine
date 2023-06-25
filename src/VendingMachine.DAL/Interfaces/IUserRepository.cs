namespace VendingMachine.DAL.Interfaces
{
    public interface IUserRepository
    {
        int GetUserBalance(long userId);

        void AddBalance(long userId, int value);

        void ReduceBalance(long userId, int value);
    }
}
