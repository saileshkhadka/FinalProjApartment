using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CityId { get; set; }

        public City Cities { get; set; }

        public List<Home> Homes { get; set; }
    }
}