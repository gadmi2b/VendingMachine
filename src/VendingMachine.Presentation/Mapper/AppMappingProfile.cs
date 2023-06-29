using AutoMapper;
using VendingMachine.BLL.DTO;
using VendingMachine.DAL.Entities;
using VendingMachine.Presentation.Models;

namespace VendingMachine.Presentation.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Drink, DrinkDTO>().ReverseMap();
            CreateMap<Coin, CoinDTO>();

            CreateMap<IndexDTO, IndexViewModel>();
            CreateMap<MaintainDTO, MaintainViewModel>()
                .ForMember(dest => dest.ErrorMessages, opt => opt.Ignore());
        }
    }
}
