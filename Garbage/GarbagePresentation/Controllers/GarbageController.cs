using GarbagePresentation.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbagePresentation.Controllers
{
    public class GarbageController : Controller
    {
        //
        // GET: /Garbage/
        #region Common
        readonly string _conString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString();
        public JsonResult AllTypeofUserLogin(string Email = "", string Password = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = Password });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = Email});
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "Login" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            UserModel List = new UserModel();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.UserId = row["UserId"].ToString();
                List.Name = row["Name"].ToString();
                List.Email = row["Email"].ToString();
                List.RoleId = row["RoleId"].ToString();
                List.RoleName = row["RoleName"].ToString();
                List.Available = row["Available"].ToString() == "0" ? "Not Available" : row["Available"].ToString() == "1" ? "Available" : row["Available"].ToString();
                List.ContactNo = row["ContactNo"].ToString();
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ClientRegistration(string Name = "", string Email = "", string Password = "", string RoleId = "", string ContactNo = "")
        {
            if (RoleId == "") { RoleId = "3"; }
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = Name });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = Email });
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = Password });
            cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.NVarChar) { Value = RoleId });
            cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.NVarChar) { Value = ContactNo });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "Registration" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllTypeofUserProfile(string UserId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "AllTypeofUserProfile" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            UserModel List = new UserModel();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.UserId = row["UserId"].ToString();
                List.Name = row["Name"].ToString();
                List.Email = row["Email"].ToString();
                List.RoleId = row["RoleId"].ToString();
                List.RoleName = row["RoleName"].ToString();
                List.Available = row["Available"].ToString() == "0" ? "Not Available" : row["Available"].ToString() == "1" ? "Available" : row["Available"].ToString();
                List.ContactNo = row["ContactNo"].ToString();
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllTypeofUserUpdateProfile(string Name = "", string ContactNo = "",string UserId="")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = Name });
            cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.NVarChar) { Value = ContactNo });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "UpdateProfile" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePasswordByAllTypeOfUser(string NewPassword = "", string OldPassword = "", string UserId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@NewPassword", SqlDbType.NVarChar) { Value = NewPassword });
            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = OldPassword });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ChangePasswordByAllTypeOfUser" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Client
        public JsonResult ListOfRequestForClient(string UserId="")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfRequestForClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel> List = new List<CleintRequestModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListOfRequestDetailForClient(string RequestId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfRequestDetailForClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestDetailModel> List = new List<CleintRequestDetailModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestDetailModel
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                    Date = row["Date"].ToString() == "" ? "" : Convert.ToDateTime(row["Date"].ToString()).ToString("dd-MM-yyyy"),
                    Time = row["Time"].ToString(),
                    RequestDetailStatus = row["RequestDetailStatus"].ToString() == "0" ? "Pending" : row["RequestDetailStatus"].ToString() == "1" ? "Confirm"
                    : row["RequestDetailStatus"].ToString() == "2" ? "Collected" : row["RequestDetailStatus"].ToString() == "3" ? "Closed" : row["RequestDetailStatus"].ToString(),
                    RequestDetailId = row["RequestDetailId"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertRequestByClient(List<CleintRequestDetailModel1> CleintRequestDetailList)
        {
            CleintRequestDetailModel List = new CleintRequestDetailModel();
            foreach (var item in CleintRequestDetailList)
            {
                List.Lat = item.Lat;
                List.Long = item.Long;
                List.TotalAmount = item.TotalAmount;
                List.UserId = item.UserId;
                List.Address = item.Address;
                List.GarbageType = item.GarbageType;
                break;
            }
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Lat", SqlDbType.NVarChar) { Value = List.Lat });
            cmd.Parameters.Add(new SqlParameter("@Long", SqlDbType.NVarChar) { Value = List.Long });
            cmd.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.NVarChar) { Value = List.TotalAmount });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = List.UserId });
            cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = List.Address });
            cmd.Parameters.Add(new SqlParameter("@GarbageType", SqlDbType.NVarChar) { Value = List.GarbageType });
            cmd.Parameters.Add(new SqlParameter("@RequestDetailType", SqlDbType.Structured) { Value = new UtilityClass().RequestDetailType(CleintRequestDetailList)});
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertRequestByClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            if (result > 0) { result = 1; } else { result = 0; }
            return Json(data:result,behavior:JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConfirmRequestAfterPayementByClient(string RequestId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ConfirmRequestAfterPayementByClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CloseRequestAfterCollectedByClient(string RequestId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "CloseRequestAfterCollectedByClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListofRateForClient()
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofRateForClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<RateModel> List = new List<RateModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new RateModel {
                    RateId = row["RateId"].ToString(),
                    Title = row["Title"].ToString(),
                    CreatedOn = row["CreatedOn"].ToString(),
                    Rate = row["Rate"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListofFeedbackForClient(string UserId="")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofFeedbackForClient" });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<FeedbackModel> List = new List<FeedbackModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new FeedbackModel
                {
                    FeedbackId = row["FeedbackId"].ToString(),
                    Feedback = row["Feedback"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    FeedbackBy = row["Name"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertFeedbackByClient(string FeedBack = "", string UserId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@FeedBack", SqlDbType.NVarChar) { Value = FeedBack });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertFeedbackByClient" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Employee
        public JsonResult ListOfPendingRequestForEmployee(string UserId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfPendingRequestForEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel> List = new List<CleintRequestModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListOfAllRequestForEmployee(string UserId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfAllRequestForEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel> List = new List<CleintRequestModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListOfRequestDetailForEmployee(string RequestId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfRequestDetailForEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestDetailModel> List = new List<CleintRequestDetailModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestDetailModel
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                    Date = row["Date"].ToString() == "" ? "" : Convert.ToDateTime(row["Date"].ToString()).ToString("dd-MM-yyyy"),
                    Time = row["Time"].ToString(),
                    RequestDetailStatus = row["RequestDetailStatus"].ToString() == "0" ? "Pending" : row["RequestDetailStatus"].ToString() == "1" ? "Confirm"
                    : row["RequestDetailStatus"].ToString() == "2" ? "Collected" : row["RequestDetailStatus"].ToString() == "3" ? "Closed" : row["RequestDetailStatus"].ToString(),
                    RequestDetailId = row["RequestDetailId"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeSubRequestStatusAfterCollectedByEmployee(string RequestDetailId = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestDetailId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ChangeSubRequestStatusAfterCollectedByEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DateWiseReportofRequestForEmployee(string UserId = "", string CreatedDate = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.NVarChar) { Value = CreatedDate });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "DateWiseReportofRequestForEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel> List = new List<CleintRequestModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FromToReportofRequestForEmployee(string UserId = "", string DateFrom = "", string DateTo = "")
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = UserId });
            cmd.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.NVarChar) { Value = DateFrom });
            cmd.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.NVarChar) { Value = DateTo });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "FromToReportofRequestForEmployee" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel> List = new List<CleintRequestModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    GarbageType = row["GarbageType"].ToString(),
                    Lat = row["Lat"].ToString(),
                    Long = row["Long"].ToString(),
                    RequestId = row["RequestId"].ToString(),
                    RequestStatus = row["Status"].ToString() == "0" ? "Pending" : row["Status"].ToString() == "1" ? "Confirm"
                    : row["Status"].ToString() == "2" ? "Collected" : row["Status"].ToString() == "3" ? "Closed" : row["Status"].ToString(),
                    TotalAmount = row["TotalAmount"].ToString(),
                });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
