using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models.Domain;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        WebApplication.Models.Domain.database db = new Models.Domain.database();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["username"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (Session["UserName"] == null)
            {
                var q = (from a in db.tbl_users
                         where a.Username.Equals(username) && a.Password.Equals(password)
                         select a).SingleOrDefault();
                if (q != null)
                {
                    if (q.Status == false)
                    {
                        ViewBag.Message = "The user is not authorized to use.";
                        ViewBag.style = "color:red;";
                        return View();
                    }
                    Session["UserName"] = username;
                    Session["Access"] = q.Access;

                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    string Date = pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now);

                    tbl_logininformation logininformation = new tbl_logininformation();
                    logininformation.UserID = q.ID;
                    logininformation.Date = DateTime.Now;
                    logininformation.Time = DateTime.Now.ToLongTimeString();
                    db.tbl_logininformation.Add(logininformation);
                    db.SaveChanges();


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Username or password is incorrect";
                    ViewBag.style = "color:red;";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


        public ActionResult Logout()
        {
            if (Session["username"] != null)
            {
                Session["username"] = null;
                Session["access"] = null;
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}