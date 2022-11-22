using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineStore.Models.DTO
{
    public class ProductCreateDTO
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        [ValidateNever]

        public int? Discount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
