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
    /// Interaction logic for frm_Warehouse.xaml
    /// </summary>
    public partial class frm_Warehouse : UserControl
    {
        private ApplicationDbContext _context;
        private int selectedProductID;
        public frm_Warehouse()
        {
            InitializeComponent();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            InitializeSizeComboBox();
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            var productName = NotInStockProductNameTextBox.Text;
            var quantityText = NotInStockQuantityTextBox.Text;
            var size = NotInStockSizeComboBox.SelectedItem?.ToString();

            // Kiểm tra nếu các trường dữ liệu không hợp lệ
            if (string.IsNullOrEmpty(productName) ||
                !int.TryParse(quantityText, out int quantity) ||
                quantity <= 0 ||
                string.IsNullOrEmpty(size))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm, kích thước và số lượng hợp lệ.");
                return;
            }

            // Tìm SanPhamID dựa trên tên sản phẩm
            var product = _context.SanPham.SingleOrDefault(sp => sp.TenSanPham == productName);
            if (product == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại.");
                return;
            }

            var sanPhamID = product.SanPhamID;

            // Kiểm tra nếu sản phẩm đã tồn tại trong kho với kích thước đó
            var existingStock = _context.Kho
                .SingleOrDefault(k => k.SanPhamID == sanPhamID && k.KichThuocID == size);

            if (existingStock != null)
            {
                MessageBox.Show("Sản phẩm với kích thước này đã tồn tại trong kho.");
                return;
            }

            // Tạo đối tượng kho mới và thêm vào cơ sở dữ liệu
            var newStock = new Kho
            {
                SanPhamID = sanPhamID,
                KichThuocID = size,
                SoLuong = quantity
            };

            _context.Kho.Add(newStock);

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            MessageBox.Show("Sản phẩm đã được thêm vào kho thành công.");

            // Tải lại danh sách sản phẩm không có trong kho
            LoadNotInStockProducts();
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabItem selectedTab = (sender as TabControl).SelectedItem as TabItem;

                if (selectedTab.Header.ToString() == "Sản phẩm chưa có trong kho")
                {
                    LoadNotInStockProducts();
                    btn_Luu.IsEnabled = false;
                    btn_Sua.IsEnabled = false;
                    btn_Xoa.IsEnabled = false;
                }
                else
                {
                    btn_Luu.IsEnabled = true;
                    btn_Sua.IsEnabled = true;
                    btn_Xoa.IsEnabled = true;
                }
            }
        }
        private void InStockProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void NotInStockProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = (dynamic)NotInStockProductDataGrid.SelectedItem;
            if (selectedProduct != null)
            {
                // Lưu ID sản phẩm để sử dụng trong các hàm khác nếu cần
                selectedProductID = selectedProduct.SanPhamID;

                NotInStockProductNameTextBox.Text = selectedProduct.TenSanPham;
                NotInStockQuantityTextBox.Text = "0";
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem ô nhập liệu có được chọn không
            if (int.TryParse(NotInStockQuantityTextBox.Text, out int quantity))
            {
                quantity = Math.Max(0, quantity - 1); // Giảm số lượng, không nhỏ hơn 0
                NotInStockQuantityTextBox.Text = quantity.ToString();
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem ô nhập liệu có được chọn không
            if (int.TryParse(NotInStockQuantityTextBox.Text, out int quantity))
            {
                quantity++; // Tăng số lượng
                NotInStockQuantityTextBox.Text = quantity.ToString();
            }
        }
        //==============Truy vấn========================
        //sản phẩm không có trong kho
        private void LoadNotInStockProducts()
        {
            var product = (from sp in _context.SanPham
                           where !_context.Kho.Any(k => k.SanPhamID == sp.SanPhamID)
                           select new
                           {
                               sp.SanPhamID,
                               sp.TenSanPham
                           }).ToList();
            NotInStockProductDataGrid.ItemsSource = product;
        }
        private void InitializeSizeComboBox()
        {
            var sizes = Enumerable.Range(35, 10).Select(x => x.ToString("0.0")).ToList();
            NotInStockSizeComboBox.ItemsSource = sizes;
            InStockSizeComboBox.ItemsSource = sizes;
        }
    }
}
