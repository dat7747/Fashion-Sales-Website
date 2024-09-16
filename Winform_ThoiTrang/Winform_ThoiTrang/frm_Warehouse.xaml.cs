using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            InitializeComponent();
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

                if (selectedTab.Header.ToString() == "Danh sách sản phẩm")
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
        // Sự kiện SelectionChanged của DataGrid
        private void InStockProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InStockProductDataGrid.SelectedItem != null)
            {
                // Giả sử đối tượng sản phẩm có các thuộc tính: SanPhamID, TenSanPham, SoLuong, KichThuocID
                var selectedProduct = (dynamic)InStockProductDataGrid.SelectedItem;

                // Cập nhật giá trị biến toàn cục selectedProductID
                selectedProductID = selectedProduct.SanPhamID;

                // Cập nhật các textbox và combobox trên form
                InStockProductNameTextBox.Text = selectedProduct.TenSanPham ?? "";
                InStockQuantityTextBox.Text = selectedProduct.SoLuong.ToString();

                // Cập nhật giá trị ComboBox (nếu KichThuocID tồn tại)
                if (selectedProduct.KichThuocID != null)
                {
                    InStockSizeComboBox.SelectedItem = selectedProduct.KichThuocID;
                }
            }
            else
            {
                // Nếu không có sản phẩm nào được chọn
                selectedProductID = 0;
                InStockProductNameTextBox.Clear();
                InStockQuantityTextBox.Clear();
                InStockSizeComboBox.SelectedItem = null;
            }
        }


        private void NotInStockProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotInStockProductDataGrid.SelectedItem != null)
            {
                // Explicitly cast to the correct type
                var selectedProduct = NotInStockProductDataGrid.SelectedItem as dynamic;
                if (selectedProduct != null)
                {
                    // Assign values while handling potential nulls
                    NotInStockProductNameTextBox.Text = selectedProduct.TenSanPham ?? ""; // Ensure string is not null
                   
                }
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
            string kichThuocID = InStockSizeComboBox.SelectedItem?.ToString() + ",0";
            // Tìm sản phẩm trong kho với ID đã cho
            var khoItem = _context.Kho.SingleOrDefault(k => k.SanPhamID == selectedProductID && k.KichThuocID == kichThuocID);
            if (khoItem != null)
            {
                _context.Kho.Remove(khoItem);
                _context.SaveChanges();
                MessageBox.Show("Xóa thành công sản phẩm.");
                LoadNotInStockProducts();
                LoadInStockProducts();
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

            string kichThuocID = InStockSizeComboBox.SelectedItem?.ToString();
            int soLuong;

            if (int.TryParse(InStockQuantityTextBox.Text, out soLuong))
            {
                // Kiểm tra xem sản phẩm đã tồn tại trong kho hay chưa
                var khoItem = _context.Kho
                    .Where(k => k.SanPhamID == selectedProductID && k.KichThuocID == kichThuocID)
                    .FirstOrDefault();

                if (khoItem != null)
                {
                    // Nếu đã tồn tại, cập nhật số lượng
                    khoItem.SoLuong = soLuong;
                    _context.Kho.Update(khoItem);
                }
                else
                {
                    // Nếu không tồn tại, thêm mới sản phẩm vào kho
                    khoItem = new Kho
                    {
                        SanPhamID = selectedProductID,
                        KichThuocID = kichThuocID,
                        SoLuong = soLuong
                    };
                    _context.Kho.Add(khoItem);
                }

                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Lưu thành công.");
                    LoadInStockProducts();
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Có lỗi khi lưu dữ liệu: " + ex.Message);
                }

                // Cập nhật DataGrid hoặc các thành phần khác nếu cần
            }
            else
            {
                MessageBox.Show("Số lượng không hợp lệ.");
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            // Tìm sản phẩm có trong kho
            var inStockProducts = (from sp in _context.SanPham
                                   join kho in _context.Kho on sp.SanPhamID equals kho.SanPhamID
                                   where sp.TenSanPham.ToLower().Contains(searchText)
                                   select new
                                   {
                                       kho.SanPhamID,      // ID Sản phẩm
                                       sp.TenSanPham,      // Tên sản phẩm
                                       kho.KichThuocID,    // Kích thước (size)
                                       kho.SoLuong         // Số lượng
                                   }).ToList();

            // Tìm sản phẩm chưa có trong kho
            var notInStockProducts = (from sp in _context.SanPham
                                      where !_context.Kho.Any(kho => kho.SanPhamID == sp.SanPhamID)
                                            && sp.TenSanPham.ToLower().Contains(searchText)
                                      select new
                                      {
                                          sp.SanPhamID,
                                          sp.TenSanPham
                                      }).ToList();

            // Cập nhật DataGrid cho các sản phẩm trong kho
            InStockProductDataGrid.ItemsSource = inStockProducts;

            // Cập nhật DataGrid cho các sản phẩm chưa có trong kho
            NotInStockProductDataGrid.ItemsSource = notInStockProducts;
        }

        //==============Truy vấn========================
        //sản phẩm không có trong kho
        private void LoadNotInStockProducts()
        {
            var products = (from sp in _context.SanPham
                            select new
                            {
                                sp.SanPhamID,
                                sp.TenSanPham
                            }).ToList();

            // Kiểm tra số lượng sản phẩm chưa có trong kho
            Debug.WriteLine($"Số lượng sản phẩm chưa có trong kho: {products.Count}");

            NotInStockProductDataGrid.ItemsSource = products;
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
                                      kho.SanPhamID,      // ID Sản phẩm
                                      sp.TenSanPham,      // Tên sản phẩm
                                      kho.KichThuocID,    // Kích thước (size)
                                      kho.SoLuong         // Số lượng
                                  };

            // Kiểm tra số lượng sản phẩm trong kho
            var productsList = inStockProducts.ToList();
            Debug.WriteLine($"Số lượng sản phẩm trong kho: {productsList.Count}");

            // Gán dữ liệu vào DataGrid
            InStockProductDataGrid.ItemsSource = productsList;
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
