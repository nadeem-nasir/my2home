using Dapper;
using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Infrastructure.Repository
{
    public class DashboardRepository: BaseRepository, IDashboardRepository
    {
        public DashboardRepository(SqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
        {

        }

        public async Task<entity.DashboardViewModel> GetByHostelIdAsync(int hostelId)
        {
            var sql = @"SELECT * FROM View_dashboard WHERE TenantHostelId =@TenantHostelId";
            var param = new DynamicParameters();
            param.Add("@TenantHostelId", hostelId);
            
            using (var connection = GetSqlConnectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<entity.DashboardViewModel>(sql, param);
            }
        }


    }
}
