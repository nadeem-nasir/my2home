using Dapper;
using Dapper.FastCrud;
using My2Home.Core.CommonEnums;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Infrastructure.Repository
{

    public class BedRepository : BaseRepository, IBedRepository
    {
        public BedRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.BedEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.BedEntity>(new entity.BedEntity { BedNumber = id });
            }
        }


        public async Task<IEnumerable<entity.BedEntity>> GetPageListAsync(int hostelId, int pageNumber, int rowsPerPage, string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT b.* FROM Beds b inner join Room r on b.RoomId = r.RoomId
                    Where r.RoomHostelId = @RoomHostelId ";

                var param = new DynamicParameters();
                param.Add("@RoomHostelId", hostelId);

                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " Where  BedNumber LIKE @BedNumber ";
                    param.Add("@BedNumber", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY BedNumber  "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);


                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.BedEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<entity.BedEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.BedEntity>();
            }
        }


        public async Task<IEnumerable<entity.BedEntity>> GetUnOccupiedBedAsync(int hostelId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.BedEntity>("SELECT b.*, r.RoomNumber  FROM Beds b INNER JOIN ROOM r On b.RoomId = r.RoomId WHERE BedStatus = 'UnOccupied' AND r.RoomHostelId =@RoomHostelId", new { RoomHostelId = hostelId });
            }
        }

        public async Task<IEnumerable<entity.BedEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.BedEntity>(query, filters);
            }
        }

        public async Task<int?> InsertAsync(entity.BedEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    entityToInsert.BedStatus = PropertyStatus.UnOccupied.ToString();
                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.BedNumber;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.BedEntity entityToUpdate)
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

        public async Task<bool> UpdateAsync(int bedNumber, string bedStatus)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("UPDATE Beds SET BedStatus = @BedStatus WHERE BedNumber =@BedNumber", new { BedStatus = bedStatus, BedNumber = bedNumber });
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
                var sql = @" DELETE from Beds Where BedNumber = @BedNumber 
                             UPDATE Tenant  SET TenantBedId = NULL, TenantStatus = @TenantStatus
                             Where TenantBedId = @BedNumber  ";
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteAsync(sql, new { BedNumber = id, TenantStatus = TenantStatus.inactive.ToString() },
                            transaction: transaction);
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
