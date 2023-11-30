using Microsoft.EntityFrameworkCore;
using DropShipping.Models;

namespace DropShipping.Data;


public class DropshippingContext: DbContext
{
    public DropshippingContext(DbContextOptions<DropshippingContext> options): base(options)
    {

    }

    public DbSet<Product> Products { get; set;} = null!;

    public DbSet<Item> Items { get; set;} = null!;
    
    public DbSet<Order> Orders { get; set;} = null!;
    
    public DbSet<User> Users { get; set;} = null!;
    
    public DbSet<UserData> UsersData { get; set;} = null!;
    
    public DbSet<UserAddress> UsersAddress { get; set;} = null!;
    
    public DbSet<UserRole> UsersRole { get; set;} = null!;
    public DbSet<Role> Roles { get; set;} = null!;
    
    public DbSet<PaymentStatus> Payments { get; set;} = null!;

    public DbSet<ShipmentState> ShipmentStates { get; set;} = null!;

    public DbSet<ShipmentAgency> ShipmentAgencies { get; set;} = null!;

    public DbSet<Office> Offices { get; set;} = null!;

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Transport> Transports {get;set;}
    public DbSet<Link> Links {get;set;}
    public DbSet<Image> Images {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        // PRODUCT done
        modelBuilder.Entity<Product>()
            .HasMany(p  => p.Items)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Link>()
            .HasOne<Product>(l => l.Product)
            .WithOne(p => p.Link)
            .HasForeignKey<Link>(l => l.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
            
        modelBuilder.Entity<Image>()
            .HasOne<Product>(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
        
            
        // ORDER done 
        modelBuilder.Entity<Order>()
            .HasMany<Item>(o  => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // TRANSPORT
        modelBuilder.Entity<Transport>()
            .HasOne<Order>(t => t.Order)
            .WithOne(o => o.Transport)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Office>()
            .HasMany<Transport>(o => o.Transports)
            .WithOne(t => t.Office)
            .HasForeignKey(t => t.OfficeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Office>()
            .HasIndex(o => o.Name)
            .IsUnique();


        //USER done 
        modelBuilder.Entity<User>()
            .HasMany<Order>(u => u.Orders)
            .WithOne(o => o.User)
            .OnDelete(DeleteBehavior.Cascade);
        
        //USER_DATA done 
        modelBuilder.Entity<UserData>()
            .HasOne<User>(ud => ud.User);
        
        modelBuilder.Entity<UserData>()
            .HasIndex(ud => ud.Email)
            .IsUnique();
        
        modelBuilder.Entity<UserData>()
            .HasIndex(ud => ud.Name)
            .IsUnique();

        modelBuilder.Entity<UserData>()
            .HasIndex(ud => ud.IdentificationDocument)
            .IsUnique();
           
        //USER_ADDRESS done
        modelBuilder.Entity<UserAddress>()
            .HasOne<User>(ua => ua.User)
            .WithOne(u => u.UserAddress)
            .HasForeignKey<UserAddress>(x => x.UserId);

        //USER_ROLE done
        modelBuilder.Entity<UserRole>()
            .HasOne<User>(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(u => u.UserId);

        //ROLES done 
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasConversion<string>();
            
        modelBuilder.Entity<Role>()
            .HasMany<UserRole>(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId);

        //PAYMENTS done
        modelBuilder.Entity<PaymentStatus>()
            .HasOne<Order>(p => p.Order)
            .WithOne(o => o.PaymentStatus);
            
        modelBuilder.Entity<PaymentStatus>()
            .Property(p => p.PaymentMethod)
            .HasConversion<string>();
        
        //SHIPMENT_STATE done 
        modelBuilder.Entity<ShipmentState>()
            .HasMany(ss => ss.Orders)
            .WithOne(o => o.ShipmentState);

        modelBuilder.Entity<ShipmentState>()
            .Property(s => s.ShipmentStatus)
            .HasConversion<string>();
        
        modelBuilder.Entity<ShipmentState>()
            .HasMany(p  => p.Orders)
            .WithOne(i => i.ShipmentState)
            .HasForeignKey(i => i.ShipmentStateId)
            .OnDelete(DeleteBehavior.NoAction);

        //SHIPMENT_AGENCY
        modelBuilder.Entity<ShipmentAgency>()
            .HasMany<ShipmentState>(sa => sa.ShipmentStates)
            .WithOne(ss => ss.ShipmentAgency)
            .HasForeignKey(ss => ss.ShipmentAgencyId);

        modelBuilder.Entity<ShipmentAgency>()
            .HasIndex(sa => sa.Name)
            .IsUnique();

        // CITY
        modelBuilder.Entity<City>()
            .HasMany<Office>(c => c.Offices)
            .WithOne(o => o.City)
            .HasForeignKey(o => o.CityId);
        
        /* done = city, item, product, order, user, u.data, u.dir, u.roles, roles, payments, 
        shipment_state, shipment_agency, office, transport
        */ 
	}
}