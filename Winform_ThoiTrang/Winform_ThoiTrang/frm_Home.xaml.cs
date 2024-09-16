using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for frm_Home.xaml
    /// </summary>
    public partial class frm_Home : UserControl
    {
        private ApplicationDbContext _context;
        public static event EventHandler CartUpdated;

        public frm_Home()
        {
            InitializeComponent();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            LoadLoaiSanPham();
            LoadProducts();
            CartUpdated += (s, e) => UpdateCartItemCount();
        }

        // Xử lý sự kiện khi nhấp vào hình ảnh
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            var product = image.Tag as SanPham;

            // Hiển thị cửa sổ chi tiết sản phẩm
            var productDetailsWindow = new ProductDetails(product);

            // Đăng ký sự kiện khi sản phẩm được thêm vào giỏ hàng
            productDetailsWindow.ProductAddedToCart += ProductDetailsWindow_ProductAddedToCart;

            productDetailsWindow.ShowDialog();
        }

        // Xử lý sự kiện khi sản phẩm được thêm vào giỏ hàng
        private void ProductDetailsWindow_ProductAddedToCart(object sender, EventArgs e)
        {
            UpdateCartItemCount();  // Cập nhật số lượng sản phẩm trong giỏ hàng
        }


        private void LoaiSanPhamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoaiSanPhamComboBox == null || LoaiSanPhamComboBox.SelectedItem == null)
            {
                Debug.WriteLine("LoaiSanPhamComboBox is null or no item selected.");
                return;
            }

            var selectedLoaiSanPham = LoaiSanPhamComboBox.SelectedItem as dynamic;
            int? selectedLoaiSanPhamID = selectedLoaiSanPham?.Id;

            LoadProducts(selectedLoaiSanPhamID, SearchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoaiSanPhamComboBox == null || LoaiSanPhamComboBox.SelectedItem == null)
            {
                Debug.WriteLine("LoaiSanPhamComboBox is null or no item selected.");
                return;
            }

            var searchText = SearchTextBox.Text;
            var selectedLoaiSanPham = LoaiSanPhamComboBox.SelectedItem as dynamic;
            int? selectedLoaiSanPhamID = selectedLoaiSanPham?.Id;

            LoadProducts(selectedLoaiSanPhamID, searchText);
        }

        private void CartIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CartView cartView  = new CartView();
            cartView.ShowDialog();
        }
        //=======================Truy vấn==============================
        private void LoadProducts(int? loaiSanPhamID = null, string searchText = "")
        {
            var productsQuery = _context.SanPhams.Include(p => p.HinhAnhSanPham).AsQueryable();

            if (loaiSanPhamID.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.LoaiSanPhamID == loaiSanPhamID.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                // Sử dụng EF.Functions.Like thay vì Contains để tránh lỗi
                productsQuery = productsQuery.Where(p => EF.Functions.Like(p.TenSanPham, $"%{searchText}%"));
            }

            var products = productsQuery.ToList();

            ProductsWrapPanel.Children.Clear(); // Clear existing products

            foreach (var product in products)
            {
                var firstImage = product.HinhAnhSanPham.FirstOrDefault();
                var productImage = firstImage != null ? ByteArrayToImage(firstImage.HinhAnh) : null;

                var productBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    Width = 180,
                    Height = 250,
                    Padding = new Thickness(5)
                };

                var stackPanel = new StackPanel();
                productBorder.Child = stackPanel;

                var image = new Image
                {
                    Source = productImage,
                    Height = 100,
                    Stretch = Stretch.Uniform,
                    Margin = new Thickness(0, 0, 0, 5),
                    Tag = product // Gắn dữ liệu sản phẩm vào Tag
                };

                image.MouseLeftButtonUp += Image_MouseLeftButtonUp;

                stackPanel.Children.Add(image);

                var nameTextBlock = new TextBlock
                {
                    Text = product.TenSanPham,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14
                };
                stackPanel.Children.Add(nameTextBlock);

                var descriptionTextBlock = new TextBlock
                {
                    Text = product.MoTa,
                    FontStyle = FontStyles.Italic,
                    FontSize = 12,
                    Foreground = Brushes.Gray
                };
                stackPanel.Children.Add(descriptionTextBlock);

                var priceTextBlock = new TextBlock
                {
                    Text = $"đ{product.Gia:0,0}",
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                stackPanel.Children.Add(priceTextBlock);

                ProductsWrapPanel.Children.Add(productBorder);
            }
        }

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

        private void LoadLoaiSanPham()
        {
            var listLoaiSP = _context.LoaiSanPham.Select(lsp => new
            {
                Id = lsp.LoaiSanPhamID,
                Name = lsp.TenLoai
            }).ToList();

            LoaiSanPhamComboBox.ItemsSource = listLoaiSP;
            LoaiSanPhamComboBox.DisplayMemberPath = "Name";
            LoaiSanPhamComboBox.SelectedValuePath = "Id";
        }

        private void UpdateCartItemCount()
        {
            // Đếm tổng số lượng sản phẩm trong giỏ hàng của khách hàng (giả sử ID khách hàng là 1)
            int cartItemCount = _context.CartItem.Where(ci => ci.KhachHangID == 1).Sum(ci => ci.SoLuong);

            // Cập nhật số lượng trên TextBlock của icon giỏ hàng
            CartItemCountTextBlock.Text = cartItemCount.ToString();
        }

        public static void OnCartUpdate()
        {
            CartUpdated?.Invoke(null,EventArgs.Empty);
        }

    }

}