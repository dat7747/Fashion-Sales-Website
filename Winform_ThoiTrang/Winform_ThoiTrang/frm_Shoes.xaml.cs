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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Model;
namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_Shoes.xaml
    /// </summary>
    public partial class frm_Shoes : UserControl
    {
        private ApplicationDbContext _context;
        public frm_Shoes()
        {
            InitializeComponent();
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            getPruduts();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addproducts = new frm_AddProducts();
            addproducts.ShowDialog();

            getPruduts();
        }

        private void EditButton_Click(Object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductDataGrid.SelectedItem;

            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để Sửa");
                return;
            }

            var sanPhamID = (int)selectedProduct.GetType().GetProperty("SanPhamID").GetValue(selectedProduct, null);

            var editproducts = new frm_EditProducts(sanPhamID);
            editproducts.ShowDialog();
            getPruduts();
        }
            
        private void DeleteButton_Click(Object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductDataGrid.SelectedItem;

            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa");
                return;
            }

            var sanPhamID = (int)selectedProduct.GetType().GetProperty("SanPhamID").GetValue(selectedProduct, null);

            var result = MessageBox.Show("Bạn có chắc muốn xóa không", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var images = _context.HinhAnhSanPham.Where(img => img.SanPhamID == sanPhamID).ToList();
                        _context.HinhAnhSanPham.RemoveRange(images);

                        var productToDelete = _context.SanPham.Find(sanPhamID);
                        if (productToDelete != null)
                        {
                            _context.SanPham.Remove(productToDelete);
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        getPruduts();
                        MessageBox.Show("Xóa Thành Công","Thông báo",MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
        }

        //==========================================TRUY VẤN=====================
        //Lấy danh sách hiển thị giày 
        private void getPruduts()
        {
            var result = _context.SanPham
                .Select(sp => new
                {
                    sp.SanPhamID,
                    sp.TenSanPham,
                    sp.Gia,
                    sp.ChatLieu,
                    sp.NhaSanXuat,
                    sp.LoaiSanPhamID,
                    HinhAnh = sp.HinhAnhSanPham.FirstOrDefault().HinhAnh
                }).ToList();
            ProductDataGrid.ItemsSource = result;
        }
    }
}
