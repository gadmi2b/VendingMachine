using VendingMachine.BLL.DTO;

namespace VendingMachine.Presentation.Models
{
    public class IndexViewModel
    {
        public List<DrinkDTO> drinks { get; set; } = new List<DrinkDTO>();
        public List<CoinDTO> coins { get; set; } = new List<CoinDTO>();
        public int CurrentBallance { get; set; }
    }
}
