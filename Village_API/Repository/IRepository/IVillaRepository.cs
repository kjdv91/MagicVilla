using Village_API.Models;

namespace Village_API.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>

    {
       
        Task<Villa> Update(Villa entity);

    }
}
