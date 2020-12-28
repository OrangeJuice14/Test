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
    public class InBangCongAll : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int thang = context.Request.Params["thang"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["thang"].ToString());
            int nam = context.Request.Params["nam"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["nam"].ToString());
            Guid userId = context.Request.Params["userId"] != null ? new Guid(context.Request.Params["userId"].ToString()) : Guid.Empty;
            Guid congTyId = context.Request.Params["congTy"] != null ? new Guid(context.Request.Params["congTy"].ToString()) : Guid.Empty;
            //
            WebUser_Factory factory = new WebUser_Factory();
            WebUser currentUser = factory.GetByID(userId);
            if (currentUser != null)
            {
                BangChamCongExportToExcelProcess(thang, nam, userId, currentUser.WebGroupID.Value, congTyId, context);
            }
        }

        public void BangChamCongExportToExcelProcess(int thang, int nam, Guid userId, Guid groupId, Guid congTy, HttpContext context)
        {
            try
            {
                IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> resultList = null;
                CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();

                //Lấy danh sách đơn vị phòng bàn được phân quyền
                List<BoPhan> donViList = (new BoPhan_Factory()).GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(userId, congTy).Where(q => q.CongTy == congTy).ToList();

                List<DTO_NgayChamCong> listNgay = factory.GetList_NgayTrongKyChamCong(thang, nam, Guid.Empty, groupId, congTy).ToList();
                int songay = listNgay.Count();
                CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam, congTy);
                if (kyChamCong == null) return;
                //
                DateTime now = DateTime.Now;

                string checkStatus = context.Request.Params["type"];
                string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", donViList[0].CongTy1.BoPhan.MaQuanLy, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
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
                //Set độ rộng cột
                sheet.Columns[0].Width = 1500;
                sheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Columns[1].Width = 5500;
                sheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Columns[2].Width = 2000;
                sheet.Columns[3].CellFormat.Alignment = HorizontalCellAlignment.Left;
                sheet.Columns[3].Width = 7000;
                for (int i = 4; i <= songay + 4; i++)
                {
                    sheet.Columns[i].Width = 900;
                    sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
                }
                for (int i = songay + 4; i <= songay + 8; i++)
                {
                    sheet.Columns[i].Width = 2000;
                    sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
                }
                //
                int rowIndex = 0;

                //Tên trường
                WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 3);
                merged1.Value = donViList[0].CongTy1.BoPhan.TenBoPhan.ToUpper();
                merged1.CellFormat.Alignment = HorizontalCellAlignment.Left;
                merged1.CellFormat.Font.Name = "Times New Roman";
                merged1.CellFormat.Font.Height = 240;
                merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                //Tiêu đề bảng công
                WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex, 36);
                merged2.Value = "BẢNG CHẤM CÔNG";
                //Khoảng trắng để wrap text                                                                                                           
                merged2.Value += " THÁNG " + thang + "/" + nam;
                merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
                merged2.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                merged2.CellFormat.Font.Name = "Times New Roman";
                merged2.CellFormat.Font.Height = 300;
                merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                merged2.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                //Phụ lục
                WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 37, rowIndex, 41);
                merged3.Value = "Số: NSĐT/QT-07/M03";
                merged3.CellFormat.Alignment = HorizontalCellAlignment.Left;
                merged3.CellFormat.Font.Name = "Times New Roman";
                merged3.CellFormat.Font.Height = 240;
                merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

                // Tăng số dòng lên
                rowIndex++;

                //Tháng chấm công
                WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex, 36);
                merged4.Value = "Tháng " + String.Format("{0}", kyChamCong.Thang) + " năm " + String.Format("{0}", kyChamCong.Nam) + "";
                merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
                merged4.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                merged4.CellFormat.Font.Name = "Times New Roman";
                merged4.CellFormat.Font.Height = 300;
                merged4.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                //Ngày lập
                WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 37, rowIndex, 41);
                merged5.Value = "Ngày: " + String.Format("{0:dd/MM/yyyy}", now.Date);
                merged5.CellFormat.Alignment = HorizontalCellAlignment.Left;
                merged5.CellFormat.Font.Name = "Times New Roman";
                merged5.CellFormat.Font.Height = 240;
                merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.False;

                // Tăng số dòng lên
                rowIndex++;
                rowIndex++;

                #region TargetGroup
                //Ngày trong tháng
                BackgroundColor(sheet, rowIndex, 0, songay + 8, Color.LightGray);
                BackgroundColor(sheet, rowIndex + 1, 2, songay + 8, Color.LightGray);
                BackgroundColor(sheet, rowIndex + 2, 0, songay + 8, Color.LightGray);

                WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
                mergedStt.Value = "STT";
                SetRegionBorder(mergedStt, true, false, true, true);
                mergedStt.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedMaNhanVien = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 2, 1);
                mergedMaNhanVien.Value = "MÃ NV";
                SetRegionBorder(mergedMaNhanVien, true, false, true, true);
                mergedMaNhanVien.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedMocNgay = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 2, 2);
                mergedMocNgay.Value = "MỐC NGÀY";
                SetRegionBorder(mergedMocNgay, true, false, true, true);
                mergedMocNgay.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex + 2, 3);
                mergedHoTen.Value = "HỌ TÊN";
                SetRegionBorder(mergedHoTen, true, false, true, true);
                mergedHoTen.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedNgayTrongThang = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex, songay + 3);
                mergedNgayTrongThang.Value = "SỐ NGÀY TRONG THÁNG";
                SetRegionBorder(mergedNgayTrongThang, true, false, true, true);
                mergedNgayTrongThang.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedSoNgayCong = sheet.MergedCellsRegions.Add(rowIndex, songay + 4, rowIndex, songay + 8);
                mergedSoNgayCong.Value = "SỐ NGÀY CÔNG";
                SetRegionBorder(mergedSoNgayCong, true, false, true, true);
                mergedSoNgayCong.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedNgayHuongLuong = sheet.MergedCellsRegions.Add(rowIndex + 1, songay + 4, rowIndex + 2, songay + 4);
                mergedNgayHuongLuong.Value = "Hưởng lương công ty";
                SetRegionBorder(mergedNgayHuongLuong, true, false, true, true);
                mergedNgayHuongLuong.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedHuongBHXH = sheet.MergedCellsRegions.Add(rowIndex + 1, songay + 5, rowIndex + 2, songay + 5);
                mergedHuongBHXH.Value = "Hưởng trợ cấp BHXH";
                SetRegionBorder(mergedHuongBHXH, true, false, true, true);
                mergedHuongBHXH.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedNghiPhepNam = sheet.MergedCellsRegions.Add(rowIndex + 1, songay + 6, rowIndex + 2, songay + 6);
                mergedNghiPhepNam.Value = "Nghỉ phép năm";
                SetRegionBorder(mergedNghiPhepNam, true, false, true, true);
                mergedNghiPhepNam.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedKhongLuong = sheet.MergedCellsRegions.Add(rowIndex + 1, songay + 7, rowIndex + 2, songay + 7);
                mergedKhongLuong.Value = "Không hưởng lương";
                SetRegionBorder(mergedKhongLuong, true, false, true, true);
                mergedKhongLuong.CellFormat.Alignment = HorizontalCellAlignment.Center;

                WorksheetMergedCellsRegion mergedTongCong = sheet.MergedCellsRegions.Add(rowIndex + 1, songay + 8, rowIndex + 2, songay + 8);
                mergedTongCong.Value = "Tổng cộng";
                SetRegionBorder(mergedTongCong, true, false, true, true);
                mergedTongCong.CellFormat.Alignment = HorizontalCellAlignment.Center;

                int index = 4;
                rowIndex++;
                foreach (DTO_NgayChamCong date in listNgay)
                {
                    sheet.Rows[rowIndex].Cells[index].Value = date.Ngay;
                    sheet.Rows[rowIndex + 1].Cells[index].Value = date.Thu;
                    index++;
                }
                SetCellFormat(sheet, rowIndex, 2, 4 + songay, true, false, false, true, false);
                SetCellFormat(sheet, rowIndex + 1, 2, 4 + songay, true, false, false, false, false);
                rowIndex++;
                rowIndex++;

                //Duyệt qua tất cả phòng ban
                int sttPB = 1;
                foreach (var donVi in donViList)
                {
                    sheet.Rows[rowIndex].Cells[0].Value = sttPB.ToString() + ". " + donVi.TenBoPhan;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 41);
                    CellBorder(sheet, rowIndex, 0);

                    //Tăng lên 1 dòng
                    rowIndex++;

                    //Lấy danh sách cá nhân theo đơn vị
                    resultList = factory.QuanLyChamCong_ThongTinChamCongThangAll(thang, nam, donVi.Oid, string.Empty, Guid.Empty, userId, congTy);

                    //
                    index = 1;
                    foreach (DTO_QuanLyChamCong_ThongTinChamCongThang ccThang in resultList)
                    {
                        sheet.Rows[rowIndex].Cells[0].Value = index;
                        sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, 0);

                        sheet.Rows[rowIndex].Cells[1].Value = ccThang.MaTapDoan;
                        CellBorder(sheet, rowIndex, 1);

                        sheet.Rows[rowIndex].Cells[2].Value = kyChamCong.SoNgay;
                        CellBorder(sheet, rowIndex, 2);

                        sheet.Rows[rowIndex].Cells[3].Value = ccThang.HoTen;
                        CellBorder(sheet, rowIndex, 3);

                        int idx = 4;
                        foreach (DTO_QuanLyChamCong_ChamCongNgay cc in ccThang.ChiTietChamCong)
                        {
                            sheet.Rows[rowIndex].Cells[idx].Value = cc.MaHinhThucNghi;
                            CellBorder(sheet, rowIndex, idx);
                            idx++;
                        }
                        sheet.Rows[rowIndex].Cells[idx].Value = ccThang.NgayHuongLuong;
                        sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Left;
                        CellBorder(sheet, rowIndex, idx);

                        sheet.Rows[rowIndex].Cells[idx + 1].Value = ccThang.NgayHuongBHXH;
                        sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, idx + 1);

                        sheet.Rows[rowIndex].Cells[idx + 2].Value = ccThang.NgayPhep;
                        sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, idx + 2);

                        sheet.Rows[rowIndex].Cells[idx + 3].Value = ccThang.NgayKhongLuong;
                        sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, idx + 3);

                        sheet.Rows[rowIndex].Cells[idx + 4].Value = ccThang.TongCong;
                        sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, idx + 4);
                        //
                        rowIndex++;
                        index++;
                    }

                    //
                    sttPB += 1;
                }
                //
                rowIndex++;

                //
                WorksheetMergedCellsRegion mergedNguoiKy = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 18);
                mergedNguoiKy.Value = "NGƯỜI LẬP BẢNG";
                mergedNguoiKy.CellFormat.Alignment = HorizontalCellAlignment.Center;
                mergedNguoiKy.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                mergedNguoiKy.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                mergedNguoiKy.CellFormat.Font.Name = "Times New Roman";
                mergedNguoiKy.CellFormat.Font.Height = 300;
                mergedNguoiKy.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                //
                WorksheetMergedCellsRegion mergedLanhDao = sheet.MergedCellsRegions.Add(rowIndex, 19, rowIndex, 41);
                mergedLanhDao.Value = "TRƯỞNG ĐƠN VỊ";
                mergedLanhDao.CellFormat.Alignment = HorizontalCellAlignment.Center;
                mergedLanhDao.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                mergedLanhDao.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                mergedLanhDao.CellFormat.Font.Name = "Times New Roman";
                mergedLanhDao.CellFormat.Font.Height = 300;
                mergedLanhDao.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                //
                rowIndex++;
                rowIndex++;
                rowIndex++;
                rowIndex++;
                //
                /*
                DateTime cur = DateTime.Now;
                sheet.Rows[rowIndex].Cells[30].Value = "Ngày " + cur.Day.ToString() + " tháng " + cur.Month.ToString() + " năm " + cur.Year.ToString();
                sheet.Rows[rowIndex].Cells[30].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[30].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                sheet.Rows[rowIndex].Cells[30].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;*/
                //
                rowIndex++;
                rowIndex++;
                //
                sheet.Rows[rowIndex].Cells[0].Value = "KÝ HIỆU CHẤM CÔNG: ";
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                //
                rowIndex++;
                //
                CC_HinhThucNghi_Factory factoryHTN = new CC_HinhThucNghi_Factory();
                List<DTO_HinhThucNghi> listHinhThucNghi = factoryHTN.GetAll_GCRecordIsNull().Map<DTO_HinhThucNghi>().ToList();
                int a = Convert.ToInt32(listHinhThucNghi.Count() / 2); /// Lưu ý
                int b = 1;
                int rowIndex2 = rowIndex;
                int rowIndex3 = rowIndex - 1;
                foreach (DTO_HinhThucNghi htn in listHinhThucNghi)
                {
                    if (b <= a)
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
                //
                BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
                context.Response.Clear();
                context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
                context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
                reader.Close();
                context.Response.Flush();
            }
            catch (Exception ex)
            {
                Helpers.Helper.ErrorLog("InBangCongAll/BangChamCongExportToExcelProcess", ex);
                throw;
            }

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