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
    public class HomesController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: Homes
        public ActionResult Index()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            var homes = db.Homes.Include(h => h.areas).Include(h => h.HouseOwners);
            int HouseOwnerId=(int)Session["HouseOwnerId"];
            var homesOfSameOwner = homes.Where(a => a.HouseOwnerId == HouseOwnerId).ToList();
            return View(homesOfSameOwner.ToList());
        }

        // GET: Homes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // GET: Homes/Create
        public ActionResult Create()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            //ViewBag.AreaId = new SelectList(db.Areas, "Id", "Name");
            //ViewBag.HouseOwnerId = new SelectList(db.HouseOwners, "Id", "Name");
            ViewBag.CityId =db.Citis.ToList();
            return View();
        }

        // POST: Homes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Home home,int CityId)
        {
            ViewBag.CityId = db.Citis.ToList();
            int HomeCount = db.Homes.Count()+1;
            home.HomeNo = CityId.ToString("00") + home.AreaId.ToString("000") + HomeCount;
            home.HouseOwnerId = (int)Session["HouseOwnerId"]; 
           
                
                db.Homes.Add(home);
                db.SaveChanges();
                return RedirectToAction("Index");

            //ViewBag.AreaId = new SelectList(db.Areas, "Id", "Name", home.AreaId);
            //ViewBag.HouseOwnerId = new SelectList(db.HouseOwners, "Id", "Name", home.HouseOwnerId);
        }

        // GET: Homes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
         
            return View(home);
        }

        // POST: Homes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HomeNo,Name,AreaId,HouseOwnerId,Address,Description")] Home home)
        {
          
            
            
                db.Entry(home).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            
        
        }

        // GET: Homes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: Homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Home home = db.Homes.Find(id);
            db.Homes.Remove(home);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetAreaByCityId(int cityId)
        {
           var areas=db.Areas.ToList();
           List<Area> areasBYCity = areas.Where(a => a.CityId == cityId).ToList();
            //return Json(course);
           return Json(areasBYCity, JsonRequestBehavior.AllowGet);
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
