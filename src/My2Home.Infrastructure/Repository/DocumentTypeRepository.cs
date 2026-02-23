using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using entity = My2Home.Core.Entities;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Dapper;

namespace My2Home.Infrastructure.Repository
{
    
    public class DocumentTypeRepository: BaseRepository, IDocumentTypeRepository
    {
        public DocumentTypeRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.DocumentTypeEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.DocumentTypeEntity>(new entity.DocumentTypeEntity { DocumentTypeId = id });
            }
        }


        public async Task<IEnumerable<entity.DocumentTypeEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.DocumentTypeEntity>();
            }
        }

        public async Task<IEnumerable<entity.DocumentTypeEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.DocumentTypeEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.DocumentTypeEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.DocumentTypeId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.DocumentTypeEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.DocumentTypeEntity>(new entity.DocumentTypeEntity { DocumentTypeId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
