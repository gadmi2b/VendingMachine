using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface IDrinkRepository
    {
        List<Drink> GetDrinks();

        Drink GetDrink(long drinkId);

        void UpdateQuantity(Drink updatedDrink);
    }
}
