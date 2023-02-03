using System.Linq.Expressions;
using Village_API.Datos;
using Village_API.Models;
using Village_API.Repository.IRepository;

namespace Village_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Villa> Update(Villa entity)
        {
            entity.EmitionCreated = DateTime.Now;
            _context.Villas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
            
        }
    }
}
