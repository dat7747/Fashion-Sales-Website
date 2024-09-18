using Microsoft.EntityFrameworkCore;
using Model;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
    /// Interaction logic for frm_BillStatistics.xaml
    /// </summary>
    public partial class frm_BillStatistics : UserControl
    {
        private ApplicationDbContext _context;
        public frm_BillStatistics()
        {
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            InitializeComponent();
        }
        private void OnThongKeClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn ngày bắt đầu và kết thúc chưa
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc.");
                return;
            }

            DateTime startDate = (DateTime)StartDatePicker.SelectedDate;
            DateTime endDate = (DateTime)EndDatePicker.SelectedDate;

            // Truy vấn dữ liệu hóa đơn trong khoảng thời gian
            var bills = _context.HoaDon
                .Include(h => h.KhachHang)  // Load tên khách hàng từ bảng KhachHang
                .Include(h => h.HoaDonChiTiet)  // Load chi tiết hóa đơn
                .ThenInclude(cd => cd.SanPham)  // Load sản phẩm từ chi tiết hóa đơn
                .Where(h => h.NgayTao >= startDate && h.NgayTao <= endDate)
                .ToList();

            // Tính tổng số lượng các sản phẩm theo loại
            int totalClothes = 0;
            int totalShoes = 0;
            int totalBags = 0;

            foreach (var bill in bills)
            {
                foreach (var chiTiet in bill.HoaDonChiTiet)
                {
                    // Kiểm tra xem chi tiết hóa đơn có chứa sản phẩm không
                    if (chiTiet.SanPham != null)
                    {
                        if (chiTiet.SanPham.LoaiSanPhamID == 1) // Quần áo
                        {
                            totalClothes += chiTiet.SoLuong;
                        }
                        else if (chiTiet.SanPham.LoaiSanPhamID == 2) // Giày
                        {
                            totalShoes += chiTiet.SoLuong;
                        }
                        else if (chiTiet.SanPham.LoaiSanPhamID == 3) // Túi xách
                        {
                            totalBags += chiTiet.SoLuong;
                        }
                    }
                }
            }

            // Gán dữ liệu cho DataGrid
            BillDataGrid.ItemsSource = bills;

            // Hiển thị tổng số lượng từng loại sản phẩm
            TotalClothesText.Text = $"Tổng số hàng quần, áo: {totalClothes}";
            TotalShoesText.Text = $"Tổng số hàng giày: {totalShoes}";
            TotalBagsText.Text = $"Tổng số hàng túi xách: {totalBags}";
        }
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem sender có phải là Button không
            var button = sender as Button;
            if (button != null)
            {
                // Kiểm tra xem Tag có phải là số nguyên không
                if (button.Tag != null && int.TryParse(button.Tag.ToString(), out int selectedBillID))
                {
                    // Truy vấn hóa đơn từ cơ sở dữ liệu
                    var bill = GetBillById(selectedBillID);

                    if (bill != null)
                    {
                        // Hiển thị cửa sổ chi tiết hóa đơn
                        var detailWindow = new frm_DetailBill(bill);
                        detailWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn.");
                    }
                }
                else
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Nút không hợp lệ.");
            }
        }

        //===============Truy vấn=================
        private HoaDon GetBillById(int billId)
        {
            // Truy vấn hóa đơn từ cơ sở dữ liệu
            return _context.HoaDon
                .Include(h => h.KhachHang)
                .Include(h => h.HoaDonChiTiet)
                .ThenInclude(cd => cd.SanPham)
                .FirstOrDefault(h => h.HoaDonID == billId);
        }
    }
}
