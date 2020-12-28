using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC
{
    public class ABC_RatingLevelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public Guid CriterionId { get; set; }
    }
}
