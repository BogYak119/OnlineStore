using Microsoft.AspNetCore.Mvc;
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
    public class SubcategoryVM
    {
        [BindProperty]
        public SubcategoryDTO SubcategoryDTO { get; set; }

        [BindProperty]
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
