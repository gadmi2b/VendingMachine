namespace VendingMachine.DAL.Entities
{
    /// <summary>
    /// Represents coin used by Vending Machine
    /// </summary>
    public class Coin
    {
        public long Id { get; set; }
        public int Nominal { get; set; }
        public bool IsJammed { get; set; }
    }
}
