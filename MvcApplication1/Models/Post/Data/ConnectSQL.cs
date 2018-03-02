using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inzCloud.Models.Data_Layer
{
    public class ConnectSQL
    {
        //public static SqlConnection SqlConnect()
        //{

        //    string connectionString = GetConnectionString();
        //    SqlConnection con = new SqlConnection(connectionString);
        //    con.Open();
        //    return con;
        //}

        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["InzNetworkLocalDB"].ConnectionString;
             
        }

        //public static SqlCommand ExecuteCommand(string Query)
        //{

        //    SqlCommand cmd = new SqlCommand(Query, ConnectSQL.SqlConnect());
        //    cmd.CommandType = CommandType.Text;
        //    return cmd;
        //}

        //public static SqlCommand ExecuteProcedure(string Query)
        //{

        //    SqlCommand cmd = new SqlCommand(Query, ConnectSQL.SqlConnect());
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    return cmd;
        //}
    }
}