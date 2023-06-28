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

        /// <summary>
        /// Returns all drinks presented in the database
        /// </summary>
        /// <returns>all drinks</returns>
        public async Task<List<Drink>> GetDrinksAsync()
        {
            return await _context.Drinks.AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns a drink by drinkId
        /// </summary>
        /// <param name="drinkId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>drink</returns>
        public async Task<Drink> GetDrinkAsync(long drinkId)
        {
            Drink? drink = await _context.Drinks.AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Id == drinkId);
            ArgumentNullException.ThrowIfNull(drink, nameof(drink));

            return drink;
        }

        /// <summary>
        /// Updates quantity of drink
        /// </summary>
        /// <param name="updatedDrink"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public async Task UpdateQuantityAsync(Drink updatedDrink)
        {
            Drink? originalDrink = _context.Drinks
                            .FirstOrDefault(u => u.Id == updatedDrink.Id);

            ArgumentNullException.ThrowIfNull(originalDrink, nameof(originalDrink));

            originalDrink.Quantity = updatedDrink.Quantity;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes drink by drinkId
        /// </summary>
        /// <param name="drinkId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public async Task DeleteDrinkAsync(long drinkId)
        {
            Drink? drink = await _context.Drinks
                            .FirstOrDefaultAsync(u => u.Id == drinkId);

            ArgumentNullException.ThrowIfNull(drink, nameof(drink));

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if there is a drink in the database with same name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>true if a drink with same name is already exist</returns>
        public async Task<bool> IsExistWithSameNameAsync(string Name)
        {
            return await _context.Drinks
                            .AnyAsync(u => u.Name == Name);
                            
        }

        /// <summary>
        /// Adds a new drink to the database
        /// </summary>
        /// <param name="drink"></param>
        /// <returns></returns>
        public async Task AddDrinkAsync(Drink drink)
        {
            await _context.Drinks.AddAsync(drink);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates existing drink in the database
        /// </summary>
        /// <param name="updatedDrink"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public async Task UpdateDrinkAsync(Drink updatedDrink)
        {
            Drink? initialDrink = await _context.Drinks
                            .FirstOrDefaultAsync(u => u.Id == updatedDrink.Id);

            ArgumentNullException.ThrowIfNull(initialDrink, nameof(initialDrink));

            initialDrink.Name = updatedDrink.Name;
            initialDrink.Cost = updatedDrink.Cost;
            initialDrink.Quantity = updatedDrink.Quantity;

            await _context.SaveChangesAsync();
        }
    }
}
