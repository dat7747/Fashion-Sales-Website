   M S B A M L   `   `   `  ����  Winform_ThoiTrang!    Winform_ThoiTrang.frm_Shoes   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      $	��620��5      $	��974��6   .��5      � .+�5   
   ��   �� Orientation$  
Horizontal=�6   ���#FFB100$��50��5	      $	��Top=�5
      .+�5      �� $*��$Hệ Thống Quản Lý Sản Phẩm��6   $��20(�5      $
��Bold"�5      
�������$��Center=�5      $��20,0,0,0q�5      5      �� 5   
   $  
Horizontal=�6   $��
20,70,20,0q�5      $	��Top=�5      .+�5      �� $	��300��6   $��30��5      $��5q�5      $!��Tìm kiếm sản phẩm...��5      
����������#FFB100$��1q�5      �� 5      $	��200��6   $��10,0,0,0q�5       
����������#FFB100$��1q�5#      5%      �� 5&      $��Tìm kiếm��6   $	��100��5'      $��10,0,0,0q�5(      ���#FFB100
����������#FFB100$��1q�5,      5-      &   System.Windows.Controls.DataGrid  50   
   -   5          Name$ ProductDataGrid��50      $��20,120,20,60q�51         AutoGenerateColumns$ False��52      
�������   
IsReadOnly$
 true��54      ���#FFB100$��1q�56       � 	Resources 57      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value��%�   '        �� 58      "  6   .�59      ��  � ��$ Center=�6C   �� 5:       � ��$ Center=�6A   �� 5;       � ��$
 Bold"�63   �� 5<       � ��$ 14(�61   5=      5>         Columns 5?      4  .System.Windows.Controls.DataGridTemplateColumn  5@      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Hình Ảnh��6)   
   Width5  /System.Windows.Controls.DataGridLengthConverter$	
 100 6;      CellTemplate 5A      �� 5B      .R�5C      �� $��80��5D      $��80��5E      $��UniformToFill=�5F      ��5C   $   �� *	HinhAnh+5F      5G      5I      0  *System.Windows.Controls.DataGridTextColumn  5K      $	 Tên Sản Phẩm��6%   $
 * 5M      1  +System.Windows.Controls.DataGridBoundColumn   Binding 5L      �� *
TenSanPham+ �� UpdateSourceTrigger$ PropertyChanged=�5M        5O      $
	 Giá��6%   $	
 100 5Q       5P      �� *Gia+ �� StringFormat$ {0:N0}đ��$ PropertyChanged=�5Q        5T      $	 Loại Sản Phẩm��6%   $	
 150 5V       5U      �� *LoaiSanPhamID+$ PropertyChanged=�5V        5Y      $	 Chất Liệu��6%   $	
 150 5[       5Z      �� *
ChatLieu+$ PropertyChanged=�5[        5^      $	 Nhà Sản Xuất��6%   $	
 200 5`       5_      �� *
NhaSanXuat+$ PropertyChanged=�5`      5b      �� 5e   
   $  
Horizontal=�6   $	��Top=�5f      $
��Left=�5g      $��340,560,0,0q�5h      $��39��6*   $	��390��66   .+�5i      �� -   5        $��Thêm��5i      $��90��5j      $��5q�5k      ���#FFB100
����������#FFB100$��1q�5o      5p      �� 5q      -   5        $��Sửa��5q      $��90��5r      $��5q�5s      ���#FFB100
����������#FFB100$��1q�5w      5x      �� 5y      -   5        $
��Xóa��5y      $��90��5z      $��5q�5{      ���#FFB100
����������#FFB100$��1q�5      5�      5�      �� 5�   
   $B��<Bản quyền © 2024 - Hệ Thống Quản Lý Sản Phẩm��6   $��Bottom=�5�      $��Center=�5�      $��0,0,0,10q�5�      
�������5�      5�      5�      