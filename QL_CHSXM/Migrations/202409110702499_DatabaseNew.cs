namespace QL_CHSXM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_BookService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 150),
                        Phone = c.String(nullable: false, maxLength: 12),
                        Email = c.String(nullable: false, maxLength: 150),
                        NameCar = c.String(nullable: false, maxLength: 150),
                        VisitCount = c.Int(nullable: false),
                        Note = c.String(maxLength: 150),
                        ServiceTitle = c.String(),
                        ServiceCategoryId = c.String(),
                        TypePayment = c.Int(nullable: false),
                        ProductCategoryId = c.String(),
                        ServiceId = c.Int(),
                        ProductId = c.Int(),
                        BookingDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                        Product_Id = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.Product_Id)
                .ForeignKey("dbo.tb_Service", t => t.Service_Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId)
                .ForeignKey("dbo.tb_Service", t => t.ServiceId)
                .Index(t => t.ServiceId)
                .Index(t => t.ProductId)
                .Index(t => t.Product_Id)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.tb_OrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BookService_Id = c.Int(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_BookService", t => t.BookService_Id)
                .ForeignKey("dbo.tb_Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.tb_Service", t => t.Service_Id)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId)
                .Index(t => t.BookService_Id)
                .Index(t => t.Service_Id);
            
            CreateTable(
                "dbo.tb_Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        CustomerName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 11),
                        Address = c.String(nullable: false, maxLength: 250),
                        Email = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        TypePayment = c.Int(nullable: false),
                        PaymentDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(maxLength: 250),
                        ProductCode = c.String(maxLength: 50),
                        Description = c.String(),
                        Detail = c.String(),
                        Image = c.String(maxLength: 250),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceSale = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                        BookService_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_ProductCategory", t => t.ProductCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.tb_BookService", t => t.BookService_Id)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.BookService_Id);
            
            CreateTable(
                "dbo.tb_ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 250),
                        Icon = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_ProductImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Image = c.String(maxLength: 250),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.tb_Service",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Alias = c.String(maxLength: 250),
                        ServiceCode = c.String(maxLength: 50),
                        Description = c.String(maxLength: 250),
                        Detail = c.String(maxLength: 250),
                        Image = c.String(maxLength: 250),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceSale = c.Decimal(precision: 18, scale: 2),
                        ViewCount = c.Int(nullable: false),
                        ServiceCategoryId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                        BookService_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_ServiceCategory", t => t.ServiceCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.tb_BookService", t => t.BookService_Id)
                .Index(t => t.ServiceCategoryId)
                .Index(t => t.BookService_Id);
            
            CreateTable(
                "dbo.tb_ServiceCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(nullable: false, maxLength: 150),
                        Icon = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ServiceCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_ServiceImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        Image = c.String(maxLength: 250),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Service", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.tb_Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(maxLength: 250),
                        Link = c.String(maxLength: 150),
                        Description = c.String(maxLength: 150),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(),
                        Description = c.String(),
                        Detail = c.String(),
                        Image = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.tb_Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Email = c.String(maxLength: 150),
                        Website = c.String(maxLength: 250),
                        Message = c.String(maxLength: 4000),
                        CreatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Modifiedby = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.tb_Subscribe",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tb_SystemSetting",
                c => new
                    {
                        SettingKey = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(maxLength: 4000),
                        SettingDescription = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.SettingKey);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Phone = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
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
            
            CreateTable(
                "dbo.ProductCategoryBookServices",
                c => new
                    {
                        ProductCategory_Id = c.Int(nullable: false),
                        BookService_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductCategory_Id, t.BookService_Id })
                .ForeignKey("dbo.tb_ProductCategory", t => t.ProductCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.tb_BookService", t => t.BookService_Id, cascadeDelete: true)
                .Index(t => t.ProductCategory_Id)
                .Index(t => t.BookService_Id);
            
            CreateTable(
                "dbo.ServiceCategoryBookServices",
                c => new
                    {
                        ServiceCategory_Id = c.Int(nullable: false),
                        BookService_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceCategory_Id, t.BookService_Id })
                .ForeignKey("dbo.tb_ServiceCategory", t => t.ServiceCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.tb_BookService", t => t.BookService_Id, cascadeDelete: true)
                .Index(t => t.ServiceCategory_Id)
                .Index(t => t.BookService_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.tb_News", "CategoryId", "dbo.tb_Category");
            DropForeignKey("dbo.tb_Service", "BookService_Id", "dbo.tb_BookService");
            DropForeignKey("dbo.tb_BookService", "ServiceId", "dbo.tb_Service");
            DropForeignKey("dbo.tb_Product", "BookService_Id", "dbo.tb_BookService");
            DropForeignKey("dbo.tb_BookService", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_ServiceImage", "ServiceId", "dbo.tb_Service");
            DropForeignKey("dbo.tb_Service", "ServiceCategoryId", "dbo.tb_ServiceCategory");
            DropForeignKey("dbo.ServiceCategoryBookServices", "BookService_Id", "dbo.tb_BookService");
            DropForeignKey("dbo.ServiceCategoryBookServices", "ServiceCategory_Id", "dbo.tb_ServiceCategory");
            DropForeignKey("dbo.tb_OrderDetail", "Service_Id", "dbo.tb_Service");
            DropForeignKey("dbo.tb_BookService", "Service_Id", "dbo.tb_Service");
            DropForeignKey("dbo.tb_ProductImage", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_Product", "ProductCategoryId", "dbo.tb_ProductCategory");
            DropForeignKey("dbo.ProductCategoryBookServices", "BookService_Id", "dbo.tb_BookService");
            DropForeignKey("dbo.ProductCategoryBookServices", "ProductCategory_Id", "dbo.tb_ProductCategory");
            DropForeignKey("dbo.tb_OrderDetail", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_BookService", "Product_Id", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderDetail", "OrderId", "dbo.tb_Order");
            DropForeignKey("dbo.tb_OrderDetail", "BookService_Id", "dbo.tb_BookService");
            DropIndex("dbo.ServiceCategoryBookServices", new[] { "BookService_Id" });
            DropIndex("dbo.ServiceCategoryBookServices", new[] { "ServiceCategory_Id" });
            DropIndex("dbo.ProductCategoryBookServices", new[] { "BookService_Id" });
            DropIndex("dbo.ProductCategoryBookServices", new[] { "ProductCategory_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.tb_News", new[] { "CategoryId" });
            DropIndex("dbo.tb_ServiceImage", new[] { "ServiceId" });
            DropIndex("dbo.tb_Service", new[] { "BookService_Id" });
            DropIndex("dbo.tb_Service", new[] { "ServiceCategoryId" });
            DropIndex("dbo.tb_ProductImage", new[] { "ProductId" });
            DropIndex("dbo.tb_Product", new[] { "BookService_Id" });
            DropIndex("dbo.tb_Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.tb_OrderDetail", new[] { "Service_Id" });
            DropIndex("dbo.tb_OrderDetail", new[] { "BookService_Id" });
            DropIndex("dbo.tb_OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.tb_OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.tb_BookService", new[] { "Service_Id" });
            DropIndex("dbo.tb_BookService", new[] { "Product_Id" });
            DropIndex("dbo.tb_BookService", new[] { "ProductId" });
            DropIndex("dbo.tb_BookService", new[] { "ServiceId" });
            DropTable("dbo.ServiceCategoryBookServices");
            DropTable("dbo.ProductCategoryBookServices");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.tb_SystemSetting");
            DropTable("dbo.tb_Subscribe");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.tb_Contact");
            DropTable("dbo.tb_News");
            DropTable("dbo.tb_Category");
            DropTable("dbo.tb_ServiceImage");
            DropTable("dbo.tb_ServiceCategory");
            DropTable("dbo.tb_Service");
            DropTable("dbo.tb_ProductImage");
            DropTable("dbo.tb_ProductCategory");
            DropTable("dbo.tb_Product");
            DropTable("dbo.tb_Order");
            DropTable("dbo.tb_OrderDetail");
            DropTable("dbo.tb_BookService");
        }
    }
}
