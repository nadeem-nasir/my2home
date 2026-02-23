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
    
    public class ExpenseCategoryRepository: BaseRepository, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.ExpenseCategoryEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.ExpenseCategoryEntity>(new entity.ExpenseCategoryEntity { ExpenseCategoryId = id });
            }
        }

        public async Task<IEnumerable<entity.ExpenseCategoryEntity>> GetPageListAsync(int pageNumber, int rowsPerPage,  string searchConditions = null)
        {
            try
            {
                var sql = @" WITH Data_CTE AS ( SELECT * FROM ExpenseCategory  ";

                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(searchConditions))
                {
                    sql += " Where  ExpenseCategoryName LIKE @ExpenseCategoryName";
                    param.Add("@ExpenseCategoryName", '%' + searchConditions + '%');
                }

                sql += "), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY ExpenseCategoryName "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";

                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                

                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();
                    return await connection.QueryAsync<entity.ExpenseCategoryEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<entity.ExpenseCategoryEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.ExpenseCategoryEntity>();
            }
        }

        public async Task<IEnumerable<entity.ExpenseCategoryEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.ExpenseCategoryEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.ExpenseCategoryEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.ExpenseCategoryId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.ExpenseCategoryEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.ExpenseCategoryEntity>(new entity.ExpenseCategoryEntity { ExpenseCategoryId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
