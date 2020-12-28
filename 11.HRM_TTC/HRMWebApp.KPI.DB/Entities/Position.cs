using System;
using System.Collections.Generic;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Position
    {

        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double HSPCChucVu { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual int? GCRecord { get; set; }
        public virtual bool LaQuanLy { get; set; }
        public virtual WebGroup WebGroup { get; set; }
    }
}
