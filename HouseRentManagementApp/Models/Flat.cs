using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Flat
    {
        public int Id { get; set; }
         [Required(ErrorMessage = "Please Select Home Id")]
        public int HomeId { get; set; }

        public Home Homes { get; set; }
         [Required(ErrorMessage = "Please Enter  FloorNo")]
        public int FloorNo { get; set; }
          [Required(ErrorMessage = "Please Enter  FlatNo")]
        public int FlatNo { get; set; }
          [Required(ErrorMessage = "Please Select Type")]
        public int TypeId { get; set; }

        public Type Types { get; set; }
         [Required(ErrorMessage = "Please Enter  RentPrice")]
        public decimal RentPrice { get; set; }
         [Required(ErrorMessage = "Please Enter  Flat Description")]
        public string Description { get; set; }

         public List<RentedBy> RentedBys { get; set; }
    }
}