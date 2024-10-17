using System;
using Microsoft.Data.SqlClient;

namespace LoanManagementLibrary.util
{
    public static class DBConnUtil
    {
        public static SqlConnection GetConnection()
        {
            
            string connectionString = DBPropertyUtil.GetPropertyString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
