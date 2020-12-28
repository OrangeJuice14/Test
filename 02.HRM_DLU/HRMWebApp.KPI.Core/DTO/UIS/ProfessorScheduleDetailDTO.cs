using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ProfessorScheduleDetailDTO
    {
        public string DisPlayWeek { get; set; }
        public string Year { get; set; }
        public string Week { get; set; }
        public string YearStudy { get; set; }
        public string TermID { get; set; }    
        public string RoomID { get; set; }
        public string Buoi { get; set; }
        public string Date { get; set; }   
        public string TKBHienThi{ get; set; }
        public string DayOfWeek { get; set; }
        public string Color { get; set; }

    }
}
