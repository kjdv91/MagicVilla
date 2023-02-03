using Village_API.Datos;
using Village_API.Models;
using Village_API.Repository.IRepository;

namespace Village_API.Repository
{
    public class VillageNumberRepository : Repository<VillageNumber>, IVillageNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public VillageNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<VillageNumber> Update(VillageNumber entity)
        {
            entity.CreatedDate = DateTime.Now;
            _context.VillageNumbers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }
    }
}
