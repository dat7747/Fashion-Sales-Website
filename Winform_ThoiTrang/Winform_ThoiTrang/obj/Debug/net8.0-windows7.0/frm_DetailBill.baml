   M S B A M L   `   `   `  ����  Winform_ThoiTrang&     Winform_ThoiTrang.frm_DetailBill   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Chi Tiết Hóa Đơn��5   	   $	��700��6"   $
��1000��6/   .��5      � $��20q�6   .+�5
   
   ��  �� Orientation$ Vertical=�6   $��0,0,0,20q�6,   $��Center=�6>   .+�5      �� $��CÔNG TY ABC��6   $��24(�5      $
��Bold"�5      ���#FF5722$��Center=�5      $��0,0,0,5q�5      �� 5      $7��1Địa chỉ công ty: 123 Đường ABC, TP. HCM��6   $��16(�5      $��Center=�5      �� 5      $*��$Số điện thoại: (012) 345-6789��6   $��16(�5      $��Center=�5      �� 5      $#��Email: contact@companyabc.com��6   $��16(�5      $��Center=�5      �� 5      $��HÓA ĐƠN BÁN HÀNG��6   $��24(�5      $
��Bold"�5      ���#FF5722$��Center=�5      $��0,20,0,5q�5      5       �� 5#   
   ���#FF5722$��2q�6'    �� CornerRadius$ 8��6;    �� Padding$ 10q�6L   $��0,0,0,20q�6Y   ._�5$      �� $ Vertical=�6   .+�5%      �� $��Mã Hóa Đơn:��6   $
��Bold"�6/   $��16(�6A   �� 5&      -   5         ��Name$ 
BillIDText��5&      $��16(�60   $��0,0,0,10q�6>   �� 5(      $��Tên Khách Hàng:��6   $
��Bold"�63   $��16(�6E   �� 5)      -   5        $ CustomerNameText��5)      $��16(�66   $��0,0,0,10q�6D   �� 5+      $��Ngày Tạo:��6   $
��Bold"�6-   $��16(�6?   �� 5,      -   5        $ CreationDateText��5,      $��16(�66   $��0,0,0,10q�6D   5-      5.      �� 51   
   ���#FF5722$��2q�6'   $ 8��6;   $ 10q�6L   ���#F5F5F5$��
0,180,0,20q�6n   ._�52      &   System.Windows.Controls.DataGrid  -   5          Name$ ProductDataGrid��52         AutoGenerateColumns$ False��53      
�������
�������$��1q�56         CanUserAddRows$ False��57      $	��Top=�58         Columns 59      0  *System.Windows.Controls.DataGridTextColumn  5:      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Sản Phẩm��6)   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6\   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   ��  �� Path$ SanPham�6\     5;      $	 Số Lượng��6)   $
 * 6\    6;   �� $ SoLuong�6\     5<      $
	 Giá��6)   $
 * 6g    66   �� $ DonGia� �� StringFormat$ N0��6g     5=      $	 Kích Thước��6)   $
 * 6[    6=   �� $
 Size�6[   5?      5@      �� 5C   
   ���#FF5722$��2q�6'   $ 8��6;   $ 10q�6L   ���#FFF8F1$��Bottom=�6n   $��0,0,0,20q�6�   ._�5D      �� $ Vertical=�6   $��Right=�60   .+�5E      �� $��Tổng Tiền:��6   $
��Bold"�6.   $��16(�6@   �� 5F      -   5        $ TotalAmountText��5F      $��16(�65   $
��Bold"�6C   $��0,5,0,0q�6U   5G      5H      5I      5J      