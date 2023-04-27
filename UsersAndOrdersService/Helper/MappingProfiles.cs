using AutoMapper;
using UsersAndOrdersService.Data.DTO;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserCreationDTO>();
            CreateMap<UserCreationDTO, User>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, UserLoginDTO>();
            CreateMap<UserLoginDTO, User>();

            CreateMap<OrderedItem, OrderedItemDTO>();
            CreateMap<OrderedItemDTO, OrderedItem>();

            CreateMap<Order, OrderInfoDTO>();
            CreateMap<OrderInfoDTO, Order>();
        }
    }
}
