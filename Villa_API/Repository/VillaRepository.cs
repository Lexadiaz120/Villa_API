using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Repository
{
    public class VillaRepository : Repository<Villa>,  IVillaRepository
    {

        private readonly ApplicationDBContext _db; 

        public VillaRepository(ApplicationDBContext db) : base(db)
        {
            _db = db; 
        } 

      
       
        public async  Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
