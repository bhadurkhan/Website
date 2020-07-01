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
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        readonly string _conString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString();
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult UserProfile()
        {
            if (Session["UserId"] != null)
            {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = Session["UserId"] });
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
                List.Status = row["Status"].ToString() == "0" ? "In-Active" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString();
                List.ContactNo = row["ContactNo"].ToString();
            }
            return View(List);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #region Admin
        public ActionResult ListofRateForManger()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
                    SqlConnection conn = new SqlConnection(_conString);
                    SqlCommand cmd = new SqlCommand("Garbage", conn);
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofRateForManger" });
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
                        List.Add(new RateModel
                        {
                            RateId = row["RateId"].ToString(),
                            Title = row["Title"].ToString(),
                            CreatedOn = row["CreatedOn"].ToString(),
                            Rate = row["Rate"].ToString(),
                        });
                    }
                    return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult InsertRateByManger(RateModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar) { Value = obj.Title });
            cmd.Parameters.Add(new SqlParameter("@Rate", SqlDbType.NVarChar) { Value = obj.Rate });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertRateByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRateByManger(RateModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar) { Value = obj.Title });
            cmd.Parameters.Add(new SqlParameter("@Rate", SqlDbType.NVarChar) { Value = obj.Rate });
            cmd.Parameters.Add(new SqlParameter("@RateId", SqlDbType.NVarChar) { Value = obj.RateId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "UpdateRateByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListofEmployeeForManager()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
                    SqlConnection conn = new SqlConnection(_conString);
                    SqlCommand cmd = new SqlCommand("Garbage", conn);
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofEmployeeForManager" });
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    List<UserModel> List = new List<UserModel>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        List.Add(new UserModel
                        {
                            UserId = row["UserId"].ToString(),
                            Name = row["Name"].ToString(),
                            Email = row["Email"].ToString(),
                            RoleId = row["RoleId"].ToString(),
                            RoleName = row["RoleName"].ToString(),
                            Available = row["Available"].ToString() == "0" ? "Not Available" : row["Available"].ToString() == "1" ? "Available" : row["Available"].ToString(),
                            Status = row["Status"].ToString() == "0" ? "In-Active" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                            ContactNo = row["ContactNo"].ToString(),
                        });
                    }
                    return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult UpdateEmployeeByManger(UserModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = obj.Name });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = obj.Email });
            cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.NVarChar) { Value = obj.ContactNo });
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = obj.UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "UpdateEmployeeByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeStatusofAllTypeofUserByManager(UserModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = obj.UserId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ChangeStatusofAllTypeofUserByManager" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListofClientForManager()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
                    SqlConnection conn = new SqlConnection(_conString);
                    SqlCommand cmd = new SqlCommand("Garbage", conn);
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofClientForManager" });
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    List<UserModel> List = new List<UserModel>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        List.Add(new UserModel
                        {
                            UserId = row["UserId"].ToString(),
                            Name = row["Name"].ToString(),
                            Email = row["Email"].ToString(),
                            RoleId = row["RoleId"].ToString(),
                            RoleName = row["RoleName"].ToString(),
                            Available = row["Available"].ToString() == "0" ? "Not Available" : row["Available"].ToString() == "1" ? "Available" : row["Available"].ToString(),
                            Status = row["Status"].ToString() == "0" ? "In-Active" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                            ContactNo = row["ContactNo"].ToString(),
                        });
                    }
                    return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListofCompanyForManager()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
                    SqlConnection conn = new SqlConnection(_conString);
                    SqlCommand cmd = new SqlCommand("Garbage", conn);
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofCompanyForManager" });
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    List<UserModel> List = new List<UserModel>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        List.Add(new UserModel
                        {
                            UserId = row["UserId"].ToString(),
                            Name = row["Name"].ToString(),
                            Email = row["Email"].ToString(),
                            RoleId = row["RoleId"].ToString(),
                            RoleName = row["RoleName"].ToString(),
                            Available = row["Available"].ToString() == "0" ? "Not Available" : row["Available"].ToString() == "1" ? "Available" : row["Available"].ToString(),
                            Status = row["Status"].ToString() == "0" ? "In-Active" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                            ContactNo = row["ContactNo"].ToString(),
                        });
                    }
                    return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListOfPendingRequestForAdmin()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfPendingRequestForAdmin" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel1> List = new List<CleintRequestModel1>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel1
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    EmployeeName = row["EmployeeName"].ToString(),
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
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListOfAllRequestForAdmin()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfAllRequestForAdmin" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestModel1> List = new List<CleintRequestModel1>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestModel1
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    EmployeeName = row["EmployeeName"].ToString(),
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
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListOfRequestDetailForAdmin(string RequestId = "")
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.NVarChar) { Value = RequestId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListOfRequestDetailForAdmin" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<CleintRequestDetailModel2> List = new List<CleintRequestDetailModel2>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new CleintRequestDetailModel2
                {
                    UserId = row["UserId"].ToString(),
                    Name = row["Name"].ToString(),
                    EmployeeName = row["EmployeeName"].ToString(),
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
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListofAuctionForManger()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofAuctionForManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<AuctionModel> List = new List<AuctionModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new AuctionModel
                {
                    AuctionId = row["AuctionId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    DateFrom1 = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("yyyy-MM-dd"),
                    DateTo1 = Convert.ToDateTime(row["DateTo"].ToString()).ToString("yyyy-MM-dd"),
                    DateFrom = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("dd-MM-yyyy"),
                    DateTo = Convert.ToDateTime(row["DateTo"].ToString()).ToString("dd-MM-yyyy"),
                    Detail = row["Detail"].ToString(),
                    EstimatedRate = row["EstimatedRate"].ToString(),
                    Status = row["Status"].ToString() == "0" ? "Closed" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                    TimeFrom = row["TimeFrom"].ToString(),
                    TimeTo = row["TimeTo"].ToString(),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult InsertAuctionByManger(AuctionModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = obj.Address });
            cmd.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.NVarChar) { Value = obj.DateFrom });
            cmd.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.NVarChar) { Value = obj.DateTo });
            cmd.Parameters.Add(new SqlParameter("@Detail", SqlDbType.NVarChar) { Value = obj.Detail });
            cmd.Parameters.Add(new SqlParameter("@EstimatedRate", SqlDbType.NVarChar) { Value = obj.EstimatedRate });
            cmd.Parameters.Add(new SqlParameter("@TimeFrom", SqlDbType.NVarChar) { Value = obj.TimeFrom });
            cmd.Parameters.Add(new SqlParameter("@TimeTo", SqlDbType.NVarChar) { Value = obj.TimeTo });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertAuctionByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateAuctionByManger(AuctionModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = obj.Address });
            cmd.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.NVarChar) { Value = obj.DateFrom });
            cmd.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.NVarChar) { Value = obj.DateTo });
            cmd.Parameters.Add(new SqlParameter("@Detail", SqlDbType.NVarChar) { Value = obj.Detail });
            cmd.Parameters.Add(new SqlParameter("@EstimatedRate", SqlDbType.NVarChar) { Value = obj.EstimatedRate });
            cmd.Parameters.Add(new SqlParameter("@TimeFrom", SqlDbType.NVarChar) { Value = obj.TimeFrom });
            cmd.Parameters.Add(new SqlParameter("@TimeTo", SqlDbType.NVarChar) { Value = obj.TimeTo });
            cmd.Parameters.Add(new SqlParameter("@AuctionId", SqlDbType.NVarChar) { Value = obj.AuctionId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "UpdateAuctionByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeAuctionStatusByManger(AuctionModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@AuctionId", SqlDbType.NVarChar) { Value = obj.AuctionId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ChangeAuctionStatusByManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AuctionBiddingDetailForManger(string AuctionId="")
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@AuctionId", SqlDbType.NVarChar) { Value = AuctionId });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "AuctionBiddingDetailForManger" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<AuctionBiddingModel> List = new List<AuctionBiddingModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new AuctionBiddingModel
                {
                    AuctionId = row["AuctionId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    DateFrom = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("dd-MM-yyyy"),
                    DateTo = Convert.ToDateTime(row["DateTo"].ToString()).ToString("dd-MM-yyyy"),
                    Detail = row["Detail"].ToString(),
                    EstimatedRate = row["EstimatedRate"].ToString(),
                    Status = row["Status"].ToString() == "0" ? "Closed" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                    TimeFrom = row["TimeFrom"].ToString(),
                    TimeTo = row["TimeTo"].ToString(),
                    BidingId = row["BidingId"].ToString(),
                    BidingAmount = row["BidingAmount"].ToString(),
                    CompanyId = row["CompanyId"].ToString(),
                    CompanyName = row["CompanyName"].ToString(),
                    BiddingContactNo = row["BiddingContactNo"].ToString(),
                    BiddingCreatedDate = Convert.ToDateTime(row["BiddingCreatedDate"].ToString()).ToString("dd-MM-yyyy"),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListofFeedbackForManager()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofFeedbackForManager" });
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
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult InsertMessageUser(UserMessageModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = obj.Name });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = obj.Email });
            cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.NVarChar) { Value = obj.ContactNo });
            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = obj.Message });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertMessageUser" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListofUserMessageForManager()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "1")
           {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofUserMessageForManager" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<UserMessageModel> List = new List<UserMessageModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new UserMessageModel
                {
                    Message = row["Message"].ToString(),
                    MessageId = row["MessageId"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    Name = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion
        #region Company
        public ActionResult ListofActiveAuctionForCompany()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "4")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofActiveAuctionForCompany" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<AuctionModel> List = new List<AuctionModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new AuctionModel
                {
                    AuctionId = row["AuctionId"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    DateFrom1 = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("yyyy-MM-dd"),
                    DateTo1 = Convert.ToDateTime(row["DateTo"].ToString()).ToString("yyyy-MM-dd"),
                    DateFrom = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("dd-MM-yyyy"),
                    DateTo = Convert.ToDateTime(row["DateTo"].ToString()).ToString("dd-MM-yyyy"),
                    Detail = row["Detail"].ToString(),
                    EstimatedRate = row["EstimatedRate"].ToString(),
                    Status = row["Status"].ToString() == "0" ? "Closed" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                    TimeFrom = row["TimeFrom"].ToString(),
                    TimeTo = row["TimeTo"].ToString(),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult InsertBiddingOnAuctionByCompany(AuctionBiddingModel obj)
        {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = obj.CompanyId });
            cmd.Parameters.Add(new SqlParameter("@AuctionId", SqlDbType.NVarChar) { Value = obj.AuctionId });
            cmd.Parameters.Add(new SqlParameter("@BidingAmount", SqlDbType.NVarChar) { Value = obj.BidingAmount });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "InsertBiddingOnAuctionByCompany" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListofAuctionForOwnByCompany()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "4")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = @Session["UserId"] });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofAuctionForOwnByCompany" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<AuctionModel> List = new List<AuctionModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new AuctionModel
                {
                    AuctionId = row["AuctionId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    DateFrom1 = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("yyyy-MM-dd"),
                    DateTo1 = Convert.ToDateTime(row["DateTo"].ToString()).ToString("yyyy-MM-dd"),
                    DateFrom = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("dd-MM-yyyy"),
                    DateTo = Convert.ToDateTime(row["DateTo"].ToString()).ToString("dd-MM-yyyy"),
                    Detail = row["Detail"].ToString(),
                    EstimatedRate = row["EstimatedRate"].ToString(),
                    Status = row["Status"].ToString() == "0" ? "Closed" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                    TimeFrom = row["TimeFrom"].ToString(),
                    TimeTo = row["TimeTo"].ToString(),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ListofBiddingOnAuctionForCompany()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Role"].ToString() == "4")
                {
            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand("Garbage", conn);
            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = Session["UserId"] });
            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "ListofBiddingOnAuctionForCompany" });
            cmd.CommandTimeout = 99999;
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            List<AuctionBiddingModel> List = new List<AuctionBiddingModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List.Add(new AuctionBiddingModel
                {
                    AuctionId = row["AuctionId"].ToString(),
                    Name = row["Name"].ToString(),
                    ContactNo = row["ContactNo"].ToString(),
                    Address = row["Address"].ToString(),
                    CreatedDate = row["CreatedDate"].ToString(),
                    DateFrom = Convert.ToDateTime(row["DateFrom"].ToString()).ToString("dd-MM-yyyy"),
                    DateTo = Convert.ToDateTime(row["DateTo"].ToString()).ToString("dd-MM-yyyy"),
                    Detail = row["Detail"].ToString(),
                    EstimatedRate = row["EstimatedRate"].ToString(),
                    Status = row["Status"].ToString() == "0" ? "Closed" : row["Status"].ToString() == "1" ? "Active" : row["Status"].ToString(),
                    TimeFrom = row["TimeFrom"].ToString(),
                    TimeTo = row["TimeTo"].ToString(),
                    BidingId = row["BidingId"].ToString(),
                    BidingAmount = row["BidingAmount"].ToString(),
                    CompanyId = row["CompanyId"].ToString(),
                    CompanyName = row["CompanyName"].ToString(),
                    BiddingContactNo = row["BiddingContactNo"].ToString(),
                    BiddingCreatedDate = Convert.ToDateTime(row["BiddingCreatedDate"].ToString()).ToString("dd-MM-yyyy"),
                });
            }
            return View(List);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        #endregion


    }
}
