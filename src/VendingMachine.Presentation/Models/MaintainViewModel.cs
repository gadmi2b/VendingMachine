using VendingMachine.BLL.DTO;

namespace VendingMachine.Presentation.Models
{
    public class MaintainViewModel
    {
        public List<DrinkDTO> Drinks { get; set; } = new List<DrinkDTO>();
        public List<CoinDTO> Coins { get; set; } = new List<CoinDTO>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
