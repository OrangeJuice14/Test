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
using System.Data.OleDb;
using System.IO;

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

        public DataTable ImportDiemQuaTrinh()
        {
            try
            {
                string FolderPath = "~/Uploads/";
                if (FileUpload1.HasFile)
                {
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string FileName2 = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName) + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + Extension;
                    if (Extension == ".xls" || Extension == ".xlsx")
                    {
                        string FilePath = Server.MapPath(FolderPath + FileName2);
                        FileUpload1.SaveAs(FilePath);

                        DataTable tbSheet = GetExcelSheets(FilePath, Extension, "Yes");
                        DataTable tbContent = ReadFileExcel(FilePath, tbSheet.Rows[0]["TABLE_NAME"].ToString(), Extension);

                        DataTable _tbSource = new DataTable();
                        // ScheduleStudyUnitId = Session["ScheduleStudyUnitId"].ToString();
                        _tbSource = MyUIS.StudentScheduleStudyUnitAssignments_Sel(_SCheduleStudyUnitID);


                        DataTable _tbTableStudentMarks = _tbSource;
                        _tbTableStudentMarks.AcceptChanges();

                        int count = _tbTableStudentMarks.Rows.Count;

                        DataTable tbMark = new DataTable();
                        tbMark.Columns.Add("STT");
                        tbMark.Columns.Add("MaSinhVien");
                        tbMark.Columns.Add("HoLot");
                        tbMark.Columns.Add("Ten");
                        tbMark.Columns.Add("DiemQT");
                        // tbMark.Columns.Add("DiemThi");
                        // tbMark.Columns.Add("GhiChu");

                        for (int i = 5; i < 5 + count; i++)
                        {
                            DataRow r = tbMark.NewRow();
                            r["STT"] = tbContent.Rows[i][0];
                            r["MaSinhVien"] = tbContent.Rows[i][1];
                            r["HoLot"] = tbContent.Rows[i][2];
                            r["Ten"] = tbContent.Rows[i][3];
                            r["DiemQT"] = tbContent.Rows[i][6];
                            // r["DiemThi"] = tbContent.Rows[i][7].ToString();
                            // r["GhiChu"] = tbContent.Rows[i][8];
                            tbMark.Rows.Add(r);
                        }

                        foreach (DataRow r in _tbTableStudentMarks.Rows)
                        {
                            foreach (DataRow r2 in tbMark.Rows)
                            {
                                if (r["StudentID"].ToString().Trim() == r2[1].ToString().Trim())
                                {
                                    string item = r["ASS04"].ToString();
                                    item = r2["DiemQT"].ToString();
                                    // r["Mark10"] = r2["DiemThi"];
                                    // r["Note"] = r2["GhiChu"];
                                    r["ASS04"] = item;
                                }
                            }
                        }

                        DataTable dt = _tbTableStudentMarks;
                        //SetFunctionJavascriptEX(gvDanhSachSinhVien, 5, "txtMark");
                    }
                }
                return new DataTable();
            }
            catch { return null; }

        }

        private DataTable GetExcelSheets(string FilePath, string Extension, string isHDR)
        {
            //string connectionString03 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
            string connectionString07 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
            string connectionString03 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = connectionString03;
                    break;
                case ".xlsx": //Excel 07
                    conStr = connectionString07;
                    break;
            }

            //Get the Sheets in Excel WorkBoo
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            return connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        }

        private DataTable ReadFileExcel(string FilePath, string SheetName, string Extension)
        {
            string connectionString07 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
            string connectionString03 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = connectionString03;
                    break;
                case ".xlsx": //Excel 07
                    conStr = connectionString07;
                    break;
            }
            conStr = String.Format(conStr, FilePath, "Yes");
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            cmdExcel.CommandText = "SELECT * FROM [" + SheetName + "]";

            DataTable dt = new DataTable();
            dt.TableName = SheetName;

            OleDbDataAdapter da = new OleDbDataAdapter(cmdExcel);
            da.Fill(dt);
            return dt;
        }

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

        protected void btnImportDiem_Click(object sender, EventArgs e)
        {
            Session["DiemQuaTrinh"] = "2";
            DrawingTableAssignment();
        }

        protected void btnExportDiem_Click(object sender, EventArgs e)
        {
            DataTable dt = MyUIS.StudentScheduleStudyUnitAssignments_Import(_SCheduleStudyUnitID);

            //DataTable dt = MyUIS.StudentExamination_Sel_Marks(_Examination);
            //Create a dummy GridView

            GridView GridView1 = new GridView();

            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DataTable.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {


                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }

            GridView1.RenderControl(hw);

            //style to format numbers to string

            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}