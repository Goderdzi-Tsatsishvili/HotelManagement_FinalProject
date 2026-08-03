
using HotelManagement.Application.Models.Auth;
using HotelManagement.Application.Models.Hotel;
using HotelManagement.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;

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

            config.NewConfig<RegistrationRequestDto, AppUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper())
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper())
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName);
        }
    }
}
