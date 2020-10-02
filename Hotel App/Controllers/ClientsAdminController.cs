using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_App.Models;

namespace Hotel_App.Controllers
{
    public class ClientsAdminController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View(db.Clients.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        public ActionResult Create()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "client_id,client_first_name,client_last_name,client_age,client_phone_number,client_address")] Clients clients)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            try
            {
                if (clients.client_first_name != null && clients.client_last_name != null &&
                    clients.client_address != null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Clients.Add(clients);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                    ViewBag.msg = "Input all client's data!";
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(e.StackTrace);
            }

            return View(clients);
        }

        public ActionResult Edit(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "client_id,client_first_name,client_last_name,client_age,client_phone_number,client_address")] Clients clients)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clients);
        }

        public ActionResult Delete(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Find(id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (IsLogin())
                    return RedirectToAction("Index", "Home");

                Clients clients = db.Clients.Find(id);
                db.Clients.Remove(clients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Guest has reservation!");
                Console.WriteLine(e.StackTrace);
            }
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
    }
}
