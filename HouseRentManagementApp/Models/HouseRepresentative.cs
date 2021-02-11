using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class HouseRepresentative
    {
        public int Id { get; set; }
         [Required(ErrorMessage = "Please enter Name ")]
        public string Name { get; set; }
         [Required(ErrorMessage = "Please enter NationalId ")]
        public string NationalId { get; set; }
         [Required(ErrorMessage = "Please enter MobileNo ")]
        public string MobileNo { get; set; }
         [Required(ErrorMessage = "Please enter Your Occupation ")]
         public int OccupationId { get; set; }
         public Occupation Occupations { get; set; }
         [Required(ErrorMessage = "Please enter Password ")]
        public string Password { get; set; }

         [Required(ErrorMessage = "Please Re-enter Password ")]
        [NotMapped]
         [Compare("Password")]
         public string ConfirmPassword { get; set; }

         public List<FlatMember> FlatMembers { get; set; }

         public List<RentedBy> RentedBy { get; set; }
    }
}