namespace VendingMachine.BLL.DTO
{
    public class IndexDTO
    {
        public List<DrinkDTO> drinks { get; set; }
        public List<CoinDTO> coins { get; set; }
        public int CurrentBallance { get; set; }
    }
}
