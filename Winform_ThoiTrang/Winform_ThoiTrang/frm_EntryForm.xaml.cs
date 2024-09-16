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
    /// Interaction logic for frm_EntryForm.xaml
    /// </summary>
    public partial class frm_EntryForm : UserControl
    {
        private ApplicationDbContext _context;
        public frm_EntryForm()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            InitializeComponent();
            LoadComboBoxData();
        }
        private void NhapHangButton_Click  (object sender, RoutedEventArgs e)
        {
            kiemtra();

            if (string.IsNullOrEmpty(GiaNhapTextBox.Text) || !decimal.TryParse(GiaNhapTextBox.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ");
                return;
            }
            if (string.IsNullOrEmpty(SoLuongTextBox.Text) || !int.TryParse(SoLuongTextBox.Text, out int soLuong))
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ.");
                return;
            }
            var sanPham = _context.SanPham.FirstOrDefault(sp => sp.TenSanPham == SanPhamComboBox.Text);
            if (sanPham == null) {
                MessageBox.Show("Không tồn tại sản phẩm.");
                return;
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var phieuNhap = new PhieuNhap
                    {
                        NhaCungCapID = (int)NhaCungCapComboBox.SelectedValue,
                        NgayNhap = DateTime.Now,
                        TongTien = giaNhap * soLuong,
                        GhiChu = GhiChuTextBox.Text
                    };

                    _context.PhieuNhap.Add(phieuNhap);
                    _context.SaveChanges();

                    var sanPhamTrongKho = _context.Kho.FirstOrDefault(sp => sp.SanPhamID == sanPham.SanPhamID && sp.KichThuocID == KichThuocComboBox.Text);
                    if (sanPhamTrongKho != null)
                    {
                        sanPhamTrongKho.SoLuong += soLuong;
                        _context.Entry(sanPhamTrongKho).State = EntityState.Modified;

                    }
                    else
                    {
                        var kho = new Kho
                        {
                            SanPhamID = sanPham.SanPhamID,
                            KichThuocID = KichThuocComboBox.Text,
                            SoLuong = soLuong
                        };
                        _context.Kho.Add(kho);
                    }


                    var chiTietPhieuNhap = new ChiTietPhieuNhap
                    {
                        PhieuNhapID = phieuNhap.PhieuNhapID,
                        SanPhamID = sanPham.SanPhamID,
                        SoLuong = soLuong,
                        KichThuoc = KichThuocComboBox.Text,
                        GiaNhap = giaNhap,
                    };
                    _context.ChiTietPhieuNhap.Add(chiTietPhieuNhap);
                    _context.SaveChanges();

                    transaction.Commit();
                    MessageBox.Show("Tạo phiếu nhập thành công");
                    reset();
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    MessageBox.Show("Errol: " + ex.Message);
                }
            }
        }
        //=====================Truy vấn=======================
        private void SanPhamComboBox_DropDownOpened(object sender, EventArgs e)
        {
           
        }
        private void SanPhamComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SanPhamComboBox.Text;

            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var filteredProducts = _context.SanPham
                .Where(sp => sp.TenSanPham.Contains(searchText))
                .ToList();

            // Cập nhật ComboBox với danh sách sản phẩm tìm thấy
            SanPhamComboBox.ItemsSource = filteredProducts;
            SanPhamComboBox.DisplayMemberPath = "TenSanPham"; // Hiển thị tên sản phẩm trong ComboBox
            SanPhamComboBox.IsDropDownOpen = true; // Mở dropdown để hiển thị kết quả tìm kiếm
        }

        private void LoadComboBoxData()
        {
            // Load Nhà Cung Cấp
            NhaCungCapComboBox.ItemsSource = _context.NhaCungCap.ToList();
            NhaCungCapComboBox.DisplayMemberPath = "TenNhaCungCap";
            NhaCungCapComboBox.SelectedValuePath = "NhaCungCapID";

            // Load Sản Phẩm
            SanPhamComboBox.ItemsSource = _context.SanPham.ToList();
            SanPhamComboBox.DisplayMemberPath = "TenSanPham";
            SanPhamComboBox.SelectedValuePath = "SanPhamID";
        }

        private void kiemtra()
        {
            if(NhaCungCapComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp");
                return;
            }
            if (string.IsNullOrEmpty(SanPhamComboBox.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần nhập");
                return;
            }
        }

        private void reset()
        {
            SoLuongTextBox.Text  = string.Empty;
            GiaNhapTextBox.Text = string.Empty;
            GhiChuTextBox.Text = string.Empty;
            NhaCungCapComboBox.SelectedItem = -1;
            SanPhamComboBox.SelectedItem = -1;
            KichThuocComboBox.Text = string.Empty;
            SanPhamComboBox.SelectedItem = -1;
        }
    }
}
