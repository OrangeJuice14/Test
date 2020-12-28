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
    public class InBangCong : IHttpHandler
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
            Guid idNhanVien = context.Request.Params["idNhanVien"] != "null" ? new Guid(context.Request.Params["idNhanVien"].ToString()) : Guid.Empty;
            string webGroupId = context.Request.Params["webGroupId"] != null ? context.Request.Params["webGroupId"].ToString() : "";
            BangChamCongExportToExcelProcess(thang, nam, bophanId, idNhanVien, webGroupId, null, Guid.Empty, context);
        }

        public void BangChamCongExportToExcelProcess(int thang, int nam, Guid bophanId, Guid idNhanVien, string webGroupId, string maNhanSu, Guid idLoaiNhanSu, HttpContext context)
        {
            IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> result = null;

            CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                result = factory.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(thang, nam, idNhanVien);
            else
                result = factory.QuanLyChamCong_ThongTinChamCongThang(thang, nam, bophanId, maNhanSu, idLoaiNhanSu);
            BoPhan_Factory bpfac = new BoPhan_Factory();
            BoPhan DonVi = bpfac.GetByID(bophanId);

            List<DTO_NgayChamCong> listNgay = factory.GetList_NgayTrongKyChamCong(thang, nam).ToList();
            int songay = listNgay.Count();


            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}",DonVi.MaQuanLy, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
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
            sheet.Columns[1].Width = 4000;
            for (int i=3;i<= songay + 2;i++)
            {
                sheet.Columns[i].Width = 900;
                sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            }
            for (int i = songay+4; i <= songay + 7; i++)
            {
                sheet.Columns[i].Width = 3000;
                sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            }
            int rowIndex = 0;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged1.Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 240;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged2.Value = "TRƯỜNG ĐẠI HỌC ĐÀ LẠT";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 240;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged4.Value = "LÂM ĐỒNG";
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 240;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(0,12, 2, songay + 7);
            merged3.Value = "BẢNG CHẤM CÔNG VÀ PHÂN LOẠI LAO ĐỘNG";               
            //Khoảng  trắng để wrap text                                                                                                           
            merged3.Value+= "                                                                                                              THÁNG " + thang + "/" + nam;
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 300;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged3.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, songay+7);
            merged5.Value = "ĐƠN VỊ: " +DonVi.TenBoPhan.ToUpper();
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 260;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            rowIndex++;

            #region TargetGroup
            BackgroundColor(sheet, rowIndex, 0, songay + 6, Color.LightGray);
            BackgroundColor(sheet, rowIndex+1, 2, songay+2, Color.LightGray);
            BackgroundColor(sheet, rowIndex+2, 0, songay+2, Color.LightGray);

            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
            mergedStt.Value = "STT";
            SetRegionBorder(mergedStt, true, false, true, true);
            mergedStt.CellFormat.Alignment= HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedHo = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 2,1);
            mergedHo.Value = "Họ";
            SetRegionBorder(mergedHo, true, false, true, true);
            mergedHo.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTen = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 2, 2);
            mergedTen.Value = "Tên";
            SetRegionBorder(mergedTen, true, false, true, true);
            mergedTen.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNgayTrongThang = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex,songay+2);
            mergedNgayTrongThang.Value = "Ngày trong tháng";
            SetRegionBorder(mergedNgayTrongThang, true, false, true, true);
            mergedNgayTrongThang.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNgayCong = sheet.MergedCellsRegions.Add(rowIndex, songay+3, rowIndex + 2, songay + 3);
            mergedNgayCong.Value = "Số công hưởng lương thời gian";
            SetRegionBorder(mergedNgayCong, true, false, true, true);
            mergedNgayCong.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedHuongLuong = sheet.MergedCellsRegions.Add(rowIndex, songay + 4, rowIndex + 2, songay + 4);
            mergedHuongLuong.Value = "Số công nghỉ không lương";
            SetRegionBorder(mergedHuongLuong, true, false, true, true);
            mergedHuongLuong.CellFormat.Alignment = HorizontalCellAlignment.Center;

            //WorksheetMergedCellsRegion mergedDiHoc = sheet.MergedCellsRegions.Add(rowIndex, songay + 5, rowIndex + 2, songay + 5);
            //mergedDiHoc.Value = "Tổng ngày đi học, bồi dưỡng ngắn hạn";
            //SetRegionBorder(mergedDiHoc, true, false, true, true);
            //mergedDiHoc.CellFormat.Alignment = HorizontalCellAlignment.Center;

            //WorksheetMergedCellsRegion mergedKhongLuong = sheet.MergedCellsRegions.Add(rowIndex, songay + 6, rowIndex + 2, songay + 6);
            //mergedKhongLuong.Value = "Tổng không lương";
            //SetRegionBorder(mergedKhongLuong, true, false, true, true);
            //mergedKhongLuong.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedBHXH = sheet.MergedCellsRegions.Add(rowIndex, songay + 5, rowIndex + 2, songay + 5);
            mergedBHXH.Value = "Số công hưởng BHXH";
            SetRegionBorder(mergedBHXH, true, false, true, true);
            mergedBHXH.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDanhGia = sheet.MergedCellsRegions.Add(rowIndex, songay + 6, rowIndex + 2, songay + 6);
            mergedDanhGia.Value = "Đánh giá";
            SetRegionBorder(mergedDanhGia, true, false, true, true);
            mergedDanhGia.CellFormat.Alignment = HorizontalCellAlignment.Center;

            int index = 3;
            rowIndex++;
            foreach (DTO_NgayChamCong date in listNgay)
            {
                sheet.Rows[rowIndex].Cells[index].Value =date.Ngay;
                sheet.Rows[rowIndex+1].Cells[index].Value = date.Thu;
                index++;
            }
            SetCellFormat(sheet, rowIndex, 2,1+songay, true, false, false, true,false);
            SetCellFormat(sheet, rowIndex+1, 2, 1 + songay, true, false, false,false, true);
            rowIndex++;
            rowIndex++;

            //Lấy STT
            index = 1;
            foreach (DTO_QuanLyChamCong_ThongTinChamCongThang ccThang in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = index;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = ccThang.Ho;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = ccThang.Ten;
                CellBorder(sheet, rowIndex, 2);

                int idx = 3;
                foreach (DTO_QuanLyChamCong_ChamCongNgay cc in ccThang.ChiTietChamCong)
                {
                    sheet.Rows[rowIndex].Cells[idx].Value = cc.MaHinhThucNghi;
                    CellBorder(sheet, rowIndex, idx);
                    idx++;
                }
                sheet.Rows[rowIndex].Cells[idx].Value = ccThang.NgayCong;
                CellBorder(sheet, rowIndex, idx);

                //sheet.Rows[rowIndex].Cells[idx + 1].Value = ccThang.TongHuongLuong;
                //CellBorder(sheet, rowIndex, idx+1);

                //sheet.Rows[rowIndex].Cells[idx + 2].Value = ccThang.TongDiHoc;
                //CellBorder(sheet, rowIndex, idx+2);

                sheet.Rows[rowIndex].Cells[idx + 1].Value = ccThang.TongKhongLuong;
                CellBorder(sheet, rowIndex, idx + 1);

                sheet.Rows[rowIndex].Cells[idx + 2].Value = ccThang.TongBHXH;
                CellBorder(sheet, rowIndex, idx+2);

                sheet.Rows[rowIndex].Cells[idx + 3].Value = ccThang.DanhGia;
                CellBorder(sheet, rowIndex, idx + 3);
                //
                rowIndex++;
                index++;
            }
            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "KÝ HIỆU CHẤM CÔNG: ";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold= ExcelDefaultableBoolean.True;

            DateTime cur = DateTime.Now;
            sheet.Rows[rowIndex].Cells[35].Value = "Ngày " + cur.Day.ToString() +" tháng " + cur.Month.ToString() + " năm " + cur.Year.ToString();
            sheet.Rows[rowIndex].Cells[35].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[35].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[35].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;

            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "+";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;

            sheet.Rows[rowIndex].Cells[1].Value = "Lương thời gian";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            sheet.Rows[rowIndex].Cells[15].Value = "Người chấm công";
            sheet.Rows[rowIndex].Cells[15].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[15].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            sheet.Rows[rowIndex].Cells[27].Value = "Trưởng Đơn vị";
            sheet.Rows[rowIndex].Cells[27].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[27].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            sheet.Rows[rowIndex].Cells[35].Value = "Phòng Tổ chức - Cán bộ";
            sheet.Rows[rowIndex].Cells[35].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[35].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;

            CC_HinhThucNghi_Factory factoryHTN = new CC_HinhThucNghi_Factory();
            List<DTO_HinhThucNghi> listHinhThucNghi = factoryHTN.GetListForUpdate().Map<DTO_HinhThucNghi>().ToList();
            int a=Convert.ToInt32(listHinhThucNghi.Count()/2);
            int b = 1;
            int rowIndex2 = rowIndex-1;
            int rowIndex3= rowIndex - 1;
            foreach (DTO_HinhThucNghi htn in listHinhThucNghi)
            {
                if(b<=a)
                {
                    sheet.Rows[rowIndex].Cells[0].Value = htn.KyHieu;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;

                    sheet.Rows[rowIndex].Cells[1].Value = htn.TenHinhThucNghi;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;
                    rowIndex++;
                    b++;
                } 
                else
                {
                    sheet.Rows[rowIndex2].Cells[4].Value = htn.KyHieu;
                    sheet.Rows[rowIndex2].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Left;

                    sheet.Rows[rowIndex2].Cells[6].Value = htn.TenHinhThucNghi;
                    sheet.Rows[rowIndex2].Cells[6].CellFormat.Alignment = HorizontalCellAlignment.Left;
                    sheet.Rows[rowIndex2].Cells[6].CellFormat.WrapText = ExcelDefaultableBoolean.False;
                    rowIndex2++;
                    b++;
                }            
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