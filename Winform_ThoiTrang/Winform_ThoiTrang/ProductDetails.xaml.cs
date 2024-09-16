using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ProductDetails.xaml
    /// </summary>
    public partial class ProductDetails : Window
    {
        public event EventHandler ProductAddedToCart;

        private SanPham _product;
        private ApplicationDbContext _context;
        public ProductDetails(SanPham product)
        {
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            InitializeComponent();
            _product = product;
            // Cập nhật UI với dữ liệu sản phẩm
            ProductImage.Source = ByteArrayToImage(_product.HinhAnhSanPham.FirstOrDefault()?.HinhAnh);
            ProductName.Text = _product.TenSanPham;
            ProductDescription.Text = _product.MoTa;
            ProductPrice.Text = $"đ{_product.Gia:0,0}";
        }
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBlock.Text, out int quantity) && quantity > 1)
            {
                QuantityTextBlock.Text = (quantity - 1).ToString();
            }
        }
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBlock.Text, out int quantity))
            {
                QuantityTextBlock.Text = (quantity + 1).ToString();
            }
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var selectedSize = (SizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "C";

            if (int.TryParse(QuantityTextBlock.Text, out int quantity) && quantity > 0)
            {
                var existingItem = _context.CartItem
                    .FirstOrDefault(ci => ci.SanPhamID == _product.SanPhamID
                                          && ci.KhachHangID == 1 // Giả sử ID khách hàng là 1
                                          && ci.Size == selectedSize);

                if (existingItem != null)
                {
                    // Nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng
                    existingItem.SoLuong += quantity;
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                    var cartItem = new CartItem
                    {
                        SanPhamID = _product.SanPhamID,
                        KhachHangID = 1,
                        SoLuong = quantity,
                        Size = selectedSize
                    };

                    _context.CartItem.Add(cartItem);
                }

                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButton.OK);

                    // Gọi sự kiện ProductAddedToCart để thông báo cho frm_Home cập nhật giỏ hàng
                    ProductAddedToCart?.Invoke(this, EventArgs.Empty);
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.InnerException?.Message}", "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Số lượng sản phẩm không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //===============================Truy vấn=======================
        private BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null) return null;

            using (var stream = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
