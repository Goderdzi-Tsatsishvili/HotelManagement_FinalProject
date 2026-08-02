
using HotelManagement.Application.Models.Hotel;
using HotelManagement.Domain.Entities;
using Mapster;

namespace HotelManagement.Application.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<HotelForCreatingDto, Hotel>();
            config.NewConfig<Hotel, HotelForGettingDto>();
            config.NewConfig<HotelForUpdatingDto, Hotel>();
            config.NewConfig<Hotel, HotelListForGettingDto>();
        }
    }
}
