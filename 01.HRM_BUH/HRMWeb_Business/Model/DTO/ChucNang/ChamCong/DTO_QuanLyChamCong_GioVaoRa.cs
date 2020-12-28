using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    [DataContract]
    public partial class DTO_QuanLyChamCong_GioVaoRa
    {
        [DataMember]
        public Guid? NhanVienID { get; set; }
        [DataMember]
        public int? MaNhanSu { get; set; }
        [DataMember]
        public String HoTen { get; set; }
        [DataMember]
        public String BoPhan { get; set; }
        [DataMember]
        public List<DTO_QuanLyChamCong_GioVaoRaTungCa> ChiTietVaoRa { get; set; }
    }
}
