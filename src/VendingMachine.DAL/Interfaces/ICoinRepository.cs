using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface ICoinRepository
    {
        List<Coin> GetCoins();

        Coin GetCoin(long coinId);
    }
}
