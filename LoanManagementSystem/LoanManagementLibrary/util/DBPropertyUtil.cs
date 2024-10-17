using System;
using System.IO;
using System.Text.Json;

namespace LoanManagementLibrary.util
{
    public static class DBPropertyUtil
    {
        public static string GetPropertyString()
        {
            
            string jsonFilePath = "Appsettings.json";

            try
            {
                
                string jsonString = File.ReadAllText(jsonFilePath);
                var config = JsonSerializer.Deserialize<DbConfig>(jsonString);

                
                return config.ConnectionStrings.LoanManagementDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading config file: " + ex.Message);
                throw;
            }
        }
    }

   
    public class DbConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string LoanManagementDB { get; set; }
    }
}
