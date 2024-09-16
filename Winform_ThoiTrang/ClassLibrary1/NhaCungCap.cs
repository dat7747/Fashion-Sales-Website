using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class NhaCungCap
    {
        [Key]
        public int NhaCungCapID { get; set; }

        [Required]
        [StringLength(255)]
        public string TenNhaCungCap { get; set; }

        [StringLength(500)]
        public string DiaChi { get; set; }

        [StringLength(20)]
        public string SoDienThoai { get; set; }

        [StringLength(255)]
        public string Email { get; set; }
    }

}
