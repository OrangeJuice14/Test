using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Documents.Excel;
using ERP_Core;
using System.IO;
using System.Drawing;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;

namespace HRMChamCong.ExcelExport
{
    /// <summary>
    /// Summary description for InBangCong
    /// </summary>
    public class InBangCong : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ngay = context.Request.Params["ngay"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["ngay"].ToString());
            int thang = context.Request.Params["thang"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["thang"].ToString());
            int nam = context.Request.Params["nam"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["nam"].ToString());
            Guid bophanId = (context.Request.Params["bophanId"] != null && context.Request.Params["bophanId"].ToString() != "undefined") ? new Guid(context.Request.Params["bophanId"].ToString()) : Guid.Empty;

            BangChamCongExportToExcelProcess(ngay, thang, nam, bophanId, context);
        }

        public void BangChamCongExportToExcelProcess(int ngay, int thang, int nam, Guid bophanId, HttpContext context)
        {
            var factory = CC_KyChamCong_Factory.New();
            CC_KyChamCong kyCC = factory.GetByDate(new DateTime(nam, thang, ngay));
            var result = new List<spd_Report_ChamCong_TheoDoiChamCong_Result>();
            result = factory.Context.spd_Report_ChamCong_TheoDoiChamCong(kyCC.TuNgay, kyCC.DenNgay, bophanId, false).ToList();

            DateTime now = DateTime.Now;
            string tenDonVi = "";
            string tenFile = "Toàn trường";
            if (bophanId != Guid.Empty)
            {
                if (result.Count > 0)
                {
                    tenDonVi = result.First().TenBoPhan;
                    tenFile = tenDonVi;
                }
                else
                {
                    tenDonVi = "Không có dữ liệu";
                    tenFile = "HBU";
                }
            }
            else
            {
                tenDonVi = "Toàn trường";
            }
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", tenFile, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            str = RemoveWhitespace(str);
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 240;
            workbook.Styles.NormalStyle.StyleFormat.WrapText = ExcelDefaultableBoolean.True;
            Worksheet sheet = workbook.Worksheets.Add("GioQuetGoc");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["GioQuetGoc"];
            sheet.PrintOptions.PaperSize = PaperSize.A3;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.38;
            sheet.PrintOptions.RightMargin = 0.38;
            sheet.PrintOptions.BottomMargin = 0.38;
            sheet.PrintOptions.TopMargin = 0.38;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;

            sheet.Columns[0].Width = 1500;
            sheet.Columns[1].Width = 3000;
            sheet.Columns[2].Width = 7500;
            if (bophanId != Guid.Empty)
            {
                sheet.Columns[3].Width = 5000;
                sheet.Columns[4].Width = 3000;
                sheet.Columns[5].Width = 2000;
            }
            else
            {
                sheet.Columns[3].Width = 8000;
                sheet.Columns[4].Width = 5000;
                sheet.Columns[5].Width = 3000;
            }

            int rowIndex = 0;
            int maxColumn = 0;
            if (bophanId == Guid.Empty)
                maxColumn = 15;
            else maxColumn = 14;

            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, maxColumn);
            merged1.Value = "TRƯỜNG ĐẠI HỌC QUỐC TẾ HỒNG BÀNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 240;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, maxColumn);
            merged2.Value = "BẢNG GIỜ QUÉT VÂN TAY";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 240;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, maxColumn);
            merged3.Value = "(" + kyCC.TuNgay.ToString("dd/MM/yyyy") + " - " + kyCC.DenNgay.ToString("dd/MM/yyyy") + ")";
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 240;
            rowIndex++;

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, maxColumn);
            merged5.Value = "Đơn vị: " + tenDonVi;
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 240;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            rowIndex++;

            int column = 0;
            sheet.Rows[rowIndex].Cells[column++].Value = "STT";
            sheet.Rows[rowIndex].Cells[column++].Value = "Mã quản lý";
            sheet.Rows[rowIndex].Cells[column++].Value = "Họ và tên";
            if (bophanId == Guid.Empty)
            {
                sheet.Rows[rowIndex].Cells[column++].Value = "Tên bộ phận";
            }
            sheet.Rows[rowIndex].Cells[column++].Value = "Tên chức danh";
            sheet.Rows[rowIndex].Cells[column++].Value = "Ngày";
            sheet.Rows[rowIndex].Cells[column++].Value = "Thứ";
            sheet.Rows[rowIndex].Cells[column++].Value = "Máy vào";
            sheet.Rows[rowIndex].Cells[column++].Value = "Giờ vào";
            sheet.Rows[rowIndex].Cells[column++].Value = "Thời gian vào sáng";
            sheet.Rows[rowIndex].Cells[column++].Value = "Đi trễ";
            sheet.Rows[rowIndex].Cells[column++].Value = "Máy ra";
            sheet.Rows[rowIndex].Cells[column++].Value = "Giờ ra";
            sheet.Rows[rowIndex].Cells[column++].Value = "Thời gian ra chiều";
            sheet.Rows[rowIndex].Cells[column++].Value = "Về sớm";
            sheet.Rows[rowIndex].Cells[column].Value = "Vắng";
            SetCellFormat(sheet, rowIndex, 0, column, true, false, true, true, true);
            BackgroundColor(sheet, rowIndex, 0, column, Color.LightGray);
            rowIndex++;

            int stt = 1;
            foreach(spd_Report_ChamCong_TheoDoiChamCong_Result item in result)
            {
                column = 0;
                sheet.Rows[rowIndex].Cells[column].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[column++].Value = stt++;
                sheet.Rows[rowIndex].Cells[column++].Value = item.MaQuanLy;
                sheet.Rows[rowIndex].Cells[column++].Value = item.HoTen;
                if (bophanId == Guid.Empty)
                    sheet.Rows[rowIndex].Cells[column++].Value = item.TenBoPhan;
                sheet.Rows[rowIndex].Cells[column++].Value = item.TenChucDanh;
                sheet.Rows[rowIndex].Cells[column++].Value = item.Ngay;
                sheet.Rows[rowIndex].Cells[column++].Value = item.Thu;
                sheet.Rows[rowIndex].Cells[column++].Value = item.MayVao;
                sheet.Rows[rowIndex].Cells[column++].Value = item.GioVao;
                sheet.Rows[rowIndex].Cells[column++].Value = item.ThoiGianVaoSang;
                sheet.Rows[rowIndex].Cells[column++].Value = item.DiTre;
                sheet.Rows[rowIndex].Cells[column++].Value = item.MayRa;
                sheet.Rows[rowIndex].Cells[column++].Value = item.GioRa;
                sheet.Rows[rowIndex].Cells[column++].Value = item.ThoiGianRaChieu;
                sheet.Rows[rowIndex].Cells[column++].Value = item.VeSom;
                sheet.Rows[rowIndex].Cells[column].Value = item.Vang;
                SetCellFormat(sheet, rowIndex, 0, column, false, false, false, true, true);
                rowIndex++;
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


        public static void CellBorder(Worksheet sheet, int indexrow, int indexcolumn)
        {
            sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
            sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
            sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
            sheet.Rows[indexrow].Cells[indexcolumn].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
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

        public static string RemoveWhitespace(string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }

            return new String(newarr, 0, j);
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