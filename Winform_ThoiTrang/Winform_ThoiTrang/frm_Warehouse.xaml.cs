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
            themSPVaoKho();

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
                    btn_Them.IsEnabled = true;
                }
                else
                {
                    LoadInStockProducts();
                    btn_Them.IsEnabled = false;
                    btn_Luu.IsEnabled = true;
                    btn_Sua.IsEnabled = true;
                    btn_Xoa.IsEnabled = true;
                }
            }
        }
        private void InStockProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = (dynamic)InStockProductDataGrid.SelectedItem;
            if (selectedProduct != null)
            {
                // Hiển thị thông tin sản phẩm đã chọn
                InStockProductNameTextBox.Text = selectedProduct.TenSanPham;
                InStockSizeComboBox.ItemsSource = GetAvailableSizes(); // Nạp dữ liệu cho ComboBox
                InStockSizeComboBox.SelectedItem = selectedProduct.KichThuocID.ToString();
                InStockQuantityTextBox.Text = selectedProduct.SoLuong.ToString();
            }
        }

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
            TextBox quantityTextBox = null;

            if (InStockProductDataGrid.IsVisible)
            {
                quantityTextBox = InStockQuantityTextBox;
            }
            else if (NotInStockProductDataGrid.IsVisible)
            {
                quantityTextBox = NotInStockQuantityTextBox;
            }

            if (quantityTextBox != null && int.TryParse(quantityTextBox.Text, out int quantity))
            {
                quantity = Math.Max(0, quantity - 1); // Giảm số lượng, không nhỏ hơn 0
                quantityTextBox.Text = quantity.ToString();
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            TextBox quantityTextBox = null;

            if (InStockProductDataGrid.IsVisible)
            {
                quantityTextBox = InStockQuantityTextBox;
            }
            else if (NotInStockProductDataGrid.IsVisible)
            {
                quantityTextBox = NotInStockQuantityTextBox;
            }

            if (quantityTextBox != null && int.TryParse(quantityTextBox.Text, out int quantity))
            {
                quantity++; // Tăng số lượng
                quantityTextBox.Text = quantity.ToString();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem sản phẩm đã được chọn hay chưa
            if (selectedProductID <= 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trước khi xóa.");
                return;
            }

            // Tìm sản phẩm trong kho với ID đã cho
            var khoItem = _context.Kho.SingleOrDefault(k => k.SanPhamID == selectedProductID);
            if (khoItem != null)
            {
                _context.Kho.Remove(khoItem);
                _context.SaveChanges();
                MessageBox.Show("Xóa thành công sản phẩm.");
                // Cập nhật DataGrid hoặc các thành phần khác nếu cần
            }
            else
            {
                MessageBox.Show("Sản phẩm không tồn tại trong kho.");
            }
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProductID <= 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trước khi lưu.");
                return;
            }

            // Lấy dữ liệu từ TextBox và ComboBox
            string kichThuocID = InStockSizeComboBox.SelectedItem?.ToString();
            int soLuong;

            if (int.TryParse(InStockQuantityTextBox.Text, out soLuong))
            {
                // Kiểm tra nếu sản phẩm đã tồn tại trong kho
                var khoItem = _context.Kho.SingleOrDefault(k => k.SanPhamID == selectedProductID && k.KichThuocID == kichThuocID);

                if (khoItem != null)
                {
                    // Nếu tồn tại, cập nhật số lượng
                    khoItem.SoLuong = soLuong;
                    _context.Kho.Update(khoItem);
                }
                else
                {
                    themSPVaoKho();
                }

                _context.SaveChanges();
                MessageBox.Show("Lưu thành công.");
                // Cập nhật DataGrid hoặc các thành phần khác nếu cần
            }
            else
            {
                MessageBox.Show("Số lượng không hợp lệ.");
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

        //Sản phẩm có trong kho
        private void LoadInStockProducts()
        {
            var inStockProducts = from kho in _context.Kho
                                  join sp in _context.SanPham on kho.SanPhamID equals sp.SanPhamID
                                  select new
                                  {
                                      sp.TenSanPham,
                                      kho.KichThuocID,
                                      kho.SoLuong
                                  };

            InStockProductDataGrid.ItemsSource = inStockProducts.ToList();
        }
        private List<string> GetAvailableSizes()
        {
            // Trả về danh sách các kích thước sản phẩm, ví dụ như:
            return new List<string> { "35", "36", "37", "38", "39", "40", "41", "42", "43", "44" };
        }

        private void themSPVaoKho()
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

    }
}
