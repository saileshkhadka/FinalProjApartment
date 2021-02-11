using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Area> Areas { get; set; }

        public List<Home> Homes { get; set; }
    }
}