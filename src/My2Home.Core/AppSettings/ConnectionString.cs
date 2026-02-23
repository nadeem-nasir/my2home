using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Core.AppSettings
{
    public class ConnectionString
    {
        public ConnectionString(string connectionString)
        {
            GetConnectionString = connectionString;
        }
        public string GetConnectionString { get; }
    }

}
