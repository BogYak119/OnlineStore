using OnlineStore.DataAccess.Data;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository
{
    public class SubcategoryRepository : Repository<Subcategory>, ISubcategoryRepository
    {
        private ApplicationDbContext _db;

        public SubcategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Subcategory> UpdateAsync(Subcategory subcategory)
        {
            Subcategory subcategoryFromDb = _db.Subcategories.FirstOrDefault(sc => sc.Id == subcategory.Id);
            if (subcategoryFromDb != null)
            {
                subcategoryFromDb.Name = subcategory.Name;
                subcategoryFromDb.CategoryId = subcategory.CategoryId;
            }
            await _db.SaveChangesAsync();
            return subcategory;
        }
    }
}
