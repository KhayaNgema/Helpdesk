namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        IncidentId = c.Int(nullable: false, identity: true),
                        ReferenceNumber = c.String(),
                        OnboardingId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        SubCategoryId = c.Int(nullable: false),
                        Subject = c.String(),
                        Description = c.String(),
                        IssueFile = c.String(),
                        ProductVersion = c.String(),
                        DatabaseTypeId = c.Int(nullable: false),
                        HardwareDescriptionId = c.Int(nullable: false),
                        EnvironmentTypeId = c.Int(nullable: false),
                        VirtualizedPlatformId = c.Int(nullable: false),
                        Title = c.Int(nullable: false),
                        CallersName = c.String(),
                        CallersSurname = c.String(),
                        EmailAddress = c.String(),
                        CellNumber = c.String(),
                        DesignationId = c.Int(nullable: false),
                        LoggedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TicketStatus = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IncidentId)
                .ForeignKey("dbo.ApprovedRequests", t => t.OnboardingId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.DatabaseTypes", t => t.DatabaseTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Designations", t => t.DesignationId, cascadeDelete: true)
                .ForeignKey("dbo.EnvironmentTypes", t => t.EnvironmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.HardwareDescriptions", t => t.HardwareDescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.SubCategories", t => t.SubCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.VirtualizedPlatforms", t => t.VirtualizedPlatformId, cascadeDelete: true)
                .Index(t => t.OnboardingId)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubCategoryId)
                .Index(t => t.DatabaseTypeId)
                .Index(t => t.HardwareDescriptionId)
                .Index(t => t.EnvironmentTypeId)
                .Index(t => t.VirtualizedPlatformId)
                .Index(t => t.DesignationId);
            
            CreateTable(
                "dbo.ApprovedRequests",
                c => new
                    {
                        OnboardingId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        DesignationId = c.Int(nullable: false),
                        EmployeeOfficeAddress = c.String(),
                        OfficeAddress = c.String(),
                        CountryId = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        PostalCode = c.Int(nullable: false),
                        PeriodFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PeriodTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OpretaionalHoursOpen = c.Time(nullable: false, precision: 7),
                        OpretationalHourseClose = c.Time(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        ClientAbbr = c.String(),
                    })
                .PrimaryKey(t => t.OnboardingId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.ClientProducts",
                c => new
                    {
                        ClientProductId = c.Int(nullable: false, identity: true),
                        OnboardingId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientProductId)
                .ForeignKey("dbo.ApprovedRequests", t => t.OnboardingId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OnboardingId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductKey = c.String(),
                        ProductDescription = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Alpha3Code = c.String(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        SubCategoryId = c.Int(nullable: false, identity: true),
                        faultCode = c.String(),
                        SubCategoryName = c.String(),
                        PriorityId = c.Int(nullable: false),
                        SLAValueId = c.Int(nullable: false),
                        Category_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.SubCategoryId)
                .ForeignKey("dbo.Priorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.SLAValues", t => t.SLAValueId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .Index(t => t.PriorityId)
                .Index(t => t.SLAValueId)
                .Index(t => t.Category_CategoryId);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        PriorityId = c.Int(nullable: false, identity: true),
                        PriorityNo = c.Int(nullable: false),
                        PriorityName = c.String(),
                    })
                .PrimaryKey(t => t.PriorityId);
            
            CreateTable(
                "dbo.SLAValues",
                c => new
                    {
                        SLAValueId = c.Int(nullable: false, identity: true),
                        SLAValueName = c.String(),
                    })
                .PrimaryKey(t => t.SLAValueId);
            
            CreateTable(
                "dbo.DatabaseTypes",
                c => new
                    {
                        DatabaseTypeId = c.Int(nullable: false, identity: true),
                        DatabaseName = c.String(),
                    })
                .PrimaryKey(t => t.DatabaseTypeId);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DesignationName = c.String(),
                    })
                .PrimaryKey(t => t.DesignationId);
            
            CreateTable(
                "dbo.EnvironmentTypes",
                c => new
                    {
                        EnvironmentTypeId = c.Int(nullable: false, identity: true),
                        EnvironmentName = c.String(),
                    })
                .PrimaryKey(t => t.EnvironmentTypeId);
            
            CreateTable(
                "dbo.HardwareDescriptions",
                c => new
                    {
                        HardwareDescriptionId = c.Int(nullable: false, identity: true),
                        HardwareDescriptionName = c.String(),
                    })
                .PrimaryKey(t => t.HardwareDescriptionId);
            
            CreateTable(
                "dbo.VirtualizedPlatforms",
                c => new
                    {
                        VirtualizedPlatformId = c.Int(nullable: false, identity: true),
                        VirtualizedPlatformName = c.String(),
                    })
                .PrimaryKey(t => t.VirtualizedPlatformId);
            
            CreateTable(
                "dbo.CategorySubcategories",
                c => new
                    {
                        CategorySubCategoryId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        SubCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategorySubCategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.SubCategories", t => t.SubCategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.SubCategoryId);
            
            CreateTable(
                "dbo.ClientOnboardings",
                c => new
                    {
                        OnboardingId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        OfficeAddress = c.String(),
                        CountryId = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        PostalCode = c.Int(nullable: false),
                        PeriodFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PeriodTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OpretaionalHoursOpen = c.Time(nullable: false, precision: 7),
                        OpretationalHourseClose = c.Time(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        ClientAbbr = c.String(),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        DesignationId = c.Int(nullable: false),
                        EmployeeOfficeAddress = c.String(),
                    })
                .PrimaryKey(t => t.OnboardingId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Designations", t => t.DesignationId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.DesignationId);
            
            CreateTable(
                "dbo.DailySequentialNumbers",
                c => new
                    {
                        SeqNumberId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SequentialNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeqNumberId);
            
            CreateTable(
                "dbo.DeclinedRequests",
                c => new
                    {
                        OnboardingId = c.Int(nullable: false, identity: true),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        DesignationId = c.Int(nullable: false),
                        EmployeeOfficeAddress = c.String(),
                        ClientName = c.String(),
                        OfficeAddress = c.String(),
                        CountryId = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        PostalCode = c.Int(nullable: false),
                        PeriodFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PeriodTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OpretaionalHoursOpen = c.Time(nullable: false, precision: 7),
                        OpretationalHourseClose = c.Time(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        ClientAbbr = c.String(),
                    })
                .PrimaryKey(t => t.OnboardingId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientName = c.String(),
                        ClientEmail = c.String(),
                        OfficeAddress = c.String(),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CellNumber = c.String(),
                        DesignationId = c.Int(nullable: false),
                        EmployeeOfficeAddress = c.String(),
                        Active = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Designations", t => t.DesignationId, cascadeDelete: true)
                .Index(t => t.DesignationId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Incidents", "VirtualizedPlatformId", "dbo.VirtualizedPlatforms");
            DropForeignKey("dbo.Incidents", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.Incidents", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Incidents", "HardwareDescriptionId", "dbo.HardwareDescriptions");
            DropForeignKey("dbo.Incidents", "EnvironmentTypeId", "dbo.EnvironmentTypes");
            DropForeignKey("dbo.Incidents", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Incidents", "DatabaseTypeId", "dbo.DatabaseTypes");
            DropForeignKey("dbo.Incidents", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Incidents", "OnboardingId", "dbo.ApprovedRequests");
            DropForeignKey("dbo.DeclinedRequests", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ClientOnboardings", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.ClientOnboardings", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.CategorySubcategories", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.CategorySubcategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.SubCategories", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.SubCategories", "SLAValueId", "dbo.SLAValues");
            DropForeignKey("dbo.SubCategories", "PriorityId", "dbo.Priorities");
            DropForeignKey("dbo.ApprovedRequests", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ClientProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ClientProducts", "OnboardingId", "dbo.ApprovedRequests");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DesignationId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DeclinedRequests", new[] { "CountryId" });
            DropIndex("dbo.ClientOnboardings", new[] { "DesignationId" });
            DropIndex("dbo.ClientOnboardings", new[] { "CountryId" });
            DropIndex("dbo.CategorySubcategories", new[] { "SubCategoryId" });
            DropIndex("dbo.CategorySubcategories", new[] { "CategoryId" });
            DropIndex("dbo.SubCategories", new[] { "Category_CategoryId" });
            DropIndex("dbo.SubCategories", new[] { "SLAValueId" });
            DropIndex("dbo.SubCategories", new[] { "PriorityId" });
            DropIndex("dbo.ClientProducts", new[] { "ProductId" });
            DropIndex("dbo.ClientProducts", new[] { "OnboardingId" });
            DropIndex("dbo.ApprovedRequests", new[] { "CountryId" });
            DropIndex("dbo.Incidents", new[] { "DesignationId" });
            DropIndex("dbo.Incidents", new[] { "VirtualizedPlatformId" });
            DropIndex("dbo.Incidents", new[] { "EnvironmentTypeId" });
            DropIndex("dbo.Incidents", new[] { "HardwareDescriptionId" });
            DropIndex("dbo.Incidents", new[] { "DatabaseTypeId" });
            DropIndex("dbo.Incidents", new[] { "SubCategoryId" });
            DropIndex("dbo.Incidents", new[] { "CategoryId" });
            DropIndex("dbo.Incidents", new[] { "ProductId" });
            DropIndex("dbo.Incidents", new[] { "OnboardingId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DeclinedRequests");
            DropTable("dbo.DailySequentialNumbers");
            DropTable("dbo.ClientOnboardings");
            DropTable("dbo.CategorySubcategories");
            DropTable("dbo.VirtualizedPlatforms");
            DropTable("dbo.HardwareDescriptions");
            DropTable("dbo.EnvironmentTypes");
            DropTable("dbo.Designations");
            DropTable("dbo.DatabaseTypes");
            DropTable("dbo.SLAValues");
            DropTable("dbo.Priorities");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Countries");
            DropTable("dbo.Products");
            DropTable("dbo.ClientProducts");
            DropTable("dbo.ApprovedRequests");
            DropTable("dbo.Incidents");
        }
    }
}
