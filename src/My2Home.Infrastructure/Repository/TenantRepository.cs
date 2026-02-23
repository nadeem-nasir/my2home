using Dapper.FastCrud;
using Dapper;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
using System.Linq;
using My2Home.Core.CommonEnums;

namespace My2Home.Infrastructure.Repository
{
   
    public  class TenantRepository: BaseRepository, ITenantRepository
    {
        public TenantRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<IEnumerable<entity.TenantEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage, string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT t.*,  r.RoomNumber
                             FROM Tenant t inner join Beds b on t.TenantBedId = b.BedNumber
                             inner join Room r on b.RoomId = r.RoomId where t.TenantHostelId =@TenantHostelId ";

                var param = new DynamicParameters();
                param.Add("@TenantHostelId", hostelId);

                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " AND   t.TenantName LIKE @TenantName";
                    param.Add("@TenantName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY TenantName "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.TenantEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<entity.TenantEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.TenantEntity>(new entity.TenantEntity { TenantId = id });
            }
        }

        public async Task<IEnumerable<entity.TenantEntity>> GeTenantListAsync(int hostelId, string tenantStatus = "active")
        {
            var sql = @"SELECT t.*, b.* from Tenant t  INNER JOIN  Beds b on t.TenantBedId = b.BedNumber
                        Where t.TenantHostelId = @TenantHostelId AND t.TenantStatus = @TenantStatus ";
            var param = new DynamicParameters();
            param.Add("@TenantHostelId", hostelId);
            param.Add("@TenantStatus", tenantStatus);
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                var result = await connection.QueryAsync<entity.TenantEntity, entity.BedEntity, entity.TenantEntity>(sql,
                    (t, b) =>
                    {
                        t.TenantBed = b;
                        return t;
                    }, param, splitOn: "BedNumber");
                return result.Distinct().ToList();
            }
        }

        public async Task<entity.TenantEntity> GeTenantAsync(int hostelId, int tenantId ,string tenantStatus = "active")
        {
            var sql = @"SELECT t.*, b.* from Tenant t  INNER JOIN  Beds b on t.TenantBedId = b.BedNumber
                        Where t.TenantHostelId = @TenantHostelId AND t.TenantStatus = @TenantStatus
                              AND t.TenantId = @TenantId ";
            var param = new DynamicParameters();
            param.Add("@TenantHostelId", hostelId);
            param.Add("@TenantId", tenantId);
            param.Add("@TenantStatus", tenantStatus);

            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                var result = await connection.QueryAsync<entity.TenantEntity, entity.BedEntity, entity.TenantEntity>(sql,
                    (t, b) =>
                    {
                        t.TenantBed = b;
                        return t;
                    }, param, splitOn: "BedNumber");
                return result.Distinct().SingleOrDefault();
            }
        }




        public async Task<IEnumerable<entity.TenantEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.TenantEntity>();
            }
        }

        public async Task<IEnumerable<entity.TenantEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.TenantEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.TenantEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    var sql = @"Update Beds SET BedStatus = @BedStatus
                                Where BedNumber = @BedNumber ";
                    using (var transaction = connection.BeginTransaction())
                    {
                        //change bed status 
                        await connection.InsertAsync(entityToInsert, st => st.AttachToTransaction(transaction));
                        await connection.ExecuteAsync(sql, new
                        { BedStatus = PropertyStatus.Occupied.ToString(),
                          BedNumber = entityToInsert.TenantBedId }, transaction: transaction);
                        //return entityToInsert.TenantId;
                        transaction.Commit();
                        return entityToInsert.TenantId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.TenantEntity entityToUpdate)
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
                var sql = @" 
                            Update b SET b.BedStatus= @BedStatus FROM  Beds b
                            INNER JOIN Tenant t on b.BedNumber = t.TenantBedId
                            Where t.TenantId = @TenantId 
                            
                            DELETE FROM Tenant WHERE TenantId  =@TenantId
                           
                            ";
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteAsync(sql, new { TenantId = id, BedStatus = PropertyStatus.UnOccupied.ToString() },
                            transaction: transaction);
                        transaction.Commit();
                        if (result > 0)
                        {
                            return true;
                        }
                        return false;
                        //return await connection.DeleteAsync<entity.TenantEntity>(new entity.TenantEntity { TenantId = id });
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
