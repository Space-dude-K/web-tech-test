using AutoMapper;
using Entities.DTO;
using Entities.Models;

namespace WebApi.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Role, string>()
                .ConvertUsing(src => src.Name);
            CreateMap<User, UserDTO>()
                .ReverseMap();
        }
    }
}