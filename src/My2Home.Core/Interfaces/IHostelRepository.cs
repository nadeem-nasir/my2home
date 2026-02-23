using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IHostelRepository: IBaseRepository
    {
        Task<entity.HostelEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.HostelEntity>> GetByOrganizationIdAsync(int organizationId);
        Task<IEnumerable<entity.HostelEntity>> GetByCityIdAsync(int cityId);
        Task<IEnumerable<entity.HostelEntity>> GetAllAsync();
        Task<IEnumerable<entity.HostelEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.HostelEntity entityToInsert);
        Task<bool> UpdateAsync(entity.HostelEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

        Task<IEnumerable<entity.HostelEntity>> GetPageListAsync(int pageNumber, int rowsPerPage,  string searchConditions = null);
    }
}
