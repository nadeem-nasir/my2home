using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IRentRepository : IBaseRepository
    {
        Task<entity.RentEntity> GetByIdAsync(int rentId);
        Task<IEnumerable<entity.RentEntity>> GetAllAsync();
        Task<IEnumerable<entity.RentEntity>> GetWhereAsync(string query, object filters);       
        Task<int?> InsertAsync(entity.RentEntity entityToInsert);
        Task<bool> UpdateAsync(entity.RentEntity entityToUpdate);
        Task<entity.RentEntity> GetByNameAsync(string RentName);
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<entity.RentEntity>> GetPageListAsync(int hostelId, int pageNumber, int rowsPerPage, string monthName = null, int year = 0);
        //Task<IEnumerable<entity.RentEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, int countryId = 1, string searchConditions = null);

        Task<int?> InsertRentListAsync(entity.RentEntity entityToInsert);
        Task<IEnumerable<entity.AccountViewModel>> GetAccountPageListAsync(int hostelId, int year, int pageNumber, int rowsPerPage, string monthName = "");
        Task<bool> UpdateAsync(int rentId, string rentStatus);
            //Task<bool> DeleteWhereAsync(int id);
            //Task<IEnumerable<entity.RentEntity>> GetWhereAsync(object filters);

    }
}
