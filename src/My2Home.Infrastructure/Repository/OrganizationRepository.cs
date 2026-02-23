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

    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {
        }

        public async Task<IEnumerable<entity.OrganizationIdentityUser>> GetByOrganizationIdAsync(int OrganizationId)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    return await connection.QueryAsync<entity.OrganizationIdentityUser>
                        (@" SELECT u.UserName, u.id as UserId, u.email, o.OrganizationId, u.FirstName, u.LockoutEnd,u.LockoutEnabled
                    FROM Organization o 
                    INNER JOIN OrganizationUsers ou on o.OrganizationId =  ou.OrganizationId
                    INNER JOIN AspNetUsers u on ou.OrganizationIdentityUserId = u.Id Where o.OrganizationId =@OrganizationId ",
                        new { OrganizationId = OrganizationId });
                }
            }
            catch(Exception ex )
            {
                throw ex;
            }

        }
            public async Task<entity.OrganizationEntity> GetByIdentityUserIdAsync(string identityUserId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<entity.OrganizationEntity>(@"SELECT o.* FROM Organization o inner join OrganizationUsers u 
                on o.OrganizationId = u.OrganizationId  where u.OrganizationIdentityUserId = @OrganizationIdentityUserId ", new { OrganizationIdentityUserId = identityUserId });
            }
        }
        //public int OrganizationId {get;set;}
        //public string OrganizationName { get; set; }
        //public string OrganizationAddress { get; set; }
        //public string OrganizationPhoneNumber { get; set; }

        public async  Task<IEnumerable<entity.OrganizationEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM Organization  WHERE OrganizationId = @OrganizationId  ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " AND  OrganizationName LIKE @OrganizationName ";
                    param.Add("@OrganizationName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY OrganizationName "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.OrganizationEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public async Task<entity.OrganizationEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.OrganizationEntity>(new entity.OrganizationEntity { OrganizationId = id });
            }
        }


        public async Task<IEnumerable<entity.OrganizationEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.OrganizationEntity>();
            }
        }

        public async Task<IEnumerable<entity.OrganizationEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.OrganizationEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.OrganizationEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.OrganizationId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.OrganizationEntity entityToUpdate)
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
                    return await connection.DeleteAsync<entity.OrganizationEntity>(new entity.OrganizationEntity { OrganizationId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteOrganizationUserAsync(string userId)

        {
            var sql = @"
            Delete FROM OrganizationUsers where OrganizationIdentityUserId =@UserId
            Delete from AspNetUserClaims where UserId =@UserId
            Delete from  AspNetUserLogins where UserId =@UserId
            Delete from  AspNetUserRoles where UserId =@UserId
            Delete from  AspNetUserTokens where UserId =@UserId ";
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteAsync(sql, new { UserId = userId }, transaction: transaction);
                        transaction.Commit();
                        if (result > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
