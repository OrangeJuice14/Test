﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Jobs //Công việc
    {
        public virtual Guid Id { get; set; }
        public virtual string ManageCode { get; set; } //Mã quản lý
        public virtual string Name { get; set; } //Tên công việc
        public virtual StaffType StaffType { get; set; } //Loại nhân sự
    }
}