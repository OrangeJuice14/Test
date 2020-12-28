using MyUisCoreUIS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using System.IO;
using System.Data.OleDb;

namespace MyUIS_MVC.Pages.Professor
{
    public partial class NhapDiemThiTheoLHP : System.Web.UI.Page
    {
        MyUISBL MyUIS = new MyUISBL();
        string _SCheduleStudyUnitID = "";
        string _Msg = "";

        #region protected DataTable TableStudents
        protected DataTable TableStudents
        {
            get
            {
                return MyUIS.DanhSachNhapDiemTheoLopHocPhan(_SCheduleStudyUnitID);
            }
        }


        public void LoadImportSV()
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
                    string ScheduleStudyUnitId = string.Empty;

                    _tbSource = MyUIS.DanhSachNhapDiemTheoLopHocPhan(_SCheduleStudyUnitID);

                    DataTable _tbTableStudentMarks = _tbSource;
                    _tbTableStudentMarks.AcceptChanges();

                    int count = _tbTableStudentMarks.Rows.Count;

                    DataTable tbMark = new DataTable();
                    tbMark.Columns.Add("STT");
                    tbMark.Columns.Add("MaSinhVien");
                    tbMark.Columns.Add("HoLot");
                    tbMark.Columns.Add("Ten");
                    // tbMark.Columns.Add("DiemQT");
                    tbMark.Columns.Add("DiemThi");
                    //tbMark.Columns.Add("GhiChu");

                    for (int i = 0; i < count; i++)
                    {
                        DataRow r = tbMark.NewRow();
                        r["STT"] = tbContent.Rows[i][0];
                        r["MaSinhVien"] = tbContent.Rows[i][1];
                        r["HoLot"] = tbContent.Rows[i][2];
                        r["Ten"] = tbContent.Rows[i][3];
                        // r["DiemQT"] = tbContent.Rows[i][6].ToString();
                        r["DiemThi"] = tbContent.Rows[i][6].ToString();
                        //r["GhiChu"] = tbContent.Rows[i][8];
                        tbMark.Rows.Add(r);
                    }

                    foreach (DataRow r in _tbTableStudentMarks.Rows)
                    {
                        foreach (DataRow r2 in tbMark.Rows)
                        {
                            if (r["StudentID"].ToString().Trim() == r2[1].ToString().Trim())
                            {
                                //   r["ASS04"] = r2["DiemQT"];
                                r["Mark10"] = r2["DiemThi"];
                                // r["Note"] = r2["GhiChu"];
                            }
                        }
                    }

                    // gvDanhSachSinhVien.DataSource = _tbTableStudentMarks;

                    foreach (GridViewRow row in gvDanhSachSinhVien.Rows)
                    {
                        int rowIndex = 0;
                        for (int i = 0; i < gvDanhSachSinhVien.Rows.Count; i++)
                        {
                            TextBox txt = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_TH");
                            TextBox txt10 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10");
                            TextBox txt10_2 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("Mark10_2");
                            TextBox txt10_2TH = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_2TH");

                            if (ddlLanThi.SelectedValue == "1")
                            {
                                txt10.Text = _tbTableStudentMarks.Rows[i]["Mark10"].ToString();
                            }
                            else
                            {
                                txt10_2.Text = _tbTableStudentMarks.Rows[i]["Mark10_2"].ToString();
                            }

                        }
                    }

                    //gvDanhSachSinhVien.DataBind();
                }
            }
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

        #endregion

        #region function
        public void LoadDanhSachSinhVien()
        {
            gvDanhSachSinhVien.DataSource = TableStudents;
            gvDanhSachSinhVien.DataBind();

            if(TableStudents.Rows.Count>0)
            {
                if(TableStudents.Rows[0]["CoDiemLT"].ToString()=="1")
                {
                    gvDanhSachSinhVien.Columns[6].Visible = true;
                }
                else { gvDanhSachSinhVien.Columns[6].Visible = false; }
                if (TableStudents.Rows[0]["CoDiemTH"].ToString() == "2")
                {
                    gvDanhSachSinhVien.Columns[7].Visible = true;
                }
                else { gvDanhSachSinhVien.Columns[7].Visible = false; }

                SetTextBox(gvDanhSachSinhVien, "txtMark10");
                SetTextBox(gvDanhSachSinhVien, "txtMark10_2");
                SetTextBox(gvDanhSachSinhVien, "txtMark10_TH");
                SetTextBox(gvDanhSachSinhVien, "txtMark10_2TH");
            }
        }

        public void LoadThongTinLHP()
        {
            try
            {
                DataTable _tbinfo = MyUIS.GetScheduleStudyUnitinfo(_SCheduleStudyUnitID);
                if (_tbinfo.Rows.Count > 0)
                {
                    lblCurriculumName.Text = _tbinfo.Rows[0]["CurriculumName"].ToString();
                    lblCurriculumID.Text = _tbinfo.Rows[0]["ScheduleStudyUnitAlias"].ToString();
                    lblYearStudy.Text = _tbinfo.Rows[0]["YearStudy"].ToString();
                    lblTermID.Text = _tbinfo.Rows[0]["TermID"].ToString();
                    lblFromdate.Text = _tbinfo.Rows[0]["ExamFromDate"].ToString() + " đến " + _tbinfo.Rows[0]["ExamTodate"].ToString(); ;
                    lblAssignmentdetail.Text = _tbinfo.Rows[0]["MarkDetail"].ToString()+"%";

                }
            }
            catch { }
        }

        private void KiemTraLanNhapDiem(string Msg)
        {
            string LanThi = ddlLanThi.SelectedValue;
           // string ChoPhepNhapDiem = KiemTraDieuKienNhapDiem(1);
            //string ChoPhepNhapDiemL2 = KiemTraDieuKienNhapDiem(2);
            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark10 = gr.FindControl("txtMark10") as TextBox;
                TextBox txtMark10_TH = gr.FindControl("txtMark10_TH") as TextBox;
                TextBox txtMark10_2 = gr.FindControl("txtMark10_2") as TextBox;
                TextBox txtMark10_2TH = gr.FindControl("txtMark10_2TH") as TextBox;
                if (LanThi == "1")
                {
                    txtMark10_2.Visible = false;
                    txtMark10_2TH.Visible = false;
                    txtMark10.Visible = true;
                    txtMark10_TH.Visible = true;

                    if (Msg.Contains("..."))
                    {
                        txtMark10.Enabled = true;
                        txtMark10_TH.Enabled = true;
                    }
                    else
                    {
                        txtMark10.Enabled = false;
                        txtMark10_TH.Enabled = false;
                    }
                }
                else if (LanThi == "2")
                {
                    txtMark10.Visible = false;
                    txtMark10_TH.Visible = false;
                    txtMark10_2.Visible = true;
                    txtMark10_2TH.Visible = true;

                    if (Msg.Contains("..."))
                    {

                        txtMark10_2.Enabled = true;
                        txtMark10_2TH.Enabled = true;
                    }
                    else
                    {
                        txtMark10_2.Enabled = false;
                        txtMark10_2TH.Enabled = false;
                    }
                }
                else
                {
                    txtMark10_2.Visible = false;
                    txtMark10_2TH.Visible = false;
                    txtMark10.Visible = false;
                    txtMark10_TH.Visible = false;
                }
            }

            KiemTraKhoaDiem();
        }

        public string KiemTraDieuKienNhapDiem(int Times)
        {
            DataTable _tb = MyUIS.KiemTraThoiHanNhapDiem_sel(_SCheduleStudyUnitID, "2");
            string Msg = "";
            if (_tb.Rows.Count > 0)
            {
                if (Times == 1)
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
                if (Times == 2)
                {
                    if (_tb.Rows[0]["Time2"].ToString() == "1")
                    {
                        Msg = "Cho phép nhập điểm ...";
                    }
                    else
                    {
                        Msg = _tb.Rows[0]["ReVal2"].ToString();
                    }
                }
            }

            return Msg;
        }

        private void KiemTraKhoaDiem()
        {
            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark10 = gr.FindControl("txtMark10") as TextBox;
                TextBox txtMark10_TH = gr.FindControl("txtMark10_TH") as TextBox;
                TextBox txtMark10_2 = gr.FindControl("txtMark10_2") as TextBox;
                TextBox txtMark10_2TH = gr.FindControl("txtMark10_2TH") as TextBox;
                Label lblLocked_1 = gr.FindControl("lblLocked_1") as Label;
                Label lblLocked_2 = gr.FindControl("lblLocked_2") as Label;
                if (lblLocked_1.Text == "1")
                {
                    txtMark10.Enabled = txtMark10_TH.Enabled = false;
                }
                else
                {
                    txtMark10.Enabled = txtMark10_TH.Enabled = true;
                }
                if (lblLocked_2.Text == "1")
                {
                    txtMark10_2TH.Enabled = txtMark10_2.Enabled = false;
                }
                else
                {
                    txtMark10_2TH.Enabled = txtMark10_2.Enabled = true;
                }
            }
        }

        #region  private bool KiemtraMS()
        private bool KiemtraMS()
        {
            string A01;
            bool isOk = true;
            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark10 = gr.FindControl("txtMark10") as TextBox;
                TextBox txtMark10_TH = gr.FindControl("txtMark10_TH") as TextBox;
                TextBox txtMark10_2 = gr.FindControl("txtMark10_2") as TextBox;
                TextBox txtMark10_2TH = gr.FindControl("txtMark10_2TH") as TextBox;

                A01 = txtMark10.Text;
                if (A01 != "VT")
                {
                    decimal Mark = 0;
                    if ((A01.Trim() != "" && !decimal.TryParse(A01, out Mark)) || (Mark < 0) || Mark > 10)
                    {
                        isOk = false;
                        txtMark10.Focus(); txtMark10.BorderColor = Color.Red;
                    }
                }

                A01 = txtMark10_TH.Text;
                if (A01 != "VT")
                {
                    decimal Mark = 0;
                    if ((A01.Trim() != "" && !decimal.TryParse(A01, out Mark)) || (Mark < 0) || Mark > 10)
                    {
                        isOk = false;
                        txtMark10_TH.Focus(); txtMark10_TH.BorderColor = Color.Red;
                    }
                }

                A01 = txtMark10_2.Text;
                if (A01 != "VT")
                {
                    decimal Mark = 0;
                    if ((A01.Trim() != "" && !decimal.TryParse(A01, out Mark)) || (Mark < 0) || Mark > 10)
                    {
                        isOk = false;
                        txtMark10_2.Focus(); txtMark10_2.BorderColor = Color.Red;
                    }
                }

                A01 = txtMark10_2TH.Text;
                if (A01 != "VT")
                {
                    decimal Mark = 0;
                    if ((A01.Trim() != "" && !decimal.TryParse(A01, out Mark)) || (Mark < 0) || Mark > 10)
                    {
                        isOk = false;
                        txtMark10_2TH.Focus(); txtMark10_2TH.BorderColor = Color.Red;
                    }
                }
            }

            return isOk;
        }
        #endregion

        #region private void SetTextBox(Repeater rpt, string TextBoxControlName)
        private void SetTextBox(GridView gv, string TextBoxControlName)
        {
            try
            {
                int j = 0;
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    TextBox text = ((TextBox)gv.Rows[i].FindControl(TextBoxControlName));
                    if (text.Enabled == true && text.ReadOnly == false)
                    {
                        for (j = i; j < gv.Rows.Count - 1; j++)
                        {
                            try
                            {
                                TextBox text1 = ((TextBox)gv.Rows[j + 1].FindControl(TextBoxControlName));
                                if (text1.Enabled == true && text1.ReadOnly == false)
                                {
                                    break;
                                }
                            }
                            catch { }
                        }
                        if (j+1 < gv.Rows.Count)
                        {
                            TextBox Nexttext = ((TextBox)gv.Rows[j + 1].FindControl(TextBoxControlName));
                            if (text.Enabled == true && text.ReadOnly == false)
                                text.Attributes.Add("onkeypress", "return clickEnter('" + text.ClientID + "','" + Nexttext.ClientID + "', event)");
                        }
                        else {
                            TextBox Nexttext = ((TextBox)gv.Rows[j].FindControl(TextBoxControlName));
                            if (text.Enabled == true && text.ReadOnly == false)
                                text.Attributes.Add("onkeypress", "return clickEnter('" + text.ClientID + "','" + Nexttext.ClientID + "', event)");
                        }

                    }


                }
            }
            catch { }
        }
        #endregion


        #region private void LuudiemMS(string xml)
        private void LuudiemMS(string xml)
        {
            DataTable dt = new DataTable();
            if (dt.Rows.Count == 0)
            {
                string KQ = MyUIS.LuuDiemThiTheoLHP(xml);
                if (KQ == "0")
                {
                    _Msg = KiemTraDieuKienNhapDiem(int.Parse(ddlLanThi.SelectedValue));
                    if (_Msg.Contains("..."))
                    {
                        btnLuuDiem.Visible = true;
                        btnKhoaDiem.Visible = true;
                        LoadDanhSachSinhVien();
                        lblThongbao.Text = "";
                    }
                    else
                    {
                        btnLuuDiem.Visible = false;
                        btnKhoaDiem.Visible = false;
                        btnBoGanVang.Visible = false;
                        btnGanVang.Visible = false;
                        btnExportDiem.Visible = false;
                        btnImportDiem.Visible = false;
                        FileUpload1.Visible = false;

                        lblThongbao.Text = _Msg;
                        lblThongbao.ForeColor = Color.Red;
                    }
                    KiemTraLanNhapDiem(_Msg);

                    WebCommand.ShowMessageBox(this.Page, "Lưu Điểm Thành công!");

                }
                else
                {
                    WebCommand.ShowMessageBox(this.Page, "Chú ý : Lưu Điểm thất bại !");
                }
            }
            else
            {
                WebCommand.ShowMessageBox(this.Page, "Đã khoá điểm không thể lưu !");
            }
        }
        #endregion

        public void KiemTraKhoaNhapDiem()
        {
            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark10 = gr.FindControl("txtMark10") as TextBox;
                TextBox txtMark10_TH = gr.FindControl("txtMark10_TH") as TextBox;
                TextBox txtMark10_2 = gr.FindControl("txtMark10_2") as TextBox;
                TextBox txtMark10_2TH = gr.FindControl("txtMark10_2TH") as TextBox;

                txtMark10.Enabled = false;
                txtMark10_TH.Enabled = false;
                txtMark10_2.Enabled = false;
                txtMark10_2TH.Enabled = false;
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
                if (lblMaLopHocPhan.Text != "" && lblMaLopHocPhan.Text != _SCheduleStudyUnitID)
                {
                    WebCommand.ShowMessageBox(Page, "Mã học phần bị thay đổi vui lòng đóng trang này và mở lại");
                    return;
                }
                if (!IsPostBack)
                {
                    // Kiem tra lop hoc phan co duoc phan cong cho giang vien hay khong
                    string _Check = MyUIS.KiemTraGiangVienNhapDiemLHP(Session["UIS_MemberID"].ToString(), _SCheduleStudyUnitID);
                    if (!_Check.Contains("..."))
                    {
                        Response.Redirect("~/");
                        return;
                    }
                    //Load thoong tin lop hoc phan

                    lblMaLopHocPhan.Text = _SCheduleStudyUnitID;
                    LoadThongTinLHP();
                    _Msg = KiemTraDieuKienNhapDiem(int.Parse(ddlLanThi.SelectedValue));
                    if (_Msg.Contains("..."))
                    {
                        btnLuuDiem.Visible = true;
                        btnKhoaDiem.Visible = true;
                        LoadDanhSachSinhVien();
                        KiemTraLanNhapDiem(_Msg);
                        lblThongbao.Text = "";
                    }
                    else
                    {
                        btnLuuDiem.Visible = false;
                        btnKhoaDiem.Visible = false;
                        lblThongbao.Text = _Msg;
                        lblThongbao.ForeColor = Color.Red;
                        KiemTraKhoaNhapDiem();
                    }
                }
            }
            catch { }
        }

        protected void btnKhoaDiem_Click(object sender, EventArgs e)
        {
            string _kq = MyUIS.KhoaDiemQuaThiTheoLHP(_SCheduleStudyUnitID, int.Parse(ddlLanThi.SelectedValue));
            if(_kq=="0")
            {
                _Msg = KiemTraDieuKienNhapDiem(int.Parse(ddlLanThi.SelectedValue));
                if (_Msg.Contains("..."))
                {
                    btnLuuDiem.Visible = true;
                    btnKhoaDiem.Visible = true;
                    LoadDanhSachSinhVien();
                    lblThongbao.Text = "";
                }
                else
                {
                    btnLuuDiem.Visible = false;
                    btnKhoaDiem.Visible = false;
                    lblThongbao.Text = _Msg;
                    lblThongbao.ForeColor = Color.Red;
                }
                KiemTraLanNhapDiem(_Msg);
                WebCommand.ShowMessageBox(Page,"Khóa điểm thành công");
            }
            else
            {
                WebCommand.ShowMessageBox(Page, "Khóa điểm thất bại");
            }
        }

        protected void btnLuuDiem_Click(object sender, EventArgs e)
        {
            if (KiemtraMS())
            {
                string xml = "";
                foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
                {
                    TextBox txtMark10 = gr.FindControl("txtMark10") as TextBox;
                    TextBox txtMark10_TH = gr.FindControl("txtMark10_TH") as TextBox;
                    TextBox txtMark10_2 = gr.FindControl("txtMark10_2") as TextBox;
                    TextBox txtMark10_2TH = gr.FindControl("txtMark10_2TH") as TextBox;
                    Label lblStudentID = gr.FindControl("lblStudentID") as Label;

                    if (ddlLanThi.SelectedValue == "1")
                    {
                        if ((txtMark10.Enabled && txtMark10.Visible && gvDanhSachSinhVien.Columns[5].Visible) || (txtMark10_TH.Enabled && txtMark10_TH.Visible && gvDanhSachSinhVien.Columns[6].Visible))
                            xml += "<D STDID=\"" + lblStudentID.Text.Trim() + "\" A01=\"" + txtMark10.Text.Trim() + "\" A02=\"" + txtMark10_TH.Text + "\" LanThi=\""+ddlLanThi.SelectedValue+"\" />";
                    }
                    else if (ddlLanThi.SelectedValue == "2")
                    {
                        if ((txtMark10_2.Enabled && txtMark10_2.Visible && gvDanhSachSinhVien.Columns[5].Visible) || (txtMark10_2TH.Enabled && txtMark10_2TH.Visible && gvDanhSachSinhVien.Columns[6].Visible))
                           xml += "<D STDID=\"" + lblStudentID.Text.Trim() + "\" A01=\"" + txtMark10_2.Text.Trim() + "\" A02=\"" + txtMark10_2TH.Text + "\" LanThi=\"" + ddlLanThi.SelectedValue + "\" />";
                    }
                }
                if (xml != "")
                {
                    LuudiemMS("<Root><SCEX ID=\"" + _SCheduleStudyUnitID + "\"/><UDT ID=\"" + Session["UIS_MemberID"] + "\"/>" + xml + "</Root>");
                }
            }
            else
            {
                WebCommand.ShowMessageBox(this.Page, "Điểm không hợp lệ .");
            }
        }

        protected void ddlLanThi_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Msg = KiemTraDieuKienNhapDiem(int.Parse(ddlLanThi.SelectedValue));
            if (_Msg.Contains("..."))
            {
                btnLuuDiem.Visible = true;
                btnKhoaDiem.Visible = true;
                LoadDanhSachSinhVien();
                lblThongbao.Text = "";
            }
            else
            {
                btnLuuDiem.Visible = false;
                btnKhoaDiem.Visible = false;
                lblThongbao.Text = _Msg;
                lblThongbao.ForeColor = Color.Red;
                gvDanhSachSinhVien.DataSource = null;
                gvDanhSachSinhVien.DataBind();
            }
            KiemTraLanNhapDiem(_Msg);

        }
        #endregion

        protected void btnGanVang_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDanhSachSinhVien.Rows)
            {
                int rowIndex = 0;
                for (int i = 0; i < gvDanhSachSinhVien.Rows.Count; i++)
                {
                    TextBox txt = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_TH");
                    TextBox txt10 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10");
                    TextBox txt10_2 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("Mark10_2");
                    TextBox txt10_2TH = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_2TH");
                    if (txt10_2.Text == "")
                    {
                        txt10_2.Text = "VT";
                    }
                    if (txt10_2TH.Text == "")
                    {
                        txt10_2TH.Text = "VT";
                    }
                    if (txt.Text == "")
                    {
                        txt.Text = "VT";
                    }
                    if (txt10.Text == "")
                    {
                        txt10.Text = "VT";
                    }
                }
            }
        }

        protected void btnBoGanVang_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDanhSachSinhVien.Rows)
            {
                int rowIndex = 0;
                for (int i = 0; i < gvDanhSachSinhVien.Rows.Count; i++)
                {
                    TextBox txt = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_TH");
                    TextBox txt10 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10");
                    TextBox txt10_2 = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("Mark10_2");
                    TextBox txt10_2TH = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark10_2TH");
                    if (txt10_2.Text == "VT")
                    {
                        txt10_2.Text = "";
                    }
                    if (txt10_2TH.Text == "VT")
                    {
                        txt10_2TH.Text = "";
                    }
                    if (txt.Text == "VT")
                    {
                        txt.Text = "";
                    }
                    if (txt10.Text == "VT")
                    {
                        txt10.Text = "";
                    }

                }
            }
        }

        protected void btnImportDiem_Click(object sender, EventArgs e)
        {
            DataTable dt = MyUIS.StudentScheduleStudyUnits_Import_Marks(_SCheduleStudyUnitID);

            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + _SCheduleStudyUnitID + ".xls");

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

        protected void btnExportDiem_Click(object sender, EventArgs e)
        {
            LoadImportSV();
        }

    }
}