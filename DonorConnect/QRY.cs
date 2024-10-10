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
        private SqlCommand command;

        public QRY()
        {
            command = new SqlCommand();
        }

        public DataTable GetData(string _SQL, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(_SQL, con))
                {
                    if (parameters != null)
                    {
                       
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

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

        //public bool ExecuteNonQuery(string sql)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //                return true; // success
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return false; // failure
        //    }
        //}

        public bool ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Add parameters if any
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

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

        public object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    connection.Open(); // Open the database connection
                    object result = command.ExecuteScalar(); // Execute the scalar query

                    return result; // Return the result (could be null if no data found)
                }
            }
        }


        //public string GetScalarValue(string sql, Dictionary<string, object> parameters = null)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                object result = cmd.ExecuteScalar();
        //                return result != null ? result.ToString() : null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("An error occurred while executing the scalar query.", ex);
        //    }
        //}

        public string GetScalarValue(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                      
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                               
                                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : null;
                    }
                }
            }
            catch (Exception ex)
            {
                // You may want to log the exception message or details here for further troubleshooting
                throw new Exception("An error occurred while executing the scalar query.", ex);
            }
        }

        public void AddParameter(string parameterName, object value)
        {
            command.Parameters.AddWithValue(parameterName, value);
        }

    }
}