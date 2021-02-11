using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class RentedBy
    {
        public int Id { get; set; }
        public int HouseRepresentative { get; set; }
        
        public HouseRepresentative HouseRepresentatives { get; set; }
        public int FlatId { get; set; }

        public Flat Flats { get; set; }

        [Required(ErrorMessage="EntryDate Must Be Filled")]
        [Display(Name="Entry Date")]
        public DateTime EntryDate { get; set; }

        public Nullable<DateTime> LeavingDate { get; set; }
    }
}