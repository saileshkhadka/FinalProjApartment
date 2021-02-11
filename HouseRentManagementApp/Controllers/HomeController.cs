using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseRentManagementApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LogOut()
        {
            if( Session["houseRepresentativeMobileName"]!= null)
            {
                Session["houseRepresentativeMobileName"] = null;
                Session["houseRepresentativeName"] =null;
                Session["houseRepresentativeId"] = null;
               
                return RedirectToAction("Index", "HouseRepresentatives");
            }
            else if (Session["HouseOwnerMobileNo"] != null)
            {
                Session["HouseOwnerMobileNo"] = null;
                Session["HouseOwnerName"] = null;
                Session["HouseOwnerId"] = null;
                return RedirectToAction("Index", "HouseOwners");
            }

            else
            {
                Session["AdminName"] = null;
                return RedirectToAction("Index", "Admins");
            }
         
           
        }

      
    }
}