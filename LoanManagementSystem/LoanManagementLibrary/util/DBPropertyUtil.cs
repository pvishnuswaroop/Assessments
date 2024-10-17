using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LoanManagementLibrary.util
{
    public static class DBPropertyUtil
    {
        public static string GetPropertyString()
        {
            
            return "Server=.\\SQLEXPRESS;Database=LoanManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;";
        }
    }
}
