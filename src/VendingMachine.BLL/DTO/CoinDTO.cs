namespace VendingMachine.BLL.DTO
{
    public class CoinDTO
    {
        public long Id { get; set; }
        public int Nominal { get; set; }
        public bool IsJammed { get; set; }
    }
}
