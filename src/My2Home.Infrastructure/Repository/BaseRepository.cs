using My2Home.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Infrastructure.Repository
{
    public class BaseRepository: IBaseRepository
    {
        private SqlConnectionFactory _sqlConnectionFactory;
        public BaseRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public SqlConnectionFactory GetSqlConnectionFactory
        {
            get { return _sqlConnectionFactory; }
        }
    }
}
