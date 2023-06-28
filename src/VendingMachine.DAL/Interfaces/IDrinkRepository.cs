using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface IDrinkRepository
    {
        Task<List<Drink>> GetDrinksAsync();

        Task<Drink> GetDrinkAsync(long drinkId);

        Task UpdateQuantityAsync(Drink updatedDrink);
        Task DeleteDrinkAsync(long drinkId);
        Task<bool> IsExistWithSameNameAsync(string Name);
        Task AddDrinkAsync(Drink drink);
        Task UpdateDrinkAsync(Drink updatedDrink);
    }
}
