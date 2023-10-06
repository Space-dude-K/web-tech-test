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
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}