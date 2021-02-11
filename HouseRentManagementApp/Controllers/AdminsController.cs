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
    public class AdminsController : Controller
    {
        private HouseRentDBContext db = new HouseRentDBContext();

        // GET: Admins
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Admin admin)
        {
            var admins = db.Admins.ToList();
            var adminCheck = admins.Find(a => a.UserName == admin.UserName && a.Password == admin.Password);

            if(adminCheck!= null)
            {
                Session["AdminName"] = adminCheck.UserName;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult HouseInformtion()
        {
            if(Session["AdminName"] ==null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CityId = db.Citis.ToList();
            return View();
        }

        public ActionResult FlatInfo()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult MemberInfo()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult RenterInfo()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Admins/Details/5
        
        public JsonResult GetAreaByCityId(int cityId)
        {
            var areas = db.Areas.ToList();
            List<Area> areasBYCity = areas.Where(a => a.CityId == cityId).ToList();
            //return Json(course);
            return Json(areasBYCity, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFlatByHouse(string HouseNo)
        {
            List<Flat> Flats = db.Flats.Include(a=>a.Homes).ToList().Where(a => a.Homes.HomeNo == HouseNo).ToList();
            List<Flat> FlatByHouse = new List<Flat>();
            foreach(var flat in Flats)
            {
                Flat aflat = new Flat()
                {
                    Id=flat.Id,
                    FlatNo=flat.FlatNo
                };
                FlatByHouse.Add(aflat);
            }
          
            //return Json(course);
            return Json(FlatByHouse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHouseInfo(int AreaId)
        {
            List<Home> Houses = db.Homes.Include(a=>a.HouseOwners).ToList();
            List<Home> HousesOfSameArea = Houses.Where(a => a.AreaId == AreaId).ToList();
            List<ViewHouseInformationsForAdmin> houseInformations = new List<ViewHouseInformationsForAdmin>();
            if (HousesOfSameArea.Count != 0)
            {
                

                foreach (var house in HousesOfSameArea)
                {
                    ViewHouseInformationsForAdmin houseInformation = new ViewHouseInformationsForAdmin()
                    {
                        HouseName=house.Name,
                        HouseNo=house.HomeNo,
                        OwnerMobileNo=house.HouseOwners.MobileNo,
                        OwnerName=house.HouseOwners.Name,
                        Description=house.Description,
                        Address=house.Address
                    };
                    houseInformations.Add(houseInformation);
                }
            }


            return Json(houseInformations, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFlatInfo(int FlatNo)
        {
            List<RentedBy> rentedBys = db.RentedBys.Where(a=>a.FlatId==FlatNo).ToList();
            var HRs=db.HouseRepresentatives.ToList();
            List<ViewFlatRentInfoForAdmin> RentedInfos = new List<ViewFlatRentInfoForAdmin>();
            if (rentedBys.Count != 0)
            {
              

                foreach (var rentedBy in rentedBys)
                {
                    var HR = HRs.Find(a => a.Id == rentedBy.HouseRepresentative);
                    string to;
                    if (rentedBy.LeavingDate == null)
                    {
                         to = "Not Leave Yet!!";
                    }
                    else
                    {
                         to = rentedBy.LeavingDate.ToString();
                    }
                    ViewFlatRentInfoForAdmin RentedInfo = new ViewFlatRentInfoForAdmin()
                    {
                       Name=HR.Name,
                       MobileNo=HR.MobileNo,
                       NationalId=HR.NationalId,
                       From=rentedBy.EntryDate.Date.ToString(),
                       To=to

                       
                    };
                    RentedInfos.Add(RentedInfo);
                }
            }


            return Json(RentedInfos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRenerFlatsInfo(string HouseRenter)
        {
            var HRs = db.HouseRepresentatives.ToList();
            HouseRepresentative HR = HRs.Find(a => a.MobileNo == HouseRenter);
            List<ViewRenterFlatInfo> RenterInfos = new List<ViewRenterFlatInfo>();
            if (HR != null)
            {
                var RentedByAll = db.RentedBys.Include(a => a.Flats.Homes.HouseOwners).ToList();
                var RentedBys = RentedByAll.Where(a => a.Id == HR.Id).ToList();

                if (RentedBys.Count != 0)
                {


                    foreach (var rentedBy in RentedBys)
                    {
                        string to;
                        if (rentedBy.LeavingDate == null)
                        {
                            to = "Not Leave";
                        }
                        else
                        {
                            to = rentedBy.LeavingDate.Value.ToString();
                        }
                        ViewRenterFlatInfo RenterInfo = new ViewRenterFlatInfo()
                        {
                            HouseName = rentedBy.Flats.Homes.Name,
                            HouseNo = rentedBy.Flats.Homes.HomeNo,
                            FlatNo = rentedBy.Flats.FlatNo,
                            OwnerName = rentedBy.Flats.Homes.HouseOwners.Name,
                            OwnerMobleNo = rentedBy.Flats.Homes.HouseOwners.MobileNo,
                            From = rentedBy.EntryDate.Date.ToString(),
                            To = to
                        };
                        RenterInfos.Add(RenterInfo);
                    }
                }


            }
          



           
            
            return Json(RenterInfos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFlatsMemberInfo(string HouseRenter)
        {
            var HRs = db.HouseRepresentatives.ToList();
            HouseRepresentative HR = HRs.Find(a => a.MobileNo == HouseRenter);
            var flatMembers = db.FlatMembers.Where(a => a.HouseRepresentativeId == HR.Id).ToList();
            List<ViewMemberList> viewMemberLists = new List<ViewMemberList>();
            foreach (var flatMember in flatMembers)
            {
                string to;
                if(flatMember.LeavingDate==null)
                {
                    to="Not Leave";
                }
                else
                {
                    to=flatMember.LeavingDate.Value.ToString();
                }
                ViewMemberList viewMemberList = new ViewMemberList()
                {
                    Name = flatMember.Name,
                    MobileNo = flatMember.MobileNo,
                    From = flatMember.EntryDate.Date.ToString(),
                    To = to
                };
                viewMemberLists.Add(viewMemberList);
            }
            return Json(viewMemberLists, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetFlatsMemberInfoByMobileNo(string MobileNo)
        {

            var flatMembers = db.FlatMembers.Include(a => a.HouseRepresentatives).Include(a=>a.Occupations).Where(a => a.MobileNo == MobileNo).ToList();
            List<ViewMemberList> viewMemberLists = new List<ViewMemberList>();
            
            foreach (var flatMember in flatMembers)
            {
                string to;
                if (flatMember.LeavingDate == null)
                {
                    to = "Not Leave";
                }
                else
                {
                    to = flatMember.LeavingDate.Value.ToString();
                }
                ViewMemberList viewMemberList = new ViewMemberList()
                {
                    Name = flatMember.Name,
                    Occupations = flatMember.Occupations.Name,
                    NationalId=flatMember.NationalIdOrBirthCertificate,
                    RepresentativeMobileNo=flatMember.HouseRepresentatives.MobileNo,
                    RepresentativeName=flatMember.HouseRepresentatives.Name,
                    From = flatMember.EntryDate.Date.ToString(),
                    To = to
                };
                viewMemberLists.Add(viewMemberList);
            }
            return Json(viewMemberLists, JsonRequestBehavior.AllowGet);

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
