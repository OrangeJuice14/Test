using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_CriterionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CriterionType { get; set; }
        public int OrderNumber { get; set; }

    }
}
