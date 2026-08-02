
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
        }
    }
}
