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


namespace HRMWebApp.ExcelExport
{
    public class StaffSyntheticEvaluation : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating)
            Guid evaluationId = context.Request.Params["evaluationId"] != null ? new Guid(context.Request.Params["evaluationId"].ToString()) : Guid.Empty;
            Guid departmentId = context.Request.Params["departmentId"] != null ? new Guid(context.Request.Params["departmentId"].ToString()) : Guid.Empty;
            string type = context.Request.Params["type"];
            StaffSyntheticEvaluationToExcelProcess(evaluationId, departmentId, type, context);
        }
        public void StaffSyntheticEvaluationToExcelProcess(Guid evaluationId, Guid departmentId, string type, HttpContext context)
        {
            //type=1: phân loại NV,GV
            //type=2: phân loại trưởng đơn vị
            List<ABC_EvaluationBoardStaffDTO> detailResult = new List<ABC_EvaluationBoardStaffDTO>();

            ABC_EvaluationBoardApiController controller = new ABC_EvaluationBoardApiController();
            if (type == "1")
                detailResult = controller.GetListStaffSyntheticEvaluationExcel(evaluationId, departmentId, false);
            else if (type == "2")
                detailResult = controller.GetListDepartmentLeaderSyntheticEvaluationExcel(evaluationId);

            ABC_EvaluationBoard eb = controller.GetObj(evaluationId);
            DepartmentDTO dept = controller.GetDepartment(departmentId);
            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = "";
            if (type == "1")
                str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", dept.Name, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            else if (type == "2")
                str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", "Đánh giá phân loại Trưởng đơn vị", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            str = RemoveWhitespace(str);
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 240;
            workbook.Styles.NormalStyle.StyleFormat.Font.Bold = ExcelDefaultableBoolean.True;
            workbook.Styles.NormalStyle.StyleFormat.Alignment = HorizontalCellAlignment.Center;
            workbook.Styles.NormalStyle.StyleFormat.VerticalAlignment = VerticalCellAlignment.Center;
            workbook.Styles.NormalStyle.StyleFormat.WrapText = ExcelDefaultableBoolean.True;


            Worksheet sheet = workbook.Worksheets.Add("DanhGia");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["DanhGia"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 1 cm
            sheet.PrintOptions.LeftMargin = 0.57;
            sheet.PrintOptions.RightMargin = 0.38;
            sheet.PrintOptions.BottomMargin = 0.38;
            sheet.PrintOptions.TopMargin = 0.38;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Portrait;

            sheet.Columns[0].Width = 1000;
            sheet.Columns[1].Width = 6000;
            sheet.Columns[2].Width = 4000;
            sheet.Columns[3].Width = 3000;
            sheet.Columns[4].Width = 2500;
            sheet.Columns[5].Width = 2500;
            sheet.Columns[6].Width = 3000;

            int rowIndex = 0;
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            //merged2.Value = "NGÂN HÀNG NHÀ NƯỚC VIỆT NAM";

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            merged3.Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            rowIndex++;
            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged4.Value = "TRƯỜNG ĐẠI HỌC KINH TẾ - LUẬT";

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            merged5.Value = "Độc lập - Tự do - Hạnh phúc";

            rowIndex++;
            WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged6.Value = "TP. HỒ CHÍ MINH";

            WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            merged7.Value = "-----------------------------------";

            rowIndex++;
            WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged8.Value = "------------------------------------------";

            rowIndex++;
            rowIndex++;
            WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 6);
            if (type == "1")
                merged9.Value = "PHIẾU TỔNG HỢP ĐÁNH GIÁ VÀ PHÂN LOẠI CÔNG CHỨC, VIÊN CHỨC";
            else if (type == "2")
                merged9.Value = "PHIẾU TỔNG HỢP ĐÁNH GIÁ VÀ PHÂN LOẠI CÁC TRƯỞNG ĐƠN VỊ";
            rowIndex++;

            WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 6);
            if (type == "1")
            {
                if (eb.EvaluationBoardType == 3)
                    merged10.Value = "VÀ NGƯỜI LAO ĐỘNG THÁNG " + eb.Month.ToString() + "/NĂM " + eb.Year.ToString();
                else if (eb.EvaluationBoardType == 2)
                    merged10.Value = "VÀ NGƯỜI LAO ĐỘNG 06 THÁNG/NĂM " + eb.Year.ToString();
                else if (eb.EvaluationBoardType == 1)
                    merged10.Value = "VÀ NGƯỜI LAO ĐỘNG NĂM " + eb.Year.ToString();
            }
            else if (type == "2")
            {
                if (eb.EvaluationBoardType == 3)
                    merged10.Value = "THÁNG " + eb.Month.ToString() + "/NĂM " + eb.Year.ToString();
                else if (eb.EvaluationBoardType == 2)
                    merged10.Value = "06 THÁNG/NĂM " + eb.Year.ToString();
                else if (eb.EvaluationBoardType == 1)
                    merged10.Value = "NĂM " + eb.Year.ToString();
            }



            rowIndex++;
            WorksheetMergedCellsRegion merged11 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 6);
            if (type == "1")
                merged11.Value = "Đơn vị: " + dept.Name;

            rowIndex++;
            rowIndex++;
            WorksheetMergedCellsRegion merged12 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
            merged12.Value = "TT";
            SetBorder(merged12);

            WorksheetMergedCellsRegion merged13 = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 2, 1);
            merged13.Value = "Họ và tên";
            SetBorder(merged13);

            WorksheetMergedCellsRegion merged14 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 2, 2);
            if (type == "1")
                merged14.Value = "Chức vụ/vị trí công việc";
            else if (type == "2")
                merged14.Value = "Chức vụ/Đơn vị công tác";

            SetBorder(merged14);

            WorksheetMergedCellsRegion merged15 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex + 2, 3);
            if (eb.EvaluationBoardType == 3)
                merged15.Value = "Số điểm tự xếp loại";
            else if (eb.EvaluationBoardType == 2)
                merged15.Value = "Số điểm BQ của 6 tháng";
            else if (eb.EvaluationBoardType == 1)
                merged15.Value = "Số điểm BQ của năm";

            SetBorder(merged15);

            WorksheetMergedCellsRegion merged16 = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex, 6);
            if (type == "1")
                merged16.Value = "Trưởng đơn vị phân loại";
            else if (type == "2" || eb.EvaluationBoardType == 2 || eb.EvaluationBoardType == 1)
                merged16.Value = "Hiệu trưởng phân loại";

            SetBorder(merged16);

            WorksheetMergedCellsRegion merged17 = sheet.MergedCellsRegions.Add(rowIndex + 1, 4, rowIndex + 2, 4);
            merged17.Value = "Số điểm";
            SetBorder(merged17);

            WorksheetMergedCellsRegion merged18 = sheet.MergedCellsRegions.Add(rowIndex + 1, 5, rowIndex + 2, 5);
            merged18.Value = "Loại (A.B.C.D)";
            SetBorder(merged18);

            WorksheetMergedCellsRegion merged19 = sheet.MergedCellsRegions.Add(rowIndex + 1, 6, rowIndex + 2, 6);
            merged19.Value = "Ghi chú";
            SetBorder(merged19);
            rowIndex += 3;
            //Lấy STT
            int index = 1;
            foreach (ABC_EvaluationBoardStaffDTO r in detailResult)
            {
                sheet.Rows[rowIndex].Cells[0].Value = index;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = r.StaffName;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = r.PositionName;
                sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Left;
                CellBorder(sheet, rowIndex, 2);

                sheet.Rows[rowIndex].Cells[3].Value = r.StaffRecord;
                sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                CellBorder(sheet, rowIndex, 3);

                sheet.Rows[rowIndex].Cells[4].Value = r.Record;
                sheet.Rows[rowIndex].Cells[4].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                CellBorder(sheet, rowIndex, 4);

                //Nếu hội đồng đã sửa thì ưu tiên
                if (r.ClassificationThird != null && r.ClassificationThird != "")
                    sheet.Rows[rowIndex].Cells[5].Value = r.ClassificationThird;
                //Nếu hội đồng chưa sửa thì ưu tiên trưởng đv sửa
                else if (r.ClassificationSecond != null && r.ClassificationSecond != "")
                    sheet.Rows[rowIndex].Cells[5].Value = r.ClassificationSecond;
                else
                    sheet.Rows[rowIndex].Cells[5].Value = r.Classification;
                sheet.Rows[rowIndex].Cells[5].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                CellBorder(sheet, rowIndex, 5);

                if (r.NoteThird != null && r.NoteThird != "")
                    sheet.Rows[rowIndex].Cells[6].Value = r.NoteThird;
                else if (r.NoteSecond != null && r.NoteSecond != "")
                    sheet.Rows[rowIndex].Cells[6].Value = r.NoteSecond;
                else
                    sheet.Rows[rowIndex].Cells[6].Value = "";
                sheet.Rows[rowIndex].Cells[6].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                CellBorder(sheet, rowIndex, 6);

                rowIndex++;
                index++;
            }
            rowIndex += 2;
            WorksheetMergedCellsRegion merged20 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            //merged20.Value = "TP. Hồ Chí Minh, ngày "+now.Day +" tháng "+ now.Month +" năm " + now.Year;
            merged20.Value = "TP. Hồ Chí Minh, ngày .... tháng .... năm 20...";
            merged20.CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
            rowIndex++;

            if (eb.EvaluationBoardType == 3)
            {
                WorksheetMergedCellsRegion merged21 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
                merged21.Value = "Thường trực Hội đồng TĐKT";
                WorksheetMergedCellsRegion merged22 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
                if (type == "1")
                    merged22.Value = "Trưởng đơn vị";
                else if (type == "2")
                    merged22.Value = "HIỆU TRƯỞNG";

                rowIndex++;

                WorksheetMergedCellsRegion merged23 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
                merged23.Value = "Phòng Tổ chức cán bộ";
                WorksheetMergedCellsRegion merged24 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
                merged24.Value = "";
                rowIndex++;

                WorksheetMergedCellsRegion merged25 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
                if (type == "1")
                    merged25.Value = "Ký nhận, ngày .... tháng .... năm 20...";
                merged25.CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
                merged25.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

                WorksheetMergedCellsRegion merged26 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
                if (type == "1")
                    merged26.Value = "(Ký và ghi họ và tên)";
                merged25.CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
                merged25.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                rowIndex++;
            }
            else if (eb.EvaluationBoardType == 2 || eb.EvaluationBoardType == 1)
            {
                rowIndex++;
                WorksheetMergedCellsRegion merged21 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
                merged21.Value = "Trưởng đơn vị";

                WorksheetMergedCellsRegion merged22 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 4);
                merged22.Value = "Trưởng phòng P.TCCB";

                WorksheetMergedCellsRegion merged23 = sheet.MergedCellsRegions.Add(rowIndex,5, rowIndex, 6);
                merged23.Value = "Hiệu trưởng";
            }



                string filename = "/Temp/Excel/1.xls";
            rowIndex++;
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
        protected void SetBorder(WorksheetMergedCellsRegion region)
        {
            region.CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
            region.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
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