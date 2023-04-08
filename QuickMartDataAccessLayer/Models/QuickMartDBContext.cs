using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;


#nullable disable

namespace QuickMartDataAccessLayer.Models
{
    public partial class QuickMartDBContext : DbContext
    {
        public QuickMartDBContext()
        {           
        }

        public QuickMartDBContext(DbContextOptions<QuickMartDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CardDetails> CardDetails { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<PurchaseDetails> PurchaseDetails { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=SAHANE-PC\\SQLEXPRESS;Database=QuickMartDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
                //optionsBuilder.UseSqlServer("Server=tcp:hxs0uxo-mssqlserver.database.windows.net,1433;Initial Catalog=QuickMart;Persist Security Info=False;User ID=hxs0uxo;Password=quickmart@2023;MultipleActiveResultSets=False;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardDetails>(entity =>
            {
                entity.HasKey(e => e.CardNumber)
                    .HasName("pk_CardNumber");

                entity.Property(e => e.CardNumber).HasColumnType("numeric(16, 0)");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CardType)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Cvvnumber)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("CVVNumber");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.NameOnCard)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasIndex(e => e.CategoryName, "uq_CategoryName")
                    .IsUnique();

                entity.HasKey(e => e.CategoryId).HasName("pk_CategoryId");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "ix_CategoryId");

                entity.HasIndex(e => e.ProductName, "uq_ProductName")
                    .IsUnique();

                entity.HasKey(e => e.ProductId).HasName("pk_ProductId");
                    //.HasMaxLength(4)
                    //.IsUnicode(false)
                    //.IsFixedLength(true);

                entity.Property(e => e.Price).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_CategoryId");
            });

            modelBuilder.Entity<PurchaseDetails>(entity =>
            {
                entity.HasKey(e => e.PurchaseId)
                    .HasName("pk_PurchaseId");

                entity.HasIndex(e => e.EmailId, "ix_EmailId");

                entity.HasIndex(e => e.ProductId, "ix_ProductId");

                entity.Property(e => e.DateOfPurchase)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Email)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.EmailId)
                    .HasConstraintName("fk_EmailId");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_ProductId");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasIndex(e => e.RoleName, "uq_RoleName")
                    .IsUnique();

                entity.HasKey(e => e.RoleId).HasName("pk_RoleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.EmailId)
                    .HasName("pk_EmailId");

                entity.HasIndex(e => e.RoleId, "ix_RoleId");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("fk_RoleId");
            });
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                     .HasName("pk_CartId");

                entity.Property(e => e.DateOfPurchase)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Items).IsUnicode(false);

                entity.HasOne(d => d.Email)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.EmailId)
                    .HasConstraintName("fk_CartEmailId");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FinalItems).IsUnicode(false);

                entity.Property(e => e.OrderDate)
                            .HasColumnType("datetime")
                            .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Email)
                            .WithMany(p => p.Orders)
                            .HasForeignKey(d => d.EmailId)
                            .HasConstraintName("fk_OrdersEmailId");
            });
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("pk_FeedbackId");

                entity.ToTable("Feedback");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .IsUnicode(false);
                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Email).WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EmailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_FeedbackEmailId");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
