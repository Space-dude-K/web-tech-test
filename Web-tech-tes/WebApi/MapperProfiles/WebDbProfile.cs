using AutoMapper;
using Entities.DTO;
using Entities.DTO.Update;
using Entities.Models;

namespace WebApi.MapperProfiles
{
    public class WebDbProfile : Profile
    {
        public WebDbProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Role, string>()
                .ConvertUsing(src => src.Name);
            CreateMap<User, UserDTO>()
                .ReverseMap();

            CreateMap<User, UserUpdateDTO>();
            CreateMap<User, UserUpdateDTO>()
                .ReverseMap();

            CreateMap<Role, RoleDTO>();
            CreateMap<Role, RoleDTO>()
                .ReverseMap();
        }
    }
}