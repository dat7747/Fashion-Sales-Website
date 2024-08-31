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
    /// Interaction logic for frm_EditProducts.xaml
    /// </summary>
    public partial class frm_EditProducts : Window
    {
        private ApplicationDbContext _context;
        private SanPham _product;
        private int _sanphamID;
        public frm_EditProducts(int  sanphamID)
        {
            InitializeComponent();
            _sanphamID = sanphamID;
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            loadSanPhamDuocChon();
            LoadLoaiSanPham();
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra giá sản phẩm
            if (!int.TryParse(GiaTextBox.Text, out int gia) || gia <= 0)
            {
                MessageBox.Show("Giá phải là số nguyên lớn hơn 0.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //kiểm tra combobox 
            if (LoaiSanPhamComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); return;

            }

            //Kiểm tra các trường nhập liệu 
            if (string.IsNullOrWhiteSpace(TenSanPhamTextBox.Text) && string.IsNullOrWhiteSpace(MoTaTextBox.Text) && string.IsNullOrWhiteSpace(ChatLieuTextBox.Text) && string.IsNullOrWhiteSpace(NhaSanXuatTextBox.Text))
            {
                MessageBox.Show("Các trường không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var productUpdate = _context.SanPham.Find(_sanphamID);
                if (productUpdate != null) {
                    productUpdate.TenSanPham = TenSanPhamTextBox.Text;
                    productUpdate.Gia = decimal.Parse(GiaTextBox.Text);
                    productUpdate.MoTa = MoTaTextBox.Text;
                    productUpdate.ChatLieu = ChatLieuTextBox.Text;
                    productUpdate.NhaSanXuat = NhaSanXuatTextBox.Text;
                    productUpdate.LoaiSanPhamID = (int)LoaiSanPhamComboBox.SelectedValue;

                    _context.SaveChanges();
                    MessageBox.Show("Lưu thông tin sản phẩm thành công");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thấy sản phẩm");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }


        //================================Truy vấn=============================
        private void LoadLoaiSanPham()
        {
            var listLoaiSP = _context.LoaiSanPham
                .Select(lsp => new
                {
                    Id = lsp.LoaiSanPhamID,
                    Name = lsp.TenLoai

                }).ToList();

            LoaiSanPhamComboBox.ItemsSource = listLoaiSP;
            LoaiSanPhamComboBox.DisplayMemberPath = "Name";
            LoaiSanPhamComboBox.SelectedValuePath = "Id";
        }
        private void loadSanPhamDuocChon()
        {
            _product = _context.SanPham.Find(_sanphamID);
            if (_product != null)
            {
                TenSanPhamTextBox.Text = _product.TenSanPham;
                GiaTextBox.Text = _product.Gia.ToString();
                MoTaTextBox.Text = _product.MoTa;
                ChatLieuTextBox.Text = _product.ChatLieu;
                NhaSanXuatTextBox.Text = _product.NhaSanXuat;
                LoaiSanPhamComboBox.SelectedItem = _product.LoaiSanPhamID;
            }
            else
            {
                MessageBox.Show("Không thấy sản phẩm");
                this.Close();
            }
        }
    }
}
