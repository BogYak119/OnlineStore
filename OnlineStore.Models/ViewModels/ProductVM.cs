using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class ProductVM
    {
        public ProductDTO ProductDTO { get; set; }
        //[ValidateNever]
        //public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SubcategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ManufacturerList { get; set; }
    }
}
