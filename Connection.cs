using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NimapInfotechMachineTest.SqlDbConnection
{
    public class Connection
    {
        SqlCommand _sqlCmd;
        SqlDataAdapter da;
        SqlConnection _sqlConn = null;
        public static string connectionString = @"Data Source=DESKTOP-OJ8RAEB; Initial Catalog=DbNimapInfotech ; User Id=sa;Password=Game@123";

        public SqlConnection Connect()
        {
            try
            {

                _sqlConn = new SqlConnection(connectionString);
                _sqlConn.Close();
                if (_sqlConn.State == ConnectionState.Open)
                    _sqlConn.Close();
                _sqlConn.Open();
            }
            catch (Exception ex)
            {

            }

            return _sqlConn;
        }
        public DataTable FillCombo(string query)
        {
            DataTable dt = new DataTable();

            _sqlConn = Connect();
            _sqlCmd = new SqlCommand();
            _sqlCmd.Connection = _sqlConn;
            da = new SqlDataAdapter(query, _sqlConn);
            da.Fill(dt);


            return dt;
        }
    }
}