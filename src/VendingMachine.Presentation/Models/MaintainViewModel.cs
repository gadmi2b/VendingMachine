using VendingMachine.BLL.DTO;

namespace VendingMachine.Presentation.Models
{
    public class MaintainViewModel
    {
        public List<DrinkDTO> drinks { get; set; } = new List<DrinkDTO>();
        public List<CoinDTO> coins { get; set; } = new List<CoinDTO>();
    }
}
