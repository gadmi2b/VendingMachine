namespace VendingMachine.DAL.Entities
{
    /// <summary>
    /// Represents drink used in Vending Machine
    /// </summary>
    public class Drink
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }
    }
}
