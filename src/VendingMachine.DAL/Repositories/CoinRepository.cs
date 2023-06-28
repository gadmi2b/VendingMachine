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

        /// <summary>
        /// Returns all coins from Database
        /// </summary>
        /// <returns>all coins</returns>
        public async Task<List<Coin>> GetCoinsAsync()
        {
            return await _context.Coins.AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns a coin from Database by coinId
        /// </summary>
        /// <param name="coinId"></param>
        /// <returns>coin</returns>
        public async Task<Coin> GetCoinAsync(long coinId)
        {
            Coin? coin = await _context.Coins.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == coinId);
            ArgumentNullException.ThrowIfNull(coin, nameof(coin));

            return coin;
        }

        /// <summary>
        /// Changes IsJammed state of coin by coinId
        /// returns actual IsJammed state of coin
        /// </summary>
        /// <param name="coinId"></param>
        /// <returns>actual IsJammed state of coin</returns>
        public async Task<bool> ChangeIsJammedStateReturnState(long coinId)
        {
            Coin? coin = await _context.Coins
                .FirstOrDefaultAsync(c => c.Id == coinId);
            ArgumentNullException.ThrowIfNull(coin, nameof(coin));

            coin.IsJammed = !coin.IsJammed;
            await _context.SaveChangesAsync();

            return coin.IsJammed;
        }
    }
}
