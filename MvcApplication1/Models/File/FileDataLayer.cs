using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using inzCloud.Models.Data_Layer;

namespace MvcApplication1.Models.File
{

    public enum FileStatus
    {
        Sucess = 1,
        Error = 0,
        Exists = -1
    }

    public class FileResponse
    {
        public string Message { get; set; }
        public FileStatus Status { get; set; }
    }

    public class FileDataLayer
    {
        public FileResponse SaveFile(int UserId, int FileId)
        {
                 FileResponse fileResponse;
            using(SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FileSave", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FileId", FileId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    int Response =int.Parse(cmd.ExecuteScalar().ToString());
                    if(Response==(int)FileStatus.Sucess)
                    {
                        fileResponse = new FileResponse
                        {
                            Message="Partner added sucessfully",
                            Status = FileStatus.Sucess
                        };
                    }
                    else if (Response == (int)FileStatus.Exists)
                    {
                        fileResponse = new FileResponse
                        {
                            Message = "Partner already exists",
                            Status = FileStatus.Exists
                        };
                    }
                    else
                    {
                        fileResponse = new FileResponse
                        {
                            Message = "Some thing went wrong",
                            Status = FileStatus.Error
                        };
                    }
                }
                catch (Exception ex)
                {

                    fileResponse = new FileResponse
                    {
                        Message = ex.Message.ToString(),
                        Status = FileStatus.Error
                    };
                }
                finally
                {
                    con.Close();

                }
            }



       


            return fileResponse;
        }


        public bool UpdatePartnerStatus(int FileId,int UserId)
        {
            using(SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("PartnerStatusUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId",UserId);
                    cmd.Parameters.AddWithValue("@FileId", FileId);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}