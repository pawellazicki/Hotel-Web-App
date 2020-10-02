using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Hotel_App.Models;

namespace Hotel_App.Controllers
{
    public class AccountsController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult Create()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,account_login,account_password,account_type")] Accounts account)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Accounts.Where(x => x.account_login == account.account_login).Any())
                    {
                        ModelState.AddModelError("", "Account already exists");
                    }
                    else
                    {
                        if (account.account_type != null)
                        {
                            if (account.account_type.Equals("employee") || account.account_type.Equals("admin"))
                            {
                                account.account_password = Encrypt(account.account_password);

                                db.Accounts.Add(account);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                                ViewBag.accountAlert = "account type should be admin or employee";
                        }
                        else
                            ViewBag.accountAlert = "account type should be admin or employee";
                    }
                }
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(e.StackTrace);
            }

            return View(account);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Accounts account)
        {
            Accounts findedAccount = db.Accounts.Where
                (x => x.account_login == account.account_login).SingleOrDefault();


            if (findedAccount != null)
            {
                if (!Decrypt(findedAccount.account_password).Equals(account.account_password))
                {
                    ViewBag.msg = "Invalid password!";
                    return View();
                }

                Session["account_id"] = findedAccount.account_id;
                Session["account_type"] = findedAccount.account_type;
                Session["account_login"] = findedAccount.account_login;


                if(findedAccount.account_type.Equals("admin"))
                    return RedirectToAction("AdminDashBoard");
                if (findedAccount.account_type.Equals("employee"))
                    return RedirectToAction("EmployeeDashBoard");
                
            }
            else
                ViewBag.msg = "Invalid username";

            return View();
        }

        [HttpGet]
        public ActionResult EmployeeDashBoard()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            var room_reservation = db.Room_reservation.Include(r => r.Clients).Include(r => r.Rooms);
            return View(room_reservation.ToList());
        }

        [HttpGet]
        public ActionResult AdminDashBoard()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            List<Employees> employees = db.Employees.ToList();
            List<string> positionList = new List<string>();
            foreach (var employee in employees)
                if(!positionList.Contains(employee.employee_position))
                    positionList.Add(employee.employee_position);
            string[] positions = positionList.ToArray();

            int[] counterPositions = new int[positions.Length];
            for (int i = 0; i < counterPositions.Length; ++i)
                counterPositions[i] = 0;

            int iterator = 0;
            foreach(var position in positions)
            {
                foreach (var employee in employees)
                    if (employee.employee_position.Equals(position))
                        counterPositions[iterator]++;
                iterator++;
            }

            string[] strCounter = new string[counterPositions.Length];
            for(int i = 0; i < counterPositions.Length; ++i)
            {
                strCounter[i] = counterPositions[i].ToString();
            }

            var myChart = new Chart(width: 1000, height: 600)
                .AddTitle("Hotel Emploees")
                .AddSeries(
                    name: "Employee",
                    xValue: positions,
                    yValues: strCounter);
            myChart.Save("~/Content/Images/chart.jpeg");

            return View();
        }

        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");


            if (Session["account_type"] != null)
            {
                if (Session["account_type"].ToString().Equals("employee"))
                    return RedirectToAction("EmployeeDashBoard");
                //if (Session["account_type"].ToString().Equals("admin"))
                    //return RedirectToAction("AdminDashBoard");
            }
            return View(db.Accounts.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "account_id,account_login,account_password,account_type")] Accounts accounts)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Accounts.Where(x => x.account_login == accounts.account_login).Any())
                    {
                        ModelState.AddModelError("", "Account already exists");
                    }
                    else
                    {
                        accounts.account_password = Encrypt(accounts.account_password);
                        accounts.account_type = "employee";

                        db.Accounts.Add(accounts);
                        db.SaveChanges();

                        return RedirectToAction("Login");
                    }
                }
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(e.StackTrace);
            }

            return View(accounts);
        }

        public ActionResult Edit(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,account_login,account_password,account_type")] Accounts accounts)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    Accounts accountFinded = db.Accounts.Where(x => x.account_login == accounts.account_login).FirstOrDefault();
                    accountFinded.account_password = Encrypt(accounts.account_password);

                    db.Entry(accountFinded).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(accounts);
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(e.StackTrace);
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accounts accounts = db.Accounts.Find(id);
            db.Accounts.Remove(accounts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool IsLogin()
        {
            if (Session["account_id"] == null)
                return true;
            return false;
        }

        public string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException("plainText");

            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipher)
        {
            if (cipher == null) throw new ArgumentNullException("cipher");
            
            byte[] data = Convert.FromBase64String(cipher);
            
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }
    }
}
