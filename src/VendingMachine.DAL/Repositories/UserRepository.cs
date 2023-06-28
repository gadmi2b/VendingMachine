using Microsoft.EntityFrameworkCore;
using VendingMachine.DAL.EF;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Interfaces;

namespace VendingMachine.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext ctx) => _context = ctx;

        /// <summary>
        /// Returns user balance by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user balance</returns>
        public async Task<int> GetUserBalanceAsync(long userId)
        {
            User? user = await _context.Users.AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Id == userId);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return user.Balance;
        }

        /// <summary>
        /// Adds value to user balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task AddBalanceAsync(long userId, int value)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            user!.Balance = user.Balance + value;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Reduces user balance for value
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task ReduceBalanceAsync(long userId, int value)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            user!.Balance = user.Balance - value;

            await _context.SaveChangesAsync();
        }
    }
}
