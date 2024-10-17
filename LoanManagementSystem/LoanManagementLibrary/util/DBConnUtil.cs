using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

