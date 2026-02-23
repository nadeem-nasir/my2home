using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IExpenseCategoryRepository: IBaseRepository
    {
        Task<entity.ExpenseCategoryEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.ExpenseCategoryEntity>> GetAllAsync();
        Task<IEnumerable<entity.ExpenseCategoryEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null);
        Task<IEnumerable<entity.ExpenseCategoryEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.ExpenseCategoryEntity entityToInsert);
        Task<bool> UpdateAsync(entity.ExpenseCategoryEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
