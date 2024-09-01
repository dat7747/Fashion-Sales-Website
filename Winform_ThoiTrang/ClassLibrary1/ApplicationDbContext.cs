using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPham { get; set; }
        public DbSet<KhachHang> khachHangs { get; set; }
        public DbSet<CartItem> CartItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPham>()
                .HasKey(p => p.SanPhamID);

            modelBuilder.Entity<SanPham>()
                .Property(p => p.SanPhamID)
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartItemID);

            modelBuilder.Entity<CartItem>()
                .Property(c => c.CartItemID)
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.SanPham)
                .WithMany()
                .HasForeignKey(c => c.SanPhamID);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.KhachHang)
                .WithMany()
                .HasForeignKey(c => c.KhachHangID);
        }

    }
}
