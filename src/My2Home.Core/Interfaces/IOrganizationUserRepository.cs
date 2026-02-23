using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IOrganizationUserRepository: IBaseRepository
    {
        Task<entity.OrganizationUserEntity> GetByIdAsync(int id);
        Task<entity.OrganizationUserEntity> GetByIdentityUserIdAsync(string identityUserId);
        Task<IEnumerable<entity.OrganizationUserEntity>> GetAllAsync();
        Task<IEnumerable<entity.OrganizationUserEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.OrganizationUserEntity entityToInsert);
        Task<bool> UpdateAsync(entity.OrganizationUserEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

    }
}
