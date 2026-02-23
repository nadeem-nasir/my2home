using Dapper.FastCrud;
using Dapper;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
using System.Linq;

namespace My2Home.Infrastructure.Repository
{

    public class RentRepository : BaseRepository, IRentRepository
    {
        private readonly ITenantRepository tenantRepository;
        public RentRepository(SqlConnectionFactory sqlConnectionFactory,
            ITenantRepository tenantRepository) : base(sqlConnectionFactory)
        {
            this.tenantRepository = tenantRepository;
        }

        public async Task<entity.RentEntity> GetByIdAsync(int rentId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                var sql = @"SELECT r.*, t.* FROM Rent r
                            INNER JOIN Tenant t on r.RentTenantId = t.TenantId
                            WHERE  @RentId = @RentId";
                var param = new DynamicParameters();

                param.Add("@RentId", rentId);

                var result = await connection.QueryAsync<entity.RentEntity, entity.TenantEntity, entity.RentEntity>(sql,
                           (r, t) =>
                           {
                               r.RentTenant = t;
                               return r;
                           }, param, splitOn: "TenantId");
                return result.FirstOrDefault();

            }
        }

        public async Task<entity.RentEntity> GetByNameAsync(string RentName)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstAsync<entity.RentEntity>
                    ("SELECT * FROM Rent Where LOWER(RentName) = @RentName", new { RentName = RentName.ToLower() });
                //return await connection.QueryFirst<entity.RentEntity>("", new { });

                //GetAsync<entity.RentEntity>(new entity.RentEntity { RentId = id });
            }
        }

        public async Task<IEnumerable<entity.AccountViewModel>> GetAccountPageListAsync(int hostelId, int year, int pageNumber, int rowsPerPage, string monthName ="")
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM View_Account 
                             WHERE                      
                             RentHostelId = @RentHostelId AND RentYear =@RentYear ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(monthName))
                {
                    sql += "  AND RentMonth =@RentMonth ";
                    param.Add("@RentMonth", monthName);
                }
                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY RentYear, RentMonth "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@RentHostelId", hostelId);
                param.Add("@RentYear", year);
                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);

                
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.AccountViewModel>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<entity.RentEntity>> GetPageListAsync(int hostelId, int pageNumber, int rowsPerPage,
            string monthName = null, int year = 0)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT r.*,t.*,b.RoomId as RoomNumber FROM Rent  r
                INNER JOIN Tenant t on r.RentTenantId = t.TenantId
                INNER JOIN Beds b on t.TenantBedId = b.BedNumber
                INNER JOIN Room rr on rr.RoomId = b.RoomId
                WHERE RentHostelId = @RentHostelId 
                         ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(monthName))
                {
                    sql += " AND  RentMonth = @RentMonth";
                    param.Add("@RentMonth", monthName);
                }

                if (year > 0)
                {
                    sql += " AND  RentYear = @RentYear";
                    param.Add("@RentYear", year);
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY RentMonth "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                param.Add("@RentHostelId", hostelId);

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    //return await connection.QueryAsync<entity.RentEntity>(sql, param);

                    var result = await connection.QueryAsync<entity.RentEntity, entity.TenantEntity, entity.RentEntity>(sql,
                        (r, t) =>
                        {
                            r.RentTenant = t;
                            return r;
                        }, param, splitOn: "TenantId");
                    return result.Distinct().ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<entity.RentEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.RentEntity>();
            }
        }

        public async Task<IEnumerable<entity.RentEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {

                return await connection.QueryAsync<entity.RentEntity>(query, filters);
            }
        }

        private bool CheckIfRentAlreadyExists(entity.RentEntity rentEntity)
        {
            var sql = @"SELECT * FROM Rent Where 
                      RentHostelId=@RentHostelId AND RentTenantId=@RentTenantId AND RentMonth=@RentMonth AND RentYear=@RentYear ";

            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                var result = connection.Query<entity.RentEntity>(sql,
                 new
                 {
                     RentHostelId = rentEntity.RentHostelId,
                     RentMonth = rentEntity.RentMonth,
                     RentYear = rentEntity.RentYear,
                     RentTenantId = rentEntity.RentTenantId
                 });

                if (result != null && result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<int?> InsertRentListAsync(entity.RentEntity entityToInsert)
        {
            try
            {
                int result = 0;
                var sql = @"SELECT * FROM Rent Where 
                      RentHostelId=@RentHostelId AND RentTenantId=@RentTenantId AND RentMonth=@RentMonth AND RentYear=@RentYear ";

                //if (!CheckIfRentAlreadyExists(entityToInsert))
                //{
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    var tenantList = await this.tenantRepository.GeTenantListAsync(entityToInsert.RentHostelId);
                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var tenant in tenantList)
                        {
                            var isAlready = connection.Query<entity.RentEntity>(sql,
                              new
                              {
                                  RentHostelId = entityToInsert.RentHostelId,
                                  RentMonth = entityToInsert.RentMonth,
                                  RentYear = entityToInsert.RentYear,
                                  RentTenantId = tenant.TenantId
                              }, transaction);
                            if (isAlready != null && isAlready.Count() <= 0)
                            {
                                entityToInsert.RentTenantId = tenant.TenantId;
                                entityToInsert.RentAmount = entityToInsert.RentAmount <= 0 ? tenant.TenantBed.BedRent : entityToInsert.RentAmount;

                                await connection.InsertAsync(entityToInsert, st => st.AttachToTransaction(transaction));

                                result += entityToInsert.RentId;

                            }
                        }
                        transaction.Commit();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int?> InsertAsync(entity.RentEntity entityToInsert)
        {
            try
            {
                if (!CheckIfRentAlreadyExists(entityToInsert))
                {
                    using (var connection = GetSqlConnectionFactory.CreateConnection())
                    {
                        var tenant = await this.tenantRepository.GeTenantAsync(entityToInsert.RentHostelId, entityToInsert.RentTenantId);
                        entityToInsert.RentTenantId = tenant.TenantId;
                        entityToInsert.RentAmount = entityToInsert.RentAmount <= 0 ? tenant.TenantBed.BedRent : entityToInsert.RentAmount;
                        await connection.InsertAsync(entityToInsert);
                        return entityToInsert.RentId;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.RentEntity entityToUpdate)
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
        public async Task<bool> UpdateAsync(int rentId, string rentStatus)
        {
            var sql = @"Update Rent SET RentStatus =@RentStatus Where RentId=@RentId";
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    var result = await connection.ExecuteAsync(sql , new
                    { RentStatus= rentStatus,
                        RentId = rentId
                    });
                    return result > 0;
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

                    return await connection.DeleteAsync<entity.RentEntity>(new entity.RentEntity { RentId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //https://www.c-sharpcorner.com/article/crud-operations-in-asp-net-core-2-razor-page-with-dapper-and-repository-pattern/
    }
}
