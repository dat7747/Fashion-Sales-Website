using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Kho
    {
        [Key]
        public int SanPhamID { get; set; }
        public string KichThuocID { get; set; }
        public int SoLuong { get; set; }
    }

}
