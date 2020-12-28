using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Core.Common
{
    public static class HamDungChung
    {
        

        /// <summary>
        /// Get DataTable from excel file
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string excelFilePath, string tableName)
        {
            string connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", excelFilePath);
            //string connectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", excelFilePath);
            using (OleDbConnection cnn = new OleDbConnection(connectionString))
            {
                string query = String.Format("Select * from {0}", tableName);
                using (OleDbDataAdapter da = new OleDbDataAdapter(query, cnn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Set Time for DateTime value (use in Tax calculator)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="style">0: start hour of day, 1: end hour of day, 2: start day of month, 3: end day of month, 4: start month of year, 5: end month of year</param>
        /// <returns></returns>
        public static DateTime SetTime(DateTime source, int style)
        {
            int hh, mm, ss;
            if (style == 0)
            {
                hh = source.Hour;
                mm = source.Minute;
                ss = source.Second;

                source = source.AddHours(-hh);
                source = source.AddMinutes(-mm);
                source = source.AddSeconds(-ss);
            }
            else if (style == 1)
            {
                hh = 23 - source.Hour;
                mm = 59 - source.Minute;
                ss = 59 - source.Second;

                source = source.AddHours(hh);
                source = source.AddMinutes(mm);
                source = source.AddSeconds(ss);
            }
            else if (style == 2)
            {
                source = new DateTime(source.Year, source.Month, 1);
                source = SetTime(source, 0);
            }
            else if (style == 3)
            {
                source = new DateTime(source.Year, source.Month, 1).AddMonths(1).AddDays(-1);
                source = SetTime(source, 1);
            }
            else if (style == 4)
            {
                source = new DateTime(source.Year, 1, 1);
                source = SetTime(source, 0);
            }
            else if (style == 5)
            {
                source = new DateTime(source.Year, 12, 31);
                source = SetTime(source, 1);
            }

            return source;
        }

        /// <summary>
        /// Get date number 
        /// </summary>
        /// <param name="tungay"></param>
        /// <param name="dengnay"></param>
        /// <returns></returns>
        public static decimal GetBusinessDays(DateTime fromDate, DateTime toDate)
        {
            decimal result = 0;
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
                    result++;
            }
            return result;
        } 
        
        /// <summary>
          /// Get date number 
          /// </summary>
          /// <param name="tungay"></param>
          /// <param name="dengnay"></param>
          /// <returns></returns>
        public static decimal GetBusinessDays1(DateTime fromDate, DateTime toDate)
        {
            decimal result = 0;
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                    result++;
            }
            return result;
        }
    }

}
