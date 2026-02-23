using Dapper.FastCrud;
using Dapper;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
using My2Home.Core.CommonEnums;

namespace My2Home.Infrastructure.Repository
{
    
    public class RoomRepository: BaseRepository, IRoomRepository
    {
        public RoomRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<IEnumerable<entity.RoomEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage,  string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM Room  Where RoomHostelId = @RoomHostelId ";

                var param = new DynamicParameters();
                param.Add("@RoomHostelId", hostelId);

                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " AND   RoomNumber LIKE @RoomNumber";
                    param.Add("@CityName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY RoomNumber "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.RoomEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<entity.RoomEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.RoomEntity>(new entity.RoomEntity { RoomId = id });
            }
        }
        public async Task<IEnumerable<entity.RoomEntity>> GetByHostelIdAsync(int hostelId)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.RoomEntity>(@"SELECT * from Room where RoomHostelId =@RoomHostelId ", new { RoomHostelId = hostelId });
            }
        }

        public async Task<IEnumerable<entity.RoomEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.RoomEntity>();
            }
        }

        public async Task<IEnumerable<entity.RoomEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.RoomEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.RoomEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.RoomId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int?> InsertAsync(entity.RoomEntity entityToInsert, int numberOfRoomesToCreate)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    //await connection.InsertAsync(entityToInsert);
                    var start = entityToInsert.RoomNumber;
                    using (var transaction = connection.BeginTransaction())
                    {
                        for (int i = 1; i <= numberOfRoomesToCreate; i++)
                        {
                            entityToInsert.RoomNumber =string.Format("{0}{1}", start, i);
                         await connection.InsertAsync(entityToInsert, st => st.AttachToTransaction(transaction));
                            for (int j = 0; j < entityToInsert.RoomNoOfBeds; j++)
                            {
                               await  connection.InsertAsync(new entity.BedEntity
                                {
                                    RoomId = entityToInsert.RoomId,
                                    BedRent = entityToInsert.RoomRentPerBed,       
                                    BedStatus = PropertyStatus.UnOccupied.ToString(),
                                    
                               }, st => st.AttachToTransaction(transaction));
                            }
                        }
                        transaction.Commit();

                        return entityToInsert.RoomId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.RoomEntity entityToUpdate)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result =  await connection.UpdateAsync(entityToUpdate, st => st.AttachToTransaction(transaction));
                        var bedResult = await connection.ExecuteAsync(
                            @"update Beds set BedRent = @BedRent where RoomId =@RoomId", new { BedRent= entityToUpdate.RoomRentPerBed,  RoomId = entityToUpdate.RoomId }, transaction);
                        transaction.Commit();
                        return result;
                    }
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
                var sql = @" DELETE from Beds Where RoomId = @RoomId 
                             DELETE from Room Where RoomId = @RoomId 
                             UPDATE t  SET TenantBedId = NULL, TenantStatus = @TenantStatus
                             FROM Tenant t
                             INNER JOIN Beds b on t.TenantBedId = b.BedNumber
                             Where b.RoomId = @RoomId  ";
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteAsync(sql, new { RoomId = id, TenantStatus = TenantStatus.inactive.ToString() },
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
