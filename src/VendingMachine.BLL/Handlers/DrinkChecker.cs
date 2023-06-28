using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Infrastructure;

namespace VendingMachine.BLL.Handlers
{
    /// <summary>
    /// Provides checks of drink
    /// </summary>
    internal class DrinkChecker
    {
        /// <summary>
        /// Checks a drink before it will be added to database
        /// for critical parameters
        /// </summary>
        /// <param name="drink"></param>
        /// <exception cref="VendingMachineException"></exception>
        public void CheckDrinkBeforeAdd(DrinkDTO drink)
        {
            CheckDrink(drink, "add");
        }

        /// <summary>
        /// Checks a drink before it will be updated in the database
        /// for critical parameters
        /// </summary>
        /// <param name="drink"></param>
        /// <exception cref="VendingMachineException"></exception>
        public void CheckDrinkBeforeUpdate(DrinkDTO drink)
        {
            CheckDrink(drink, "update");
        }

        /// <summary>
        /// Privides a common check of drink for different operations
        /// </summary>
        /// <param name="drink"></param>
        /// <param name="checkName">the name of check operation</param>
        /// <exception cref="VendingMachineException"></exception>
        private void CheckDrink(DrinkDTO drink, string checkName)
        {
            if (string.IsNullOrEmpty(drink.Name) || drink.Name.Length > 50)
                throw new VendingMachineException($"Unable to {checkName} drink. " +
                                                   "A length of drink name should be between 1 and 50 symbols.");

            if (drink.Cost <= 0)
                throw new VendingMachineException($"Unable to {checkName} drink. " +
                                                   "A cost of drink should be greater than zero");

            if (drink.Quantity < 0)
                throw new VendingMachineException($"Unable to {checkName} drink. " +
                                                   "A quantity of drink can't be less than zero");
        }
    }
}

