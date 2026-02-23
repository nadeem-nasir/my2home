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
    
    public class CityRepository : BaseRepository, ICityRepository
    {
        public CityRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {
        }

        public async Task<entity.CityEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.CityEntity>(new entity.CityEntity { CityId = id });
            }
        }

        public async Task<entity.CityEntity> GetByNameAsync(string  cityName)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<entity.CityEntity>
                        (" SELECT * FROM City Where LOWER(CityName) = @CityName ", new { CityName = cityName.ToLower() });
                    //return await connection.QueryFirst<entity.CityEntity>("", new { });

                    //GetAsync<entity.CityEntity>(new entity.CityEntity { CityId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<entity.CityEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, int countryId = 1, string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM City  WHERE CityCountryId = @CountryId  ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " AND  CityName LIKE @CityName";
                    param.Add("@CityName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY CityName "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                param.Add("@CountryId", countryId);

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                   // connection.Open();
                    return await connection.QueryAsync<entity.CityEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<entity.CityEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.CityEntity>();
            }
        }

        public async Task<IEnumerable<entity.CityEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.CityEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.CityEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.CityId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.CityEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.CityEntity>(new entity.CityEntity { CityId = id });
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
