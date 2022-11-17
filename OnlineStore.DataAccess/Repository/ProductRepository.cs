using OnlineStore.DataAccess.Data;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            //Product.UpdatedDate = DateTime.Now;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
