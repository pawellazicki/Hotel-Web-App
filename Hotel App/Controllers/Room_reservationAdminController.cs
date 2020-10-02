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
    public class Room_reservationAdminController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            var room_reservation = db.Room_reservation.Include(r => r.Clients).Include(r => r.Rooms);
            return View(room_reservation.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_reservation room_reservation = db.Room_reservation.Find(id);
            if (room_reservation == null)
            {
                return HttpNotFound();
            }
            return View(room_reservation);
        }

        public ActionResult Create()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_first_name");
            ViewBag.room_id = new SelectList(db.Rooms, "room_id", "room_number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "room_reservation_id,client_id,room_id,price,check_in,check_out")] Room_reservation room_reservation)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Room_reservation.Add(room_reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_first_name", room_reservation.client_id);
            ViewBag.room_id = new SelectList(db.Rooms, "room_id", "room_number", room_reservation.room_id);
            return View(room_reservation);
        }

        public ActionResult Edit(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_reservation room_reservation = db.Room_reservation.Find(id);
            if (room_reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_first_name", room_reservation.client_id);
            ViewBag.room_id = new SelectList(db.Rooms, "room_id", "room_number", room_reservation.room_id);
            return View(room_reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "room_reservation_id,client_id,room_id,price,check_in,check_out")] Room_reservation room_reservation)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(room_reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_first_name", room_reservation.client_id);
            ViewBag.room_id = new SelectList(db.Rooms, "room_id", "room_number", room_reservation.room_id);
            return View(room_reservation);
        }

        public ActionResult Delete(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_reservation room_reservation = db.Room_reservation.Find(id);
            if (room_reservation == null)
            {
                return HttpNotFound();
            }
            return View(room_reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            Room_reservation room_reservation = db.Room_reservation.Find(id);
            db.Room_reservation.Remove(room_reservation);
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
