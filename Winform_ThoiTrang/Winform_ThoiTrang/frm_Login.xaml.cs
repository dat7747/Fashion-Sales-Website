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
using System.Windows.Shapes;

namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for frm_Login.xaml
    /// </summary>
    public partial class frm_Login : Window
    {
        private ApplicationDbContext _context;
        public frm_Login()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuilder.Options);
            InitializeComponent();
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordBox.Password;

            var account = _context.Account.FirstOrDefault(a => a.TenDangNhap == username && 
            a.MatKhau ==  password);


            if (account != null)
            {
                MessageBox.Show("Đăng Nhập Thành Công");
                MainWindow fr = new MainWindow();
                fr.Show();
                this.Close();
            }
            else
            {
                LoginMessageTextBlock.Text = "Tên đăng nhập hoặc mật khẩu không đúng";
            }    
        }
    }
}
