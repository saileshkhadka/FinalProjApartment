using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class ViewRenterFlatInfo
    {
        public string HouseName { get; set; }
        public string HouseNo { get; set; }

        public int FlatNo { get; set; }
        public string OwnerName { get; set; }
        public string OwnerMobleNo { get; set; }
        public string From { get; set; }
        public string To { get; set; }

    }
}