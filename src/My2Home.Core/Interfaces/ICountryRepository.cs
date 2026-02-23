using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Core.Interfaces
{
    
    public interface ICountryRepository : IBaseRepository
    {
        Task<entity.CountryEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.CountryEntity>> GetAllAsync();
        Task<IEnumerable<entity.CountryEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null);
        Task<IEnumerable<entity.CountryEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.CountryEntity entityToInsert);
        Task<bool> UpdateAsync(entity.CountryEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);


    }
}
