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


namespace HRMWebApp.ExcelExport
{
    public class InDanhSachNghiPhep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int nam = context.Request.Params["nam"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["nam"].ToString());
            Guid bophanId = context.Request.Params["bophanId"] != null ? new Guid(context.Request.Params["bophanId"].ToString()) : Guid.Empty;
            string webGroupId = context.Request.Params["webGroupId"] != null ? context.Request.Params["webGroupId"].ToString() : "";
            //
            BangChamCongExportToExcelProcess(nam, bophanId, webGroupId, context);
        }

        public void BangChamCongExportToExcelProcess(int nam, Guid bophanId, string webGroupId, HttpContext context)
        {
            IQueryable<DTO_ChamCongNgayNghi_Find> result = null;

            CC_ChamCongNgayNghi_Factory factory = new CC_ChamCongNgayNghi_Factory();
            result = factory.QuanLyNghiPhep_DanhSachDaDuyet(nam, bophanId, webGroupId);
            if (result == null) return;
            //
            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = "DanhSachNghiPhep_" + nam + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("DanhSachNghiPhep");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["DanhSachNghiPhep"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 1 cm
            sheet.PrintOptions.LeftMargin = 0.385;
            sheet.PrintOptions.RightMargin = 0.385;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            sheet.Columns[0].Width = 1500;
            sheet.Columns[1].Width = 4000;
            sheet.Columns[3].Width = 7000;
            sheet.Columns[4].Width = 3000;
            sheet.Columns[5].Width = 3000;
            sheet.Columns[6].Width = 7000;
            sheet.Columns[7].Width = 10000;
            sheet.Columns[8].Width = 10000;
            //
            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 4);
            merged1.Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 240;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
            rowIndex++;
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 4);
            merged2.Value = "TRƯỜNG ĐẠI HỌC ĐÀ LẠT";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 240;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 8);
            merged3.Value = "DANH SÁCH CÁN BỘ VIÊN CHỨC ĐĂNG KÝ NGHỈ PHÉP NĂM " + nam;
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 300;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged3.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            rowIndex++;
            rowIndex++;

            #region TargetGroup
            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
            mergedStt.Value = "STT";
            SetRegionBorder(mergedStt, true, false, true, true);
            mergedStt.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedHo = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
            mergedHo.Value = "Họ";
            SetRegionBorder(mergedHo, true, false, true, true);
            mergedHo.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTen = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 2);
            mergedTen.Value = "Tên";
            SetRegionBorder(mergedTen, true, false, true, true);
            mergedTen.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedChucVu = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex + 1, 3);
            mergedChucVu.Value = "Chức danh / Chức vụ";
            SetRegionBorder(mergedChucVu, true, false, true, true);
            mergedChucVu.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedThoiGianNghiPhep = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex,5);
            mergedThoiGianNghiPhep.Value = "Thời gian nghỉ phép";
            SetRegionBorder(mergedThoiGianNghiPhep, true, false, true, true);
            mergedThoiGianNghiPhep.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTuNgay = sheet.MergedCellsRegions.Add(rowIndex + 1, 4, rowIndex + 1, 4);
            mergedTuNgay.Value = "Từ ngày";
            SetRegionBorder(mergedTuNgay, true, false, true, true);
            mergedTuNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDenNgay = sheet.MergedCellsRegions.Add(rowIndex +1 , 5, rowIndex + 1, 5);
            mergedDenNgay.Value = "Đến ngày";
            SetRegionBorder(mergedDenNgay, true, false, true, true);
            mergedDenNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDiaDiem = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex, 6);
            mergedDiaDiem.Value = "Địa điểm nghỉ phép";
            SetRegionBorder(mergedDiaDiem, true, false, true, true);
            mergedDiaDiem.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedLyDo = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 7);
            mergedLyDo.Value = "Lý do";
            SetRegionBorder(mergedLyDo, true, false, true, true);
            mergedLyDo.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 8, rowIndex, 8);
            mergedDonVi.Value = "Đơn vị";
            SetRegionBorder(mergedDonVi, true, false, true, true);
            mergedDonVi.CellFormat.Alignment = HorizontalCellAlignment.Center;

            rowIndex++;
            rowIndex++;

            //Lấy STT
            int index = 1;
            foreach (DTO_ChamCongNgayNghi_Find item in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = index;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = item.Ho;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = item.Ten;
                CellBorder(sheet, rowIndex, 2);

                sheet.Rows[rowIndex].Cells[3].Value = item.ChucDanh;
                CellBorder(sheet, rowIndex, 3);

                sheet.Rows[rowIndex].Cells[4].Value = item.TuNgay.Value.ToString("dd/MM/yyyy");
                CellBorder(sheet, rowIndex, 4);

                sheet.Rows[rowIndex].Cells[5].Value = item.DenNgay.Value.ToString("dd/MM/yyyy");
                CellBorder(sheet, rowIndex, 5);

                sheet.Rows[rowIndex].Cells[6].Value = item.NoiNghiPhep;
                CellBorder(sheet, rowIndex, 6);

                sheet.Rows[rowIndex].Cells[7].Value = item.DienGiai;
                CellBorder(sheet, rowIndex, 7);

                sheet.Rows[rowIndex].Cells[8].Value = item.TenPhongBan;
                CellBorder(sheet, rowIndex, 8);
                //
                rowIndex++;
                index++;
            }
            rowIndex++;

            DateTime cur = DateTime.Now;
            WorksheetMergedCellsRegion mergedNgayKy = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 8);
            mergedNgayKy.Value =
            sheet.Rows[rowIndex].Cells[7].Value = "Lâm đồng, Ngày " + cur.Day.ToString() + " tháng " + cur.Month.ToString() + " năm " + cur.Year.ToString();
            mergedNgayKy.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedNgayKy.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedNgayKy.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
            mergedNgayKy.CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
            mergedNgayKy.CellFormat.Font.Name = "Times New Roman";
            mergedNgayKy.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            rowIndex++;

            WorksheetMergedCellsRegion mergedNguoiKy = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            mergedNguoiKy.Value = "LẬP BẢNG";
            mergedNguoiKy.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedNguoiKy.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedNguoiKy.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedNguoiKy.CellFormat.Font.Name = "Times New Roman";
            mergedNguoiKy.CellFormat.Font.Height = 300;
            mergedNguoiKy.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion mergedTruongPhongTC = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            mergedTruongPhongTC.Value = "TRƯỞNG PHÒNG TỔ CHỨC – HÀNH CHÍNH";
            mergedTruongPhongTC.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedTruongPhongTC.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedTruongPhongTC.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedTruongPhongTC.CellFormat.Font.Name = "Times New Roman";
            mergedTruongPhongTC.CellFormat.Font.Height = 300;
            mergedTruongPhongTC.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion mergedHieuTruong = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 8);
            mergedHieuTruong.Value = "HIỆU TRƯỞNG";
            mergedHieuTruong.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedHieuTruong.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedHieuTruong.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedHieuTruong.CellFormat.Font.Name = "Times New Roman";
            mergedHieuTruong.CellFormat.Font.Height = 300;
            mergedHieuTruong.CellFormat.WrapText = ExcelDefaultableBoolean.True;

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