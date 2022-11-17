using AutoMapper;
using OnlineStore.Models;
using OnlineStore.Models.DTO;

namespace OnlineStoreAPI
{
    public class MappingConfing : Profile
    {
        public MappingConfing()
        {
            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryCreateDTO>().ReverseMap();

            //Subcategory
            CreateMap<Subcategory, SubcategoryDTO>().ReverseMap();
            CreateMap<Subcategory, SubcategoryCreateDTO>().ReverseMap();
            CreateMap<SubcategoryDTO, SubcategoryCreateDTO>().ReverseMap();
        }
    }
}
