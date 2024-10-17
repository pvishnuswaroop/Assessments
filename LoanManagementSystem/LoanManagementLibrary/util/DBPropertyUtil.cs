using System;
using System.IO;
using System.Text.Json;

namespace LoanManagementLibrary.util
{
    public static class DBPropertyUtil
    {
        public static string GetPropertyString()
        {
            // Path to the Appsettings.json file
            string jsonFilePath = "Appsettings.json";

            try
            {
                // Read and deserialize the JSON file
                string jsonString = File.ReadAllText(jsonFilePath);
                var config = JsonSerializer.Deserialize<DbConfig>(jsonString);

                // Return the connection string from the JSON config
                return config.ConnectionStrings.LoanManagementDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading config file: " + ex.Message);
                throw;
            }
        }
    }

    // Define a class for deserializing the JSON structure
    public class DbConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string LoanManagementDB { get; set; }
    }
}
