using AutoMapper;
using UsersAndOrdersService.Data.DTO;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserLoginDTO>();
            CreateMap<UserLoginDTO, User>();
        }
    }
}
