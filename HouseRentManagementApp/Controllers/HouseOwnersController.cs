using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Rotativa;
using System.Web.Mvc;
using HouseRentManagementApp.Models;

namespace HouseRentManagementApp.Controllers
{
    public class HouseOwnersController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: HouseOwners
        public ActionResult Index()
        {
            if (Session["HouseOwnerMobileNo"]!=null)
            {
                return RedirectToAction("Index", "Homes");
            }
           return View();
        }
        [HttpPost]
        public ActionResult Index(HouseOwner houseOwner)
        {
            var houseOwners = db.HouseOwners.ToList();

            HouseOwner houseOwnerSearch = houseOwners.Find(a => a.MobileNo == houseOwner.MobileNo && a.Password == houseOwner.Password);
            if(houseOwnerSearch!=null)
            {
                Session["HouseOwnerMobileNo"] = houseOwnerSearch.MobileNo;
                Session["HouseOwnerName"] = houseOwnerSearch.Name;
                Session["HouseOwnerId"] = houseOwnerSearch.Id;
                return RedirectToAction("Index","Homes");
            }
            else
            {
                ViewBag.LogInError = "Either Password or UserName Not match!";
                return View();
            }
          
        }

        // GET: HouseOwners/Details/5
        public ActionResult Details(HouseOwner houseOwner)
        {
            
            var houseOwners = db.HouseOwners.ToList();
            HouseOwner houseOwnerRegInf = houseOwners.Find(a => a.NationalId == houseOwner.NationalId);
            if (houseOwner == null)
            {
                return HttpNotFound();
            }
            return View(houseOwner);
        }

        // GET: HouseOwners/Create
        public ActionResult Create()
        {
            if (Session["HouseOwnerMobileNo"] != null)
            {
                return RedirectToAction("Index", "Homes");
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name");
            return View();
        }

        // POST: HouseOwners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( HouseOwner houseOwner)
         {
            
            
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseOwner.OccupationId);
            if (ModelState.IsValid) 
            {
                var nationalIdWithMobileNos = db.NationalIdWithMobileNos.ToList();
                var houseOwners = db.HouseOwners.ToList();

               
                ViewBag.NationalId = nationalIdWithMobileNos.Find(a => a.NationalId == houseOwner.NationalId && a.MobileNo == houseOwner.MobileNo);
                ViewBag.ExistNationalId = houseOwners.Find(a => a.NationalId == houseOwner.NationalId);
                if (ViewBag.NationalId != null && ViewBag.ExistNationalId==null)
                {
                    db.HouseOwners.Add(houseOwner);
                    db.SaveChanges();
                    var houseOwners2 = db.HouseOwners.ToList();

                    HouseOwner houseOwnerSearch = houseOwners2.Find(a => a.MobileNo == houseOwner.MobileNo && a.Password == houseOwner.Password);
                   
                        Session["HouseOwnerMobileNo"] = houseOwnerSearch.MobileNo;
                        Session["HouseOwnerName"] = houseOwnerSearch.Name;
                        Session["HouseOwnerId"] = houseOwnerSearch.Id;

                        return RedirectToAction("Index", "Homes");
                    
                }

                else
                {
                    if (ViewBag.NationalId == null)
                    { ViewBag.NationalIdFound = "Id  not Found"; }
                    return View();
                }
                
                
            }

            return View(houseOwner);
        }

        // GET: HouseOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseOwner houseOwner = db.HouseOwners.Find(id);
            if (houseOwner == null)
            {
                return HttpNotFound();
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseOwner.OccupationId);
            return View(houseOwner);
        }

        // POST: HouseOwners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NationalId,MobileNo,OccupationId,UserName,Password,ConfirmPassword")] HouseOwner houseOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(houseOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", houseOwner.OccupationId);
            return View(houseOwner);
        }

        // GET: HouseOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseOwner houseOwner = db.HouseOwners.Find(id);
            if (houseOwner == null)
            {
                return HttpNotFound();
            }
            return View(houseOwner);
        }

        // POST: HouseOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseOwner houseOwner = db.HouseOwners.Find(id);
            
            db.HouseOwners.Remove(houseOwner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RentedRoomInfo()
        {
            if (Session["HouseOwnerMobileNo"] == null)
            {
                return RedirectToAction("Index");
            }
            var Homes = db.Homes.ToList();

            ViewBag.HomeId = Homes.Where(a => a.HouseOwnerId == (int)Session["HouseOwnerId"]);

            return View();
        }
        [HttpPost]
        public ActionResult RentedRoomInfo(int FlatId)
        {
            var rentedBys = db.RentedBys.Include(a => a.Flats.Homes).ToList();
            var rentedBy = rentedBys.Find(a => a.FlatId == FlatId && a.LeavingDate == null);

            return new ActionAsPdf("RentInfoPdf", rentedBy) { FileName = "RentInfos.pdf" }; ;
        }
        public ActionResult RentInfoPdf(RentedBy rentedBy1)
        {
            var rentedBys = db.RentedBys.Include(a => a.Flats.Homes.HouseOwners).ToList();
            var rentedBy = rentedBys.Find(a =>a.Id==rentedBy1.Id);

            var representatives = db.HouseRepresentatives.ToList();
            var representative = representatives.Find(a => a.Id == rentedBy.HouseRepresentative);
            ViewBag.HR = representative.Name;
            ViewBag.HRNo = representative.MobileNo;
            var flatMember = db.FlatMembers.Where(a => a.HouseRepresentativeId == representative.Id && a.LeavingDate==null).ToList();
            ViewBag.HouseName = rentedBy.Flats.Homes.Name;
            ViewBag.FlatNo= rentedBy.Flats.FlatNo;
            ViewBag.Owner = rentedBy.Flats.Homes.HouseOwners.Name;
            return View(flatMember);
        }
        public JsonResult GetFlatByHomeId(int HomeId)
        {

            var flats = db.Flats.ToList();
            List<Flat> flatsByHome = flats.Where(a => a.HomeId == HomeId).ToList();
            //return Json(course);

            return Json(flatsByHome, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRentedRoomInfo(int FlatId)
        {
            var rentedBys=db.RentedBys.ToList();
            var rentedBy=rentedBys.Find(a=>a.FlatId==FlatId && a.LeavingDate==null);
            List<ViewRentedInfo> viewRentInfos = new List<ViewRentedInfo>();
            var representatives = db.HouseRepresentatives.ToList();
            if (representatives != null && rentedBy!=null)
            {
               
                var representative = representatives.Find(a => a.Id == rentedBy.HouseRepresentative);
                
                var flatMembers = db.FlatMembers.ToList();
                var flatMembersOfSameHouse = flatMembers.Where(a => a.HouseRepresentativeId == representative.Id && a.LeavingDate == null);
                if(flatMembersOfSameHouse==null)
                {
                    ViewRentedInfo viewRentInfo = new ViewRentedInfo()
                    {
                        RepresentativeName = representative.Name,
                        RepresentativeContactNo = representative.MobileNo,
                    };
                    viewRentInfos.Add(viewRentInfo);
                }
                else
                {
                    foreach (var flatMember in flatMembersOfSameHouse)
                    {
                        ViewRentedInfo viewRentInfo = new ViewRentedInfo()
                        {
                            RepresentativeName = representative.Name,
                            RepresentativeContactNo = representative.MobileNo,
                            ContactNo = flatMember.MobileNo,
                            MemberName = flatMember.Name
                        };
                        viewRentInfos.Add(viewRentInfo);
                    }
                }
               
                
            }
            return Json(viewRentInfos, JsonRequestBehavior.AllowGet);
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
