using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Acme.ProductManagement.Products;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.OrderItems;

namespace Acme.ProductManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ProductManagementDbContext :
    AbpDbContext<ProductManagementDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    #region Entities from the modules

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        // Configure Product
        builder.Entity<Product>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Products", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");

            // Product-Category relationship
            b.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Category
        builder.Entity<Category>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Categories", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(512);
        });

        // Configure Customer
        builder.Entity<Customer>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Customers", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);

            // Customer-Order relationship (One-to-Many)
            b.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Order
        builder.Entity<Order>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Orders", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OrderDate).IsRequired();
            b.Property(x => x.OrderStatus).IsRequired();

            // Order-Customer relationship
            b.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure OrderItem
        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "OrderItems", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Quantity).IsRequired();
            b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

            // OrderItem-Order relationship
            // Just configure it simply - EF Core will figure out the backing field
            b.HasOne(oi => oi.Order)
                .WithMany() // Leave empty - EF Core will use convention
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // OrderItem-Product relationship
            b.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}