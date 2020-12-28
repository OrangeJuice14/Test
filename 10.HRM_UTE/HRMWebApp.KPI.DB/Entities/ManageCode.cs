using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ManageCode
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int StaffType { get; set; }
        public virtual ProfessorCriterion ProfessorCriterion { get; set; }
    }
}
