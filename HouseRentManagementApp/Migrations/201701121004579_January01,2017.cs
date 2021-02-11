namespace HouseRentManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class January012017 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Homes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeNo = c.String(nullable: false),
                        Name = c.String(),
                        AreaId = c.Int(nullable: false),
                        HouseOwnerId = c.Int(nullable: false),
                        Address = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        City_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .ForeignKey("dbo.HouseOwners", t => t.HouseOwnerId)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .Index(t => t.AreaId)
                .Index(t => t.HouseOwnerId)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.Flats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeId = c.Int(nullable: false),
                        FloorNo = c.Int(nullable: false),
                        FlatNo = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        RentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Homes", t => t.HomeId)
                .ForeignKey("dbo.Types", t => t.TypeId)
                .Index(t => t.HomeId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.RentedBies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseRepresentative = c.Int(nullable: false),
                        FlatId = c.Int(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        LeavingDate = c.DateTime(),
                        HouseRepresentatives_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId)
                .ForeignKey("dbo.HouseRepresentatives", t => t.HouseRepresentatives_Id)
                .Index(t => t.FlatId)
                .Index(t => t.HouseRepresentatives_Id);
            
            CreateTable(
                "dbo.HouseRepresentatives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NationalId = c.String(nullable: false),
                        MobileNo = c.String(nullable: false),
                        OccupationId = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Occupations", t => t.OccupationId)
                .Index(t => t.OccupationId);
            
            CreateTable(
                "dbo.FlatMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        LeavingDate = c.DateTime(),
                        OccupationId = c.Int(nullable: false),
                        HouseRepresentativeId = c.Int(nullable: false),
                        NationalIdOrBirthCertificate = c.String(nullable: false),
                        MobileNo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HouseRepresentatives", t => t.HouseRepresentativeId)
                .ForeignKey("dbo.Occupations", t => t.OccupationId)
                .Index(t => t.OccupationId)
                .Index(t => t.HouseRepresentativeId);
            
            CreateTable(
                "dbo.Occupations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Occupation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Occupations", t => t.Occupation_Id)
                .Index(t => t.Occupation_Id);
            
            CreateTable(
                "dbo.HouseOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NationalId = c.String(nullable: false),
                        MobileNo = c.String(),
                        OccupationId = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Occupations", t => t.OccupationId)
                .Index(t => t.OccupationId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NationalIdWithMobileNoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NationalId = c.String(),
                        MobileNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Homes", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Flats", "TypeId", "dbo.Types");
            DropForeignKey("dbo.RentedBies", "HouseRepresentatives_Id", "dbo.HouseRepresentatives");
            DropForeignKey("dbo.FlatMembers", "OccupationId", "dbo.Occupations");
            DropForeignKey("dbo.Occupations", "Occupation_Id", "dbo.Occupations");
            DropForeignKey("dbo.HouseRepresentatives", "OccupationId", "dbo.Occupations");
            DropForeignKey("dbo.HouseOwners", "OccupationId", "dbo.Occupations");
            DropForeignKey("dbo.Homes", "HouseOwnerId", "dbo.HouseOwners");
            DropForeignKey("dbo.FlatMembers", "HouseRepresentativeId", "dbo.HouseRepresentatives");
            DropForeignKey("dbo.RentedBies", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.Flats", "HomeId", "dbo.Homes");
            DropForeignKey("dbo.Homes", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "CityId", "dbo.Cities");
            DropIndex("dbo.HouseOwners", new[] { "OccupationId" });
            DropIndex("dbo.Occupations", new[] { "Occupation_Id" });
            DropIndex("dbo.FlatMembers", new[] { "HouseRepresentativeId" });
            DropIndex("dbo.FlatMembers", new[] { "OccupationId" });
            DropIndex("dbo.HouseRepresentatives", new[] { "OccupationId" });
            DropIndex("dbo.RentedBies", new[] { "HouseRepresentatives_Id" });
            DropIndex("dbo.RentedBies", new[] { "FlatId" });
            DropIndex("dbo.Flats", new[] { "TypeId" });
            DropIndex("dbo.Flats", new[] { "HomeId" });
            DropIndex("dbo.Homes", new[] { "City_Id" });
            DropIndex("dbo.Homes", new[] { "HouseOwnerId" });
            DropIndex("dbo.Homes", new[] { "AreaId" });
            DropIndex("dbo.Areas", new[] { "CityId" });
            DropTable("dbo.NationalIdWithMobileNoes");
            DropTable("dbo.Types");
            DropTable("dbo.HouseOwners");
            DropTable("dbo.Occupations");
            DropTable("dbo.FlatMembers");
            DropTable("dbo.HouseRepresentatives");
            DropTable("dbo.RentedBies");
            DropTable("dbo.Flats");
            DropTable("dbo.Homes");
            DropTable("dbo.Cities");
            DropTable("dbo.Areas");
            DropTable("dbo.Admins");
        }
    }
}
