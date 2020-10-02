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
    public class RoomsEmployeeController : Controller
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
