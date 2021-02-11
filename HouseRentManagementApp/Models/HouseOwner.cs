using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class HouseOwner
    {
        public int Id { get; set; }
         [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
         [Required(ErrorMessage = "Please enter a NationalId")]
        public string NationalId {get;set;}

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid  Number")]
        public string MobileNo {get;set;}
         [Required(ErrorMessage = "Please select your Occupation")]
        public int OccupationId {get;set;}

        public Occupation occupation {get;set;}

         [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
          [NotMapped]
          [Required(ErrorMessage = "Please enter Confirm password")]
          [Compare("Password")]
         public string ConfirmPassword { get; set; }

          public List<Home> Homes { get; set; }
        
    }
}