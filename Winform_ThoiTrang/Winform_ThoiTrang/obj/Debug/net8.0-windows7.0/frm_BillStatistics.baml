   M S B A M L   `   `   `  ����  Winform_ThoiTrang*    $Winform_ThoiTrang.frm_BillStatistics   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5	      � ���#f9f9f9$��20q�6    .+�5   
   �� $��Thống Kê Hóa Đơn��6   $��28(�5      $
��Bold"�5      ���#FF5722$��Center=�5      $��0,0,0,30q�5      � 5   
   $��Center=�6   $	��Top=�6,   $��	0,60,0,20q�6D     � ColumnDefinitions  5      �� 5      $
��Auto �6#   �� 5      $	��200 �6#   �� 5      $
��Auto �6#   �� 5      $	��200 �6#   �� 5      $
��Auto �6#   .+�5      �� $��Ngày Bắt Đầu:��6   $��14(�5      $��SemiBold"�5      $	��Top=�5       $��
-7,6,105,0q�5!      $��2��6,   (  "System.Windows.Controls.DatePicker  5"      -   5          Name$ StartDatePicker��5"      ���#FF5722$��1q�5$      $��100,6,153,10q�6-   $��1��6C   $��3��6S   �� 5'      $��Ngày Kết Thúc:��6   $��14(�5(      $��SemiBold"�5)      $	��Top=�5*      $��134,6,316,0q�5+      $��3��5,      $��2��6(     5-      -   5        $ EndDatePicker��5-      $��4��5.      ���#FF5722$��1q�50      $��35,6,151,10q�6-   �� 53      -   5        $��Thống Kê��53      $��4��54      $	��120��55      $��35��56      ���#FF5722
�������$
��Bold"�59      $��0q�5:      $��	226,0,0,0q�5;      $
��Left=�5<      $��Center=�5=      5>      5?      &   System.Windows.Controls.DataGrid  5B   
   -   5          Name$ BillDataGrid��5B         AutoGenerateColumns$ False��5C      $��
0,150,0,20q�5D      $��Stretch=�5E      $��Stretch=�5F      
�������   CanUserAddRows$ False��5H         CanUserDeleteRows$ False��5I         Columns 5J      0  *System.Windows.Controls.DataGridTextColumn  5L      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã Hóa Đơn��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6V   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 69   �� *
HoaDonID+6V     5O      $ Tên Khách Hàng��6%   $ * 6_   	 6=   �� *KhachHang.Ten+6_     5R      $ Ngày Tạo��6%   $ * 6S   	 67   �� *	NgayTao+6S     5U      $ Tổng Tiền��6%   $ * 6f   	 68   �� *
TongTien+
 �� StringFormat$
 N0��6f     5X      $! Phương Thức Thanh Toán��6%   $ * 6m   	 6E   �� *PhuongThucThanhToan+6m   4  .System.Windows.Controls.DataGridTemplateColumn  5[      $ Hành Động��6)   $	 150 6<      CellTemplate 5\      �� 5]      .R�5^      �� -   5        $��Xem Chi Tiết��5^   %   
 � Tag 6<   �� *
HoaDonID+6U   5_      5a      5c      �� 5e   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��Bottom=�6K   $��0,20,0,0q�6f   .+�5f      �� -   5         ��Name$ TotalClothesText��5f      $&�� Tổng số hàng quần, áo: 0��5g      $��16(�5h      $��SemiBold"�5i      
���   �$	��0,5q�5k      �� 5l      -   5        $ TotalShoesText��5l      $ ��Tổng số hàng giày: 0��5m      $��16(�5n      $��SemiBold"�5o      
���   �$
��20,5q�5q      $	��152��6   �� 5r      -   5        $ TotalBagsText��5r      $%��Tổng số hàng túi xách: 0��5s      $��16(�5t      $��SemiBold"�5u      
���   �$
��20,5q�5w      5x      5y      5z      