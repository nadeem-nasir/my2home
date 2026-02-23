using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;


namespace My2Home.Core.Interfaces
{
   
    public interface  ITenantRepository : IBaseRepository
    {
        Task<entity.TenantEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.TenantEntity>> GetAllAsync();
        Task<IEnumerable<entity.TenantEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.TenantEntity entityToInsert);
        Task<bool> UpdateAsync(entity.TenantEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

        Task<IEnumerable<entity.TenantEntity>> GetPageListAsync(int hostelId, int pageNumber, int rowsPerPage, string searchConditions = null);
        Task<IEnumerable<entity.TenantEntity>> GeTenantListAsync(int hostelId, string tenantStatus = "active");

        Task<entity.TenantEntity> GeTenantAsync(int hostelId, int tenantId, string tenantStatus = "active");

    }
}
