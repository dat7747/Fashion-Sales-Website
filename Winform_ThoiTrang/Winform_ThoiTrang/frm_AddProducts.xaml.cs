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
using Microsoft.Win32;
using Model;

namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_AddProducts.xaml
    /// </summary>
    public partial class frm_AddProducts : Window
    {
        private ApplicationDbContext _context;
        private string _pathimages;
        private string _pathimages1;
        private string _pathimages2;
        public frm_AddProducts()
        {
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            InitializeComponent();
            LoadLoaiSanPham();
        }
        private void ChooseImageButton1_Click(object sender, RoutedEventArgs e)
        {
            ChooseImage(out _pathimages, ImageFileNameTextBlock1);
        }
        private void ChooseImageButton2_Click(object sender, RoutedEventArgs e)
        {
            ChooseImage(out _pathimages2, ImageFileNameTextBlock2);
        }
        private void ChooseImageButton3_Click(object sender, RoutedEventArgs e)
        {
            ChooseImage(out _pathimages2, ImageFileNameTextBlock3);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra giá sản phẩm
            if (!int.TryParse(GiaTextBox.Text, out int gia) || gia <= 0)
            {
                MessageBox.Show("Giá phải là số nguyên lớn hơn 0.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //kiểm tra combobox 
            if(LoaiSanPhamComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);return;

            }

            //Kiểm tra các trường nhập liệu 
            if (string.IsNullOrWhiteSpace(TenSanPhamTextBox.Text) && string.IsNullOrWhiteSpace(MoTaTextBox.Text) && string.IsNullOrWhiteSpace(ChatLieuTextBox.Text) && string.IsNullOrWhiteSpace(NhaSanXuatTextBox.Text))
            {
                MessageBox.Show("Các trường không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Kiểm tra ảnh
            if (string.IsNullOrEmpty(_pathimages) && string.IsNullOrEmpty(_pathimages1) && string.IsNullOrEmpty(_pathimages2))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ ảnh sản phẩm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tam = (dynamic)LoaiSanPhamComboBox.SelectedValue;
             
            int loaiSPID = tam.Id;

            var sanPham = new SanPham
            {
                TenSanPham = TenSanPhamTextBox.Text,
                Gia = gia,
                MoTa = MoTaTextBox.Text,             
                LoaiSanPhamID = loaiSPID,
                ChatLieu = ChatLieuTextBox.Text,
                NhaSanXuat = NhaSanXuatTextBox.Text
            };

            var hinhAnhSP = new List<HinhAnhSanPham>
            {
                new HinhAnhSanPham { HinhAnh = System.IO.File.ReadAllBytes(_pathimages), SanPham = sanPham },
                new HinhAnhSanPham { HinhAnh = System.IO.File.ReadAllBytes(_pathimages1), SanPham = sanPham },
                new HinhAnhSanPham { HinhAnh = System.IO.File.ReadAllBytes(_pathimages2), SanPham = sanPham }
            };

            _context.SanPham.Add(sanPham);
            _context.HinhAnhSanPhams.AddRange(hinhAnhSP);
            _context.SaveChanges();

            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        //=========================Truy vấn ===================================
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
            LoaiSanPhamComboBox.SelectedValue = "Id";
        }


        private void ChooseImage(out string path, TextBlock textBlock)
        {
            path = string.Empty;
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    textBlock.Text = System.IO.Path.GetFileName(path);
                }
                else
                {
                    textBlock.Text = "Chưa chọn hình ảnh";
                }    
            }
        }
    }
}
