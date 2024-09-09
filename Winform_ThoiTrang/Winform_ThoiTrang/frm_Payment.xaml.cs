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

            // Lưu thông tin hóa đơn vào database
            HoaDon hoaDon = new HoaDon
            {
                KhachHangID = 1,
                TongTien = totalAmount,
                PhuongThucThanhToan = paymentMethod,
                NgayTao = DateTime.Now
            };

            _context.HoaDon.Add(hoaDon);
            _context.SaveChanges();

            var cartItems = _context.CartItem.Where(c => c.KhachHangID == hoaDon.KhachHangID).ToList();
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
            }

            _context.SaveChanges();

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
