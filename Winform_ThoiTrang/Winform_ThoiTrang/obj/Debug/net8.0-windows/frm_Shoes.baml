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
   -   5          Name$ ProductDataGrid��50      $��20,120,20,60q�51         AutoGenerateColumns$ False��52         
IsReadOnly$
 True��53      
����������#FFB100$��1q�56         Columns 57      4  .System.Windows.Controls.DataGridTemplateColumn  58      ,  &System.Windows.Controls.DataGridColumn   Header$ Hình Ảnh��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6;      CellTemplate 59      �� 5:      .R�5;      �� $��80��5<   $   $��80��5=   $   $��UniformToFill=�5>   $   ��5;   $   �� *	HinhAnh+5>   $   5?      5A      0  *System.Windows.Controls.DataGridTextColumn  5B      $ Tên Sản Phẩm��6%   $ * 5D   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5C   %   �� *
TenSanPham+5D   %     5E      $
 Giá��6%   $	 100 5G   %    5F   %   �� *Gia+5G   %     5H      $ Loại Sản Phẩm��6%   $	 150 5J   %    5I   %   �� *LoaiSanPhamID+5J   %     5K      $ Kích Thước��6%   $	 150 5M   %    5L   %   �� *KichThuocID+5M   %     5N      $ Chất Liệu��6%   $	 150 5P   %    5O   %   �� *
ChatLieu+5P   %     5Q      $ Nhà Sản Xuất��6%   $	 200 5S   %    5R   %   �� *
NhaSanXuat+5S   %   5U      �� 5X   
   $  
Horizontal=�6   $	��Top=�5Y      $
��Left=�5Z      $��273,560,0,0q�5[      $��39��6*   $	��399��66   .+�5\      �� $��Thêm��6   $��90��5]      $��5q�5^      ���#FFB100
����������#FFB100$��1q�5b      �� 5c      $��Sửa��6   $��90��5d      $��5q�5e      ���#FFB100
����������#FFB100$��1q�5i      �� 5j      $
��Xóa��6   $��90��5k      $��5q�5l      ���#FFB100
����������#FFB100$��1q�5p      �� 5q      $
��Lưu��6   $��90��5r      $��5q�5s      ���#FFB100
����������#FFB100$��1q�5w      5x      �� 5{   
   $B��<Bản quyền © 2024 - Hệ Thống Quản Lý Sản Phẩm��6   $��Bottom=�5|      $��Center=�5}      $��0,0,0,10q�5~      
�������5      5�      5�      