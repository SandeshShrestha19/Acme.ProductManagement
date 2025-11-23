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
using Acme.ProductManagement.Order;
using Acme.ProductManagement.Inventories;

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
    public DbSet<Orders.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

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

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ProductManagementConsts.DbTablePrefix + "YourEntities", ProductManagementConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<Product>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Products", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
        });
        builder.Entity<Category>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Categories", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(512);
        });
        builder.Entity<Customer>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Customers", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);

        });
        builder.Entity<Orders.Order>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Orders", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OrderDate).IsRequired();
            b.Property(x => x.OrderStatus).IsRequired();
        });
        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "OrderItems", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
        });
        builder.Entity<Inventory>(b =>
        {
            b.ToTable(ProductManagementConsts.DbTablePrefix + "Inventories", ProductManagementConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
