using AutoMapper;
using OnlineStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class DefaultMappingConfig : Profile
    {
        public DefaultMappingConfig()
        {
            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryCreateDTO>().ReverseMap();

            //Subcategory
            CreateMap<Subcategory, SubcategoryDTO>().ReverseMap();
            CreateMap<Subcategory, SubcategoryCreateDTO>().ReverseMap();
            CreateMap<SubcategoryDTO, SubcategoryCreateDTO>().ReverseMap();

            //Manufacturer
            CreateMap<Manufacturer, ManufacturerDTO>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerCreateDTO>().ReverseMap();
            CreateMap<ManufacturerDTO, ManufacturerCreateDTO>().ReverseMap();

            //Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductCreateDTO>().ReverseMap();
        }
    }
}
