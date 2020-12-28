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
using System.IO;
using System.Drawing;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using HRMWebApp.KPI.Core.Helpers;

namespace HRMWebApp.ExcelExport
{
    /// <summary>
    /// Summary description for KPIReportExport
    /// </summary>
    public class KPIReportExport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Guid planId = context.Request.Params["planId"] != null ? new Guid(context.Request.Params["planId"].ToString()) : Guid.Empty;

            int op = context.Request.Params["option"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["option"].ToString());

            switch (op)
            {
                case 1:
                    {
                        KetQuaDanhGiaKeHoachHoatDongCaNhan(planId, context);
                    }
                    break;
                case 2:
                    {
                        KetQuaDanhGiaKeHoachHoatDongDonVi(planId, context);
                    }
                    break;
                case 3:
                    {
                        Thongkedangkychedolamviec(planId, context);
                    }
                    break;

            }
        }

        protected void SetRegionBorder(WorksheetMergedCellsRegion region, bool isBoldText)
        {
            region.CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;

            region.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            region.CellFormat.Alignment = HorizontalCellAlignment.Center;
            region.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            if (isBoldText)
                region.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        }
        public void KetQuaDanhGiaKeHoachHoatDongCaNhan(Guid planId, HttpContext context)
        {
            var result = DataClassHelper.Report_KetQuaDanhGiaKeHoachHoatDongCaNhan(planId);
            string planName = PlanKPIApiController.GetPlanMonthAndYear(planId);

            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("ThongKe");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["ThongKe"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 6);
            merged1.Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT THÀNH PHỐ HỒ CHÍ MINH";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 12);
            merged2.Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 6);
            merged3.Value = "PHÒNG TỔ CHỨC CÁN BỘ";
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 320;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 12);
            merged4.Value = "Độc lập - Tự do - Hạnh phúc";
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 320;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 1;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 12);
            merged5.Value = "BẢNG TỔNG HỢP \n KẾT QUẢ ĐÁNH GIÁ KẾ HOẠCH HOẠT ĐỘNG CÁ NHÂN " + planName.ToUpper();
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 320;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 4;

            sheet.Columns[0].Width = 50 * 30;
            sheet.Columns[1].Width = 50 * 90;
            sheet.Columns[2].Width = 50 * 130;
            sheet.Columns[3].Width = 50 * 70;
            sheet.Columns[4].Width = 50 * 70;
            sheet.Columns[5].Width = 50 * 70;
            sheet.Columns[6].Width = 50 * 70;
            sheet.Columns[7].Width = 50 * 70;
            sheet.Columns[8].Width = 50 * 70;
            sheet.Columns[9].Width = 50 * 70;
            sheet.Columns[10].Width = 50 * 70;
            sheet.Columns[11].Width = 50 * 70;
            sheet.Columns[12].Width = 50 * 70;

            IEnumerable<string> boPhanList = result.Select(q => q.BoPhan).Distinct();

            int stt = 1;
            foreach (var boPhan in boPhanList)
            {
                WorksheetMergedCellsRegion mergedBoPhan = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 12);
                mergedBoPhan.Value = "Đơn vị: " + boPhan;
                SetCellFormat(sheet, rowIndex, 0, 12, true, false, false, true, true);
                rowIndex++;

                if (stt == 1)
                {
                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    WorksheetMergedCellsRegion mergedCbvc = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
                    mergedCbvc.Value = "Mã CBVC";
                    WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 2);
                    mergedHoTen.Value = "Họ tên";
                    WorksheetMergedCellsRegion mergedTuDanhGia = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
                    mergedTuDanhGia.Value = "Tự đánh giá";
                    WorksheetMergedCellsRegion mergedCapTrenTrucTiep = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex, 10);
                    mergedCapTrenTrucTiep.Value = "Cấp trên trực tiếp";
                    WorksheetMergedCellsRegion mergedTongCong = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex + 1, 11);
                    mergedTongCong.Value = "Tổng cộng";
                    WorksheetMergedCellsRegion mergedXepLoai = sheet.MergedCellsRegions.Add(rowIndex, 12, rowIndex + 1, 12);
                    mergedXepLoai.Value = "Xếp loại";
                    SetCellFormat(sheet, rowIndex, 0, 12, true, false, true, true, true);

                    rowIndex++;

                    sheet.Rows[rowIndex].Cells[3].Value = "NMT# 1";
                    sheet.Rows[rowIndex].Cells[4].Value = "NMT# 2";
                    sheet.Rows[rowIndex].Cells[5].Value = "NMT# 3";
                    sheet.Rows[rowIndex].Cells[6].Value = "Tổng";
                    sheet.Rows[rowIndex].Cells[7].Value = "NMT# 1";
                    sheet.Rows[rowIndex].Cells[8].Value = "NMT# 2";
                    sheet.Rows[rowIndex].Cells[9].Value = "NMT# 3";
                    sheet.Rows[rowIndex].Cells[10].Value = "Tổng";
                    SetCellFormat(sheet, rowIndex, 0, 12, true, false, true, true, true);
                    rowIndex++;
                }

                foreach (var item in result)
                {
                    if (boPhan == item.BoPhan)
                    { 
                        sheet.Rows[rowIndex].Cells[0].Value = stt;
                        sheet.Rows[rowIndex].Cells[1].Value = item.SoHieuCongChuc;
                        sheet.Rows[rowIndex].Cells[2].Value = item.HoTen;
                        sheet.Rows[rowIndex].Cells[3].Value = item.NMT1NV;
                        sheet.Rows[rowIndex].Cells[4].Value = item.NMT2NV;
                        sheet.Rows[rowIndex].Cells[5].Value = item.NMT3NV;
                        sheet.Rows[rowIndex].Cells[6].Value = item.TongNV;
                        sheet.Rows[rowIndex].Cells[7].Value = item.NMT1QL;
                        sheet.Rows[rowIndex].Cells[8].Value = item.NMT2QL;
                        sheet.Rows[rowIndex].Cells[9].Value = item.NMT3QL;
                        sheet.Rows[rowIndex].Cells[10].Value = item.TongQL;
                        sheet.Rows[rowIndex].Cells[11].Value = item.TongKPI;
                        sheet.Rows[rowIndex].Cells[12].Value = item.XepLoai;

                        SetCellFormat(sheet, rowIndex, 0, 12, false, false, false, true, true);
                        sheet.Rows[rowIndex].Cells[12].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        rowIndex++;
                        stt++;
                    }
                }
            }

            string filename = "/Temp/Excel/1.xls";
            BIFF8Writer.WriteWorkbookToFile(workbook, context.Server.MapPath(filename));

            BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
            context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
            reader.Close();
            context.Response.Flush();
        }

        public void KetQuaDanhGiaKeHoachHoatDongDonVi(Guid planId, HttpContext context)
        {
            var result = DataClassHelper.Report_KetQuaDanhGiaKeHoachHoatDongDonVi(planId);
            
            string planName = PlanKPIApiController.GetPlanMonthAndYear(planId);

            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("ThongKe");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["ThongKe"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 10);
            merged1.Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT THÀNH PHỐ HỒ CHÍ MINH";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex, 21);
            merged2.Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 10);
            merged3.Value = "PHÒNG TỔ CHỨC CÁN BỘ";
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 320;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex, 21);
            merged4.Value = "Độc lập - Tự do - Hạnh phúc";
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 320;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 1;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 21);
            merged5.Value = "BẢNG TỔNG HỢP \n KẾT QUẢ ĐÁNH GIÁ KPIS " + planName.ToUpper();
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 320;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 4;

            sheet.Columns[0].Width = 50 * 30;
            sheet.Columns[1].Width = 50 * 180;
            sheet.Columns[2].Width = 50 * 60;
            sheet.Columns[3].Width = 50 * 60;
            sheet.Columns[4].Width = 50 * 60;
            sheet.Columns[5].Width = 50 * 60;
            sheet.Columns[6].Width = 50 * 60;
            sheet.Columns[7].Width = 50 * 60;
            sheet.Columns[8].Width = 50 * 120;
            sheet.Columns[9].Width = 50 * 70;
            sheet.Columns[10].Width = 50 * 60;
            sheet.Columns[11].Width = 50 * 60;
            sheet.Columns[12].Width = 50 * 60;
            sheet.Columns[13].Width = 50 * 60;
            sheet.Columns[14].Width = 50 * 60;
            sheet.Columns[15].Width = 50 * 60;
            sheet.Columns[16].Width = 50 * 60;
            sheet.Columns[17].Width = 50 * 60;
            sheet.Columns[18].Width = 50 * 60;
            sheet.Columns[19].Width = 50 * 60;
            sheet.Columns[20].Width = 50 * 60;
            sheet.Columns[21].Width = 50 * 60;

            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
            mergedStt.Value = "STT";
            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 2, 1);
            mergedDonVi.Value = "Đơn vị";
            WorksheetMergedCellsRegion mergedDiemDonVi = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 4);
            mergedDiemDonVi.Value = "Điểm tự đánh giá của lãnh đạo đơn vị";
            WorksheetMergedCellsRegion mergedDiemBGH = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex + 1, 7);
            mergedDiemBGH.Value = "Điểm do BGH phụ trách đánh giá";
            WorksheetMergedCellsRegion mergedTongDiem_XepLoai = sheet.MergedCellsRegions.Add(rowIndex, 8, rowIndex + 2, 8);
            mergedTongDiem_XepLoai.Value = "Tổng điểm \n (Xếp loại)";
            WorksheetMergedCellsRegion mergedTongSoCBVC = sheet.MergedCellsRegions.Add(rowIndex, 9, rowIndex + 2, 9);
            mergedTongSoCBVC.Value = "Tổng số \n CBVC";
            WorksheetMergedCellsRegion mergedKetQuaPhanLoai = sheet.MergedCellsRegions.Add(rowIndex, 10, rowIndex, 21);
            mergedKetQuaPhanLoai.Value = "Kết quả phân loại";
            SetCellFormat(sheet, rowIndex, 0, 21, true, false, false, true, true);

            rowIndex++;

            WorksheetMergedCellsRegion mergedA = sheet.MergedCellsRegions.Add(rowIndex, 10, rowIndex, 11);
            mergedA.Value = "Hoàn thành xuất sắc nhiệm vụ \n(A)";
            WorksheetMergedCellsRegion mergedB = sheet.MergedCellsRegions.Add(rowIndex, 12, rowIndex, 13);
            mergedB.Value = "Hoàn thành tốt nhiệm vụ \n(B)";
            WorksheetMergedCellsRegion mergedC = sheet.MergedCellsRegions.Add(rowIndex, 14, rowIndex, 15);
            mergedC.Value = "Hoàn thành nhiệm vụ \n(C)";
            WorksheetMergedCellsRegion mergedD = sheet.MergedCellsRegions.Add(rowIndex, 16, rowIndex, 17);
            mergedD.Value = "Hoàn thành xuất sắc nhiệm vụ nhưng còn sai \n(D)";
            WorksheetMergedCellsRegion mergedE = sheet.MergedCellsRegions.Add(rowIndex, 18, rowIndex, 19);
            mergedE.Value = "Chưa hoàn thành nhiệm vụ \n(E)";
            WorksheetMergedCellsRegion mergedF = sheet.MergedCellsRegions.Add(rowIndex, 20, rowIndex, 21);
            mergedF.Value = "Không xếp loại \n(F)";
            SetCellFormat(sheet, rowIndex, 0, 21, true, false, false, true, true);
            sheet.Rows[rowIndex].Height = 900;

            rowIndex++;

            sheet.Rows[rowIndex].Cells[2].Value = "NMT# 1";
            sheet.Rows[rowIndex].Cells[3].Value = "NMT# 2";
            sheet.Rows[rowIndex].Cells[4].Value = "NMT# 3";
            sheet.Rows[rowIndex].Cells[5].Value = "NMT# 1";
            sheet.Rows[rowIndex].Cells[6].Value = "NMT# 2";
            sheet.Rows[rowIndex].Cells[7].Value = "NMT# 3";

            sheet.Rows[rowIndex].Cells[10].Value = "SL";
            sheet.Rows[rowIndex].Cells[11].Value = "%";
            sheet.Rows[rowIndex].Cells[12].Value = "SL";
            sheet.Rows[rowIndex].Cells[13].Value = "%";
            sheet.Rows[rowIndex].Cells[14].Value = "SL";
            sheet.Rows[rowIndex].Cells[15].Value = "%";
            sheet.Rows[rowIndex].Cells[16].Value = "SL";
            sheet.Rows[rowIndex].Cells[17].Value = "%";
            sheet.Rows[rowIndex].Cells[18].Value = "SL";
            sheet.Rows[rowIndex].Cells[19].Value = "%";
            sheet.Rows[rowIndex].Cells[20].Value = "SL";
            sheet.Rows[rowIndex].Cells[21].Value = "%";
            SetCellFormat(sheet, rowIndex, 0, 21, true, false, false, true, true);

            rowIndex++;

            int stt = 1;
            foreach (var item in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = stt;
                sheet.Rows[rowIndex].Cells[1].Value = item.TenBoPhan;
                sheet.Rows[rowIndex].Cells[2].Value = item.NMT1QL;
                sheet.Rows[rowIndex].Cells[3].Value = item.NMT2QL;
                sheet.Rows[rowIndex].Cells[4].Value = item.NMT3QL;
                sheet.Rows[rowIndex].Cells[5].Value = item.NMT1BGH;
                sheet.Rows[rowIndex].Cells[6].Value = item.NMT2BGH;
                sheet.Rows[rowIndex].Cells[7].Value = item.NMT3BGH;
                sheet.Rows[rowIndex].Cells[8].Value = ChuoiXuongDong(item.TongDiem_XepLoai);
                sheet.Rows[rowIndex].Cells[9].Value = item.SoNV;
                sheet.Rows[rowIndex].Cells[10].Value = item.NV_A;
                sheet.Rows[rowIndex].Cells[11].Value = item.PhanTram_A;
                sheet.Rows[rowIndex].Cells[12].Value = item.NV_B;
                sheet.Rows[rowIndex].Cells[13].Value = item.PhanTram_B;
                sheet.Rows[rowIndex].Cells[14].Value = item.NV_C;
                sheet.Rows[rowIndex].Cells[15].Value = item.PhanTram_C;
                sheet.Rows[rowIndex].Cells[16].Value = item.NV_D;
                sheet.Rows[rowIndex].Cells[17].Value = item.PhanTram_D;
                sheet.Rows[rowIndex].Cells[18].Value = item.NV_E;
                sheet.Rows[rowIndex].Cells[19].Value = item.PhanTram_E;
                sheet.Rows[rowIndex].Cells[20].Value = item.NV_F;
                sheet.Rows[rowIndex].Cells[21].Value = item.PhanTram_F;

                SetCellFormat(sheet, rowIndex, 0, 21, false, false, false, true, true);
                sheet.Rows[rowIndex].Cells[8].CellFormat.Alignment = HorizontalCellAlignment.Center;
                rowIndex++;
                stt++;
            }

            string filename = "/Temp/Excel/1.xls";
            BIFF8Writer.WriteWorkbookToFile(workbook, context.Server.MapPath(filename));

            BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
            context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
            reader.Close();
            context.Response.Flush();
        }

        public void Thongkedangkychedolamviec(Guid planId, HttpContext context)
        {
            var result = DataClassHelper.Report_KetQuaDangKyCheDoLamViec(planId);

            string planName = PlanKPIApiController.GetPlanMonthAndYear(planId);

            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("ThongKe");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["ThongKe"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged1.Value = "TRƯỜNG ĐẠI HỌC SƯ PHẠM KỸ THUẬT THÀNH PHỐ HỒ CHÍ MINH";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 200;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex, 7);
            merged2.Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 200;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged3.Value = "PHÒNG TỔ CHỨC CÁN BỘ";
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 200;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex, 7);
            merged4.Value = "Độc lập - Tự do - Hạnh phúc";
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 200;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 1;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 8);
            merged5.Value = "BẢNG TỔNG HỢP \n KẾT QUẢ ĐĂNG KÝ CHẾ ĐỘ LÀM VIỆC " + planName.ToUpper();
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 250;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex += 4;

            sheet.Columns[0].Width = 50 * 30;
            sheet.Columns[1].Width = 50 * 180;
            sheet.Columns[2].Width = 50 * 180;
            sheet.Columns[3].Width = 50 * 180;
            sheet.Columns[4].Width = 50 * 180;
            sheet.Columns[5].Width = 50 * 180;
            sheet.Columns[6].Width = 50 * 60;
            sheet.Columns[7].Width = 50 * 60;
            sheet.Columns[8].Width = 50 * 60;
            sheet.Columns[9].Width = 50 * 150;

            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
            mergedStt.Value = "STT";
            SetRegionBorder(mergedStt, true);

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
            mergedDonVi.Value = "Đơn vị";
            SetRegionBorder(mergedDonVi, true);

            WorksheetMergedCellsRegion mergedMaCanBo = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 2);
            mergedMaCanBo.Value = "Mã cán bộ";
            SetRegionBorder(mergedMaCanBo, true);

            WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex + 1, 3);
            mergedHoTen.Value = "Họ tên";
            SetRegionBorder(mergedHoTen, true);

            WorksheetMergedCellsRegion mergedBoMon = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex + 1, 4);
            mergedBoMon.Value = "Bộ môn";
            SetRegionBorder(mergedBoMon, true);

            WorksheetMergedCellsRegion mergedCheDoDangKy = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex + 1, 5);
            mergedCheDoDangKy.Value = "Chế độ đăng ký";
            SetRegionBorder(mergedCheDoDangKy, true);

            WorksheetMergedCellsRegion mergedHocHam = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex + 1, 6);
            mergedHocHam.Value = "Học hàm";
            SetRegionBorder(mergedHocHam, true);

            WorksheetMergedCellsRegion mergedHocVi = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex + 1, 7);
            mergedHocVi.Value = "Học vị";
            SetRegionBorder(mergedHocVi, true);

            WorksheetMergedCellsRegion mergedDuyet = sheet.MergedCellsRegions.Add(rowIndex, 8, rowIndex + 1, 8);
            mergedDuyet.Value = "Duyệt";
            SetRegionBorder(mergedDuyet, true);

            WorksheetMergedCellsRegion mergedTime = sheet.MergedCellsRegions.Add(rowIndex, 9, rowIndex + 1, 9);
            mergedTime.Value = "Thời gian đăng ký";
            SetRegionBorder(mergedTime, true);

            SetCellFormat(sheet, rowIndex, 0, 9, true, false, false, true, true);
            

            rowIndex+= 2;

            int stt = 1;
            foreach (var item in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = stt;
                sheet.Rows[rowIndex].Cells[1].Value = item.Khoa;
                sheet.Rows[rowIndex].Cells[2].Value = item.MaCBVC;
                sheet.Rows[rowIndex].Cells[3].Value = item.HoTen;
                sheet.Rows[rowIndex].Cells[4].Value = item.BoMon;
                sheet.Rows[rowIndex].Cells[5].Value = item.CheDoDangKy;
                sheet.Rows[rowIndex].Cells[6].Value = item.HocHam;
                sheet.Rows[rowIndex].Cells[7].Value = item.HocVi;
                sheet.Rows[rowIndex].Cells[8].Value = item.Duyet == false ? "Chưa duyệt" : "Đã duyệt";
                sheet.Rows[rowIndex].Cells[9].Value = item.Time.ToString("dd/MM/yyyy HH:mm:ss");


                SetCellFormat(sheet, rowIndex, 0, 9, false, false, false, true, true);
                rowIndex++;
                stt++;
            }

            string filename = "/Temp/Excel/1.xls";
            BIFF8Writer.WriteWorkbookToFile(workbook, context.Server.MapPath(filename));

            BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
            context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
            reader.Close();
            context.Response.Flush();

        }

        public string ChuoiXuongDong(string input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    result += '\n'.ToString();
                    result += input[i].ToString();
                }
                else
                {
                    result += input[i].ToString();
                }

            }
            return result;
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