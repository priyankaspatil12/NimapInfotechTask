using NimapInfotechMachineTest.Models;
using NimapInfotechMachineTest.SqlDbConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NimapInfotechMachineTest.Controllers
{
    public class CategoryController : Controller
    {
         #region
        SqlConnection _sqlCon;
        SqlCommand _sqlCmd;
        Connection _connection;
        SqlDataAdapter da;
        #endregion
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveOrUpdate(CategoryModel model)
        {
            int _server_responce = 0;
            int _return = 0;
            string IUFlag = "";
            bool activ = true;
            try
            {
                if (model.CategoryId == 0)
                {
                    IUFlag = "I";
                    _return = 1;
                    _server_responce = 1;
                }
                else
                {
                    IUFlag = "U";
                    _return = 2;
                    _server_responce = 2;
                }
                _connection = new Connection();
                _sqlCon = _connection.Connect();
                _sqlCmd = new SqlCommand();
                _sqlCmd.CommandText = "SpCategory";
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Connection = _sqlCon;
                _sqlCmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                _sqlCmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                _sqlCmd.Parameters.AddWithValue("@IsActiv", activ);
                _sqlCmd.Parameters.AddWithValue("@IUFlag", IUFlag);

                _sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _server_responce = 3;
            }
            finally
            {
                _sqlCmd.Dispose();
                _sqlCon.Close();
            }
            if (_server_responce == 1)
            {
                TempData["message"] = "Your Data Save Successfuly..";
            }
            else if (_server_responce == 2)
            {
                TempData["message"] = "Your Data Update Successfuly";
            }
            else
            {
                TempData["Error"] = "Opps Somthing Wrong !!!";
            }
            return RedirectToAction("index");
        }
    }
}
