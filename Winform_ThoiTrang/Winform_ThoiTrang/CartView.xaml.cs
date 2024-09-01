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
    /// Interaction logic for CartView.xaml
    /// </summary>
    public partial class CartView : Window
    {
        private ApplicationDbContext _context;
        public CartView()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            InitializeComponent();
            LoadCartItems();

        }
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cartItem = button?.Tag as CartItem;

            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
                _context.SaveChanges();
                LoadCartItems();
            }
        }
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cartItem = button?.Tag as CartItem;

            if (cartItem != null && cartItem.SoLuong > 0)
            {
                cartItem.SoLuong--;
                _context.SaveChanges();
                LoadCartItems();
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cartItem = button?.Tag as CartItem;

            if (cartItem != null)
            {
                cartItem.SoLuong++;
                _context.SaveChanges();
                LoadCartItems();
            }
        }

        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle checkout logic here
        }

        //==============================Truy vấn=============================
        private void LoadCartItems()
        {
            try
            {
                // Truy vấn các mục giỏ hàng với điều kiện KhachHangID có thể là NULL
                var cartItems = _context.CartItem
                    .Include(c => c.SanPham)
                    .Where(c => c.KhachHangID == 1) 
                    .ToList();

                // Xóa các mục giỏ hàng hiện tại trên giao diện
                CartItemsPanel.Children.Clear();

                // Tạo và thêm các mục giỏ hàng vào giao diện
                foreach (var item in cartItems)
                {
                    // Kiểm tra giá trị NULL cho thuộc tính SanPham
                    if (item.SanPham != null)
                    {
                        var cartItemControl = CreateCartItemControl(item);
                        CartItemsPanel.Children.Add(cartItemControl);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc xử lý lỗi
                Console.WriteLine($"Lỗi khi tải các mục giỏ hàng: {ex.Message}");
            }
        }
        private void UpdateTotal()
        {
            var total = _context.CartItem
                                .Where(c => c.KhachHangID == null)
                                .Sum(c => c.SanPham.Gia * c.SoLuong);

            // Assume there's a TextBlock for total amount in XAML with x:Name="TotalAmountText"
            TotalPriceText.Text = $"{total:C}";
        }
        private UIElement CreateCartItemControl(CartItem item)
        {
            // Xử lý giá trị NULL cho các thuộc tính sản phẩm
            var productName = item.SanPham?.TenSanPham ?? "Tên sản phẩm không có";
            var productSize = item.Size ?? "Không xác định";
            var productPrice = item.SanPham?.Gia ?? 0;
            var totalPrice = productPrice * item.SoLuong;

            // Lấy hình ảnh đầu tiên từ tập hợp HinhAnhSanPham
            var firstImage = item.SanPham?.HinhAnhSanPham?.FirstOrDefault();
            var productImage = firstImage != null ? ByteArrayToBitmapImage(firstImage.HinhAnh) : null;

            // Tạo Border để bọc từng mục giỏ hàng
            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(10),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            // Tạo Grid để sắp xếp các phần tử trong Border
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Tạo Image để hiển thị hình ảnh sản phẩm
            var image = new Image
            {
                Source = productImage, // Sử dụng BitmapImage đã chuyển đổi
                Width = 100,
                Height = 100,
                Margin = new Thickness(0, 0, 10, 0),
                Stretch = Stretch.Fill,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            grid.Children.Add(image);

            // Tạo StackPanel để chứa thông tin sản phẩm
            var stackPanel = new StackPanel();
            Grid.SetColumn(stackPanel, 1);

            stackPanel.Children.Add(new TextBlock
            {
                Text = productName,
                FontWeight = FontWeights.Bold,
                FontSize = 18,
                Foreground = Brushes.Black
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Kích thước: {productSize}",
                FontSize = 16,
                Foreground = Brushes.Gray
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Số lượng: {item.SoLuong}",
                FontSize = 16,
                Foreground = Brushes.Gray
            });
            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{totalPrice:C}",
                FontWeight = FontWeights.Bold,
                FontSize = 18,
                Foreground = Brushes.DeepPink
            });
            grid.Children.Add(stackPanel);

            // Tạo StackPanel để chứa các nút hành động (tăng giảm số lượng, xóa sản phẩm)
            var actionStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(actionStackPanel, 2);

            // Tạo StackPanel để chứa nút tăng giảm số lượng
            var quantityPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var decreaseButton = new Button
            {
                Content = "-",
                Width = 30,
                Height = 30,
                FontWeight = FontWeights.Bold,
                Tag = item,
                Background = Brushes.LightGray,
                BorderBrush = Brushes.Gray
            };
            decreaseButton.Click += DecreaseQuantity_Click;
            quantityPanel.Children.Add(decreaseButton);

            var quantityText = new TextBlock
            {
                Text = item.SoLuong.ToString(),
                Width = 30,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            quantityPanel.Children.Add(quantityText);

            var increaseButton = new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                FontWeight = FontWeights.Bold,
                Tag = item,
                Background = Brushes.LightGray,
                BorderBrush = Brushes.Gray
            };
            increaseButton.Click += IncreaseQuantity_Click;
            quantityPanel.Children.Add(increaseButton);

            actionStackPanel.Children.Add(quantityPanel);

            // Tạo nút Xóa sản phẩm khỏi giỏ hàng
            var removeButton = new Button
            {
                Content = "Xóa",
                Width = 60,
                Margin = new Thickness(0, 5, 0, 0),
                Background = Brushes.Red,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Tag = item,
                BorderBrush = Brushes.Red
            };
            removeButton.Click += RemoveItem_Click;
            actionStackPanel.Children.Add(removeButton);

            grid.Children.Add(actionStackPanel);

            // Đặt Grid vào trong Border
            border.Child = grid;

            return border;
        }

        // Phương thức để chuyển đổi mảng byte thành BitmapImage
        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null) return null;

            using (var stream = new MemoryStream(byteArray))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Để ảnh có thể sử dụng trong nhiều thread nếu cần
                return bitmapImage;
            }
        }

    }
}
