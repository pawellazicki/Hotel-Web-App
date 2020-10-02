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
    public class ClientsEmployeeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: ClientsEmployee
        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View(db.Clients.ToList());
        }

        // GET: ClientsEmployee/Details/5
        public ActionResult Details(int? id)
        {
            if(IsLogin())
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

        // GET: ClientsEmployee/Create
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

        // GET: ClientsEmployee/Edit/5
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

        // POST: ClientsEmployee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ClientsEmployee/Delete/5
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

        // POST: ClientsEmployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            Clients clients = db.Clients.Find(id);
            db.Clients.Remove(clients);
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
    }
}
