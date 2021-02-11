using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Rotativa;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementApp.Models;

namespace HouseRentManagementApp.Controllers
{
    public class RentedBiesController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: RentedBies
        public ActionResult Index()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            var rentedBys = db.RentedBys.Include(r=>r.Flats.Homes);
            var houseRepresentatives = db.HouseRepresentatives.ToList();
            List<RentedBy> rentedBysForList = new List<RentedBy>();
            foreach(var rentedBy in rentedBys)
            {
                rentedBy.HouseRepresentatives = houseRepresentatives.Find(a => a.Id == rentedBy.HouseRepresentative);
                rentedBysForList.Add(rentedBy);
            }
            int houseOwnerId=(int)Session["HouseOwnerId"];
            var rentedHouseOfSameOwner = rentedBysForList.Where(a => a.Flats.Homes.HouseOwnerId == houseOwnerId).ToList();
            return View(rentedHouseOfSameOwner.ToList());
        }

        // GET: RentedBies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentedBy rentedBy = db.RentedBys.Find(id);
            if (rentedBy == null)
            {
                return HttpNotFound();
            }
            return View(rentedBy);
        }

        // GET: RentedBies/Create
        public ActionResult Create()
        {
            if (Session["HouseOwnerId"] == null)
            {
                return RedirectToAction("Index", "HouseOwners");
            }
            var homes=db.Homes.ToList();
            int HouseOwnerId=(int)Session["HouseOwnerId"];
            ViewBag.HomeId = homes.Where(a => a.HouseOwnerId == HouseOwnerId);
            
            return View();
        }

        // POST: RentedBies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( RentedBy rentedBy, string MobileNo)
        {
            var homes = db.Homes.ToList();
            int HouseOwnerId = (int)Session["HouseOwnerId"];
            ViewBag.HomeId = homes.Where(a => a.HouseOwnerId == HouseOwnerId);
            List<HouseRepresentative> houseRepresentatives = db.HouseRepresentatives.ToList();
            HouseRepresentative houseRepresentative = houseRepresentatives.Find(a => a.MobileNo == MobileNo);
        

            if (houseRepresentative == null)
            {
                ViewBag.error = "There is No Representative with this Mobile No";
                return View();
            }
            List<RentedBy> rentedBys=db.RentedBys.ToList();
            RentedBy HRForCheckRentedInfo=rentedBys.Find(a=>a.HouseRepresentative==houseRepresentative.Id && a.LeavingDate==null);
            if (HRForCheckRentedInfo!=null)
            {
                ViewBag.error = "This House Representaive Allready lives In A House!";
                return View();
            }
            HRForCheckRentedInfo = rentedBys.Find(a => a.LeavingDate == null && a.FlatId==rentedBy.FlatId);
            if(HRForCheckRentedInfo != null)
            {
                ViewBag.error = "This House has Allready Occupied By Other!";
                return View();
            }
            
            
            rentedBy.HouseRepresentative = houseRepresentative.Id;
            
           
                db.RentedBys.Add(rentedBy);
                db.SaveChanges();
                return RedirectToAction("Index");
        }

        // GET: RentedBies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentedBy rentedBy = db.RentedBys.Find(id);
            if (rentedBy == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlatId = new SelectList(db.Flats, "Id", "Description", rentedBy.FlatId);
            return View(rentedBy);
        }

        // POST: RentedBies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseRepresentative,FlatId,EntryDate,LeavingDate")] RentedBy rentedBy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rentedBy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlatId = new SelectList(db.Flats, "Id", "Description", rentedBy.FlatId);
            return View(rentedBy);
        }

        // GET: RentedBies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rentedBys = db.RentedBys.Include(a=>a.Flats).Include(a=>a.Flats.Homes).ToList();
            var HR = db.HouseRepresentatives.ToList();
            RentedBy rentedBy = rentedBys.Find(a => a.Id == id);
            rentedBy.HouseRepresentatives = HR.Find(a => a.Id == rentedBy.HouseRepresentative);
            if (rentedBy == null)
            {
                return HttpNotFound();
            }
            return View(rentedBy);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // POST: RentedBies/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            RentedBy rentedByForModify = db.RentedBys.Find(id);
            rentedByForModify.LeavingDate = DateTime.Now;
            db.Entry(rentedByForModify).State = EntityState.Modified;
            db.SaveChanges();


            return new ActionAsPdf("LeaveRenterPdf", rentedByForModify) { FileName = "Leave.pdf" };
        }

        public ActionResult LeaveRenterPdf(RentedBy rentedByForModify)
        {
            var rentedBys = db.RentedBys.Include(a => a.Flats.Homes.HouseOwners).ToList();
            var HR = db.HouseRepresentatives.ToList();
            RentedBy rentedBy = rentedBys.Find(a => a.Id == rentedByForModify.Id);
            rentedBy.HouseRepresentatives = HR.Find(a => a.Id == rentedBy.HouseRepresentative);

            ViewBag.FlatNo = rentedBy.Flats.FlatNo.ToString();
            ViewBag.Home = rentedBy.Flats.Homes.Name;
            ViewBag.HR = rentedBy.HouseRepresentatives.Name;
            ViewBag.HouseOwner = rentedBy.Flats.Homes.HouseOwners.Name;
            return View(rentedBy);
        }
        public JsonResult GetFlatByHomeId(int HomeId)
        {
            
            var flats = db.Flats.ToList();
            List<Flat> flatsByHome = flats.Where(a => a.HomeId == HomeId).ToList();
            //return Json(course);
            
            return Json(flatsByHome, JsonRequestBehavior.AllowGet);
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
