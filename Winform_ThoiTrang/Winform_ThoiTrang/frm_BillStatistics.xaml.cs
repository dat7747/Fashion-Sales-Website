using Microsoft.EntityFrameworkCore;
using Model;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
    /// Interaction logic for frm_BillStatistics.xaml
    /// </summary>
    public partial class frm_BillStatistics : UserControl
    {
        private ApplicationDbContext _context;
        public frm_BillStatistics()
        {
            var optionsBuider = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuider.UseSqlServer("Server=DESKTOP-BSIJE74\\SQLEXPRESS;Database=LTD;User Id=sa;Password=123;TrustServerCertificate=True;");
            _context = new ApplicationDbContext(optionsBuider.Options);
            InitializeComponent();
        }
        private void OnThongKeClick(object sender, RoutedEventArgs e)
        {
           
        }
        private void LoadPlot()
        {
            // Dữ liệu mẫu cho biểu đồ cột
            var totalClothes = 120;
            var totalShoes = 80;
            var totalBags = 50;

            // Tạo biểu đồ cột
            var plotModel = new PlotModel { Title = "Thống Kê Sản Phẩm" };

            var columnSeries = new ColumnSeries
            {
                ItemsSource = new List<ColumnItem>
                {
                    new ColumnItem(totalClothes),
                    new ColumnItem(totalShoes),
                    new ColumnItem(totalBags)
                },
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}"
            };

            plotModel.Series.Add(columnSeries);

            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "CategoryAxis",
                ItemsSource = new[] { "Quần Áo", "Giày", "Túi Xách" }
            });

            ChartView.Model = plotModel;
        }
        //===============Truy vấn=================
    }
}
