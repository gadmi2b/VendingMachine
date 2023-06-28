using VendingMachine.BLL.DTO;

namespace VendingMachine.Presentation.Models
{
    public class IndexViewModel
    {
        public List<DrinkDTO> Drinks { get; set; } = new List<DrinkDTO>();
        public List<CoinDTO> Coins { get; set; } = new List<CoinDTO>();
        public int Balance { get; set; }
    }
}
