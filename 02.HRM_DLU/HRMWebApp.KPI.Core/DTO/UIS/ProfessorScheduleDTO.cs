using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ProfessorScheduleDTO
    {
        public ProfessorScheduleDTO()
        {
            ScheduleMorning = new ProfessorScheduleDetailDTO();
            ScheduleAfternoon = new ProfessorScheduleDetailDTO();
            ScheduleNight = new ProfessorScheduleDetailDTO();
        }
        public string RoomID { get; set; }
        public bool IsToday { get; set; }
        public string DayOfWeekName { get; set; }
        public ProfessorScheduleDetailDTO ScheduleMorning { get; set; }
        public ProfessorScheduleDetailDTO ScheduleAfternoon { get; set; }
        public ProfessorScheduleDetailDTO ScheduleNight { get; set; }
     
    }
}
