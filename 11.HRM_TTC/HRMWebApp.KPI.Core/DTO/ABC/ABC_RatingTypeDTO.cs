using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC
{
    public class ABC_RatingTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public List<Guid> CriterionIds { get; set; }
    }
}
