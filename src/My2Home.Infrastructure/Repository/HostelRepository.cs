using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using entity = My2Home.Core.Entities;

using Dapper.FastCrud;
using Dapper;


namespace My2Home.Infrastructure.Repository
{

    public class HostelRepository : BaseRepository, IHostelRepository
    {
        public HostelRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {
        }

        public async Task<IEnumerable<entity.HostelEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM Hostel    ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " Where  HostelName LIKE @HostelName";
                    param.Add("@HostelName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY HostelName "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);


                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.HostelEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<entity.HostelEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.HostelEntity>(new entity.HostelEntity { HostelId = id });
            }
        }

        public async Task<IEnumerable<entity.HostelEntity>> GetByOrganizationIdAsync(int organizationId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.HostelEntity>(@"SELECT * from Hostel where HostelOrganizationId =@HostelOrganizationId ", new { HostelOrganizationId = organizationId });
            }
        }

        public async Task<IEnumerable<entity.HostelEntity>> GetByCityIdAsync(int cityId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.HostelEntity>(@"SELECT * from Hostel where HostelCityId =@HostelCityId ", new { HostelCityId = cityId });
            }
        }

        public async Task<IEnumerable<entity.HostelEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.HostelEntity>();
            }
        }

        public async Task<IEnumerable<entity.HostelEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.HostelEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.HostelEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.HostelId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.HostelEntity entityToUpdate)
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
            var sql = @"
    DELETE FROM Expense Where ExpenseHostelId = @HostelId 
    DELETE FROM Rent where RentHostelId = @HostelId
    DELETE b FROM Beds b INNER JOIN Room r on b.RoomId = r.RoomId Where RoomHostelId  = @HostelId
    Delete FROM Room Where RoomHostelId  = @HostelId
    DELETE FROM Tenant where TenantHostelId = @HostelId
    DELETE FROM Hostel Where HostelId = @HostelId";

            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteAsync(sql, new { HostelId = id }, transaction: transaction);
                        transaction.Commit();
                        if (result > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    //return await connection.DeleteAsync<entity.HostelEntity>(new entity.HostelEntity { HostelId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
