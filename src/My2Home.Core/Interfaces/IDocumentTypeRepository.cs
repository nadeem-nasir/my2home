using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Core.Interfaces
{
    
    public interface IDocumentTypeRepository: IBaseRepository
    {
        Task<entity.DocumentTypeEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.DocumentTypeEntity>> GetAllAsync();
        Task<IEnumerable<entity.DocumentTypeEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.DocumentTypeEntity entityToInsert);
        Task<bool> UpdateAsync(entity.DocumentTypeEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

    }
}
