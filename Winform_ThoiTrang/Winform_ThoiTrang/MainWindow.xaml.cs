﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
namespace Winform_ThoiTrang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private int _role;
        public MainWindow(int role)
        {
            InitializeComponent();
            _role = role;
            Enable();
        }

        private void Enable()
        {
            if(_role == 0)
            {
                ManageMenuItem.Visibility = Visibility.Collapsed;
            }
        }
        public void ShoesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            frm_Shoes shoes_form = new frm_Shoes();

            MainContent.Content = shoes_form;
        }

        public void StoreMenuitem_Click(object sender, RoutedEventArgs e) {
            MainContent.Content = new frm_Home();
            frm_Home.OnCartUpdate();
        }
        public void WarehouseMenuItem_Click(Object sender, RoutedEventArgs e)
        {
            frm_Warehouse frm = new frm_Warehouse();
            MainContent.Content = frm;
        }

        public void BillMenuItem_Click(Object sender, RoutedEventArgs a)
        {
            frm_Bill frm = new frm_Bill();
            MainContent.Content = frm;
        }
        public void EntryFormMenuItem_Click(Object sender, RoutedEventArgs a)
        {
            frm_EntryForm frm = new frm_EntryForm();
            MainContent.Content = frm;
        }

        private void BillStatisticsFormMenuItem_Click(Object sender, RoutedEventArgs a)
        {
            frm_BillStatistics frm = new frm_BillStatistics();
            MainContent.Content = frm;
        }
    }
}