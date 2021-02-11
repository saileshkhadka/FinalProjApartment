using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementApp.Models;

namespace HouseRentManagementApp.Controllers
{
    public class FlatsController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: Flats
        public ActionResult Index()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            var flats = db.Flats.Include(f => f.Homes).Include(f => f.Types);
            int houseOwnerId=(int) Session["HouseOwnerId"];
            var flatsofSameOwner = flats.Where(a => a.Homes.HouseOwnerId == houseOwnerId);
            return View(flatsofSameOwner.ToList());
        }

        // GET: Flats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flat flat = db.Flats.Find(id);
            if (flat == null)
            {
                return HttpNotFound();
            }
            return View(flat);
        }

        // GET: Flats/Create
        public ActionResult Create()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            List<Home> homes = db.Homes.ToList();
            int HouseOwnerId=(int)Session["HouseOwnerId"];
            List<Home> homesOFOwner = homes.Where(a => a.HouseOwnerId == HouseOwnerId).ToList();
            ViewBag.HomeId = new SelectList(homesOFOwner, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: Flats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HomeId,FloorNo,FlatNo,TypeId,RentPrice,Description")] Flat flat)
        {
            List<Flat> flats=db.Flats.Where(a=>a.FloorNo==flat.FloorNo).ToList();
            flat.FlatNo = flat.FloorNo*100+flats.Count+1;
            if (ModelState.IsValid)
            {
                db.Flats.Add(flat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<Home> homes = db.Homes.ToList();
            int HouseOwnerId = (int)Session["HouseOwnerId"];
            List<Home> homesOFOwner = homes.Where(a => a.HouseOwnerId == HouseOwnerId).ToList();
            ViewBag.HomeId = new SelectList(homesOFOwner, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", flat.TypeId);
            return View(flat);
        }

        // GET: Flats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flat flat = db.Flats.Find(id);
            if (flat == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeId = new SelectList(db.Homes, "Id", "HomeNo", flat.HomeId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", flat.TypeId);
            return View(flat);
        }

        // POST: Flats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HomeId,FloorNo,FlatNo,TypeId,RentPrice,Description")] Flat flat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeId = new SelectList(db.Homes, "Id", "HomeNo", flat.HomeId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", flat.TypeId);
            return View(flat);
        }

        // GET: Flats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flat flat = db.Flats.Find(id);
            if (flat == null)
            {
                return HttpNotFound();
            }
            return View(flat);
        }

        // POST: Flats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flat flat = db.Flats.Find(id);
            db.Flats.Remove(flat);
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
    }
}
