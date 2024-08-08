using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace DonorConnect
{
    public class QRY
    {

        public DataTable GetData(string _SQL)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 1000;
                    adpt.Fill(dt);
                }
            }
            return dt;
        }

        public DataSet GetDataDS(string _SQL)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    cmd.CommandTimeout = 1000;
                    adpt.Fill(ds);
                }
            }
            return ds;
        }

        public string ExecuteNonQuery2(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return "Success"; // return a success message
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error: {ex.Message}");
                return $"Error: {ex.Message}"; // return the error message
            }
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true; // success
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; // failure
            }
        }



    }
}