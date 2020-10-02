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
    public class RoomsAdminController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        public ActionResult Index()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View(db.Rooms.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Rooms rooms = db.Rooms.Find(id);
                if (rooms == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<Room_reservation> li = rooms.Room_reservation.ToList();
                    ViewData["list"] = li;
                }
                return View(rooms);
            }
            catch (DataException e)
            {
                RedirectToAction("Index", "Home");
                Console.WriteLine(e.StackTrace);
            }
            return View();
        }

        public ActionResult Create()
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "room_id,room_number,room_people_capacity,room_bathroom")] Rooms rooms)
        {
            try
            {
                if (db.Rooms.Where(x => x.room_number == rooms.room_number).Any())
                {
                    ModelState.AddModelError("", "Room with that number already exist!");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.Rooms.Add(rooms);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                Console.WriteLine(e.StackTrace);
            }

            return View(rooms);
        }

        public ActionResult Edit(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "room_id,room_number,room_people_capacity,room_bathroom")] Rooms rooms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rooms);
        }

        public ActionResult Delete(int? id)
        {
            if (IsLogin())
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rooms rooms = db.Rooms.Find(id);
            db.Rooms.Remove(rooms);
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
