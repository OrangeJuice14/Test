using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class DTO_ChucVuNhanVien
    {
        [DataMember]
        public Guid? OidChucVuKiemNhiem { get; set; }

        [DataMember]
        public Guid? OidBoPhan { get; set; }

        [DataMember]
        public String TenBoPhan { get; set; }

        [DataMember]
        public Guid? OidChucVu { get; set; }

        [DataMember]
        public String TenChucVu { get; set; }

        [DataMember]
        public String MaQuanLy { get; set; }

        [DataMember]
        public bool? ChucVuChinh { get; set; }
    }
}
