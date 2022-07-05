using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TravelAgency
{
    public partial class Rieltors_EvsyutinContext : DbContext
    {
        public Rieltors_EvsyutinContext()
        {
        }

        public Rieltors_EvsyutinContext(DbContextOptions<Rieltors_EvsyutinContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<RestType> RestTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Stuff> Stuffs { get; set; }
        public virtual DbSet<StuffInfo> StuffInfos { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VouchersInfo> VouchersInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=169.254.131.3; Initial Catalog=Rieltors_Evsyutin; Persist Security Info=True; User Id=stud; Password=Qwerty123456$");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AdditionalService>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PassportCode).HasMaxLength(20);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<RestType>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Salary).HasMaxLength(50);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Stuff>(entity =>
            {
                entity.ToTable("Stuff");

                entity.HasIndex(e => e.Phone, "UQ__Stuff__5C7E359EB556E8EA")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Stuffs)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Stuff__RoleId__276EDEB3");
            });

            modelBuilder.Entity<StuffInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("StuffInfo");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Role).HasMaxLength(100);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.BookingStatus).HasMaxLength(20);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.PaymentStatus).HasMaxLength(20);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.AdditService1)
                    .WithMany(p => p.VoucherAdditService1s)
                    .HasForeignKey(d => d.AdditService1Id)
                    .HasConstraintName("FK__Vouchers__AdditS__36B12243");

                entity.HasOne(d => d.AdditService2)
                    .WithMany(p => p.VoucherAdditService2s)
                    .HasForeignKey(d => d.AdditService2Id)
                    .HasConstraintName("FK__Vouchers__AdditS__37A5467C");

                entity.HasOne(d => d.AdditService3)
                    .WithMany(p => p.VoucherAdditService3s)
                    .HasForeignKey(d => d.AdditService3Id)
                    .HasConstraintName("FK__Vouchers__AdditS__38996AB5");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__Vouchers__Client__398D8EEE");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__Vouchers__HotelI__3A81B327");

                entity.HasOne(d => d.RestType)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.RestTypeId)
                    .HasConstraintName("FK__Vouchers__RestTy__3B75D760");

                entity.HasOne(d => d.Stuff)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.StuffId)
                    .HasConstraintName("FK__Vouchers__StuffI__31EC6D26");
            });

            modelBuilder.Entity<VouchersInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VouchersInfo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
