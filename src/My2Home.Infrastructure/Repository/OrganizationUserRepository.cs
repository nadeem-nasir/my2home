using Dapper.FastCrud;
using Dapper;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

namespace My2Home.Infrastructure.Repository
{
    
    public class OrganizationUserRepository: BaseRepository, IOrganizationUserRepository
    {
        public OrganizationUserRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.OrganizationUserEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.OrganizationUserEntity>(new entity.OrganizationUserEntity { OrganizationUserId = id });
            }
        }


        public async Task<IEnumerable<entity.OrganizationUserEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.OrganizationUserEntity>();
            }
        }

        public async Task<IEnumerable<entity.OrganizationUserEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.OrganizationUserEntity>(query, filters);
            }
        }

        public async Task<entity.OrganizationUserEntity> GetByIdentityUserIdAsync(string identityUserId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<entity.OrganizationUserEntity>(@"SELECT * FROM OrganizationUsers
                     WHERE OrganizationIdentityUserId = @OrganizationIdentityUserId", new { OrganizationIdentityUserId = identityUserId });
            }
        }

        public async Task<int?> InsertAsync(entity.OrganizationUserEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.OrganizationUserId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.OrganizationUserEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.OrganizationUserEntity>(new entity.OrganizationUserEntity { OrganizationUserId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
