using NimapInfotechMachineTest.Models;
using NimapInfotechMachineTest.SqlDbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NimapInfotechMachineTest.Controllers
{
    public class ProductController : Controller
    {
        #region
        SqlConnection _sqlCon;
        SqlCommand _sqlCmd;
        Connection _connection;
        SqlDataAdapter da;
        #endregion
        public ActionResult Index()
        {
            ViewBag.Category = CategoryList();
            return View();
        }
        public ActionResult Prtial_View()
        {
            ViewBag.Category = CategoryList();

            return View();
        }
        public List<SelectListItem> CategoryList()
        {
            DataTable dt = new DataTable();
            var _selectList = new List<SelectListItem>();
            _selectList.Add(new SelectListItem { Value = "0", Text = "--Select--" });
            try
            {
                _connection = new Connection();
                dt = _connection.FillCombo("Select * From MCategory Where IsActiv='True'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _selectList.Add(new SelectListItem { Value = dt.Rows[i]["CategoryId"].ToString(), Text = dt.Rows[i]["CategoryName"].ToString() });

                }

            }
            catch (Exception ex)
            {

            }
            return _selectList;

        }
        public ActionResult SaveOrUpdate(ProductModel model)
        {
            int _server_responce = 0;
            int _return = 0;
            string Flag = "";
            try
            {
                if (model.ProductId == 0)
                {
                    Flag = "I";
                    _return = 1;
                    _server_responce = 1;
                }
                else
                {
                    Flag = "U";
                    _return = 2;
                    _server_responce = 2;
                }
                _connection = new Connection();
                _sqlCon = _connection.Connect();
                _sqlCmd = new SqlCommand();
                _sqlCmd.CommandText = "SpProduct";
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                _sqlCmd.Connection = _sqlCon;
                _sqlCmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                _sqlCmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                _sqlCmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);               
                _sqlCmd.Parameters.AddWithValue("@Flag", Flag);
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
        public ActionResult ProductReport()
        {
            List<ProductReportModel> _list = new List<ProductReportModel>();
            DataTable dt = new DataTable();
            _connection = new Connection();
            dt = _connection.FillCombo("Select ProductId,	ProductName,C.CategoryId,CategoryName From MProduct P inner join MCategory C on C.CategoryId=P.CategoryId");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductReportModel model = new ProductReportModel();

                model.ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]);
                model.ProductName = dt.Rows[i]["ProductName"].ToString();
                model.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                model.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                _list.Add(model);
            }
            return View(_list);


        }
        public ActionResult EditData(int id)
        {
            ViewBag.Category = CategoryList();

            DataTable dt = new DataTable();
            _connection = new Connection();
            dt = _connection.FillCombo("Select * From MProduct Where ProductId=" + id);

            {
                ProductModel model = new ProductModel();
                {

                    model.ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]);
                    model.ProductName = dt.Rows[0]["ProductName"].ToString();                  
                    model.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                }
                return PartialView("index", model);

            }
        }
        public ActionResult ReportDelete(int id)
        {
            ViewBag.Category = CategoryList();

            try
            {
                List<ProductReportModel> _list = new List<ProductReportModel>();
                Connection Con = new Connection();
                DataTable dt = new DataTable();
                _connection = new Connection();
                ProductModel model = new ProductModel();
                model.ProductId = id;
                dt = _connection.FillCombo("Delete From MProduct Where ProductId =" + id);
                TempData["Delete"] = "Data Delete Sucessfully!";
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            return View("Index");

        }
    }
}