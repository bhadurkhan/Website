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
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString.ToString());
        public ActionResult Index()
        {
            int Role_Id = 0;
            if (Session["UserId"] != null)
            {
                Role_Id = Convert.ToInt16(Session["Role"]);
            }
            if (Role_Id == 1 && Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (Role_Id == 2 && Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Staff");
            }
            else if (Role_Id == 3 && Session["UserId"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                UserModel model = new UserModel();
                model.Email = Convert.ToString(TempData["Email"]);
                model.Password = Convert.ToString(TempData["Password"]);
                return View(model);
            }
        }
        public ActionResult SignIn(string EmailLogin, string PasswordLogin, string remember, string ReturnUrl)
        {
            try{
                SqlCommand cmd = new SqlCommand("Garbage", conn);
                cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = PasswordLogin });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = EmailLogin });
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar) { Value = "Login" });
                cmd.CommandTimeout = 99999;
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                conn.Close();
                UserModel List = new UserModel();
                if (ds.Tables[0].Rows.Count != 0)
                {
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
                    Session["UserId"] = List.UserId;
                    Session["UserName"] = List.Name;
                    Session["Role"] = List.RoleId;
                    Session["Email"] = List.Email;
                    Session["Available"] = List.Available;
                    Session["RoleName"] = List.RoleName;
                    Session["ContactNo"] = List.ContactNo;
                    if (List.RoleId == "1" && Session["UserId"] != null)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (List.RoleId == "4" && Session["UserId"] != null)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else {
                        TempData["Status"] = "User Name or Password is incorrect";
                        return RedirectToAction("Index", "Login");
                    }

                }
                else
                {
                    TempData["Status"] = "User Name or Password is incorrect";
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SignOut()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["Role"] = null;
            return RedirectToAction("Index", "Login");
        }

    }
}
