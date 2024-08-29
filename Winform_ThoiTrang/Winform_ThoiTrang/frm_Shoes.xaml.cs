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
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng chỉnh sửa thành công");
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng xóa thành công");
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addproducts = new frm_AddProducts();
            addproducts.ShowDialog();

            getPruduts();
        }
        //==========================================TRUY VẤN=====================
        //Lấy danh sách hiển thị giày 
        private void getPruduts()
        {
            var result = _context.SanPham.ToList();
            ProductDataGrid.ItemsSource = result;
        }

    }
}
