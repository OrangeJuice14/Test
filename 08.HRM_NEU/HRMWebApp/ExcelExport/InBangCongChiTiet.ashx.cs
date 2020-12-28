using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using NHibernate.Linq;
using Infragistics.Documents.Excel;
using System.Data;
using Infragistics.Documents.Excel.PredefinedShapes;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;
using HRMWebApp.KPI.Core.Controllers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ERP_Core;
using System.IO;
using System.Drawing;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;

namespace HRMWebApp.ExcelExport
{
    public class InBangCongChiTiet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int thang = context.Request.Params["thang"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["thang"].ToString());
            int nam = context.Request.Params["nam"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["nam"].ToString());
            Guid bophanId = (context.Request.Params["bophanId"] != null && context.Request.Params["bophanId"].ToString() != "undefined") ? new Guid(context.Request.Params["bophanId"].ToString()) : Guid.Empty;
            Guid idNhanVien = (context.Request.Params["idNhanVien"] != null && context.Request.Params["idNhanVien"].ToString() != "undefined") ? new Guid(context.Request.Params["idNhanVien"].ToString()) : Guid.Empty;
            BangChamCongChiTietExportToExcelProcess(thang, nam, bophanId, idNhanVien, context);
        }

        public void BangChamCongChiTietExportToExcelProcess(int thang, int nam, Guid? bophanId, Guid? idNhanVien, HttpContext context)
        {
            //if (bophanId == Guid.Empty && idNhanVien != Guid.Empty)
            //{
            //    ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            //    bophanId = Guid.Parse(applicationUser.DepartmentId);
            //}

            List<DTO_QuanLyChamCong_GioVaoRa> result = new List<DTO_QuanLyChamCong_GioVaoRa>();

            CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
            //if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
            //    result = factory.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(thang, nam, idNhanVien);
            //else
            //    result = factory.QuanLyChamCong_ThongTinChamCongThang(thang, nam, bophanId, maNhanSu, idLoaiNhanSu);

            if (bophanId == Guid.Empty)
                bophanId = null;
            if (idNhanVien == Guid.Empty)
                idNhanVien = null;

            List<spd_WebChamCong_InBangChiTietChamCong_Result> result1 = factory.Context.spd_WebChamCong_InBangChiTietChamCong(thang, nam, bophanId, idNhanVien).ToList();
            List<DTO_NgayChamCong> listNgay = factory.GetList_NgayTrongKyChamCong(thang, nam).ToList();

            foreach (spd_WebChamCong_InBangChiTietChamCong_Result r in result1)
            {
                int index = result.FindIndex(item => item.NhanVienID == r.IDNhanVien);
                if (index < 0)
                {
                    DTO_QuanLyChamCong_GioVaoRa a = new DTO_QuanLyChamCong_GioVaoRa();
                    a.NhanVienID = r.IDNhanVien;
                    a.MaNhanSu = r.IDNhanSu_ChamCong;
                    a.HoTen = r.HoTen;
                    a.BoPhan = r.TenBoPhan;
                    a.ChiTietVaoRa = new List<DTO_QuanLyChamCong_GioVaoRaTungCa>();
                    DTO_QuanLyChamCong_GioVaoRaTungCa b = new DTO_QuanLyChamCong_GioVaoRaTungCa();
                    b.Ngay = r.Ngay;
                    b.GioQuet = r.GioQuet;
                    b.SoLanQuet = r.SoLanQuet;
                    a.ChiTietVaoRa.Add(b);

                    result.Add(a);
                }
                else
                {
                    foreach (DTO_QuanLyChamCong_GioVaoRa aa in result)
                    {
                        if (aa.NhanVienID == r.IDNhanVien)
                        {
                            DTO_QuanLyChamCong_GioVaoRaTungCa b = new DTO_QuanLyChamCong_GioVaoRaTungCa();
                            b.Ngay = r.Ngay;
                            b.GioQuet = r.GioQuet;
                            b.SoLanQuet = r.SoLanQuet;
                            aa.ChiTietVaoRa.Add(b);
                        }
                    }
                }
            }

            BoPhan_Factory bpfac = new BoPhan_Factory();
            HoSo_Factory hsfac = new HoSo_Factory();
            BoPhan DonVi = bpfac.GetByID(bophanId.HasValue ? bophanId.Value : Guid.Empty);
            
            DateTime now = DateTime.Now;
            
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("ChiTiet");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["ChiTiet"];
            sheet.PrintOptions.PaperSize = PaperSize.A3;

            sheet.Columns[0].Width = 4000;
            sheet.Columns[1].Width = 4000;

            //làm cho đẹp
            sheet.Columns[5].Width = 3000;
            sheet.Columns[6].Width = 3000;

            //Margin 1 cm
            sheet.PrintOptions.LeftMargin = 0.385;
            sheet.PrintOptions.RightMargin = 0.385;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            //sheet.Columns[0].Width =1500;
            //sheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            //sheet.Columns[1].Width = 7000;
            //for (int i=2;i<= songay + 1;i++)
            //{
            //    sheet.Columns[i].Width = 900;
            //    sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            //}
            //for (int i = songay+2; i <= songay + 6; i++)
            //{
            //    sheet.Columns[i].Width = 2800;
            //    sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            //}
            int rowIndex = 0;

            //WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            //merged2.Value = System.Configuration.ConfigurationManager.AppSettings["TenDonViSuDung"].ToUpper();
            //merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            //merged2.CellFormat.Font.Name = "Times New Roman";
            //merged2.CellFormat.Font.Height = 240;
            //merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //rowIndex++;
            //WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            //merged4.Value = "THÀNH PHỐ HỒ CHÍ MINH";
            //merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            //merged4.CellFormat.Font.Name = "Times New Roman";
            //merged4.CellFormat.Font.Height = 240;
            //merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //rowIndex++;
            WorksheetMergedCellsRegion merged = sheet.MergedCellsRegions.Add(0, 0, 1, 6);
            merged.Value = "BẢNG CHẤM CÔNG THÁNG " + thang + "/" + nam;
            merged.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged.CellFormat.Font.Name = "Times New Roman";
            merged.CellFormat.Font.Height = 400;
            merged.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            rowIndex++;
            rowIndex++;

            #region TargetGroup

            foreach (DTO_QuanLyChamCong_GioVaoRa ccThang in result)
            {
                //WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
                //mergedHoTen.Value = "Mã nhân viên: " + ccThang.MaNhanSu + "   Tên nhân viên: " + ccThang.HoTen + "   Bộ phận: " + ccThang.BoPhan;
                //SetRegionBorder(mergedHoTen, false, false, true, true);
                sheet.Rows[rowIndex].Cells[0].Value = "Mã nhân viên: " + ccThang.MaNhanSu + "   Tên nhân viên: " + ccThang.HoTen + "   Bộ phận: " + ccThang.BoPhan;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                rowIndex++;

                WorksheetMergedCellsRegion mergedNgay = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                mergedNgay.Value = "Ngày";
                SetRegionBorder(mergedNgay, true, false, true, true);

                WorksheetMergedCellsRegion mergedThu = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
                mergedThu.Value = "Thứ";
                SetRegionBorder(mergedThu, true, false, true, true);
                
                int maxTimes = 1;
                foreach (DTO_QuanLyChamCong_GioVaoRaTungCa cc in ccThang.ChiTietVaoRa)
                {
                    int times = cc.SoLanQuet.HasValue ? cc.SoLanQuet.Value : 1;
                    if (times > maxTimes)
                        maxTimes = times;
                }

                for (int i = 1; i <= maxTimes; i++)
                {
                    WorksheetMergedCellsRegion mergedGioQuet = sheet.MergedCellsRegions.Add(rowIndex, 1 + i, rowIndex + 1, 1 + i);
                    mergedGioQuet.Value = "Lần " + i;
                    SetRegionBorder(mergedGioQuet, true, false, true, true);
                    sheet.Columns[i + 1].Width = 3000;
                }

                //WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 3);
                //merged1.Value = "1";
                //SetRegionBorder(merged1, true, false, true, true);

                //WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex, 5);
                //merged2.Value = "2";
                //SetRegionBorder(merged2, true, false, true, true);

                //WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex, 7);
                //merged3.Value = "3";
                //SetRegionBorder(merged3, true, false, true, true);

                SetCellFormat(sheet, rowIndex, 0, maxTimes + 1, true, false, false, true, true);

                //rowIndex++;

                //for (int i = 2; i < 8; i++)
                //{
                //    if (i % 2 == 0)
                //        sheet.Rows[rowIndex].Cells[i].Value = "Vào";
                //    else sheet.Rows[rowIndex].Cells[i].Value = "Ra";
                //    CellBorder(sheet, rowIndex, i);
                //}
                //SetCellFormat(sheet, rowIndex, 2, 7, true, false, true, true, true);
                rowIndex += 2;
                foreach (DTO_QuanLyChamCong_GioVaoRaTungCa cc in ccThang.ChiTietVaoRa)
                {
                    sheet.Rows[rowIndex].Cells[0].Value = cc.Ngay;
                    foreach (DTO_NgayChamCong date in listNgay)
                    {
                        if(date.Ngay == (cc.Ngay.HasValue ? cc.Ngay.Value.Day : 0))
                            sheet.Rows[rowIndex].Cells[1].Value = date.Thu;
                    }
                    List<string> gioQuetList = XuLyChuoi(cc.GioQuet);
                    int columnIndex = 2;
                    if (gioQuetList != null)
                    {
                        foreach (string gioQuet in gioQuetList)
                        {
                            sheet.Rows[rowIndex].Cells[columnIndex].Value = gioQuet;
                            columnIndex++;
                        }
                    }
                    SetCellFormat(sheet, rowIndex, 0, maxTimes + 1, false, true, false, true, true);
                    rowIndex++;
                }
                rowIndex++;
                sheet.Rows[rowIndex].Cells[1].Value = "Ký tên";
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
                rowIndex += 3;
                //HoSo leader = hsfac.GetDepartmentLeaderByStaffID(ccThang.NhanVienID.HasValue ? ccThang.NhanVienID.Value : Guid.Empty);
                //if (leader != null)
                //{
                //    sheet.Rows[rowIndex].Cells[2].Value = leader.HoTen;
                //    sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                //    sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                //}
                sheet.Rows[rowIndex].Cells[1].Value = ccThang.HoTen;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                rowIndex += 3;
            }

            rowIndex++;
            rowIndex++;  
            #endregion

            string filename = "/Temp/Excel/1.xls";
            BIFF8Writer.WriteWorkbookToFile(workbook, context.Server.MapPath(filename));

            BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
            context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
            reader.Close();
            context.Response.Flush();

        }
        public List<string> XuLyChuoi(string input)
        {
            if (input == null)
                return null;
            List<string> result = new List<string>();
            string element = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != ';' && input[i] != ' ')
                {
                    element += input[i];
                }
                if (input[i] == ';' || i == input.Length - 1)
                {
                    result.Add(element);
                    element = "";
                }
            }

            return result;
        }
        public static void SetCellFormatPointSystem(Worksheet sheet, int indexrow, int columnCount)
        {
            for (int i = 0; i <= columnCount; i++)
            {
                if (i == 0 || indexrow == 3 || indexrow == 4)
                {
                    sheet.Rows[indexrow].Cells[i].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    sheet.Rows[indexrow].Cells[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
                }
                else
                {
                    sheet.Rows[indexrow].Cells[i].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                }
                sheet.Rows[indexrow].Cells[i].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[indexrow].Cells[i].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.WrapText = ExcelDefaultableBoolean.True;
            }
        }
        public static void CellBorder(Worksheet sheet, int indexrow, int indexcolumn)
        {
                sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        }

        protected void SetRegionBorder(WorksheetMergedCellsRegion region, bool isCenter, bool isJustify, bool isMiddle, bool isBoldText)
        {
            region.CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;

            region.CellFormat.VerticalAlignment = isMiddle ? VerticalCellAlignment.Center : VerticalCellAlignment.Top;
            region.CellFormat.Alignment = isCenter ? HorizontalCellAlignment.Center : HorizontalCellAlignment.Left;
            region.CellFormat.Alignment = isJustify ? HorizontalCellAlignment.Justify : HorizontalCellAlignment.Left;
            region.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            if (isBoldText)
                region.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        }

        public static void BackgroundColor(Worksheet sheet, int indexrow, int startColumnIndex, int endColumnIndex, Color color)
        {
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                sheet.Rows[indexrow].Cells[i].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(color), null, FillPatternStyle.Solid);


            }
        }

        public static void SetCellFormat(Worksheet sheet, int indexrow, int startColumnIndex, int endColumnIndex, bool isCenter, bool isJustify, bool isBold, bool topBorder, bool bottomBorder)
        {
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                if (topBorder)
                    sheet.Rows[indexrow].Cells[i].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                if (bottomBorder)
                    sheet.Rows[indexrow].Cells[i].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                sheet.Rows[indexrow].Cells[i].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;

                sheet.Rows[indexrow].Cells[i].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                if (isBold)
                    sheet.Rows[indexrow].Cells[i].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                sheet.Rows[indexrow].Cells[i].CellFormat.WrapText = ExcelDefaultableBoolean.True;

                if (isCenter)
                {
                    sheet.Rows[indexrow].Cells[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
                }
                if (isJustify)
                {
                    sheet.Rows[indexrow].Cells[i].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}