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
using System.Windows.Shapes;

namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_DetailBill.xaml
    /// </summary>
    public partial class frm_DetailBill : Window
    {
        private HoaDon _bill;

        public frm_DetailBill(HoaDon bill)
        {
            InitializeComponent();
            _bill = bill;
            LoadBillDetails();
        }

        private void LoadBillDetails()
        {
            if (_bill != null)
            {
                // Hiển thị thông tin hóa đơn
                BillIDText.Text = _bill.HoaDonID.ToString();
                CustomerNameText.Text = _bill.KhachHang?.HoTen ?? "Chưa có thông tin";
                CreationDateText.Text = _bill.NgayTao.ToString("dd/MM/yyyy");
                TotalAmountText.Text = _bill.TongTien.ToString("N0") + " VNĐ";

                // Hiển thị danh sách sản phẩm
                ProductDataGrid.ItemsSource = _bill.HoaDonChiTiet
                    .Select(cd => new
                    {
                        SanPham = cd.SanPham?.TenSanPham ?? "Chưa có thông tin",
                        SoLuong = cd.SoLuong,
                        DonGia = cd.DonGia,
                        Size = cd.Size
                    })
                    .ToList();
            }
            else
            {
                MessageBox.Show("Hóa đơn không hợp lệ.");
            }
        }

    }

}
