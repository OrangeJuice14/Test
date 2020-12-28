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

namespace MyUIS_MVC.Pages.Professor
{
    public partial class NhapDiemQuaTrinh : System.Web.UI.Page
    {
        MyUISBL MyUIS = new MyUISBL();
        string _SCheduleStudyUnitID = "";
        string _Msg = "";

        #region FUnction
        #region protected DataTable TableAssignmentschedule
        protected DataTable TableAssignmentschedule
        {
            get
            {
                return MyUIS.ScheduleStudyUnitAssignment_Sel(_SCheduleStudyUnitID);
            }
        }
        #endregion

        #region protected DataTable TableStudentSchedule
        protected DataTable TableStudentSchedule
        {
            get
            {
                return MyUIS.StudentScheduleStudyUnitAssignments_Sel(_SCheduleStudyUnitID);
            }
        }
        #endregion
    
        #region Ve luoi nhap diem qua trinh
        public string DrawingTableAssignment()
        {
            string s = string.Empty;
            try
            {
                DataTable a = TableStudentSchedule;
                DataTable asi = TableAssignmentschedule;

                #region tieude table
                s += "<table width='100%' border='1' cellspacing='2' style='border-collapse:collapse'>";
                s += "<tr style='background: #CCC'><th>Mã số</th><th>Lớp</th><th>Họ lót</th><th>Tên</th><th>Ngày sinh</th>";
                foreach (DataRow dr in asi.Rows)
                {
                    s += "<th title='" + dr["AssignmentName"].ToString().Replace("\"", "") + "'>" + dr["Abbreviation"] + "</th>";
                }
                s += "</tr>";
                #endregion

                for (int i = 0; i < a.Rows.Count; i++)
                {
                    DataRow dr = a.Rows[i];
                    DataRow drNext = i < (a.Rows.Count - 1) ? a.Rows[i + 1] : a.Rows[i];
                    s += "<tr class='"+(i%2!=0?"trhover":"")+"'>";
                    s += "<td style='text-align:center'>" + dr["StudentID"] + "</td>";
                    s += "<td style='text-align:center'>" + dr["ClassStudentID"] + "</td>";
                    s += "<td>" + dr["LastName"] + "</td>";
                    s += "<td>" + dr["FirstName"] + "</td>";
                    s += "<td style='text-align:center'>" + dr["Birthday"] + "</td>";
                    foreach (DataRow dr1 in asi.Rows)
                    {
                        if (dr["StatusID"].ToString() == "1")
                        {
                            s += "<td style='text-align:center'> <input type='text' disabled='disabled' style='width:50px;text-align:center' name='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "' id='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "' value='" + dr[dr1["AssignmentID"].ToString()].ToString() + "' > ";
                            s += "</td>";

                        }
                        else
                        {
                            string txt1 = "txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString();
                            string txtNext = "txt_" + dr1["AssignmentID"].ToString() + "_" + drNext["StudentID"].ToString();

                            s += "<td style='text-align:center'> <input type='text' onkeypress=\"" + "return clickEnter('" + txt1 + "','" + txtNext + "',event)\" onblur=\"" + "return Onblur('" + txt1 + "','" + txtNext + "',event)\"" + " style='width:50px;text-align:center;' name='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "' id='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "' value='" + dr[dr1["AssignmentID"].ToString()].ToString() + "' >";
                            s += "<input type='text' " + " style='width:50px;display: none' name='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "_Old' id='txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString() + "_Old' value='" + dr[dr1["AssignmentID"].ToString()].ToString() + "' >";
                            s += "</td>";

                        }
                    }

                    s += "</tr>";
                }
                s += "</table>";
                return s;
            }
            catch
            {
                s = "Không lấy được danh sách sinh viên";
                return s;
            }

        }
        #endregion

        public string KiemTraDieuKienNhapDiem()
        {
            DataTable _tb = MyUIS.KiemTraThoiHanNhapDiem_sel(_SCheduleStudyUnitID, "1");
            string Msg = "";
            if (_tb.Rows.Count > 0)
            {
                if (_tb.Rows[0]["Time1"].ToString() == "1")
                {
                    Msg = "Cho phép nhập điểm ...";
                }
                else
                {
                    Msg = _tb.Rows[0]["ReVal"].ToString();
                }
            }

            return Msg;
        }

        public void LoadThongTinLHP()
        {
            DataTable _tbinfo = MyUIS.GetScheduleStudyUnitinfo(_SCheduleStudyUnitID);
            if (_tbinfo.Rows.Count > 0)
            {
                lblCurriculumName.Text = _tbinfo.Rows[0]["CurriculumName"].ToString();
                lblCurriculumID.Text = _tbinfo.Rows[0]["ScheduleStudyUnitAlias"].ToString();
                lblYearStudy.Text = _tbinfo.Rows[0]["YearStudy"].ToString();
                lblTermID.Text = _tbinfo.Rows[0]["TermID"].ToString();
                lblFromdate.Text = _tbinfo.Rows[0]["Fromdate"].ToString() + " đến " + _tbinfo.Rows[0]["Todate"].ToString(); ;
                lblAssignmentdetail.Text = _tbinfo.Rows[0]["Assignmentdetail"].ToString();

            }
        }

        private bool KiemTraTinhHopLeDiemQuaTrinh()
        {
            try
            {
                bool isOk = true;
                string diemnhap = "";
                string qs="";
                double _DiemASS04 = 0;

                DataTable a = TableStudentSchedule;
                DataTable asi = TableAssignmentschedule;

                foreach (DataRow dr in a.Rows)
                {
                    foreach (DataRow dr1 in asi.Rows)
                    {
                        if (dr["StatusID"].ToString() != "1")
                        {
                            try
                            {
                                qs = "txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString();
                                diemnhap = Request[qs];
                                if (diemnhap != "" && diemnhap != "VT")
                                {
                                    _DiemASS04 = double.Parse(diemnhap);
                                    if (_DiemASS04 < 0 || _DiemASS04 > 10)
                                    {
                                        isOk = false;
                                        break;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                isOk = false;
                                break;
                            }
                        }
                    }
                }
                if (!isOk)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void LuuDiemQuaTrinhHTML(string xml)
        {
            string _kq=MyUIS.LuuDiemQuaTrinh(xml);
            if (_kq=="0")
            {
                lrtHtml.Text = DrawingTableAssignment();
                WebCommand.ShowMessageBox(Page, "Lưu Điểm Thành công! ");
            }
            else if (_kq=="1")
                WebCommand.ShowMessageBox(Page, "Lưu thất bại");
            else
            {

            }
        }

       
        #endregion

        #region event

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string staffManageCode = "";
                SessionManager.DoWork(session =>
                {
                    Staff staff1 = ControllerHelpers.GetCurrentStaff(session);
                    staffManageCode = staff1.StaffProfile.ManageCode;
                    Session["UIS_MemberID"] = staffManageCode;
                });

                if (Request["SCH"] == null || Session["UIS_MemberID"] == null || Session["UIS_MemberID"] == "")
                {
                    Response.Redirect("~/", true);
                    return;
                }
                _SCheduleStudyUnitID = Request["SCH"];
                if (!IsPostBack)
                {
                    // Kiem tra lop hoc phan co duoc phan cong cho giang vien hay khong
                    string _Check = MyUIS.KiemTraGiangVienNhapDiemLHP(Session["UIS_MemberID"].ToString(), _SCheduleStudyUnitID);
                    if(!_Check.Contains("..."))
                    {
                        Response.Redirect("~/");
                        return;
                    }
                    //Load thoong tin lop hoc phan
                    LoadThongTinLHP();
                    _Msg = KiemTraDieuKienNhapDiem();
                    if (_Msg.Contains("..."))
                    {
                        lrtHtml.Text = DrawingTableAssignment();
                        btnLuuDiem.Visible = true;
                        btnKhoaDiem.Visible = true;
                    }
                    else
                    {
                        btnLuuDiem.Visible = false;
                        btnKhoaDiem.Visible = false;
                        lblThongbao.Text = _Msg;
                    }
                }
            }
            catch (Exception ex) { lblThongbao.Text = ex.Message; }
        }
        protected void btnLuuDiem_Click(object sender, EventArgs e)
        {
            if (KiemTraTinhHopLeDiemQuaTrinh())
            {
                string xml = "";
                string StudentID = string.Empty;
                string Marks = string.Empty;
                DataTable a = TableStudentSchedule;
                DataTable asi = TableAssignmentschedule;
                foreach (DataRow dr in a.Rows)
                {
                    foreach (DataRow dr1 in asi.Rows)
                    {
                        Marks = Request["txt_" + dr1["AssignmentID"].ToString() + "_" + dr["StudentID"].ToString()];
                        StudentID = dr["StudentID"].ToString();
                        if (dr["StatusID"].ToString() != "1")
                        {
                            xml += "<D STDID=\"" + StudentID + "\" ";
                            xml += " Ass=\"" + dr1["AssignmentID"].ToString() + "\" Mk=\"" + Marks + "\" />";

                        }
                    }
                }
                if (xml != "")
                {
                    LuuDiemQuaTrinhHTML("<Root><SCID ID=\"" + _SCheduleStudyUnitID + "\"/><UDT ID=\"" + Session["UIS_MemberID"] + "\"/>" + xml + "</Root>");
                }
            }
            else
            {

            }
        }

        protected void btnKhoaDiem_Click(object sender, EventArgs e)
        {
            string _kq = MyUIS.KhoaDiemQuaTrinh(_SCheduleStudyUnitID);
            if (_kq == "0")
            {
                lrtHtml.Text = DrawingTableAssignment();
                WebCommand.ShowMessageBox(Page,"Khóa điểm thành công");
            }
            else
            {
                WebCommand.ShowMessageBox(Page, "Khóa điểm thất bại");
            }
        }

        #endregion
    }
}