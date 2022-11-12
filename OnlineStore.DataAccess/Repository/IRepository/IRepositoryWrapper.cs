using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository.IRepository
{
    public interface IRepositoryWrapper
    {
        ICategoryRepository Category { get; }
        ISubcategoryRepository Subcategory { get; }
        IManufacturerRepository Manufacturer { get; }   
        IProductRepository Product { get; } 
        void Save();
        void Clear();
    }
}
