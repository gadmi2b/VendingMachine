using Microsoft.EntityFrameworkCore;
using VendingMachine.DAL.EF;
using VendingMachine.DAL.Entities;
using VendingMachine.DAL.Interfaces;

namespace VendingMachine.DAL.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private DataContext _context;

        public DrinkRepository(DataContext ctx) => _context = ctx;

        public List<Drink> GetDrinks()
        {
            List<Drink> result = _context.Drinks.AsNoTracking()
                .ToList();
            return result;
        }

        public Drink GetDrink(long drinkId)
        {
            Drink? drink = _context.Drinks.AsNoTracking()
                            .First(u => u.Id == drinkId);
            ArgumentNullException.ThrowIfNull(drink, nameof(drinkId));

            return drink;
        }

        public void UpdateQuantity(Drink updatedDrink)
        {
            Drink? originalDrink = _context.Drinks
                            .First(u => u.Id == updatedDrink.Id);

            ArgumentNullException.ThrowIfNull(originalDrink, nameof(updatedDrink.Id));

            originalDrink.Quantity = updatedDrink.Quantity;

            _context.SaveChanges();
        }
    }
}
