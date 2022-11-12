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
    public class SubcategoryRepository : Repository<Subcategory>, ISubcategoryRepository
    {
        private ApplicationDbContext _db;

        public SubcategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Subcategory subcategory)
        {
            _db.Subcategories.Update(subcategory);
        }
    }
}
