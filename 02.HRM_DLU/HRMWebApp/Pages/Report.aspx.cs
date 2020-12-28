using Microsoft.Reporting.WebForms;
using MyUisCoreUIS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;

namespace MyUIS_MVC.Pages
{
    public partial class Report : System.Web.UI.Page
    {
        #region Declare
        string Loai = string.Empty;
        string ScheduleStudyUnitID = string.Empty;
        string Examination = string.Empty;
        string YearStudy = "", TermID = "";
        MyUISBL MyUIS = new MyUISBL();
        #endregion

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            string staffManageCode = "";
            try {
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                    Session["UIS_MemberID"] = staffManageCode;
                });
            }
            catch { }
            if (Session["UIS_MemberID"] == null || Session["UIS_MemberID"] == "")
            {
                Response.Redirect("~/", true);
                return;
            }

            ScheduleStudyUnitID = Request["ScheduleStudyUnitID"];
            Loai = Request["Loai"];
            
            if (!IsPostBack)
            {
                if (Loai == "1" && ScheduleStudyUnitID != null)
                {
                    XuatBangDiemQuaTrinh(ScheduleStudyUnitID);
                }
                else if (Loai == "2")
                {
                    int Examination = int.Parse(Request["Examination"]);
                    XuatBangDiemThiTheoPhong(Examination);
                }
                else if (Loai == "3" && ScheduleStudyUnitID != null)
                {
                    XuatBangDiemThiHocPhan(ScheduleStudyUnitID);
                }
                 
            }
        }
        #endregion

        #region Function
        protected void XuatBangDiemQuaTrinh(string ScheduleStudyUnitID)
        {
            try
            {
                //////////////////////////////////////////////
                DataTable a = new DataTable();
                a = MyUIS.ThongTinLopHocPhan_Print(ScheduleStudyUnitID);
                /////////////////////////////////////////////////////////
                DataTable tbSource = new DataTable();
                tbSource = MyUIS.StudentScheduleStudyUnits_Print_Sel(ScheduleStudyUnitID);
                tbSource.TableName = "DS_psc_BangDiemQuaTrinh";
                DataTable dtReportConfig = MyUIS.ReportConfig();

                //ReportViewer ReportViewer1 = new ReportViewer();

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("NamHoc", a.Rows[0]["Namhoc"].ToString()));
                parameters.Add(new ReportParameter("HocKy", a.Rows[0]["Hocky"].ToString()));
                parameters.Add(new ReportParameter("Mon_NhomMon", a.Rows[0]["TenHP"].ToString()));
                parameters.Add(new ReportParameter("MaMon", a.Rows[0]["CurriculumID"].ToString() + " - " + a.Rows[0]["GroupID"].ToString()));
                parameters.Add(new ReportParameter("STC", a.Rows[0]["Credits"].ToString()));
                parameters.Add(new ReportParameter("CBGD", a.Rows[0]["ListofprofessorName"].ToString() + "(" + a.Rows[0]["ListofprofessorID"].ToString() + ")"));
                parameters.Add(new ReportParameter("QuaTrinh", a.Rows[0]["Assignmentdetail"].ToString()));
                string NgayThangNam = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                parameters.Add(new ReportParameter("NgayThangNam", NgayThangNam));
                parameters.Add(new ReportParameter("GhiChu", ""));
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Pages/Reports/BangDiemQuaTrinh.rdlc");
                ReportViewer1.LocalReport.DisplayName = "Bảng điểm quá trình";
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_psc_BangDiemQuaTrinh", tbSource));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_psc_ReportConfig", dtReportConfig));
                ReportViewer1.LocalReport.SetParameters(parameters);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension = "pdf";

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "pdf", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                ////////////////////////////
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "BangDiemQuaTrinh_"+ScheduleStudyUnitID + "." + extension);
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = "application/pdf";
                Response.StatusDescription = ScheduleStudyUnitID;
                Response.AppendHeader("", "");
                Response.BinaryWrite(bytes);
                Response.AppendHeader("", "");
                Response.End();

            }
            catch
            {
            }
        }

        protected void XuatBangDiemThiTheoPhong(int Examination)
        {
            try
            {
                DataTable a = new DataTable();
                a = MyUIS.ExaminationScheduleStudyUnits_Sel_ScheduleInfo(Examination.ToString());
                /////////////////////////////////////////////////////////
                DataTable tbSource = new DataTable();
                tbSource = MyUIS.StudentExamination_Sel_Marks(Examination.ToString());
                DataTable dtReportConfig = MyUIS.ReportConfig();

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("NamHoc", a.Rows[0]["Namhoc"].ToString()));
                parameters.Add(new ReportParameter("HocKy", a.Rows[0]["Hocky"].ToString()));
                parameters.Add(new ReportParameter("KhoaHoc", a.Rows[0]["Khoahoc"].ToString()));
                parameters.Add(new ReportParameter("He", a.Rows[0]["Hedaotao"].ToString()));
                parameters.Add(new ReportParameter("NganhChuyenNganh", a.Rows[0]["Nganh"].ToString()));
                parameters.Add(new ReportParameter("HocPhan", a.Rows[0]["MaHP"].ToString() + " - " + a.Rows[0]["TenHP"].ToString()));
                parameters.Add(new ReportParameter("LopHP", a.Rows[0]["LopHocPhan"].ToString()));
                parameters.Add(new ReportParameter("TGH", "Bắt đầu :" + a.Rows[0]["TGBatdau"].ToString() + " Kết thúc:" + a.Rows[0]["TGKetthuc"].ToString()));
                parameters.Add(new ReportParameter("TKB", a.Rows[0]["TKB1"].ToString()));
                string NgayThangNam = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                parameters.Add(new ReportParameter("NgayThangNam", NgayThangNam));
                parameters.Add(new ReportParameter("GhiChu", ""));

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Pages/Reports/BangDiemTheoPhong.rdlc");
                ReportViewer1.LocalReport.DisplayName = "Bảng điểm ";
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_sp_psc_StudentExamination_Sel_Marks", tbSource));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_psc_ReportConfig", dtReportConfig));
                ReportViewer1.LocalReport.SetParameters(parameters);

                ReportViewer1.LocalReport.Refresh();

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension = "pdf";

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "pdf", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                ////////////////////////////
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "BangDiem_" + ScheduleStudyUnitID + "." + extension);
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = "application/pdf";
                Response.StatusDescription = ScheduleStudyUnitID;
                Response.AppendHeader("", "");
                Response.BinaryWrite(bytes);
                Response.AppendHeader("", "");
                Response.End();
            }
            catch { }
        }

        protected void XuatBangDiemThiHocPhan(string ScheduleStudyUnitID)
        {
            try
            {
                ///////////////////////////////////////////////
                DataTable a = new DataTable();
                a = MyUIS.ThongTinLopHocPhan_Print(ScheduleStudyUnitID);
                /////////////////////////////////////////////////////////
                DataTable tbSource = new DataTable();
                tbSource = MyUIS.StudentScheduleStudyUnits_Print_Sel(ScheduleStudyUnitID);
                tbSource.TableName = "DS_psc_BangDiemQuaTrinh";
                DataTable dtReportConfig = MyUIS.ReportConfig();

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("NamHoc", a.Rows[0]["Namhoc"].ToString()));
                parameters.Add(new ReportParameter("HocKy", a.Rows[0]["Hocky"].ToString()));
                parameters.Add(new ReportParameter("KhoaHoc", a.Rows[0]["Khoahoc"].ToString()));
                parameters.Add(new ReportParameter("He", a.Rows[0]["Hedaotao"].ToString()));
                parameters.Add(new ReportParameter("NganhChuyenNganh", a.Rows[0]["Nganh"].ToString()));
                parameters.Add(new ReportParameter("HocPhan", a.Rows[0]["MaHP"].ToString() + " - " + a.Rows[0]["TenHP"].ToString()));
                parameters.Add(new ReportParameter("LopHP", a.Rows[0]["LopHocPhan"].ToString()));
                parameters.Add(new ReportParameter("TGH", "Bắt đầu :" + a.Rows[0]["TGBatdau"].ToString() + " Kết thúc:" + a.Rows[0]["TGKetthuc"].ToString()));
                parameters.Add(new ReportParameter("TKB", a.Rows[0]["TKB1"].ToString()));
                string NgayThangNam = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                parameters.Add(new ReportParameter("NgayThangNam", NgayThangNam));
                parameters.Add(new ReportParameter("GhiChu", ""));

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Pages/Reports/BangDiemTheoPhong.rdlc");
                ReportViewer1.LocalReport.DisplayName = "Bảng điểm thi theo lớp HP";
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_sp_psc_StudentExamination_Sel_Marks", tbSource));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DS_psc_ReportConfig", dtReportConfig));
                ReportViewer1.LocalReport.SetParameters(parameters);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension = "pdf";

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   "pdf", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                ////////////////////////////
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "BangDiem_" + ScheduleStudyUnitID + "." + extension);
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.ContentType = "application/pdf";
                Response.StatusDescription = ScheduleStudyUnitID;
                Response.AppendHeader("", "");
                Response.BinaryWrite(bytes);
                Response.AppendHeader("", "");
                Response.End();
            }
            catch
            {
            }
        }

       
        #endregion
    }
}