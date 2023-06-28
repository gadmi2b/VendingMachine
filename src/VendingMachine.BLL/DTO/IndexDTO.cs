namespace VendingMachine.BLL.DTO
{
    public class IndexDTO
    {
        public List<DrinkDTO> Drinks { get; set; }
        public List<CoinDTO> Coins { get; set; }
        public int Ballance { get; set; }
    }
}
