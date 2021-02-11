using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class FlatMember
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Name ")]
        public string Name { get; set; }
         [Required(ErrorMessage = "Please Select EntryDate ")]
        public DateTime EntryDate { get; set; }
       
        public Nullable<DateTime> LeavingDate { get; set;}
        [Required(ErrorMessage = "Please Select Occupation ")]
        [Display(Name="Select Occupation")]
         public int OccupationId { get; set; }
         public Occupation Occupations { get; set; }
         public int HouseRepresentativeId { get; set; }

         public HouseRepresentative HouseRepresentatives { get; set; }

         [Required(ErrorMessage = "Please enter NationalId ")]
         [Display(Name = "NationalId Or Birth Certificate")]
         public string NationalIdOrBirthCertificate { get; set; }
         [Required(ErrorMessage = "Please enter MobileNo ")]
         [Display(Name = "Mobile No")]

         public string MobileNo { get; set; }
    }
}