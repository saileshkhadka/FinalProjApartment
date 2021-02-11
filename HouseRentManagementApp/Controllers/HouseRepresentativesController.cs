using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HouseRentManagementApp.Models;
using Rotativa;

namespace HouseRentManagementApp.Controllers
{
    public class HouseRepresentativesController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: HouseRepresentatives
        public ActionResult Index()
        {
            if (Session["houseRepresentativeMobileName"] != null)
            {
                
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(HouseRepresentative houseRepresentative)
        {
            var houseRepresentatives = db.HouseRepresentatives.ToList();

            HouseRepresentative houseRepresentativeSearch = houseRepresentatives.Find(a => a.MobileNo == houseRepresentative.MobileNo && a.Password == houseRepresentative.Password);
            if (houseRepresentativeSearch != null)
            {
                Session["houseRepresentativeMobileName"] = houseRepresentativeSearch.MobileNo;
                Session["houseRepresentativeName"] = houseRepresentativeSearch.Name;
                Session["houseRepresentativeId"] = houseRepresentativeSearch.Id;
               return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LogInError = "Either Password or UserName Not match!";
                return View();
            }
           
        }
        public ActionResult SearchHouse()
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            ViewBag.CityId = db.Citis.ToList();
            return View();
        }
        // GET: HouseRepresentatives/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseRepresentative houseRepresentative = db.HouseRepresentatives.Find(id);
            if (houseRepresentative == null)
            {
                return HttpNotFound();
            }
            return View(houseRepresentative);
        }

        // GET: HouseRepresentatives/Create
        public ActionResult Registration()
        {
            if (Session["houseRepresentativeMobileName"] != null)
            {

                return RedirectToAction("Index", "Home");
            }

            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name");
            return View();
        }

        // POST: HouseRepresentatives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(HouseRepresentative houseRepresentative)
        {
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseRepresentative.OccupationId);
            
                var nationalIdWithMobileNos = db.NationalIdWithMobileNos.ToList();
                var houseRepresetatives=db.HouseRepresentatives.ToList();
                ViewBag.NationalId = nationalIdWithMobileNos.Find(a => a.NationalId == houseRepresentative.NationalId && a.MobileNo == houseRepresentative.MobileNo);
                ViewBag.ExistNationalId = houseRepresetatives.Find(a => a.NationalId == houseRepresentative.NationalId);
                if (ViewBag.NationalId != null && ViewBag.ExistNationalId == null)
                {
                    
                    db.HouseRepresentatives.Add(houseRepresentative);
                    db.SaveChanges();
                    var houseRepresentatives2 = db.HouseRepresentatives.ToList();

                    HouseRepresentative houseRepresentativeSearch = houseRepresentatives2.Find(a => a.MobileNo == houseRepresentative.MobileNo && a.Password == houseRepresentative.Password);
                   
                        Session["houseRepresentativeMobileName"] = houseRepresentativeSearch.MobileNo;
                        Session["houseRepresentativeName"] = houseRepresentativeSearch.Name;
                        Session["houseRepresentativeId"] = houseRepresentativeSearch.Id;

                       return  RedirectToAction("Index", "FlatMembers");

                }

                else
                {
                    if (ViewBag.NationalId == null)
                    { ViewBag.NationalIdFound = "Id  not Found"; }
                    return View();
                }
                
        }

        // GET: HouseRepresentatives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseRepresentative houseRepresentative = db.HouseRepresentatives.Find(id);
            if (houseRepresentative == null)
            {
                return HttpNotFound();
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseRepresentative.OccupationId);
            return View(houseRepresentative);
        }

        // POST: HouseRepresentatives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NationalId,MobileNo,OccupationId,UserName,Password,ConfirmPassword")] HouseRepresentative houseRepresentative)
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            if (ModelState.IsValid)
            {
                db.Entry(houseRepresentative).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseRepresentative.OccupationId);
            return View(houseRepresentative);
        }

        // GET: HouseRepresentatives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseRepresentative houseRepresentative = db.HouseRepresentatives.Find(id);
            if (houseRepresentative == null)
            {
                return HttpNotFound();
            }
            return View(houseRepresentative);
        }

        // POST: HouseRepresentatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["houseRepresentativeMobileName"] == null)
            {

                return RedirectToAction("Index", "HouseRepresentatives");
            }
            HouseRepresentative houseRepresentative = db.HouseRepresentatives.Find(id);
            db.HouseRepresentatives.Remove(houseRepresentative);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult GetAreaByCityId(int cityId)
        {
            var areas = db.Areas.ToList();
            List<Area> areasBYCity = areas.Where(a => a.CityId == cityId).ToList();
            //return Json(course);
            return Json(areasBYCity, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmptyFlatInfo(int AreaId)
        {
            var homes = db.Homes.ToList();
            List<Home> homeOfSameArea = homes.Where(a => a.AreaId == AreaId).ToList();

            var flats = db.Flats.ToList();
            var rentedBys = db.RentedBys.ToList();
            
             var houseOwners=db.HouseOwners.ToList();
            List<EmptyFlatView> emptyFlatViews = new List<EmptyFlatView>();
            foreach(var home in homeOfSameArea)
            {
                var flatsofSameHome = flats.Where(a => a.HomeId== home.Id).ToList();
                foreach (var flat in flatsofSameHome)
                {
                    var rentedByHR = rentedBys.Find(a => a.FlatId == flat.Id && a.LeavingDate == null);
                    if (rentedByHR == null)
                    {
                         var houseOwner=houseOwners.Find(a=>a.Id==home.HouseOwnerId);
                        EmptyFlatView emptyFlatView = new EmptyFlatView()
                        {
                            HouseName = home.Name,
                            FloorNo = flat.FloorNo,
                            Description = flat.Description,
                            Address = home.Address,
                           
                            ContactNo = houseOwner.MobileNo,
                            Rent = flat.RentPrice




                        };
                        emptyFlatViews.Add(emptyFlatView);
                    }
                }
            }
           
            return Json(emptyFlatViews, JsonRequestBehavior.AllowGet);
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
