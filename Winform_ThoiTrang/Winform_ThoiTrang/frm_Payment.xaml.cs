using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using System.Windows.Shapes;

namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_Payment.xaml
    /// </summary>
    public partial class frm_Payment : Window
    {
        public event Action PaymentCompleted;
        private ApplicationDbContext _context;
        private decimal totalAmount;
        public frm_Payment(decimal totalAmount)
        {
            InitializeComponent();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            this.totalAmount = totalAmount;
            TotalAmountTextBlock.Text = totalAmount.ToString("C");
        }
        private void ConfirmPayment_Click(object sender, RoutedEventArgs e)
        {
            string paymentMethod = "";
            if (CashRadioButton.IsChecked == true)
            {
                paymentMethod = "Tiền mặt";
            }
            else if (TransferRadioButton.IsChecked == true)
            {
                paymentMethod = "Chuyển khoản";
            }
            else if (VNpayRadioButton.IsChecked == true)
            {
                paymentMethod = "VNpay";
            }

            if (string.IsNullOrEmpty(paymentMethod))
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán", "Thông báo", MessageBoxButton.OK);
                return;
            }

            // Lấy danh sách các sản phẩm trong giỏ hàng của khách hàng
            var cartItems = _context.CartItem.Where(c => c.KhachHangID == 1).ToList(); // Giả sử KhachHangID = 1
            List<string> outOfStockProducts = new List<string>();

            foreach (var item in cartItems)
            {
                // Log giá trị SanPhamID và KichThuocID của item
                Console.WriteLine($"Sản phẩm trong giỏ hàng - ID: {item.SanPhamID}, Kích thước: {item.Size}");

                // Tìm sản phẩm trong kho theo SanPhamID và KichThuocID
                var khoItem = _context.Kho.FirstOrDefault(k => k.SanPhamID == item.SanPhamID && k.KichThuocID == item.Size);
                // Kiểm tra sản phẩm có tồn tại trong kho với kích thước đó hay không
                if (khoItem == null)
                {
                    // Sản phẩm không tồn tại trong kho
                    outOfStockProducts.Add($"Sản phẩm ID {item.SanPhamID} không có trong kho.");
                }
                else if (khoItem.SoLuong < item.SoLuong)
                {
                    // Số lượng sản phẩm trong kho không đủ để bán
                    outOfStockProducts.Add($"Sản phẩm ID {item.SanPhamID} không đủ số lượng (còn lại {khoItem.SoLuong}).");
                }
            }



            // Nếu có sản phẩm không đủ hàng, thông báo và dừng việc thanh toán
            if (outOfStockProducts.Any())
            {
                string message = "Thanh toán thất bại vì các lý do sau:\n" + string.Join("\n", outOfStockProducts);
                MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nếu tất cả sản phẩm đều đủ, thực hiện tiếp việc lưu hóa đơn vào database
            HoaDon hoaDon = new HoaDon
            {
                KhachHangID = 1, // Giả sử khách hàng ID = 1
                TongTien = totalAmount,
                PhuongThucThanhToan = paymentMethod,
                NgayTao = DateTime.Now
            };

            _context.HoaDon.Add(hoaDon);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                var chiTietHoaDon = new HoaDonChiTiet
                {
                    HoaDonID = hoaDon.HoaDonID,
                    SanPhamID = item.SanPhamID,
                    SoLuong = item.SoLuong,
                    Size = item.Size,
                    DonGia = _context.SanPham.Where(sp => sp.SanPhamID == item.SanPhamID).Select(sp => sp.Gia).FirstOrDefault()
                };

                _context.HoaDonChiTiet.Add(chiTietHoaDon);

                // Cập nhật số lượng trong kho sau khi bán
                var khoItem = _context.Kho.FirstOrDefault(k => k.SanPhamID == item.SanPhamID && k.KichThuocID == item.Size);
                if (khoItem != null)
                {
                    khoItem.SoLuong -= item.SoLuong; // Trừ số lượng trong kho
                }
            }

            _context.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán thành công
            _context.CartItem.RemoveRange(cartItems);
            _context.SaveChanges();

            PaymentCompleted?.Invoke();

            MessageBox.Show("Thanh toán thành công với phương thức: " + paymentMethod);
            this.Close();
        }

        private void CancelPayment_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
