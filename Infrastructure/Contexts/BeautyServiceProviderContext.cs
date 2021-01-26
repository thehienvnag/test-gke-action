﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Contexts
{
    public partial class BeautyServiceProviderContext : DbContext
    {
        public BeautyServiceProviderContext()
        {
        }

        public BeautyServiceProviderContext(DbContextOptions<BeautyServiceProviderContext> options)
            : base(options)
        {
        }
        public async Task<bool> Commit()
        {
            int result = await base.SaveChangesAsync();
            return result > 0;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountInSalon> AccountInSalons { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingActivity> BookingActivities { get; set; }
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceInCombo> ServiceInCombos { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=.;Database=BeautyServiceProvider;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "UQ__Account__A9D10534A113C252")
                    .IsUnique();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefaultAddress)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.DefaultAddressId)
                    .HasConstraintName("FK_Account_Address");

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.GalleryId)
                    .HasConstraintName("FK_Account_Gallery");
            });

            modelBuilder.Entity<AccountInSalon>(entity =>
            {
                entity.ToTable("AccountInSalon");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.AccountInSalonMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInSalon_Account1");

                entity.HasOne(d => d.SalonOwner)
                    .WithMany(p => p.AccountInSalonSalonOwners)
                    .HasForeignKey(d => d.SalonOwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInSalon_Account");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Address__Account__2F10007B");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FeedbackContent).HasMaxLength(200);

                entity.Property(e => e.Note).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Address");

                entity.HasOne(d => d.CustomerAccount)
                    .WithMany(p => p.BookingCustomerAccounts)
                    .HasForeignKey(d => d.CustomerAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__Custome__35BCFE0A");

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.GalleryId)
                    .HasConstraintName("FK_Booking_Gallery");

                entity.HasOne(d => d.SalonMemberAccount)
                    .WithMany(p => p.BookingSalonMemberAccounts)
                    .HasForeignKey(d => d.SalonMemberAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__SalonMe__36B12243");
            });

            modelBuilder.Entity<BookingActivity>(entity =>
            {
                entity.ToTable("BookingActivity");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingActivities)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookingActivity_Booking");
            });

            modelBuilder.Entity<BookingDetail>(entity =>
            {
                entity.ToTable("BookingDetail");

                entity.Property(e => e.Price)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookingDe__Booki__3D5E1FD2");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookingDe__Servi__3E52440B");
            });

            modelBuilder.Entity<FeedBack>(entity =>
            {
                entity.ToTable("FeedBack");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedBack__Bookin__412EB0B6");
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("Gallery");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShareSetting)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.DefaultImage)
                    .WithMany(p => p.Galleries)
                    .HasForeignKey(d => d.DefaultImageId)
                    .HasConstraintName("FK_Gallery_Image");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShareSetting)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.GalleryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Gallery");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Account");

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.GalleryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Gallery");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Service__Categor__32E0915F");
            });

            modelBuilder.Entity<ServiceInCombo>(entity =>
            {
                entity.ToTable("ServiceInCombo");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.ServiceCombo)
                    .WithMany(p => p.ServiceInComboServiceCombos)
                    .HasForeignKey(d => d.ServiceComboId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInCombo_Service");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceInComboServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInCombo_Service1");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
