using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;


namespace My2Home.Core.Interfaces
{
    
    public interface IOrganizationRepository : IBaseRepository
    {
        Task<entity.OrganizationEntity> GetByIdAsync(int id);
        Task<entity.OrganizationEntity> GetByIdentityUserIdAsync(string identityUserId);
        Task<IEnumerable<entity.OrganizationEntity>> GetAllAsync();
        Task<IEnumerable<entity.OrganizationEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.OrganizationEntity entityToInsert);
        Task<bool> UpdateAsync(entity.OrganizationEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<entity.OrganizationEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null);
        Task<IEnumerable<entity.OrganizationIdentityUser>> GetByOrganizationIdAsync(int OrganizationId);
        Task<bool> DeleteOrganizationUserAsync(string userId);

    }
}
