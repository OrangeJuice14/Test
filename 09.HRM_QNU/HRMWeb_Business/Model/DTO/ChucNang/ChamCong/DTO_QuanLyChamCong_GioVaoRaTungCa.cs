using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    [DataContract]
    public partial class DTO_QuanLyChamCong_GioVaoRaTungCa
    {
        [DataMember]
        public Guid NhanVienId { get; set; }
        [DataMember]
        public DateTime? Ngay { get; set; }
        [DataMember]
        public string GioQuet { get; set; }
        [DataMember]
        public int? SoLanQuet { get; set; }
    }
}
