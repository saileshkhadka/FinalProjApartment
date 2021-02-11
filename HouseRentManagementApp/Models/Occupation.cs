using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Occupation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<HouseOwner> houseOwners { get; set; }

        public List<HouseRepresentative> HouseRepresentatives { get; set; }

        public List<Occupation> Occupations { get; set; }
    }
}