using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ProfessorScheduleTotalDTO
    {
        public ProfessorScheduleTotalDTO()
        {
            ProfessorSchedules = new List<ProfessorScheduleDTO>();
            Mons = new ProfessorScheduleDTO();
            Tues = new ProfessorScheduleDTO();
            Weds = new ProfessorScheduleDTO();
            Thus = new ProfessorScheduleDTO();
            Fris = new ProfessorScheduleDTO();
            Sats = new ProfessorScheduleDTO();
            Suns = new ProfessorScheduleDTO();
        }
        public ProfessorScheduleDTO Mons { get; set; }
        public ProfessorScheduleDTO Tues { get; set; }
        public ProfessorScheduleDTO Weds { get; set; }
        public ProfessorScheduleDTO Thus { get; set; }
        public ProfessorScheduleDTO Fris { get; set; }
        public ProfessorScheduleDTO Sats { get; set; }
        public ProfessorScheduleDTO Suns { get; set; }
        public List<ProfessorScheduleDTO> ProfessorSchedules { get; set; }
    }
}
