using System.Linq.Expressions;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Repository
{
    public class VillaRepository : IVillaRepository
    {

        private readonly ApplicationDBContext _db; 

        public VillaRepository(ApplicationDBContext db)
        {
            _db = db; 
        } 

        public async Task Create(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await Save();
        }

        public Task<List<Villa>> Get(Expression<Func<Villa>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<Villa>> GetAll(Expression<Func<Villa>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Villa entity)
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
