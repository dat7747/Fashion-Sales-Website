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
    /// Interaction logic for frm_Home.xaml
    /// </summary>
    public partial class frm_Home : UserControl
    {
        private ApplicationDbContext _context;
        public frm_Home()
        {
            InitializeComponent();
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            LoadProducts();
        }




        //=======================Truy vấn==============================
        private void LoadProducts()
        {
            var products = _context.SanPhams.Include(p => p.HinhAnhSanPham).ToList();

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
                    Margin = new Thickness(0, 0, 0, 5)
                };
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

                var quantityPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var decreaseButton = new Button
                {
                    Content = "-",
                    FontSize = 20,
                    Margin = new Thickness(0, 0, 5, 0),
                    Width = 30,
                    Tag = product.SanPhamID
                };
                decreaseButton.Click += DecreaseQuantity_Click;
                quantityPanel.Children.Add(decreaseButton);

                var quantityTextBlock = new TextBlock
                {
                    Text = "0", // Default quantity
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 5, 0),
                    Tag = product.SanPhamID
                };
                quantityPanel.Children.Add(quantityTextBlock);

                var increaseButton = new Button
                {
                    Content = "+",
                    FontSize = 20,
                    Width = 30,
                    Tag = product.SanPhamID
                };
                increaseButton.Click += IncreaseQuantity_Click;
                quantityPanel.Children.Add(increaseButton);

                stackPanel.Children.Add(quantityPanel);

                // Ensure that WrapPanel is used to contain productBorder
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

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var quantityTextBlock = (TextBlock)((StackPanel)button.Parent).Children[1];

            if (int.TryParse(quantityTextBlock.Text, out int quantity))
            {
                quantityTextBlock.Text = (quantity + 1).ToString();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var quantityTextBlock = (TextBlock)((StackPanel)button.Parent).Children[1];

            if (int.TryParse(quantityTextBlock.Text, out int quantity) && quantity > 0)
            {
                quantityTextBlock.Text = (quantity - 1).ToString();
            }
        }
    }
}
