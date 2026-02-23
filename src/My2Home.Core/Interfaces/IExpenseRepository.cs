using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IExpenseRepository: IBaseRepository
    {
        Task<entity.ExpenseEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.ExpenseEntity>> GetAllAsync();
        Task<IEnumerable<entity.ExpenseEntity>> GetWhereAsync(string query, object filters);
        Task<IEnumerable<entity.ExpenseEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage, string monthName = null, int year = 0);
        Task<int?> InsertAsync(entity.ExpenseEntity entityToInsert);
        Task<bool> UpdateAsync(entity.ExpenseEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
