namespace VendingMachine.DAL.Entities
{
    /// <summary>
    /// Represents User who uses Vending Machine
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public int Balance { get; set; }
    }
}
