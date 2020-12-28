using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ResultDTOs
{
    public class RecordSingleResultDTO
    {
        public string Name { get; set; }
        public double Record { get; set; }
        public double Rate { get; set; }

        public double Record2 { get; set; }
        public double Rate2 { get; set; }

        public double TotalRecord { get; set; }
        public bool IsMainPosition { get; set; }
        public double PositionRate { get; set; }
    }


    public class ResultRecordDTO
    {
        public ResultRecordDTO()
        {
            Results = new List<RecordSingleResultDTO>();
        }
        public List<RecordSingleResultDTO> Results { get; set; }
        public double FinalResult { get; set; }
    }
}
