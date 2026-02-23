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
    
    public class ExpenseRepository: BaseRepository, IExpenseRepository
    {
        public ExpenseRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.ExpenseEntity> GetByIdAsync(int id)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.GetAsync<entity.ExpenseEntity>(new entity.ExpenseEntity { ExpenseId = id });
            }
        }

        public async Task<IEnumerable<entity.ExpenseEntity>> GetPageListAsync(int hostelId , int pageNumber, int rowsPerPage,
            string monthName = null, int year = 0)
        {
            try
            {
                var sql = @"WITH Data_CTE AS ( SELECT * FROM Expense Where ExpenseHostelId = @ExpenseHostelId  ";
                var param = new DynamicParameters();
                if (!string.IsNullOrEmpty(monthName))
                {
                    sql += " AND  ExpenseMonth = @ExpenseMonth";
                    param.Add("@ExpenseMonth", monthName);
                }

                if (year > 0)
                {
                    sql += " AND  ExpenseYear = @ExpenseYear";
                    param.Add("@ExpenseYear", year);
                }
                //if (!string.IsNullOrEmpty(searchConditions))
                //{
                //    sql += " AND  ExpenseName LIKE @ExpenseName";
                //    param.Add("@ExpenseName", '%' + searchConditions + '%');
                //}
                sql += " ), Count_CTE AS  (    SELECT COUNT(*) AS TotalRows FROM Data_CTE )  SELECT * FROM Data_CTE CROSS JOIN Count_CTE ORDER BY  ExpenseName  "
                       + " OFFSET(@PageNum - 1) * @PageSize ROWS"
                       + " FETCH NEXT @PageSize ROWS ONLY; ";
                param.Add("@PageNum", pageNumber);
                param.Add("@PageSize", rowsPerPage);
                param.Add("@ExpenseHostelId", hostelId);
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {
                    // connection.Open();

                    return await connection.QueryAsync<entity.ExpenseEntity>(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<entity.ExpenseEntity>> GetAllAsync()
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.FindAsync<entity.ExpenseEntity>();
            }
        }

        public async Task<IEnumerable<entity.ExpenseEntity>> GetWhereAsync(string query, object filters)
        {
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<entity.ExpenseEntity>(query, filters);
            }
        }
        public async Task<int?> InsertAsync(entity.ExpenseEntity entityToInsert)
        {
            try
            {
                using (var connection = GetSqlConnectionFactory.CreateConnection())
                {

                    await connection.InsertAsync(entityToInsert);
                    return entityToInsert.ExpenseId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateAsync(entity.ExpenseEntity entityToUpdate)
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

                    return await connection.DeleteAsync<entity.ExpenseEntity>(new entity.ExpenseEntity { ExpenseId = id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
