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
    
    public class HostelsDocumentRepository: BaseRepository, IHostelsDocumentRepository
    {
        public HostelsDocumentRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.HostelsDocumentEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.HostelsDocumentEntity>(new entity.HostelsDocumentEntity { HostelsDocumentId = id });
            }
        }


        public async Task<IEnumerable<entity.HostelsDocumentEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.HostelsDocumentEntity>();
            }
        }

        public async Task<IEnumerable<entity.HostelsDocumentEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.HostelsDocumentEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.HostelsDocumentEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.HostelsDocumentId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.HostelsDocumentEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.HostelsDocumentEntity>(new entity.HostelsDocumentEntity { HostelsDocumentId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
