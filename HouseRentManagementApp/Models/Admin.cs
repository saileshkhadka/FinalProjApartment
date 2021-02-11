using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage="UserName Must Be Filled")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Must Be Filled")]

        public string Password { get; set; }
    }
}