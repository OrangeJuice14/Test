using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using Infragistics.Documents.Excel;
using System.Data;
using Infragistics.Documents.Excel.PredefinedShapes;
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
    public class InGiayNghiPhep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string oidlist = context.Request.Params["oidlist"] == null
                            ? ""
                            : Convert.ToString(context.Request.Params["oidlist"]);
            Guid nhanvien = context.Request.Params["nhanvien"] == null
                            ? Guid.Empty
                            : new Guid(context.Request.Params["nhanvien"].ToString());
            Boolean type = context.Request.Params["type"] == null
                            ? false
                            : Convert.ToBoolean(context.Request.Params["type"].ToString());
            //
            GiayNghiPhepExportToExcelProcess(oidlist, type, nhanvien, context);
        }

        public void GiayNghiPhepExportToExcelProcess(string oidList, Boolean type, Guid nhanvien, HttpContext context)
        {
            string idCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
            IEnumerable<DTO_GiayDeNghiPhep> result = null;
            //
            CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
            DTO_NamHoc nh = (new NamHoc_Factory()).GetListByNam(DateTime.Now.Year);
            if (nh == null) return;
            //
            result = factory.Context.spd_WebChamCong_GiayDeNghiNghiPhep(oidList, nh.Oid, nhanvien, type).Map<DTO_GiayDeNghiPhep>();
            if (result == null) return;
            //
            string hoTen = string.Empty;
            string donVi = string.Empty;
            string chucDanh = string.Empty;
            string phongBan = string.Empty;
            string maNhanVien = string.Empty;
            decimal tongSoNgayPhepNam = 0;
            //
            foreach (var item in result)
            {
                hoTen = item.HoTen;
                phongBan = item.PhongBan;
                maNhanVien = item.MaNhanVien;
                phongBan = item.PhongBan;
                donVi = item.DonVi;
                tongSoNgayPhepNam = item.SoNgayPhepNam;
                break;
            }
            //
            DateTime now = DateTime.Now;


            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", maNhanVien, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("GiayNghiPhep");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["GiayNghiPhep"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 1 cm
            sheet.PrintOptions.LeftMargin = 0.385;
            sheet.PrintOptions.RightMargin = 0.385;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;

            //Set độ rộng cột
            sheet.Columns[0].Width = 2500;
            sheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[1].Width = 1500;
            sheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[2].Width = 2500;
            sheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[3].Width = 2500;
            sheet.Columns[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            for (int t = 4; t <= 10; t++)
            {
                sheet.Columns[t].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Columns[t].Width = 2000;
            }          
            sheet.Columns[11].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Columns[11].Width = 5000;
            sheet.Columns[12].Width = 1600;
            sheet.Columns[12].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[13].Width = 2000;
            sheet.Columns[13].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Columns[14].Width = 4000;
            sheet.Columns[14].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Columns[15].Width = 2000;
            sheet.Columns[15].CellFormat.Alignment = HorizontalCellAlignment.Left;
            //
            int rowIndex = 0;

            //Tên trường
            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 2);
            merged1.Value = "";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 240;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            //Tiêu đề bảng công
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 12);
            merged2.Value = "ĐỀ NGHỊ NGHỈ PHÉP";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 400;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            //Phụ lục
            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex, 16);
            merged3.Value = "Mã số: NSĐT/QT-07/M01";
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 240;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            // Tăng số dòng lên
            rowIndex++;

            //Tiêu đề bảng công
            WorksheetMergedCellsRegion merged2_1 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 12);
            merged2_1.Value = "NĂM " + nh.TenNamHoc.ToUpper();
            merged2_1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2_1.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged2_1.CellFormat.Font.Name = "Times New Roman";
            merged2_1.CellFormat.Font.Height = 400;
            merged2_1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            //Phụ lục
            WorksheetMergedCellsRegion merged3_1 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex, 16);
            merged3_1.Value = "Hiệu lực: 14/12/2016";
            merged3_1.CellFormat.VerticalAlignment = VerticalCellAlignment.Top;
            merged3_1.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged3_1.CellFormat.Font.Name = "Times New Roman";
            merged3_1.CellFormat.Font.Height = 240;
            merged3_1.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            // Tăng số dòng lên
            rowIndex++;
            rowIndex++;

            //Họ tên
            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 5);
            merged4.Value = "Họ tên: " + hoTen;
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged4.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 240;
            merged4.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            //Chức danh
            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex, 12);
            merged5.Value = "Chức danh: " + chucDanh;
            merged5.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged5.CellFormat.Font.Name = "Times New Roman";
            merged5.CellFormat.Font.Height = 240;
            merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            //Mã nhân viên
            WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex, 16);
            merged6.Value = "Mã số CBGVNV: " + maNhanVien;
            merged6.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged6.CellFormat.Font.Name = "Times New Roman";
            merged6.CellFormat.Font.Height = 240;
            merged6.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            // Tăng số dòng lên
            rowIndex++;

            //Đơn vị
            WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 5);
            merged7.Value = "Đơn vị: " + donVi;
            merged7.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged7.CellFormat.Font.Name = "Times New Roman";
            merged7.CellFormat.Font.Height = 240;
            merged7.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            //Phòng ban
            WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex, 12);
            merged8.Value = "Phòng ban: " + phongBan;
            merged8.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged8.CellFormat.Font.Name = "Times New Roman";
            merged8.CellFormat.Font.Height = 240;
            merged8.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

            //Số ngày phép năm
            WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex, 16);
            merged9.Value = "Số ngày phép năm được có: " + tongSoNgayPhepNam;
            merged9.CellFormat.Alignment = HorizontalCellAlignment.Left;
            merged9.CellFormat.Font.Name = "Times New Roman";
            merged9.CellFormat.Font.Height = 240;
            merged9.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            // Tăng số dòng lên
            rowIndex++;
            rowIndex++;

            #region TargetGroup

            //
            BackgroundColor(sheet, rowIndex, 0, 16, Color.LightGray);
            BackgroundColor(sheet, rowIndex + 1, 0, 16, Color.LightGray);
            BackgroundColor(sheet, rowIndex + 2, 0, 16, Color.LightGray);
            BackgroundColor(sheet, rowIndex + 3, 0, 16, Color.LightGray);

            WorksheetMergedCellsRegion mergedNgayThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 3, 0);
            mergedNgayThucHien.Value = "Ngày thực hiện";
            mergedNgayThucHien.CellFormat.Font.Height = 240;
            SetRegionBorder(mergedNgayThucHien, true, false, true, true);
            mergedNgayThucHien.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedSoNgay = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 3, 1);
            mergedSoNgay.Value = "Số ngày";
            mergedSoNgay.CellFormat.Font.Height = 240;
            SetRegionBorder(mergedSoNgay, true, false, true, true);
            mergedSoNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedThoiGianNghi = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 3);
            mergedThoiGianNghi.Value = "Thời gian nghỉ";
            SetRegionBorder(mergedThoiGianNghi, true, false, true, true);
            mergedThoiGianNghi.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTuNgay = sheet.MergedCellsRegions.Add(rowIndex + 2, 2, rowIndex + 3, 2);
            mergedTuNgay.Value = "Từ ngày";
            SetRegionBorder(mergedTuNgay, true, false, true, false);
            mergedTuNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedDenNgay = sheet.MergedCellsRegions.Add(rowIndex + 2, 3, rowIndex + 3, 3);
            mergedDenNgay.Value = "Đến ngày";
            SetRegionBorder(mergedDenNgay, true, false, true, false);
            mergedDenNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiPhepNam = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex + 3, 4);
            mergedNghiPhepNam.Value = "Nghỉ phép năm";
            SetRegionBorder(mergedNghiPhepNam, true, false, true, true);
            mergedNghiPhepNam.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNgayHuongCheDoChinhSach = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex + 1, 8);
            mergedNgayHuongCheDoChinhSach.Value = "Nghỉ hưởng chế độ BHXH ";
            mergedNgayHuongCheDoChinhSach.Value += " / Chính sách theo quy định";
            SetRegionBorder(mergedNgayHuongCheDoChinhSach, true, false, true, true);
            mergedNgayHuongCheDoChinhSach.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedNgayHuongCheDoChinhSach.CellFormat.WrapText = ExcelDefaultableBoolean.True;

            WorksheetMergedCellsRegion mergedNghiCheDo = sheet.MergedCellsRegions.Add(rowIndex + 2, 5, rowIndex + 3, 5);
            mergedNghiCheDo.Value = "Nghỉ chế độ";
            SetRegionBorder(mergedNghiCheDo, true, false, true, false);
            mergedNghiCheDo.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiThaiSan = sheet.MergedCellsRegions.Add(rowIndex + 2, 6, rowIndex + 3, 6);
            mergedNghiThaiSan.Value = "Nghỉ thai sản";
            SetRegionBorder(mergedNghiThaiSan, true, false, true, false);
            mergedNghiThaiSan.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiVoSinh = sheet.MergedCellsRegions.Add(rowIndex + 2, 7, rowIndex + 3, 7);
            mergedNghiVoSinh.Value = "Nghỉ vợ sinh";
            SetRegionBorder(mergedNghiVoSinh, true, false, true, false);
            mergedNghiVoSinh.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiOmDau = sheet.MergedCellsRegions.Add(rowIndex + 2, 8, rowIndex + 3, 8);
            mergedNghiOmDau.Value = "Nghỉ ốm đau";
            SetRegionBorder(mergedNghiOmDau, true, false, true, false);
            mergedNghiOmDau.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiKhongLuong = sheet.MergedCellsRegions.Add(rowIndex, 9, rowIndex + 3, 9);
            mergedNghiKhongLuong.Value = "Nghỉ không lương";
            SetRegionBorder(mergedNghiKhongLuong, true, false, true, true);
            mergedNghiKhongLuong.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNghiBu = sheet.MergedCellsRegions.Add(rowIndex, 10, rowIndex + 3, 10);
            mergedNghiBu.Value = "Nghỉ bù";
            SetRegionBorder(mergedNghiBu, true, false, true, true);
            mergedNghiBu.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedLyDoKhac = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex + 3, 11);
            mergedLyDoKhac.Value = "Lý do khác";
            SetRegionBorder(mergedLyDoKhac, true, false, true, true);
            mergedLyDoKhac.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedSoNgayPhepConLai = sheet.MergedCellsRegions.Add(rowIndex, 12, rowIndex + 3, 12);
            mergedSoNgayPhepConLai.Value = "Ngày phép còn lại";
            SetRegionBorder(mergedSoNgayPhepConLai, true, false, true, true);
            mergedSoNgayPhepConLai.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedChuKy = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex + 3, 13);
            mergedChuKy.Value = "Chữ ký của CBGVNV";
            SetRegionBorder(mergedChuKy, true, false, true, true);
            mergedChuKy.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedPheDuyet = sheet.MergedCellsRegions.Add(rowIndex, 14, rowIndex + 1, 16);
            mergedPheDuyet.Value = "Phê duyệt của Trưởng Đơn vị/ Ban Giám hiệu";
            SetRegionBorder(mergedPheDuyet, true, false, true, true);
            mergedPheDuyet.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTruongDonVi = sheet.MergedCellsRegions.Add(rowIndex + 2, 14, rowIndex + 3, 14);
            mergedTruongDonVi.Value = "Trưởng Đơn vị";
            SetRegionBorder(mergedTruongDonVi, true, false, true, false);
            mergedTruongDonVi.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedBGH = sheet.MergedCellsRegions.Add(rowIndex + 2, 15, rowIndex + 3, 16);
            mergedBGH.Value = "Hiệu trưởng/Phó Hiệu trưởng";
            SetRegionBorder(mergedBGH, true, false, true, false);
            mergedBGH.CellFormat.Alignment = HorizontalCellAlignment.Center;

            // Tăng số dòng lên
            rowIndex++;
            rowIndex++;
            rowIndex++;
            rowIndex++;

            //Lấy STT
            foreach (DTO_GiayDeNghiPhep ccThang in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = ccThang.NgayThuchien.ToString("dd/MM/yyyy");
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = ccThang.SoNgay;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = ccThang.TuNgay;
                CellBorder(sheet, rowIndex, 2);

                sheet.Rows[rowIndex].Cells[3].Value = ccThang.DenNgay;
                CellBorder(sheet, rowIndex, 3);

                sheet.Rows[rowIndex].Cells[4].Value = ccThang.NghiPhepNam;
                CellBorder(sheet, rowIndex, 4);

                sheet.Rows[rowIndex].Cells[5].Value = ccThang.NghiCheDo;
                CellBorder(sheet, rowIndex, 5);

                sheet.Rows[rowIndex].Cells[6].Value = ccThang.NghiThaiSan;
                CellBorder(sheet, rowIndex, 6);

                sheet.Rows[rowIndex].Cells[7].Value = ccThang.NghiVoSinh;
                CellBorder(sheet, rowIndex, 7);

                sheet.Rows[rowIndex].Cells[8].Value = ccThang.OmDau;
                CellBorder(sheet, rowIndex, 8);

                sheet.Rows[rowIndex].Cells[9].Value = ccThang.NghiKhongLuong;
                CellBorder(sheet, rowIndex, 9);

                sheet.Rows[rowIndex].Cells[10].Value = ccThang.NghiBu;
                CellBorder(sheet, rowIndex, 10);

                sheet.Rows[rowIndex].Cells[11].Value = ccThang.LyDoKhac;
                CellBorder(sheet, rowIndex, 11);

                sheet.Rows[rowIndex].Cells[12].Value = ccThang.SoNgayPhepConLai;
                CellBorder(sheet, rowIndex, 12);

                sheet.Rows[rowIndex].Cells[13].Value = string.Empty;
                CellBorder(sheet, rowIndex, 13);

                sheet.Rows[rowIndex].Cells[14].Value = string.Empty;
                CellBorder(sheet, rowIndex, 14);

                //
                sheet.MergedCellsRegions.Add(rowIndex,15, rowIndex, 16);
                CellBorder(sheet, rowIndex, 15);
                CellBorder(sheet, rowIndex, 16);
                //
                rowIndex++;
            }
            //
            rowIndex++;
            //
            sheet.Rows[rowIndex].Cells[0].Value = "Ghi chú: nếu nghỉ 0.5 ngày thì ghi rõ nghỉ buổi sáng hoặc chiều vào cột 'Thời gian nghỉ'.";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;

            #endregion

            string filename = "/Temp/Excel/1.xls";
            BIFF8Writer.WriteWorkbookToFile(workbook, context.Server.MapPath(filename));
            //
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

        public bool ExportWorkbookToPdf(string workbookPath, string outputPath)
        {
            // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }

            // Create COM Objects
            Microsoft.Office.Interop.Excel.Application excelApplication;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            // Create new instance of Excel
            excelApplication = new Microsoft.Office.Interop.Excel.Application();

            // Make the process invisible to the user
            excelApplication.ScreenUpdating = false;

            // Make the process silent
            excelApplication.DisplayAlerts = false;

            // Open the workbook that you wish to export to PDF
            excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;

                return false;
            }

            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (System.Exception ex)
            {
                // Mark the export as failed for the return value...
                exportSuccessful = false;

                // Do something with any exceptions here, if you wish...
                // MessageBox.Show...        
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;
            }

            // You can use the following method to automatically open the PDF after export if you wish
            // Make sure that the file actually exists first...
            if (System.IO.File.Exists(outputPath))
            {
                System.Diagnostics.Process.Start(outputPath);
            }

            return exportSuccessful;
        }
    }
}