using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Core.Interfaces
{
    
    public interface  IHostelsDocumentRepository: IBaseRepository
    {
        Task<entity.HostelsDocumentEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.HostelsDocumentEntity>> GetAllAsync();
        Task<IEnumerable<entity.HostelsDocumentEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.HostelsDocumentEntity entityToInsert);
        Task<bool> UpdateAsync(entity.HostelsDocumentEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
