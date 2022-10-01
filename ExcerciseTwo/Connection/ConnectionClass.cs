using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcerciseTwo.Connection
{
    public class ConnectionClass
    {
        private static readonly string JsonFile = "appsettings.json";
        private static readonly string KeyConnection = "ExcerciseDBString";
        private static readonly object _lock = new object();

        private ConnectionClass() { }

        private static string _connection;

        public static string GetConnection()
        {
            if (_connection == null)
            {
                lock (_lock)
                {
                    if (_connection == null)
                    {
                        _connection = new ConfigurationBuilder().AddJsonFile(JsonFile).Build().GetConnectionString(KeyConnection);
                    }
                }
            }
            return _connection;
        }
    }
}
