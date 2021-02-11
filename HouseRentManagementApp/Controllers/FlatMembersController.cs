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
    public class FlatMembersController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: FlatMembers
        public ActionResult Index()
        {
            if (Session["houseRepresentativeId"] == null)
            {
                return RedirectToAction("Index","houserepresentatives");
            }
            var flatMembers = db.FlatMembers.Include(f => f.HouseRepresentatives).Include(f => f.Occupations);
            int HRId=(int)Session["houseRepresentativeId"];
            var flatMembersOfSameHR = flatMembers.Where(a => a.HouseRepresentativeId == HRId).ToList();
            return View(flatMembersOfSameHR.ToList());
        }

        // GET: FlatMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlatMember flatMember = db.FlatMembers.Find(id);
            if (flatMember == null)
            {
                return HttpNotFound();
            }
            return View(flatMember);
        }

        // GET: FlatMembers/Create
        public ActionResult AddMember()
        {
            if (Session["houseRepresentativeId"]==null)
            {
                return View("HouseRepresentatives/Index");
            }
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name");
            return View();
        }

        // POST: FlatMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember( FlatMember flatMember)
        {
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", flatMember.OccupationId);
           
                flatMember.EntryDate = DateTime.Now;
                flatMember.HouseRepresentativeId = (int)Session["houseRepresentativeId"];
                db.FlatMembers.Add(flatMember);
                db.SaveChanges();
                return RedirectToAction("Index");
           
        }

        // GET: FlatMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlatMember flatMember = db.FlatMembers.Find(id);
            if (flatMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseRepresentativeId = new SelectList(db.HouseRepresentatives, "Id", "Name", flatMember.HouseRepresentativeId);
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", flatMember.OccupationId);
            return View(flatMember);
        }

        // POST: FlatMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,EntryDate,LeavingDate,OccupationId,HouseRepresentativeId,NationalIdOrBirthCertificate,MobileNo")] FlatMember flatMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flatMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseRepresentativeId = new SelectList(db.HouseRepresentatives, "Id", "Name", flatMember.HouseRepresentativeId);
            ViewBag.OccupationId = new SelectList(db.Occupations, "Id", "Name", flatMember.OccupationId);
            return View(flatMember);
        }

        // GET: FlatMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlatMember flatMember = db.FlatMembers.Find(id);
            if (flatMember == null)
            {
                return HttpNotFound();
            }
            return View(flatMember);
        }

        // POST: FlatMembers/Delete/5
  
        public ActionResult DeleteConfirmed(int id)
        {
            FlatMember flatMember = db.FlatMembers.Find(id);
            flatMember.LeavingDate = DateTime.Now;
            db.Entry(flatMember).State = EntityState.Modified;
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
