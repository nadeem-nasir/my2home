using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Dapper.FastCrud;
using Dapper;

using entity = My2Home.Core.Entities;

namespace My2Home.Infrastructure.Repository
{
    
    public class MessSubscriptionRepository: BaseRepository, IMessSubscriptionRepository
    {
        public MessSubscriptionRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.MessSubscriptionEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.MessSubscriptionEntity>(new entity.MessSubscriptionEntity { MessSubscriptionId = id });
            }
        }


        public async Task<IEnumerable<entity.MessSubscriptionEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.MessSubscriptionEntity>();
            }
        }

        public async Task<IEnumerable<entity.MessSubscriptionEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.MessSubscriptionEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.MessSubscriptionEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.MessSubscriptionId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.MessSubscriptionEntity entityToUpdate)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    return await connection.UpdateAsync(entityToUpdate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    return await connection.DeleteAsync<entity.MessSubscriptionEntity>(new entity.MessSubscriptionEntity { MessSubscriptionId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
