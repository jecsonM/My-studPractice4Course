//using Microsoft.EntityFrameworkCore;
//using WpfLaundrySystemApp.Models;

//namespace WpfLaundrySystemApp.DBContext
//{
//    public class LaundryDbContext : DbContext
//    {
//        //public DbSet<User> Users { get; set; }
//        public LaundryDbContext()
//        {
//            Database.EnsureCreated();
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=laundryStudPractice4Course;Username=postgres;Password=POSTGREmoiseiev;SSL Mode=Prefer");
//        }
//    }
//}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using WpfLaundrySystemApp.Models;

namespace WpfLaundrySystemApp.DBContext;

public partial class LaundryDbContext : DbContext
{
    public LaundryDbContext()
    {
    }

    public LaundryDbContext(DbContextOptions<LaundryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendedService> AttendedServices { get; set; }

    public virtual DbSet<BranchPoint> BranchPoints { get; set; }

    public virtual DbSet<Consumable> Consumables { get; set; }

    public virtual DbSet<ConsumableMovementType> ConsumableMovementTypes { get; set; }

    public virtual DbSet<ConsumableSuppliement> ConsumableSuppliements { get; set; }

    public virtual DbSet<ConsumableType> ConsumableTypes { get; set; }

    public virtual DbSet<ConsumableUse> ConsumableUses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeHealthState> EmployeeHealthStates { get; set; }

    public virtual DbSet<EmployeeMovement> EmployeeMovements { get; set; }

    public virtual DbSet<HealthState> HealthStates { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<PartnerType> PartnerTypes { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PriceList> PriceLists { get; set; }

    public virtual DbSet<Raiting> Raitings { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceConsumableNeed> ServiceConsumableNeeds { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<ServiceWorkNeed> ServiceWorkNeeds { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierType> SupplierTypes { get; set; }

    public virtual DbSet<UnitType> UnitTypes { get; set; }

    public virtual DbSet<Workshop> Workshops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=laundryStudPractice4Course;Username=postgres;Password=POSTGREmoiseiev;SSL Mode=Prefer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendedService>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.OrderId }).HasName("AttendedServices_pkey");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.AwaitedDateOfAttendance)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("awaitedDateOfAttendance");
            entity.Property(e => e.ConsumableUseId).HasColumnName("consumableUseID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.PriceListId).HasColumnName("priceListID");
            entity.Property(e => e.WorkshopId).HasColumnName("workshopID");

            entity.HasOne(d => d.ConsumableUse).WithMany(p => p.AttendedServices)
                .HasForeignKey(d => d.ConsumableUseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumableUses");

            entity.HasOne(d => d.Order).WithMany(p => p.AttendedServices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders");

            entity.HasOne(d => d.PriceList).WithMany(p => p.AttendedServices)
                .HasForeignKey(d => d.PriceListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PriceLists");

            entity.HasOne(d => d.Service).WithMany(p => p.AttendedServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services");

            entity.HasOne(d => d.Workshop).WithMany(p => p.AttendedServices)
                .HasForeignKey(d => d.WorkshopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workshops");
        });

        modelBuilder.Entity<BranchPoint>(entity =>
        {
            entity.HasKey(e => e.BranchPointId).HasName("BranchPoints_pkey");

            entity.Property(e => e.BranchPointId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("branchPointID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.PartnerId).HasColumnName("partnerID");

            entity.HasOne(d => d.Partner).WithMany(p => p.BranchPoints)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partners");
        });

        modelBuilder.Entity<Consumable>(entity =>
        {
            entity.HasKey(e => e.ConsumableId).HasName("Consumables_pkey");

            entity.Property(e => e.ConsumableId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consumableID");
            entity.Property(e => e.AmountInOneUnit).HasColumnName("amountInOneUnit");
            entity.Property(e => e.ConsumableDescription)
                .HasMaxLength(255)
                .HasColumnName("consumableDescription");
            entity.Property(e => e.ConsumableImage).HasColumnName("consumableImage");
            entity.Property(e => e.ConsumableName)
                .HasMaxLength(255)
                .HasColumnName("consumableName");
            entity.Property(e => e.ConsumableTypeId).HasColumnName("consumableTypeID");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.UnitTypeId).HasColumnName("unitTypeID");

            entity.HasOne(d => d.ConsumableType).WithMany(p => p.Consumables)
                .HasForeignKey(d => d.ConsumableTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumableTypes");

            entity.HasOne(d => d.UnitType).WithMany(p => p.Consumables)
                .HasForeignKey(d => d.UnitTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitTypes");
        });

        modelBuilder.Entity<ConsumableMovementType>(entity =>
        {
            entity.HasKey(e => e.ConsumableMovementTypeId).HasName("ConsumableMovementTypes_pkey");

            entity.Property(e => e.ConsumableMovementTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consumableMovementTypeID");
            entity.Property(e => e.ConsumableMovementTypeName)
                .HasMaxLength(255)
                .HasColumnName("consumableMovementTypeName");
        });

        modelBuilder.Entity<ConsumableSuppliement>(entity =>
        {
            entity.HasKey(e => e.ConsumableSuppliementId).HasName("ConsumableSuppliements_pkey");

            entity.Property(e => e.ConsumableSuppliementId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consumableSuppliementID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ConsumableId).HasColumnName("consumableID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.SupplierId).HasColumnName("supplierID");

            entity.HasOne(d => d.Consumable).WithMany(p => p.ConsumableSuppliements)
                .HasForeignKey(d => d.ConsumableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consumables");

            entity.HasOne(d => d.Supplier).WithMany(p => p.ConsumableSuppliements)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers");
        });

        modelBuilder.Entity<ConsumableType>(entity =>
        {
            entity.HasKey(e => e.ConsumableTypeId).HasName("ConsumableTypes_pkey");

            entity.Property(e => e.ConsumableTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consumableTypeID");
            entity.Property(e => e.ConsumableTypeName)
                .HasMaxLength(255)
                .HasColumnName("consumableTypeName");
        });

        modelBuilder.Entity<ConsumableUse>(entity =>
        {
            entity.HasKey(e => e.ConsumableUseId).HasName("ConsumableUses_pkey");

            entity.Property(e => e.ConsumableUseId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consumableUseID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ConsumableId).HasColumnName("consumableID");
            entity.Property(e => e.ConsumableMovementTypeId).HasColumnName("consumableMovementTypeID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");

            entity.HasOne(d => d.Consumable).WithMany(p => p.ConsumableUses)
                .HasForeignKey(d => d.ConsumableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consumables");

            entity.HasOne(d => d.ConsumableMovementType).WithMany(p => p.ConsumableUses)
                .HasForeignKey(d => d.ConsumableMovementTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumableMovementTypes");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("Employees_pkey");

            entity.Property(e => e.EmployeeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("employeeID");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.DateOfPassportIssue).HasColumnName("dateOfPassportIssue");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("passportNumber");
            entity.Property(e => e.PassportSeries)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("passportSeries");
            entity.Property(e => e.PositionId).HasColumnName("positionID");
            entity.Property(e => e.WhoIssuedPassport)
                .HasMaxLength(255)
                .HasColumnName("whoIssuedPassport");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions");
        });

        modelBuilder.Entity<EmployeeHealthState>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.HealthStateId }).HasName("EmployeeHealthStates_pkey");

            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.HealthStateId).HasColumnName("healthStateID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeHealthStates)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees");

            entity.HasOne(d => d.HealthState).WithMany(p => p.EmployeeHealthStates)
                .HasForeignKey(d => d.HealthStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HealthStates");
        });

        modelBuilder.Entity<EmployeeMovement>(entity =>
        {
            entity.HasKey(e => new { e.WorkshopId, e.EmployeeId }).HasName("EmployeeMovements_pkey");

            entity.Property(e => e.WorkshopId).HasColumnName("workshopID");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeMovements)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees");

            entity.HasOne(d => d.Workshop).WithMany(p => p.EmployeeMovements)
                .HasForeignKey(d => d.WorkshopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workshops");
        });

        modelBuilder.Entity<HealthState>(entity =>
        {
            entity.HasKey(e => e.HealthStateId).HasName("HealthStates_pkey");

            entity.Property(e => e.HealthStateId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("healthStateID");
            entity.Property(e => e.HealthStateDescription)
                .HasMaxLength(255)
                .HasColumnName("healthStateDescription");
            entity.Property(e => e.HealthStateName)
                .HasMaxLength(255)
                .HasColumnName("healthStateName");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Orders_pkey");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("orderID");
            entity.Property(e => e.AwaitedDateOfAttendance)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("awaitedDateOfAttendance");
            entity.Property(e => e.DateOfAttendance)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfAttendance");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.DateOfPaycheck)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfPaycheck");
            entity.Property(e => e.IsCancelled)
                .HasDefaultValue(false)
                .HasColumnName("isCancelled");
            entity.Property(e => e.PartnerId).HasColumnName("partnerID");

            entity.HasOne(d => d.Partner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partners");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("Partners_pkey");

            entity.Property(e => e.PartnerId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("partnerID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Inn)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("INN");
            entity.Property(e => e.LegalAddress)
                .HasMaxLength(255)
                .HasColumnName("legalAddress");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.PartnerName)
                .HasMaxLength(255)
                .HasColumnName("partnerName");
            entity.Property(e => e.PartnerTypeId).HasColumnName("partnerTypeId");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");

            entity.HasOne(d => d.PartnerType).WithMany(p => p.Partners)
                .HasForeignKey(d => d.PartnerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartnerTypes");
        });

        modelBuilder.Entity<PartnerType>(entity =>
        {
            entity.HasKey(e => e.PartnerTypeId).HasName("PartnerTypes_pkey");

            entity.Property(e => e.PartnerTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("partnerTypeId");
            entity.Property(e => e.PartnerTypeName)
                .HasMaxLength(255)
                .HasColumnName("partnerTypeName");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("Positions_pkey");

            entity.Property(e => e.PositionId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("positionID");
            entity.Property(e => e.AverageHourWage)
                .HasColumnType("money")
                .HasColumnName("averageHourWage");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("positionName");
        });

        modelBuilder.Entity<PriceList>(entity =>
        {
            entity.HasKey(e => e.PriceListId).HasName("PriceLists_pkey");

            entity.Property(e => e.PriceListId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("priceListID");
            entity.Property(e => e.DateOfCreation)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("dateOfCreation");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");

            entity.HasOne(d => d.Service).WithMany(p => p.PriceLists)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services");
        });

        modelBuilder.Entity<Raiting>(entity =>
        {
            entity.HasKey(e => e.RaitingId).HasName("Raitings_pkey");

            entity.Property(e => e.RaitingId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("raitingID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.PartnerId).HasColumnName("partnerID");
            entity.Property(e => e.Raiting1).HasColumnName("raiting");

            entity.HasOne(d => d.Partner).WithMany(p => p.Raitings)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partners");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("Services_pkey");

            entity.Property(e => e.ServiceId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("serviceID");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.ExecutionTime).HasColumnName("executionTime");
            entity.Property(e => e.MinimalCost)
                .HasColumnType("money")
                .HasColumnName("minimalCost");
            entity.Property(e => e.ServiceDescription)
                .HasMaxLength(255)
                .HasColumnName("serviceDescription");
            entity.Property(e => e.ServiceImage).HasColumnName("serviceImage");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("serviceName");
            entity.Property(e => e.ServiceTypeId).HasColumnName("serviceTypeID");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceTypes");
        });

        modelBuilder.Entity<ServiceConsumableNeed>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.ConsumableId }).HasName("ServiceConsumableNeeds_pkey");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.ConsumableId).HasColumnName("consumableID");
            entity.Property(e => e.ConsumableUseAmount).HasColumnName("consumableUseAmount");

            entity.HasOne(d => d.Consumable).WithMany(p => p.ServiceConsumableNeeds)
                .HasForeignKey(d => d.ConsumableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consumables");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceConsumableNeeds)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("ServiceTypes_pkey");

            entity.Property(e => e.ServiceTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("serviceTypeID");
            entity.Property(e => e.ServiceTypeName)
                .HasMaxLength(255)
                .HasColumnName("serviceTypeName");
        });

        modelBuilder.Entity<ServiceWorkNeed>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.PositionId }).HasName("ServiceWorkNeeds_pkey");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.PositionId).HasColumnName("positionID");
            entity.Property(e => e.OveralWorkTime).HasColumnName("overalWorkTime");

            entity.HasOne(d => d.Position).WithMany(p => p.ServiceWorkNeeds)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceWorkNeeds)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("Suppliers_pkey");

            entity.Property(e => e.SupplierId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("supplierID");
            entity.Property(e => e.Inn)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("INN");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(255)
                .HasColumnName("supplierName");
            entity.Property(e => e.SupplierTypeId).HasColumnName("supplierTypeID");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierTypes");
        });

        modelBuilder.Entity<SupplierType>(entity =>
        {
            entity.HasKey(e => e.SupplierTypeId).HasName("SupplierTypes_pkey");

            entity.Property(e => e.SupplierTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("supplierTypeID");
            entity.Property(e => e.SupplierTypeName)
                .HasMaxLength(255)
                .HasColumnName("supplierTypeName");
        });

        modelBuilder.Entity<UnitType>(entity =>
        {
            entity.HasKey(e => e.UnitTypeId).HasName("UnitTypes_pkey");

            entity.Property(e => e.UnitTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("unitTypeID");
            entity.Property(e => e.UnitTypeName)
                .HasMaxLength(255)
                .HasColumnName("unitTypeName");
        });

        modelBuilder.Entity<Workshop>(entity =>
        {
            entity.HasKey(e => e.WorkshopId).HasName("Workshops_pkey");

            entity.Property(e => e.WorkshopId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("workshopID");
            entity.Property(e => e.WorkShopName)
                .HasMaxLength(255)
                .HasColumnName("workShopName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
