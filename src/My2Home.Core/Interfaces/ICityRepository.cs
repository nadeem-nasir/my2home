using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  ICityRepository : IBaseRepository
    {
        Task<entity.CityEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.CityEntity>> GetAllAsync();
        Task<IEnumerable<entity.CityEntity>> GetWhereAsync(string query, object filters);       
        Task<int?> InsertAsync(entity.CityEntity entityToInsert);
        Task<bool> UpdateAsync(entity.CityEntity entityToUpdate);
        Task<entity.CityEntity> GetByNameAsync(string cityName);
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<entity.CityEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, int countryId = 1, string searchConditions = null);
        //Task<bool> DeleteWhereAsync(int id);
        //Task<IEnumerable<entity.CityEntity>> GetWhereAsync(object filters);

    }
}
