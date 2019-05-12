using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models.Domain;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        WebApplication.Models.Domain.database db = new Models.Domain.database();

        public ActionResult Login_info(int page = 1, int userid = 0)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var setting = (from a in db.tbl_setting
                           select a).FirstOrDefault();

            int count = Convert.ToInt16(setting.CountProductInPage);
            int take = count;
            int skip = (page * take) - take;

            var q = (from a in db.tbl_logininformation
                     join b in db.tbl_users
                     on a.UserID equals b.ID
                     where userid != 0 ? b.ID.Equals(userid) : 1 == 1
                     orderby a.ID descending
                     select a);


            ViewBag.CountPoduct = q.Count();
            ViewBag.Take = take;
            return View(q.Skip(skip).Take(take));
        }

        [HttpGet]
        public ActionResult Edit_pro()
        {
            if (Session["Username"] != null)
            {
                string username = Session["UserName"].ToString();
                return View(db.tbl_users.Where(a => a.Username.Equals(username)).SingleOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit_pro(tbl_users t)
        {
            if (Session["UserName"] != null)
            {
                string username = Session["UserName"].ToString();
                var q = db.tbl_users.Where(a => a.Username.Equals(username)).SingleOrDefault();
                q.Address = t.Address;
                q.City = t.City;
                q.Name = t.Name;
                q.Family = t.Family;
                q.NatCode = t.NatCode;
                q.Password = t.Password;
                q.PostalCode = t.PostalCode;
                q.Shire = t.Shire;

                db.tbl_users.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Message = "Seccessfully saved.";
                    ViewBag.style = "color:green;";
                    return View(q);
                }
                else
                {
                    ViewBag.Message = "Unfortunately not saved.";
                    ViewBag.style = "color:red;";
                    return View(q);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Users_manegement(int page = 1)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var setting = (from a in db.tbl_setting
                           select a).FirstOrDefault();

            int count = Convert.ToInt16(setting.CountProductInPage);

            int take = count;
            int skip = (page * take) - take;

            var q = (from a in db.tbl_users
                     where a.Access != "admin"
                     select a).OrderByDescending(a => a.ID);

            ViewBag.CountPoduct = q.Count();
            ViewBag.Take = take;

            return View(q.Skip(skip).Take(take));
        }

        public ActionResult DetailsUser(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = (from a in db.tbl_users
                     where a.ID.Equals(id) && a.Access != "admin"
                     select a).SingleOrDefault();

            if (q != null)
                return View(q);
            else
                return Content("User not found.");
        }

        public ActionResult DisableUser(int id)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = (from a in db.tbl_users
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            if (q != null)
            {
                q.Status = false;
                db.tbl_users.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Users_manegement");
            }
            else
                return Content("User not found.");
        }

        public ActionResult EnableUser(int id)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = (from a in db.tbl_users
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();
            if (q != null)
            {
                q.Status = true;
                db.tbl_users.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Users_manegement");
            }
            else
                return Content("User not found.");
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(tbl_users t)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            t.Date = DateTime.Now;

            db.tbl_users.Add(t);

            if (Convert.ToBoolean(db.SaveChanges()))
            {
                ViewBag.Message = "Seccessfully saved.";
                ViewBag.style = "color:green;";
                return View(t);
            }
            else
            {
                ViewBag.Message = "Unfortunately not saved.";
                ViewBag.style = "color:red;";
                return View(t);
            }
        }


        public ActionResult Sensor_management(int page = 1)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = from a in db.tbl_sensors
                    orderby a.ID descending
                    select a;

            var setting = (from a in db.tbl_setting
                           select a).FirstOrDefault();

            int count = Convert.ToInt16(setting.CountProductInPage);

            int take = count;
            int skip = (page * take) - take;

            ViewBag.CountPoduct = q.Count();
            ViewBag.Take = take;

            return View(q.Skip(skip).Take(take));
        }

        [HttpGet]
        public ActionResult CreateSensor()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult CreateSensor(tbl_sensors t)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            string username = Session["username"].ToString();
            var q = (from a in db.tbl_users
                     where a.Username.Equals(username)
                     select a).SingleOrDefault();

            t.UserID = q.ID;



            db.tbl_sensors.Add(t);

            if (Convert.ToBoolean(db.SaveChanges()))
            {
                ViewBag.Message = "Seccessfully saved.";
                ViewBag.style = "color:green;";
                return View(t);
            }
            else
            {
                ViewBag.Message = "Unfortunately not saved.";
                ViewBag.style = "color:red;";
                return View(t);
            }
        }

        [HttpGet]
        public ActionResult Editsens(int id)
        {

            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");


            var q = (from a in db.tbl_sensors
                     where a.ID.Equals(id)
                     select a).SingleOrDefault();


            return View(q);
        }

        [HttpPost]
        public ActionResult Editsens(tbl_sensors t)
        {
            if (Session["UserName"] != null)
            {
                var q = (from a in db.tbl_sensors
                         where a.ID.Equals(t.ID)
                         select a).SingleOrDefault();

                tbl_sensorinformation info = new tbl_sensorinformation();

                string username = Session["username"].ToString();

                var quser = (from a in db.tbl_users
                             where a.Username.Equals(username)
                             select a).SingleOrDefault();

                info.BaseID_ago = q.BaseID;
                info.BaseID_new = t.BaseID;
                info.Date = DateTime.Now;
                info.SensorID = t.ID;
                info.State_ago = q.State;
                info.State_new = t.State;
                info.TypeID_ago = q.TypeID;
                info.TypeID_new = t.TypeID;
                info.UserID = quser.ID;
                info.x_ago = q.x;
                info.x_new = t.x;
                info.y_ago = q.y;
                info.y_new = t.y;

                db.tbl_sensorinformation.Add(info);

                q.State = t.State;
                q.BaseID = t.BaseID;
                q.TypeID = t.TypeID;
                q.x = t.x;
                q.y = t.y;

                db.tbl_sensors.Attach(q);
                db.Entry(q).State = System.Data.Entity.EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges()))
                {
                    ViewBag.Message = "Seccessfully saved.";
                    ViewBag.style = "color:green;";
                    return View(q);
                }
                else
                {
                    ViewBag.Message = "Unfortunately not saved.";
                    ViewBag.style = "color:red;";
                    return View(q);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Sensor_information(int page = 1, int sensorid = 0)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var setting = (from a in db.tbl_setting
                           select a).FirstOrDefault();

            int count = Convert.ToInt16(setting.CountProductInPage);

            int take = count;
            int skip = (page * take) - take;

            var q = (from a in db.tbl_sensorinformation
                     where sensorid != 0 ? a.SensorID.Equals(sensorid) : 1 == 1
                     orderby a.ID descending
                     select a);


            ViewBag.CountPoduct = q.Count();
            ViewBag.Take = take;
            return View(q.Skip(skip).Take(take));
        }

        public ActionResult DetailsSensor(int id)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = (from a in db.tbl_sensorinformation
                    where a.ID.Equals(id)
                    select a).SingleOrDefault();

            return View(q);

        }

        [HttpGet]
        public ActionResult Setting()
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");
            var q = (from a in db.tbl_setting
                     select a).SingleOrDefault() ;
            return View(q);

        }
        [HttpPost]
        public ActionResult Setting(tbl_setting t)
        {
            if (Session["UserName"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["Access"].ToString() != "admin")
                return RedirectToAction("Index", "Home");

            var q = (from a in db.tbl_setting
                     select a).SingleOrDefault();
            q.CountProductInPage = t.CountProductInPage;
            q.Description = t.Description;
            q.Email = t.Email;
            q.EmailPass = t.EmailPass;
            q.SMTP = t.SMTP;
            q.Title = t.Title;
            db.tbl_setting.Attach(q);
            db.Entry(q).State = System.Data.Entity.EntityState.Modified;

            if (Convert.ToBoolean(db.SaveChanges()))
            {
                ViewBag.Message = "Seccessfully saved.";
                ViewBag.style = "color:green;";
                return View(t);
            }
            else
            {
                ViewBag.Message = "Unfortunately not saved.";
                ViewBag.style = "color:red;";
                return View(t);
            }

        }
    }
}