using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HouseRentManagementApp.Models
{
    public class HouseRentDBContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Citis { get; set; }

        public DbSet<Flat> Flats { get; set; }

        public DbSet<FlatMember> FlatMembers { get; set; }

        public DbSet<HouseOwner> HouseOwners { get; set; }
        public DbSet<HouseRepresentative> HouseRepresentatives { get; set; }
        public DbSet<NationalIdWithMobileNo> NationalIdWithMobileNos { get; set; }
        public DbSet<Occupation> Occupations { get; set; }

        public DbSet<RentedBy> RentedBys { get; set; }

        public DbSet<Home> Homes { get; set; }

        public DbSet<Type> Types { get; set; }
        public DbSet<Admin> Admins { get; set; }









        




    }
}