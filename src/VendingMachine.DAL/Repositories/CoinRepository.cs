using Microsoft.EntityFrameworkCore;
using VendingMachine.DAL.EF;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Interfaces;

namespace VendingMachine.DAL.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private DataContext _context;
        public CoinRepository(DataContext ctx) => _context = ctx;

        public List<Coin> GetCoins()
        {
            List<Coin> result = _context.Coins.AsNoTracking()
                .ToList();
            return result;
        }

        public Coin GetCoin(long coinId)
        {
            return _context.Coins.AsNoTracking()
                .Where(c => c.Id == coinId)
                .First();
        }
    }
}
