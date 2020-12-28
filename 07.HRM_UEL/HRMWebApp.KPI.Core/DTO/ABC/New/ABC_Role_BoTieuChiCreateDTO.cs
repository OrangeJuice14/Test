using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_Role_BoTieuChiCreateDTO
    {
        public Guid Id { get; set; }
        public Guid? GroupTuDanhGiaId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public bool? UserDanhGiaNgangHang { get; set; }
    }
}
