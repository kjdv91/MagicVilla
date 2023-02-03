using Village_API.Models;

namespace Village_API.Repository.IRepository
{
    public interface IVillageNumberRepository : IRepository<VillageNumber>
    {
        Task<VillageNumber> Update (VillageNumber entity);
    }
}
