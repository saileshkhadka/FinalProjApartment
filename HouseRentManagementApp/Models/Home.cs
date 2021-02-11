using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Home
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name ")]

        public string HomeNo { get; set; }
        public string Name { get; set; }

         [Required(ErrorMessage = "Please Select Area")]
        public int AreaId { get; set; }
        public Area areas { get; set; }

        public int HouseOwnerId { get; set; }

        public HouseOwner HouseOwners { get; set; }
    
         [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
         [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; }

         public List<Flat> Flats { get; set; }



    }
}