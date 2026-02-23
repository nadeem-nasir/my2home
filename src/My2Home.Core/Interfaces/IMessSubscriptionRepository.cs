using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Core.Interfaces
{
    
    public interface  IMessSubscriptionRepository: IBaseRepository
    {
        Task<entity.MessSubscriptionEntity> GetByIdAsync(int id);
        Task<IEnumerable<entity.MessSubscriptionEntity>> GetAllAsync();
        Task<IEnumerable<entity.MessSubscriptionEntity>> GetWhereAsync(string query, object filters);
        Task<int?> InsertAsync(entity.MessSubscriptionEntity entityToInsert);
        Task<bool> UpdateAsync(entity.MessSubscriptionEntity entityToUpdate);
        Task<bool> DeleteByIdAsync(int id);

    }
}
