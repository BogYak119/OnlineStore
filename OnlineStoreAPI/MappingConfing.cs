using AutoMapper;
using OnlineStore.Models;
using OnlineStore.Models.DTO;

namespace OnlineStoreAPI
{
    public class MappingConfing : Profile
    {
        public MappingConfing()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryCreateDTO>().ReverseMap();
        }
    }
}
