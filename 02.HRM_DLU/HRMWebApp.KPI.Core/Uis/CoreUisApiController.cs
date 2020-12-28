using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.Core.Security;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HRMWebApp.Helpers;

namespace HRMWebApp.KPI.Core.Uis
{
    public class ProfessorInfo
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string HocVi { get; set; }
        public string HocHam { get; set; }
        public string NgaySinh { get; set; }
        public string ChucVu { get; set; }
        public string NoiSinh { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string CMND { get; set; }
        public string NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string LoaiGiangVien { get; set; }
        public string ChuyenNganh { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHang { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChiThuongTru { get; set; }
        public string DienThoai { get; set; }
        public string DiDong { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
    }
    public class StudyYear
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class StudyTerm
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class StudyWeek
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ScheduleStudy
    {
        public string MaHocPhan { get; set; }
        public string MaMonHoc { get; set; }
        public string TenHocPhan { get; set; }
        public string SoTinChi { get; set; }
        public string HeDaoTao { get; set; }
        public string Buoi { get; set; }
        public string SiSo { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
    }

    public class SchedtudyUnitProfessors
    {
        public string ScheduleStudyUnitID { get; set; }
        public string CurriculumID { get; set; }
        public string StudyUnitID { get; set; }
        public string CurriculumName { get; set; }
        public string NumberOfStudents { get; set; }
        public string GraduateLevelName { get; set; }

        public string ListOfClassStudentName { get; set; }
        public string ThoiHanNhapDiemQT { get; set; }
        public string ThoiHanNhapDiemThi { get; set; }
    }
    public class ExaminationProfessors
    {
        public string Examination { get; set; }
        public string ScheduleStudyUnitID { get; set; }
        public string CurriculumID { get; set; }
        public string ScheduleStudyUnitAlias { get; set; }
        public string CurriculumName { get; set; }
        public string RoomID { get; set; }
        public string ThoiHanNhapDiemThi { get; set; }
        public string ListOfClassStudentName { get; set; }
        public string ExamDate { get; set; }
    }

    public class StudentScheduleStudyUnits
    {
        public string StudentID { get; set; }
        public string LastMiddleName { get; set; }
        public string FirstName { get; set; }
        public string BirthDay { get; set; }
        public string ASS04 { get; set; }
        public string Mark10 { get; set; }
        public string Mark10_2 { get; set; }
        public string Mark10_TH { get; set; }
        public string Mark10_2TH { get; set; }
    }

    public class GiaHanNhapDiem
    {
        public string OrderNumber { get; set; }
        public string ScheduleStudyUnitAlias { get; set; }
        public string ScheduleStudyUnitID { get; set; }
        public string CurriculumID { get; set; }
        public string CurriculumName { get; set; }
        public string LoaiHocPhan { get; set; }
        public string NgayNhap { get; set; }
        public string Credits { get; set; }
        public string HanCuoiNhapDiem { get; set; }
        public string HanCuoiGiaHan { get; set; }
        public string NgayGiaHan { get; set; }
        public string ExamDate { get; set; }
        public string RoomID { get; set; }
        public string ID { get; set; }

        public string TypeND { get; set; }
    }

    public class LoaiDiem
    {
        public string Type { get; set; }
        public string TypeName { get; set; }
    }
    public class KetQuaReval
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class DotThanhToan
    {
        public string MaCauHinhGioChot { get; set; }
        public string TenDot { get; set; }
    }

    public class ChiTietThuLaoCoHuu
    {

        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string LoaiHocPhan { get; set; }
        public string MalopHocPhan { get; set; }
        public string Nhom { get; set; }
        public string MaLop { get; set; }
        public string SiSo { get; set; }
        public string TietThucDay { get; set; }
        public string HeSoLopDong { get; set; }
        public string HeSoChucDanhChuyenMon { get; set; }
        public string HeSoBacDaoTao { get; set; }
        public string HeSoThucHanh { get; set; }
        public string TietQuyDoi { get; set; }
        public string MaGiangVien { get; set; }
    }
    public class TongThuLaoCoHuu
    {
        public string SoGioQuyDoi { get; set; }
        public string SoGioQuyDoiKhoiLuongCongThem { get; set; }
        public string TongGioGiang { get; set; }
        public string GioNckh { get; set; }
        public string GioNghiaVuGiangDay { get; set; }
        public string GioNghiaVuNckh { get; set; }
        public string SoTietThanhToan { get; set; }
        public string DonGia { get; set; }
        public string SoTien { get; set; }
    }

    public class ThuLaoThinhGiang
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string LoaiHocPhan { get; set; }
        public string MaLopHocPhan { get; set; }
        public string Nhom { get; set; }
        public string MaLop { get; set; }
        public string SiSo { get; set; }
        public string NoiDung { get; set; }
        public string TietThucDay { get; set; }
        public string HeSoQDGC { get; set; }
        public string HeSoChucDanhChuyenMon { get; set; }
        public string CongHeSo { get; set; }
        public string DonGia { get; set; }
        public string ThanhTien { get; set; }
        public string Thue { get; set; }
        public string ConNhan { get; set; }
        public string GhiChu { get; set; }
    }



    public class CoreUIsApiController : ApiController
    {

        public string CoreUisConnectionString = ConfigurationManager.ConnectionStrings["CoreUisConnnection"].ToString();//   @"Data Source=192.168.1.169\DEV;Initial Catalog=CoreUis;Persist Security Info=True;User ID=sa;Password=psc@123";
        public string MyUISConnectionString = ConfigurationManager.ConnectionStrings["MyUISConnection"].ToString();


        [Authorize]
        [Route("GetProfessorInfo")]
        public ProfessorInfo GetProfessorInfo()
        {
            ProfessorInfo result = new ProfessorInfo();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = CoreUisConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_Professors_Sel";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staff.StaffProfile.Email));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {

                        ProfessorInfo item = new ProfessorInfo();
                        result.Id = reader["ProfessorId"].ToString();
                        result.HoTen = reader["FullName"].ToString();
                        result.HocVi = reader["AcademicDegreeName"].ToString();
                        result.HocHam = reader["AcademicTitleName"].ToString();
                        result.NgaySinh = reader["BirthDay"].ToString();
                        //result.ChucVu = reader["ProfessorId"].ToString();
                        result.NoiSinh = reader["BirthPlace"].ToString();
                        result.GioiTinh = reader["Genders"].ToString();
                        result.DanToc = reader["EthnicName"].ToString();
                        result.TonGiao = reader["ReligionName"].ToString();
                        result.CMND = reader["IDCard"].ToString();
                        result.NgayCap = reader["IssueDate"].ToString();
                        result.NoiCap = reader["IssuePlace"].ToString();
                        result.LoaiGiangVien = reader["ProfessorTypeName"].ToString();
                        result.ChuyenNganh = reader["ChuyenNganh"].ToString();
                        result.SoTaiKhoan = reader["SoTaiKhoan"].ToString();
                        result.NganHang = reader["TenNganHang"].ToString();
                        result.MaSoThue = reader["MaSoThue"].ToString();
                        result.DiaChiThuongTru = reader["ContactAddress"].ToString();
                        result.DienThoai = reader["HomePhone"].ToString();
                        result.DiDong = reader["MobilePhone"].ToString();
                        result.Email = reader["Email"].ToString();
                        result.DiaChi = reader["ContactAddress"].ToString();
                        //result = Convert.ToInt32(reader["totalStudents"].ToString());
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("GetProfessorScheduleStudy")]
        public List<ScheduleStudy> GetProfessorScheduleStudy(string yearStudy, string termID)
        {
            List<ScheduleStudy> result = new List<ScheduleStudy>();
            if (yearStudy == null)
                return result;
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = CoreUisConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_ProfessorScheduleStudyUnits_sel";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staff.StaffProfile.ManageCode));
                    sqlCom.Parameters.Add(new SqlParameter("@YearStudy", yearStudy));
                    sqlCom.Parameters.Add(new SqlParameter("@TermID", termID));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        ScheduleStudy item = new ScheduleStudy()
                        {
                            MaHocPhan = reader["ScheduleStudyUnitAlias"].ToString(),
                            MaMonHoc = reader["ScheduleStudyUnitID"].ToString(),
                            TenHocPhan = reader["CurriculumName"].ToString(),
                            HeDaoTao = reader["He"].ToString(),
                            SoTinChi = reader["Credits"].ToString(),
                            SiSo = reader["NumberOfStudents"].ToString(),
                            NgayBatDau = reader["BeginDate"].ToString(),
                            NgayKetThuc = reader["EndDate"].ToString()
                        };
                        result.Add(item);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("GetListStudyYear")]
        public IEnumerable<StudyYear> GetListStudyYear()
        {
            List<StudyYear> result = new List<StudyYear>();
            SessionManager.DoWork(session =>
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_YearStudyAndTermByStudentID";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@StudentID", ""));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyYear item = new StudyYear();
                        item.Id = reader["YearStudy"].ToString();
                        item.Name = reader["YearStudy"].ToString();
                        result.Add(item);
                    }
                }
                result = result.GroupBy(r => r.Id).Select(group => group.First()).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("GetCurrentStudyYear")]
        public Dictionary<string, object> GetCurrentStudyYear()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var test = Helper.WeekOfYear(DateTime.Now);
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_YearStudyAndTermByStudentID";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@StudentID", ""));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    StudyYear item = new StudyYear();
                    if (reader["currentYearStudy"] != null && reader["currentYearStudy"].ToString() != string.Empty && reader["currentTermID"] != null && reader["currentTermID"].ToString() != string.Empty)
                    {
                        result["currentYearStudy"] = reader["currentYearStudy"].ToString();
                        result["currentTerm"] = reader["currentTermID"].ToString();
                        break;

                    }
                }
            }

            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_Weeks_Sel";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@YearStudy", result["currentYearStudy"].ToString()));
                sqlCom.Parameters.Add(new SqlParameter("@TermID", result["currentTerm"].ToString()));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        StudyYear item = new StudyYear();
                        if (reader["WeekOfYear"] != null && reader["WeekOfYear"].ToString() != string.Empty)
                        {
                            result["currentWeek"] = reader["WeekOfYear"].ToString();
                            break;

                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }




            return result;
        }

        [Authorize]
        [Route("GetListStudyTerm")]
        public IEnumerable<StudyTerm> GetListStudyTerm()
        {
            List<StudyTerm> result = new List<StudyTerm>();
            SessionManager.DoWork(session =>
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_YearStudyAndTermByStudentID";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@StudentID", ""));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyTerm item = new StudyTerm();
                        item.Id = reader["TermID"].ToString();
                        item.Name = reader["TermName"].ToString();
                        result.Add(item);
                    }
                }
                result = result.GroupBy(r => r.Id).Select(group => group.First()).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("GetListStudyWeek")]
        public IEnumerable<StudyWeek> GetListStudyWeek(string YearStudy, string TermID)
        {
            List<StudyWeek> result = new List<StudyWeek>();
            SessionManager.DoWork(session =>
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_Weeks_Sel";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@YearStudy", YearStudy == null ? "" : YearStudy));
                    sqlCom.Parameters.Add(new SqlParameter("@TermID", TermID == null ? "" : TermID));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        StudyWeek item = new StudyWeek();
                        item.Id = reader["Week"].ToString();
                        item.Name = reader["DisPlayWeek"].ToString();
                        item.StartDate = reader["beginDate"].ToString();
                        item.EndDate = reader["endDate"].ToString();
                        result.Add(item);
                    }
                }
            });
            return result;
        }


        /// <summary>
        /// Tai modify at 14/10/2016
        /// Detail:
        ///     upate singelOrDefaul = FirstOrDefault at posision (14102016)
        /// </summary>
        /// <param name="YearStudy"></param>
        /// <param name="TermID"></param>
        /// <param name="Week"></param>
        /// <returns></returns>
        [Authorize]
        [Route("GetProfessorSchedule")]
        public ProfessorScheduleTotalDTO GetProfessorSchedule(string YearStudy, string TermID, string Week)
        {
            ProfessorScheduleTotalDTO result = new ProfessorScheduleTotalDTO();
            var staffManageCode = "";
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                staffManageCode = staff.StaffProfile.ManageCode;
            });
            List<ProfessorScheduleDetailDTO> listSche = new List<ProfessorScheduleDetailDTO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_ProfessorWeekSchedules_sel";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staffManageCode));
                sqlCom.Parameters.Add(new SqlParameter("@YearStudy", YearStudy));
                sqlCom.Parameters.Add(new SqlParameter("@TermID", TermID));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    ProfessorScheduleDetailDTO item = new ProfessorScheduleDetailDTO();
                    item.Buoi = reader["ShiftName"].ToString();
                    item.DisPlayWeek = reader["DisPlayWeek"].ToString();
                    item.Year = reader["Year"].ToString();
                    item.Week = reader["Week"].ToString();
                    item.YearStudy = reader["YearStudy"].ToString();
                    item.TermID = reader["TermID"].ToString();
                    item.RoomID = reader["RoomID"].ToString();
                    item.Date = reader["Date"].ToString();
                    item.TKBHienThi = reader["TKBHienThi"].ToString();
                    item.DayOfWeek = reader["DayOfWeek"].ToString();
                    item.Color = reader["Color"].ToString();
                    listSche.Add(item);
                }
            }

            listSche = listSche.Where(a => a.Week == Week).ToList();
            int WoY = Helper.WeekOfYear(DateTime.Now) + 1;
            int DoW = (int)DateTime.Now.DayOfWeek;
            //Thứ hai
            var monsList = listSche.Where(a => a.DayOfWeek == "1");
            result.Mons = new ProfessorScheduleDTO();
            result.Mons.DayOfWeekName = "Thứ hai";

            // posision 14102016
            if (Convert.ToInt32(Week) == WoY && DoW == 1)
                result.Mons.IsToday = true;
            result.Mons.ScheduleMorning = monsList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Mons.ScheduleAfternoon = monsList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Mons.ScheduleNight = monsList.FirstOrDefault(a => a.Buoi == "Tối");

            //Thứ ba
            var tuesList = listSche.Where(a => a.DayOfWeek == "2");
            result.Tues = new ProfessorScheduleDTO();
            if (Convert.ToInt32(Week) == WoY && DoW == 2)
                result.Tues.IsToday = true;
            result.Tues.DayOfWeekName = "Thứ ba";
            result.Tues.ScheduleMorning = tuesList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Tues.ScheduleAfternoon = tuesList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Tues.ScheduleNight = tuesList.FirstOrDefault(a => a.Buoi == "Tối");

            //Thứ tư
            var wedList = listSche.Where(a => a.DayOfWeek == "3");
            result.Weds = new ProfessorScheduleDTO();
            if (Convert.ToInt32(Week) == WoY && DoW == 3)
                result.Weds.IsToday = true;
            result.Weds.DayOfWeekName = "Thứ tư";
            result.Weds.ScheduleMorning = wedList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Weds.ScheduleAfternoon = wedList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Weds.ScheduleNight = wedList.FirstOrDefault(a => a.Buoi == "Tối");

            //Thứ năm
            var thurList = listSche.Where(a => a.DayOfWeek == "4");
            result.Thus = new ProfessorScheduleDTO();
            if (Convert.ToInt32(Week) == WoY && DoW == 4)
                result.Fris.IsToday = true;
            result.Thus.DayOfWeekName = "Thứ năm";
            result.Thus.ScheduleMorning = thurList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Thus.ScheduleAfternoon = thurList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Thus.ScheduleNight = thurList.FirstOrDefault(a => a.Buoi == "Tối");

            //Thứ sáu
            var friList = listSche.Where(a => a.DayOfWeek == "5");
            result.Fris = new ProfessorScheduleDTO();
            if (Convert.ToInt32(Week) == WoY && DoW == 5)
                result.Fris.IsToday = true;
            result.Fris.DayOfWeekName = "Thứ sáu";
            result.Fris.ScheduleMorning = friList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Fris.ScheduleAfternoon = friList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Fris.ScheduleNight = friList.FirstOrDefault(a => a.Buoi == "Tối");

            //Thứ bảy
            var satList = listSche.Where(a => a.DayOfWeek == "6");
            result.Sats = new ProfessorScheduleDTO();
            if (Convert.ToInt32(Week) == WoY && DoW == 6)
                result.Sats.IsToday = true;

            result.Sats.DayOfWeekName = "Thứ bảy";
            result.Sats.ScheduleMorning = satList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Sats.ScheduleAfternoon = satList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Sats.ScheduleNight = satList.FirstOrDefault(a => a.Buoi == "Tối");

            //Chủ nhật
            var sunList = listSche.Where(a => a.DayOfWeek == "7");
            result.Suns = new ProfessorScheduleDTO();
            result.Suns.DayOfWeekName = "Chủ nhật";
            if (Convert.ToInt32(Week) == WoY && DoW == 0)
                result.Suns.IsToday = true;

            result.Suns.ScheduleMorning = sunList.FirstOrDefault(a => a.Buoi == "Sáng");
            result.Suns.ScheduleAfternoon = sunList.FirstOrDefault(a => a.Buoi == "Chiều");
            result.Suns.ScheduleNight = sunList.FirstOrDefault(a => a.Buoi == "Tối");

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("GetScheduleStudyUnitsToProfessor")]
        public List<SchedtudyUnitProfessors> GetScheduleStudyUnitsToProfessor(string YearStudy, string TermID)
        {
            List<SchedtudyUnitProfessors> result = new List<SchedtudyUnitProfessors>();
            var staffManageCode = "";
            SessionManager.DoWork(session =>
            {
                Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                staffManageCode = staff1.StaffProfile.ManageCode;
            });
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_ScheduleStudyUnitsToProfessor_sel";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staffManageCode));
                sqlCom.Parameters.Add(new SqlParameter("@YearStudy", YearStudy));
                sqlCom.Parameters.Add(new SqlParameter("@TermID", TermID));
                SqlDataReader reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    SchedtudyUnitProfessors item = new SchedtudyUnitProfessors()
                    {
                        ScheduleStudyUnitID = reader["ScheduleStudyUnitID"].ToString(),
                        CurriculumID = reader["CurriculumID"].ToString(),
                        StudyUnitID = reader["StudyUnitID"].ToString(),
                        CurriculumName = reader["CurriculumName"].ToString(),
                        NumberOfStudents = reader["NumberOfStudents"].ToString(),
                        GraduateLevelName = reader["GraduateLevelName"].ToString(),
                        ThoiHanNhapDiemQT = reader["ThoiHanNhapDiemQT"].ToString(),
                        ThoiHanNhapDiemThi = reader["ThoiHanNhapDiemThi"].ToString(),
                        ListOfClassStudentName = reader["ListOfClassStudentName"].ToString()
                    };
                    result.Add(item);
                }
            }

            return result;
        }


        [HttpGet]
        [Authorize]
        [Route("GetExamiationProfessor")]
        public List<ExaminationProfessors> GetExamiationProfessor(string YearStudy, string TermID)
        {
            List<ExaminationProfessors> result = new List<ExaminationProfessors>();
            var staffManageCode = "";
            SessionManager.DoWork(session =>
            {
                Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                staffManageCode = staff1.StaffProfile.ManageCode;
            });
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_GetExamiationProfessor_Sel";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staffManageCode));
                sqlCom.Parameters.Add(new SqlParameter("@YearStudy", YearStudy));
                sqlCom.Parameters.Add(new SqlParameter("@TermID", TermID));
                SqlDataReader reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    ExaminationProfessors item = new ExaminationProfessors()
                    {
                        Examination = reader["Examination"].ToString(),
                        ScheduleStudyUnitID = reader["ScheduleStudyUnitID"].ToString(),
                        CurriculumID = reader["CurriculumID"].ToString(),
                        CurriculumName = reader["CurriculumName"].ToString(),
                        ScheduleStudyUnitAlias = reader["ScheduleStudyUnitAlias"].ToString(),
                        RoomID = reader["RoomID"].ToString(),
                        ThoiHanNhapDiemThi = reader["ThoiHanNhapDiemThi"].ToString(),
                        ExamDate = reader["ExamDate"].ToString(),
                        ListOfClassStudentName = reader["ListClassStudents"].ToString()
                    };
                    result.Add(item);
                }
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("StudentScheduleStudyUnits_Sel_Marks")]
        public List<StudentScheduleStudyUnits> StudentScheduleStudyUnits_Sel_Marks(string ScheduleStudyUnitID)
        {
            List<StudentScheduleStudyUnits> result = new List<StudentScheduleStudyUnits>();
            var staffManageCode = "";
            SessionManager.DoWork(session =>
            {
                Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                staffManageCode = staff1.StaffProfile.ManageCode;
            });
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_Online_StudentScheduleStudyUnits_Sel_Marks";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@ScheduleStudyUnitID", ScheduleStudyUnitID));
                SqlDataReader reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    StudentScheduleStudyUnits item = new StudentScheduleStudyUnits()
                    {
                        StudentID = reader["StudentID"].ToString(),
                        LastMiddleName = reader["LastMiddleName"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        BirthDay = reader["BirthDay"].ToString(),
                        ASS04 = reader["ASS04"].ToString(),
                        Mark10 = reader["Mark10"].ToString(),
                        Mark10_2 = reader["Mark10_2"].ToString(),
                        Mark10_TH = reader["Mark10_TH"].ToString(),
                        Mark10_2TH = reader["Mark10_2TH"].ToString()
                    };
                    result.Add(item);
                }
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("GiaHanNhapDiem_sel")]
        public List<GiaHanNhapDiem> GiaHanNhapDiem_sel(string YearStudy, string TermID, string Type)
        {
            List<GiaHanNhapDiem> result = new List<GiaHanNhapDiem>();
            var staffManageCode = "";
            SessionManager.DoWork(session =>
            {
                Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                staffManageCode = staff1.StaffProfile.ManageCode;
            });
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = MyUISConnectionString;
            connection.Open();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "sp_psc_Online_GiaHanNhapDiem_sel";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.Parameters.Add(new SqlParameter("@ProfessorID", staffManageCode));
                sqlCom.Parameters.Add(new SqlParameter("@YearStudy", YearStudy));
                sqlCom.Parameters.Add(new SqlParameter("@TermID", TermID));
                sqlCom.Parameters.Add(new SqlParameter("@Type", Type));
                SqlDataReader reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    GiaHanNhapDiem item = new GiaHanNhapDiem()
                    {
                        OrderNumber = reader["OrderNumber"].ToString(),
                        ScheduleStudyUnitAlias = reader["ScheduleStudyUnitAlias"].ToString(),
                        ScheduleStudyUnitID = reader["ScheduleStudyUnitID"].ToString(),
                        CurriculumID = reader["CurriculumID"].ToString(),
                        CurriculumName = reader["CurriculumName"].ToString(),
                        LoaiHocPhan = reader["LoaiHocPhan"].ToString(),
                        NgayNhap = reader["NgayNhap"].ToString(),
                        Credits = reader["Credits"].ToString(),
                        HanCuoiNhapDiem = reader["HanCuoiNhapDiem"].ToString(),
                        HanCuoiGiaHan = reader["HanCuoiGiaHan"].ToString(),
                        NgayGiaHan = reader["NgayGiaHan"].ToString(),
                        ExamDate = reader["ExamDate"].ToString(),
                        RoomID = reader["RoomID"].ToString(),
                        ID = reader["ID"].ToString(),
                        TypeND = Type
                    };
                    result.Add(item);
                }
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("LoaiNhapDiem_sel")]
        public List<LoaiDiem> LoaiNhapDiem_sel()
        {
            List<LoaiDiem> TypeMarks = new List<LoaiDiem>();
            LoaiDiem item = new LoaiDiem();
            item.Type = "0";
            item.TypeName = "Điểm quá trình";
            TypeMarks.Add(item);

            LoaiDiem item1 = new LoaiDiem();
            item1.Type = "1";
            item1.TypeName = "Điểm thi theo LHP";
            TypeMarks.Add(item1);

            LoaiDiem item2 = new LoaiDiem();
            item2.Type = "2";
            item2.TypeName = "Điểm thi theo lớp thi";
            TypeMarks.Add(item2);

            return TypeMarks;

        }

        [HttpPost]
        [Authorize]
        [Route("GiaHanNhapDiem_Upd")]
        public string GiaHanNhapDiem_Upd(List<GiaHanNhapDiem> lgiahan, string Type)
        {
            try
            {
                string result = "1";
                string _XmlData = "";
                foreach (var it in lgiahan)
                {
                    _XmlData += string.Format("<D ID=\"{0}\" NgayGiaHan=\"{1}\" />", it.ID, it.NgayGiaHan);
                }
                string xml = "<Root>" + _XmlData + "</Root>";
                var staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                });
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_psc_Online_GiaHanNhapDiem_Upd";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@xml", xml));
                    sqlCom.Parameters.Add(new SqlParameter("@Type", Type));

                    SqlParameter paraout;
                    paraout = sqlCom.Parameters.Add("@Reval", SqlDbType.NVarChar, 255);
                    paraout.Direction = ParameterDirection.Output;
                    paraout.Value = "";

                    sqlCom.ExecuteNonQuery();

                    result = paraout.Value.ToString();


                }

                return result;
            }
            catch { return "1"; }
        }

        [HttpGet]
        [Authorize]
        [Route("CauHinhChotGio_GetByNamHocHocKy")]
        public List<DotThanhToan> CauHinhChotGio_GetByNamHocHocKy(string NamHoc, string HocKy)
        {
            try
            {
                List<DotThanhToan> result = new List<DotThanhToan>();
                var staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                });
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "cust_CauHinhChotGio_GetByNamHocHocKy";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));

                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        DotThanhToan item = new DotThanhToan()
                        {
                            MaCauHinhGioChot = reader["MaCauHinhChotGio"].ToString(),
                            TenDot = reader["TenDot"].ToString()

                        };
                        result.Add(item);
                    }

                }

                return result;
            }
            catch { return new List<DotThanhToan>(); }
        }

        [HttpGet]
        [Authorize]
        [Route("KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang0")]
        public List<ChiTietThuLaoCoHuu> KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang0(string NamHoc, string HocKy, string DotThanhToan)
        {
            try
            {
                List<ChiTietThuLaoCoHuu> result = new List<ChiTietThuLaoCoHuu>();
                var staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                });
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "cust_KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@DotThanhToan", DotThanhToan));
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorId", staffManageCode));

                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    da.SelectCommand = sqlCom;
                    da.Fill(ds);

                    result = ds.Tables[0].DataTableToList<ChiTietThuLaoCoHuu>();

                }

                return result;
            }
            catch
            {
                return new List<ChiTietThuLaoCoHuu>();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang1")]
        public List<TongThuLaoCoHuu> KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang1(string NamHoc, string HocKy, string DotThanhToan)
        {
            try
            {
                List<TongThuLaoCoHuu> result = new List<TongThuLaoCoHuu>();
                var staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                });
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "cust_KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@DotThanhToan", DotThanhToan));
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorId", staffManageCode));

                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    da.SelectCommand = sqlCom;
                    da.Fill(ds);

                    result = ds.Tables[1].DataTableToList<TongThuLaoCoHuu>();

                }

                return result;
            }
            catch
            {
                return new List<TongThuLaoCoHuu>();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang2")]
        public List<ThuLaoThinhGiang> KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang2(string NamHoc, string HocKy, string DotThanhToan)
        {
            try
            {
                List<ThuLaoThinhGiang> result = new List<ThuLaoThinhGiang>();
                var staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                });
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = MyUISConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "cust_KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@DotThanhToan", DotThanhToan));
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorId", staffManageCode));

                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    da.SelectCommand = sqlCom;
                    da.Fill(ds);

                    result = ds.Tables[2].DataTableToList<ThuLaoThinhGiang>();

                }

                return result;
            }
            catch { return new List<ThuLaoThinhGiang>(); }
        }
    }
}
