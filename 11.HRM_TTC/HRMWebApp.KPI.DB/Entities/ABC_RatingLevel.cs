using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_RatingLevel
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Color { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual IList<ABC_Criterion> ABC_Criterions { get; set; }
    }
}
