using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class spd_Service_GetSoChungTuHienWeb
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? KyTinhLuong { get; set; }
    }
}