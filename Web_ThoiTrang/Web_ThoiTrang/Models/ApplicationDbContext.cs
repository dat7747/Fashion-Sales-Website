using Microsoft.EntityFrameworkCore;

namespace Web_ThoiTrang.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPham { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
    }
}
