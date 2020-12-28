using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_NgayLamBuTrongNam_Find
    {
        [DataMember]
        public System.Guid Oid { get; set; }
        [DataMember]
        public Nullable<System.Guid> QuanLyLamBuTrongNam { get; set; }
        [DataMember]
        public string TenNgayLamBu { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayLamBu { get; set; }
        [DataMember]
        public int GCRecord { get; set; }

    }
}
