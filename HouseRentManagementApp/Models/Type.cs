﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Flat> Flats { get; set; }
    }
}