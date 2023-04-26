using AutoMapper;
using ItemsService.Model;

namespace ItemsService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Item, ItemForSearch>();
            CreateMap<ItemForSearch, Item>();
        }
    }
}
