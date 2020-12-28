using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC.New
{
    public class ABC_BoTieuChiVMDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public bool? Status { get; set; }
        public bool? ShowTen { get; set; }
        public bool? ShowDonVi { get; set; }
        public int? LoaiBoTieuChi { get; set; }// Loai 12: Nam
                                               // Loai 6: 6 tháng
                                               // Loai 3: Quy'
                                               // Loai 1: Thang'
    }
}
