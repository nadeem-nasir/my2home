using Dapper;
using Dapper.FastCrud;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Infrastructure.Repository
{
    
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {
        }

        
        public async Task<entity.CountryEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.CountryEntity>(new entity.CountryEntity { CountryId = id });
            }
        }

        public async Task<IEnumerable<entity.CountryEntity>> GetPageListAsync(int pageNumber, int rowsPerPage, string searchConditions = null)
        {
            try
            {
                var sql = @"WITH Data_CTE AS ( SELECT * FROM Country Where CountryPublished >= 1 ";
                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += "AND CountryName LIKE @CountryName ";
                    param.Add("@CountryName", '%' + searchConditions + '%');
                }
                sql += " ), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY CountrySortOrder, CountryName  "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";
                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                   // connection.Open();

                    return await connection.QueryAsync<entity.CountryEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public async Task<IEnumerable<entity.CountryEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.CountryEntity>();
            }
        }

        public async Task<IEnumerable<entity.CountryEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.CountryEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.CountryEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.CountryId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.CountryEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.CountryEntity>(new entity.CountryEntity { CountryId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
