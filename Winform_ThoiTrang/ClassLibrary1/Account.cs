using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Account
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public byte Role { get; set; } // Role dùng kiểu `byte` để tương ứng với `tinyint` trong SQL
    }

}
