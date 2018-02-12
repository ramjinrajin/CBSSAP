using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InzNetworkCorrelation.Backend
{
    public class DBConnect
    {
        public static string ConnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InzNetworkLocalDB"].ConnectionString;
            return connectionString;
            
        }
    }
}