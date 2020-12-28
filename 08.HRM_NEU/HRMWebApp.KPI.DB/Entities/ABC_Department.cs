using System;
using System.Collections.Generic;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Department
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int? GCRecord { get; set; }
        //public virtual int OrderNumber { get; set; }
        //public virtual bool IsDisabled { get; set; }
        //public virtual int DepartmentType { get; set; }
        public virtual bool IsDisable { get; set; }
        //public virtual string ManageCode { get; set; }
        public virtual ABC_Department ParentDepartment { get; set; }
    }
}
