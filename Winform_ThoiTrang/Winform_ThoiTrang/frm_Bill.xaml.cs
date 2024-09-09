using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_Bill.xaml
    /// </summary>
    public partial class frm_Bill : UserControl
    {

        private readonly ApplicationDbContext _context;
        public frm_Bill()
        {
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            InitializeComponent();
            LoadInvoices();
            InvoiceDataGrid.IsReadOnly = true;
        }

        public void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            DateTime? selectedDate = DateSearchPicker.SelectedDate;

            // Truy vấn từ HoaDon và kết hợp với HoaDonChiTiet
            var query = _context.HoaDon
                .Join(_context.HoaDonChiTiet,
                      hd => hd.HoaDonID,
                      cthd => cthd.HoaDonID,
                      (hd, cthd) => new
                      {
                          HoaDonID = hd.HoaDonID,
                          KhachHangID = hd.KhachHangID,
                          TenSanPham = cthd.SanPham.TenSanPham,
                          SoLuong = cthd.SoLuong,
                          Size = cthd.Size,  // Nếu có thông tin về Size trong bảng SanPham
                          DonGia = cthd.DonGia,
                          NgayTao = hd.NgayTao,
                          TongTien = hd.TongTien
                      }).AsQueryable();

            // Kiểm tra chuỗi tìm kiếm
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(hd => hd.HoaDonID.ToString().Contains(searchText) ||
                                          hd.TenSanPham.Contains(searchText) ||
                                          hd.KhachHangID.ToString().Contains(searchText));
            }

            // Kiểm tra ngày được chọn
            if (selectedDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTao.Date == selectedDate.Value.Date);
            }

            // Lấy kết quả và gán cho DataGrid
            var filteredInvoices = query.ToList();
            InvoiceDataGrid.ItemsSource = filteredInvoices;
        }

        public void DateSearchPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateSearchPicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DateSearchPicker.SelectedDate.Value.Date;

                var filteredInvoices = _context.HoaDon
                    .Join(_context.HoaDonChiTiet,
                          hd => hd.HoaDonID,
                          cthd => cthd.HoaDonID,
                          (hd, cthd) => new
                          {
                              HoaDonID = hd.HoaDonID,
                              KhachHangID = hd.KhachHangID,
                              TenSanPham = cthd.SanPham.TenSanPham,
                              SoLuong = cthd.SoLuong,
                              Size = cthd.Size,
                              DonGia = cthd.DonGia,
                              NgayTao = hd.NgayTao,
                              TongTien = hd.TongTien
                          })
                    .Where(hd => hd.NgayTao.Date == selectedDate)
                    .ToList();

                InvoiceDataGrid.ItemsSource = filteredInvoices;
            }
        }

        //===================================truy vấn======================
        private void LoadInvoices()
        {
            var invoices = _context.HoaDon
                .Join(_context.HoaDonChiTiet,
                      hd => hd.HoaDonID,
                      cthd => cthd.HoaDonID,
                      (hd, cthd) => new
                      {
                          HoaDonID = hd.HoaDonID,
                          KhachHangID = hd.KhachHangID,
                          TenSanPham = cthd.SanPham.TenSanPham,
                          SoLuong = cthd.SoLuong,
                          Size = cthd.Size,  
                          DonGia = cthd.DonGia,
                          NgayTao = hd.NgayTao,
                          TongTien = hd.TongTien
                      })
                .ToList();

            InvoiceDataGrid.ItemsSource = invoices;
        }

    }
}
