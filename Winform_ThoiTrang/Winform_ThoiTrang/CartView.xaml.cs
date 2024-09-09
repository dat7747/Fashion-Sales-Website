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
using static System.Net.Mime.MediaTypeNames;

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
            InitializeComponent();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            LoadCartItems();
            UpdateTotal();
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
            UpdateTotal();
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
            UpdateTotal();
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
            UpdateTotal();
        }

        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {

            if(CartItemsPanel.Children.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm trong giỏ hàng", "Thông báo", MessageBoxButton.OK);
                return;
            }
            string totalPriceText = TotalPriceText.Text;

            string cleanedPrice = totalPriceText.Replace(".", "").Replace(",", ".").Replace(" ₫", "");

            decimal totalAmount;

            if (decimal.TryParse(cleanedPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out totalAmount))
            {
                frm_Payment paymentWindow = new frm_Payment(totalAmount);
                paymentWindow.PaymentCompleted += () => LoadCartItems(); // Đăng ký sự kiện
                paymentWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Giá trị tổng tiền không hợp lệ, vui lòng kiểm tra lại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //===========================Truy vấn ==================================

        private void LoadCartItems()
        {
            try
            {
                var cartItems = _context.CartItem
                    .Include(c => c.SanPham)
                    .Where(c => c.KhachHangID == 1)
                    .ToList();

                CartItemsPanel.Children.Clear();

                foreach (var item in cartItems)
                {
                    if (item.SanPham != null)
                    {
                        var cartItemControl = CreateCartItemControl(item);
                        CartItemsPanel.Children.Add(cartItemControl);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải các mục giỏ hàng: {ex.Message}");
            }
        }

        private void UpdateTotal()
        {
            int khachHangID = 1;

            var totalPrice = _context.CartItem
                .Where(ci => ci.KhachHangID == khachHangID)
                .Sum(ci => ci.SoLuong * ci.SanPham.Gia); // Tính tổng giá trị

            // Cập nhật giá trị tổng vào TotalPriceText
            TotalPriceText.Text = $"{totalPrice:C}";
        }

        private UIElement CreateCartItemControl(CartItem item)
        {
            var productName = item.SanPham?.TenSanPham ?? "Tên sản phẩm không có";
            var productSize = item.Size ?? "Không xác định";
            var productPrice = item.SanPham?.Gia ?? 0;
            var totalPrice = productPrice * item.SoLuong;

            // Truy vấn để lấy hình ảnh đầu tiên của sản phẩm
            var firstImage = _context.HinhAnhSanPham
                .Where(h => h.SanPhamID == item.SanPhamID)
                .Select(h => h.HinhAnh)
                .FirstOrDefault();

            BitmapImage productImage = null;

            if (firstImage != null)
            {
                try
                {
                    productImage = ByteArrayToBitmapImage(firstImage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi chuyển đổi hình ảnh: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy hình ảnh cho sản phẩm này.");
            }

            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(10),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Hiển thị ảnh sản phẩm
            var image = new System.Windows.Controls.Image
            {
                Source = productImage,
                Width = 100,
                Height = 100,
                Margin = new Thickness(0, 0, 10, 0),
                Stretch = Stretch.Fill,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            grid.Children.Add(image);

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

            var actionStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(actionStackPanel, 2);

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

            var removeButton = new Button
            {
                Content = "Xóa",
                Width = 60,
                Margin = new Thickness(10, 0, 0, 0),
                Background = Brushes.Red,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Tag = item,
                BorderBrush = Brushes.Red
            };
            removeButton.Click += RemoveItem_Click;
            actionStackPanel.Children.Add(removeButton);

            grid.Children.Add(actionStackPanel);

            border.Child = grid;

            return border;
        }

        private BitmapImage ByteArrayToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze(); // Để tránh lỗi truy cập đa luồng
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo BitmapImage: {ex.Message}");
                return null;
            }
        }

    }

}
