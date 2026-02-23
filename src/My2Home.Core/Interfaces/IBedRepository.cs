using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
   
    public interface  IBedRepository
    {
        Task<entity.BedEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.BedEntity>> GetAllAsync();
        Task<IEnumerable<entity.BedEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.BedEntity entityToInsert);
        Task<bool> UpdateAsync(entity.BedEntity entityToUpdate);
        Task<bool> UpdateAsync(int bedNumber, string bedStatus);
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<entity.BedEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage,  string searchConditions = null);
        Task<IEnumerable<entity.BedEntity>> GetUnOccupiedBedAsync(int hostelId);
        //Task<bool> DeleteWhereAsync(int id);
        //Task<IEnumerable<entity.BedEntity>> GetWhereAsync(object filters);
    }
}
