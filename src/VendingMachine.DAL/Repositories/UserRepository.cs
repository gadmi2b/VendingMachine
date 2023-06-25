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

        public int GetUserBalance(long userId)
        {
            User? user = _context.Users.AsNoTracking()
                            .First(u => u.Id == userId);
            ArgumentNullException.ThrowIfNull(user, nameof(userId)); ;

            return user.Balance;
        }

        public void AddBalance(long userId, int value)
        {
            User? user = _context.Users.Find(userId);
            ArgumentNullException.ThrowIfNull(user, nameof(userId));

            user!.Balance = user.Balance + value;

            _context.SaveChanges();
        }

        public void ReduceBalance(long userId, int value)
        {
            User? user = _context.Users.Find(userId);
            ArgumentNullException.ThrowIfNull(user, nameof(userId));

            user!.Balance = user.Balance - value;

            _context.SaveChanges();
        }
    }
}
