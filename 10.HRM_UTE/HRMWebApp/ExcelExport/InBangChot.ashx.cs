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
    public class InBangChot : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int thang = context.Request.Params["thang"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["thang"].ToString());
            int nam = context.Request.Params["nam"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["nam"].ToString());
            Guid bophanId = context.Request.Params["bophanId"] != null ? new Guid(context.Request.Params["bophanId"].ToString()) : Guid.Empty;
            ChiTietChamCongExportToExcelProcess(thang, nam, Guid.Empty, context);
        }

        public void ChiTietChamCongExportToExcelProcess(int thang, int nam, Guid bophanId, HttpContext context)
        {
            List<DTO_BangChiTietChamCongThang> result = null;
            CC_ChiTietChamCongNhanVien_Factory factory = new CC_ChiTietChamCongNhanVien_Factory();
            KyTinhLuong_Factory kyFact = new KyTinhLuong_Factory();
            Guid kyTinhLuong = kyFact.GetIdByThangNam_GCRecordIsNull(thang, nam);
            try
            {
                result = factory.Context.spd_Report_CacThongTinLamCoSoTinhTraLuong(kyTinhLuong,false).Map<DTO_BangChiTietChamCongThang>().ToList();
            }
            catch (Exception) { }

            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("BangChamCong");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["BangChamCong"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 1 cm
            sheet.PrintOptions.LeftMargin = 0.385;
            sheet.PrintOptions.RightMargin = 0.385;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            sheet.Columns[0].Width =1500;
            sheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[1].Width = 6500;
            sheet.Columns[2].Width = 6500;
            for (int i = 3; i <10; i++)
            {
                sheet.Columns[i].Width = 2700;
            }
            for (int i = 10; i < 18; i++)
            {
                sheet.Columns[i].Width = 3500;
            }
            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 5);
            merged1.Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 220;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 5);
            merged2.Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 220;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex,5);
            merged4.Value = "THÀNH PHỐ HỒ CHÍ MINH";
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 220;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(0,6,2,17);
            merged3.Value = "BẢNG CHẤM CÔNG THÁNG " + thang + "/" + nam;                                                                                                                     
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 260;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged3.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            rowIndex++;

            #region TargetGroup
            BackgroundColor(sheet, rowIndex, 0, 17, Color.LightGray);
            BackgroundColor(sheet, rowIndex+1, 0, 17, Color.LightGray);

            for (int a =0;a<11;a++)
            {
                sheet.Rows[rowIndex].Cells[a].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[a].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[a].CellFormat.WrapText= ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[a].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                CellBorder(sheet, rowIndex, a);
            }
            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
            mergedStt.Value = "STT";
            SetRegionBorder(mergedStt, true, false, true, true);
            mergedStt.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
            mergedHoTen.Value = "Họ tên";
            SetRegionBorder(mergedHoTen, true, false, true, true);
            mergedHoTen.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 2);
            mergedDonVi.Value = "Đơn vị";
            SetRegionBorder(mergedDonVi, true, false, true, true);
            mergedDonVi.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 10, rowIndex + 1, 10);
            merged5.Value = "Tổng ngày công làm việc thực tế";
            SetRegionBorder(merged5, true, false, true, true);
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex + 1, 11);
            merged6.Value = "Tổng ngày công hưởng lương";
            SetRegionBorder(merged6, true, false, true, true);
            merged6.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 12, rowIndex + 1, 12);
            merged7.Value = "Tổng ngày đi học ĐT, BD";
            SetRegionBorder(merged7, true, false, true, true);
            merged7.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex + 1, 13);
            merged8.Value = "Tổng ngày công  không hưởng lương";
            SetRegionBorder(merged8, true, false, true, true);
            merged8.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 14, rowIndex + 1, 14);
            merged9.Value = "Tổng ngày công hưởng BHXH";
            SetRegionBorder(merged9, true, false, true, true);
            merged9.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 15, rowIndex + 1, 15);
            merged10.Value = "Xếp loại lao động";
            SetRegionBorder(merged10, true, false, true, true);
            merged10.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged11 = sheet.MergedCellsRegions.Add(rowIndex, 16, rowIndex + 1, 16);
            merged11.Value = "Lý do xếp loại B";
            SetRegionBorder(merged11, true, false, true, true);
            merged11.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged12 = sheet.MergedCellsRegions.Add(rowIndex, 17, rowIndex + 1, 17);
            merged12.Value = "Ghi chú";
            SetRegionBorder(merged12, true, false, true, true);
            merged12.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion merged13 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 9);
            merged13.Value = "Số ngày công giảm";
            SetRegionBorder(merged13, true, false, true, true);
            merged13.CellFormat.Alignment = HorizontalCellAlignment.Center;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[3].Value = "Nghỉ phép";
            sheet.Rows[rowIndex].Cells[4].Value = "Nghỉ đám cưới";
            sheet.Rows[rowIndex].Cells[5].Value = "Nghỉ đám tang";
            sheet.Rows[rowIndex].Cells[6].Value = "Nghỉ ốm";
            sheet.Rows[rowIndex].Cells[7].Value = "Nghỉ thai sản";
            sheet.Rows[rowIndex].Cells[8].Value = "Nghỉ việc riêng";
            sheet.Rows[rowIndex].Cells[9].Value = "Nghỉ khác";

            for (int a = 3; a < 10; a++)
            {
                sheet.Rows[rowIndex].Cells[a].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[a].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[a].CellFormat.WrapText = ExcelDefaultableBoolean.True;
                CellBorder(sheet, rowIndex, a);
            }
            rowIndex++;
            //Lấy STT
            int index = 1;
            foreach (DTO_BangChiTietChamCongThang ccThang in result)
            {
                for (int a = 3; a < 18; a++)
                {
                    sheet.Rows[rowIndex].Cells[a].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[a].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[a].CellFormat.WrapText = ExcelDefaultableBoolean.True;
                    CellBorder(sheet, rowIndex, a);
                }

                sheet.Rows[rowIndex].Cells[0].Value = ccThang.STT;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = ccThang.HoTen;
                sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.True;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = ccThang.TenBoPhan;
                sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[2].CellFormat.WrapText = ExcelDefaultableBoolean.True;
                CellBorder(sheet, rowIndex, 2);

                sheet.Rows[rowIndex].Cells[3].Value = ccThang.TongNghiPhep;
                sheet.Rows[rowIndex].Cells[4].Value = ccThang.TongNghiDamCuoi;
                sheet.Rows[rowIndex].Cells[5].Value = ccThang.TongNghiDamTang;
                sheet.Rows[rowIndex].Cells[6].Value = ccThang.TongNghiOm;
                sheet.Rows[rowIndex].Cells[7].Value = ccThang.TongNghiThaiSan;
                sheet.Rows[rowIndex].Cells[8].Value = ccThang.TongNghiViecRieng;
                sheet.Rows[rowIndex].Cells[9].Value = ccThang.TongNghiKhac;
                sheet.Rows[rowIndex].Cells[10].Value = ccThang.TongNgayCongLamViec;
                sheet.Rows[rowIndex].Cells[11].Value = ccThang.TongNgayCongHuongLuong;
                sheet.Rows[rowIndex].Cells[12].Value = ccThang.TongDiHoc;
                sheet.Rows[rowIndex].Cells[13].Value = ccThang.TongKhongLuong;
                sheet.Rows[rowIndex].Cells[14].Value = ccThang.TongBHXH;
                sheet.Rows[rowIndex].Cells[15].Value = ccThang.XepLoai;

                sheet.Rows[rowIndex].Cells[17].Value = ccThang.DienGiai;
                rowIndex++;
                index++;
            }
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