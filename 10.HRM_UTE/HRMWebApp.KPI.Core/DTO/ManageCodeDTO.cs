using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ManageCodeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StaffType { get; set; }
        public ProfessorCriterionDTO ProfessorCriterion { get; set; }
    }
}
