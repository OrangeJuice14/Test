using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class PositionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ManageCode { get; set; }
        public int AgentObjectTypeId { get; set; }
       
    }
}
