using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;


namespace My2Home.Core.Interfaces
{
    
    public interface  IRoomRepository: IBaseRepository
    {
        Task<entity.RoomEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.RoomEntity>> GetAllAsync();
        Task<IEnumerable<entity.RoomEntity>> GetByHostelIdAsync(int hostelId);
        Task<IEnumerable<entity.RoomEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.RoomEntity entityToInsert);
        Task<int?> InsertAsync(entity.RoomEntity entityToInsert, int numberOfRoomesToCreate);
        Task<bool> UpdateAsync(entity.RoomEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

        Task<IEnumerable<entity.RoomEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage,  string searchConditions = null);

    }
}
