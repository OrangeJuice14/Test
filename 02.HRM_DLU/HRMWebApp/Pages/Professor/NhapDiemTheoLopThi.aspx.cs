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
    public partial class NhapDiemTheoLopThi : System.Web.UI.Page
    {
        MyUISBL MyUIS = new MyUISBL();
        string _Examination = "";
        string _Msg = "";

        #region "Function"
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

                if (Request["EX"] == null || Session["UIS_MemberID"] == null || Session["UIS_MemberID"] == "")
                {
                    Response.Redirect("~/", true);
                    return;
                }
                _Examination = Request["EX"];
                if (!IsPostBack)
                {
                    //Kiem tra quyen nhap diem
                    string _kq = MyUIS.KiemTraGiangVienNhapDiemTheoLopThi(Session["UIS_MemberID"].ToString(), _Examination);
                    if(!_kq.Contains("..."))
                    {
                        Response.Redirect("~/");
                    }
                     
                    //load thong tin lop hoc phan
                    LoadThongTinLopThi();
                    LoadDanhSachSinhVien();
                    //
                    _Msg = KiemTraDieuKienNhapDiem();
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
                        KiemTraKhoaNhapDiem();
                    }
                }
            }
            catch { }
        }

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
                        else
                        {
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
        public string KiemTraDieuKienNhapDiem()
        {
            DataTable _tb = MyUIS.KiemTraThoiHanNhapDiem_sel(_Examination, "3");
            string Msg = "";
            if (_tb.Rows.Count > 0)
            {
                if (_tb.Rows[0]["Time3"].ToString() == "1")
                {
                    Msg = "Cho phép nhập điểm ...";
                }
                else
                {
                    Msg = _tb.Rows[0]["ReVal3"].ToString();
                }
            }

            return Msg;
        }
        private void LoadThongTinLopThi()
        {
            DataTable da = MyUIS.GetExamiationinfo_Sel(_Examination);
            if (da.Rows.Count > 0)
            {
                lblYearStudy.Text = da.Rows[0]["YearStudy"].ToString();
                lblTermID.Text = da.Rows[0]["TermID"].ToString();
                lblCurriculumID.Text = da.Rows[0]["ScheduleStudyUnitID"].ToString();
                lblCurriculumName.Text = da.Rows[0]["CurriculumName"].ToString();
                lblNgayThiPhongThiGioThi.Text = da.Rows[0]["Info"].ToString();
                lblFromdate.Text = da.Rows[0]["ThoiHanNhapDiemThi"].ToString();
    
            }
        }

        private void LoadDanhSachSinhVien()
        {
            DataTable _tbStudent = MyUIS.StudentExamination_Sel_Marks(_Examination);
            gvDanhSachSinhVien.DataSource = _tbStudent;
            gvDanhSachSinhVien.DataBind();


            SetTextBox(gvDanhSachSinhVien, "txtMark");
        }

        #region  private bool KiemtraEx()
        private bool KiemtraEx()
        {
            string A01;
            bool isOk = true;

            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark = (TextBox)gr.FindControl("txtMark");
                A01 = txtMark.Text;
                if (A01 != "VT")
                {
                    decimal Mark = 0;
                    if ((A01.Trim() != "" && !decimal.TryParse(A01, out Mark)) || (Mark < 0) || Mark > 10)
                    {
                        txtMark.Focus();
                        txtMark.BorderColor = Color.Red;
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }
        #endregion

        #region private void LuudiemEx(string xml)
        private void LuudiemEx(string xml)
        {
            string kq = MyUIS.LuuDiemThiTheoLopThi(xml);
            if (kq == "0")
            {
                LoadDanhSachSinhVien();
                WebCommand.ShowMessageBox(this.Page, "Lưu Điểm Thành công !");
            }
            else
            {
                WebCommand.ShowMessageBox(this.Page, "Lưu Điểm thât bại !");
            }
        }
        #endregion

        public void KiemTraKhoaNhapDiem()
        {
            foreach (GridViewRow gr in gvDanhSachSinhVien.Rows)
            {
                TextBox txtMark = (TextBox)gr.FindControl("txtMark");
                txtMark.Enabled = false;
            }
        }
        #endregion

        #region Event
        protected void btnKhoaDiem_Click(object sender, EventArgs e)
        {
            string _kq=MyUIS.Examination_Locks(_Examination);
            if (_kq=="0")
            {
                WebCommand.ShowMessageBox(this.Page, "Khóa điểm thành công!");
                LoadDanhSachSinhVien();
            }
            else
            {
                WebCommand.ShowMessageBox(this.Page, "Khóa điểm thất bại ! . Vui lòng liên hệ PĐT");
            }

        }

        protected void btnLuuDiem_Click(object sender, EventArgs e)
        {
            if (KiemtraEx())
            {
                try
                {
                    string xml = "", StudentID, A01, Nt;
                    foreach (GridViewRow it in gvDanhSachSinhVien.Rows)
                    {
                        StudentID = ((Label)it.FindControl("lblStudentID")).Text;
                        A01 = ((TextBox)it.FindControl("txtMark")).Text;
                        Nt = ((TextBox)it.FindControl("txtGhichu")).Text;
                        xml += "<D STDID=\"" + StudentID + "\" A01=\"" + A01 + "\" Nt=\"" + Nt + "\"/>";
                    }
                    if (xml != "")
                    {
                        LuudiemEx("<Root><SCEX ID=\"" + _Examination+ "\"/><UDT ID=\"" + Session["UIS_MemberID"].ToString() + "\"/>" + xml + "</Root>");
                    }
                }
                catch (Exception exx)
                {
                    WebCommand.ShowMessageBox(this.Page, "- Có lỗi xảy trong quá trình lưu điểm . Vui lòng liên hệ PĐT");
                }
            }
            else
            {
                WebCommand.ShowMessageBox(this.Page, "Nhập điểm không hợp lệ");
            }
        }
        #endregion

        protected void btnGanVang_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDanhSachSinhVien.Rows)
            {
                int rowIndex = 0;
                for (int i = 0; i < gvDanhSachSinhVien.Rows.Count; i++)
                {
                    TextBox txt = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark");
                    if (txt.Text == "")
                    {
                        txt.Text = "VT";
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
                    TextBox txt = (TextBox)gvDanhSachSinhVien.Rows[i].Cells[rowIndex].FindControl("txtMark");
                    if (txt.Text == "VT")
                    {
                        txt.Text = "";
                    }
                }
            }
        }

        protected void btnImportDiem_Click(object sender, EventArgs e)
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

                    _tbSource = MyUIS.StudentExamination_Sel_Marks(_Examination);


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
                                r["Mark"] = r2["DiemThi"];
                                // r["Note"] = r2["GhiChu"];
                            }
                        }
                    }

                    gvDanhSachSinhVien.DataSource = _tbTableStudentMarks;
                    gvDanhSachSinhVien.DataBind();
                    //SetFunctionJavascriptEX(gvDanhSachSinhVien, 5, "txtMark");
                }
            }
        }

        protected void btnExportDiem_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();

            DataTable dt = MyUIS.StudentExamination_Export_Marks(_Examination);

            GridView GridView1 = new GridView();

            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + _Examination + ".xls");
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
    }
}