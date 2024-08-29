using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LoaiSanPham
    {
        public int LoaiSanPhamID { get; set; }
        public string TenLoai { get; set; }
        public int? KichThuocID { get; set; }
    }
}
