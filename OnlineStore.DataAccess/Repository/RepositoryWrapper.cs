using OnlineStore.DataAccess.Data;
using OnlineStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public ICategoryRepository Category { get; private set; }
        public ISubcategoryRepository Subcategory { get; private set; }
        public IManufacturerRepository Manufacturer { get; private set; }
        public IProductRepository Product { get; private set; } 

        private ApplicationDbContext _db;

        public RepositoryWrapper(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Subcategory = new SubcategoryRepository(_db);
            Manufacturer = new ManufacturerRepository(_db);
            Product = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public void Clear()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }
    }
}
