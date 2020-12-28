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
    public class InBangVanTay : IHttpHandler
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
            BangVanTayVaoRaExportToExcelProcess(thang, nam, bophanId, idNhanVien, context);
        }

        public void BangVanTayVaoRaExportToExcelProcess(int thang, int nam, Guid bophanId, Guid idNhanVien, HttpContext context)
        {
            //if (bophanId == Guid.Empty && idNhanVien != Guid.Empty)
            //{
            //    ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            //    bophanId = Guid.Parse(applicationUser.DepartmentId);
            //}

            IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> result = null;

            CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
            result = factory.QuanLyChamCong_VanTayVaoRa(thang, nam, bophanId, idNhanVien);
            BoPhan_Factory bpfac = new BoPhan_Factory();
            BoPhan DonVi = bpfac.GetByID(bophanId);

            List<DTO_NgayChamCong> listNgay = factory.GetList_NgayTrongKyChamCong(thang, nam).ToList();
            int songay = listNgay.Count();


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
            sheet.PrintOptions.PaperSize = PaperSize.A3;

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
            sheet.Columns[1].Width = 1500;
            sheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Columns[2].Width = 7000;
            //ngày trong tháng
            for (int i=3;i<= songay + 2;i++)
            {
                sheet.Columns[i].Width = 1500;
                sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            }
            //phần tổng hợp
            for (int i = songay+3; i <= songay + 6; i++)
            {
                sheet.Columns[i].Width = 1500;
                sheet.Columns[i].CellFormat.Alignment = HorizontalCellAlignment.Center;
            }
            int rowIndex = 0;

            //WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            //merged1.Value = "BỘ GIÁO DỤC VÀ ĐÀO TẠO";
            //merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            //merged1.CellFormat.Font.Name = "Times New Roman";
            //merged1.CellFormat.Font.Height = 240;
            //merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //rowIndex++;
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 7);
            merged2.Value = "TRƯỜNG ĐẠI HỌC KINH TẾ - LUẬT";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 240;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 7);
            //merged4.Value = "Phòng Thanh Tra";
            merged4.Value = DonVi.TenBoPhan.ToUpper();
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged4.CellFormat.Font.Name = "Times New Roman";
            merged4.CellFormat.Font.Height = 240;
            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(0, 8, 2, songay + 6);
            merged3.Value = "BÁO CÁO TÌNH HÌNH THỰC HIỆN CHẤM VÂN TAY VÀO/RA NƠI LÀM VIỆC";
            //Khoảng  trắng để wrap text                                                                                                           
            merged3.Value += "                                                                                                              THÁNG " + thang + "/" + nam;
            //merged3.Value = "BẢNG CHẤM CÔNG THÁNG " + thang + "/" + nam;
            merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged3.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            merged3.CellFormat.Font.Name = "Times New Roman";
            merged3.CellFormat.Font.Height = 300;
            merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged3.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            rowIndex++;

            //WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, songay+6);
            //merged5.Value = "ĐƠN VỊ: " +DonVi.TenBoPhan.ToUpper();
            //merged5.CellFormat.Alignment = HorizontalCellAlignment.Center;
            //merged5.CellFormat.Font.Name = "Times New Roman";
            //merged5.CellFormat.Font.Height = 260;
            //merged5.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //rowIndex++;
            rowIndex++;

            #region TargetGroup
            //BackgroundColor(sheet, rowIndex, 0, songay + 6, Color.LightGray);
            //BackgroundColor(sheet, rowIndex + 2, 0, songay + 6, Color.LightGray);

            WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
            mergedStt.Value = "STT";
            SetRegionBorder(mergedStt, true, false, true, true);
            mergedStt.CellFormat.Alignment= HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedMaSo = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 2, 1);
            mergedMaSo.Value = "Mã số";
            SetRegionBorder(mergedMaSo, true, false, true, true);
            mergedMaSo.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedHoTen = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 2,2);
            mergedHoTen.Value = "Họ tên";
            SetRegionBorder(mergedHoTen, true, false, true, true);
            mergedHoTen.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedNgayTrongThang = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex+1,songay+2);
            mergedNgayTrongThang.Value = "CHI TIẾT";
            SetRegionBorder(mergedNgayTrongThang, true, false, true, true);
            mergedNgayTrongThang.CellFormat.Alignment = HorizontalCellAlignment.Center;

            WorksheetMergedCellsRegion mergedTongHop = sheet.MergedCellsRegions.Add(rowIndex, songay+3, rowIndex + 1, songay + 6);
            mergedTongHop.Value = "TỔNG HỢP";
            SetRegionBorder(mergedTongHop, true, false, true, true);
            mergedTongHop.CellFormat.Alignment = HorizontalCellAlignment.Center;

            sheet.Rows[rowIndex + 2].Cells[songay + 3].Value = "M";
            sheet.Rows[rowIndex + 2].Cells[songay + 3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 2, songay + 3);

            sheet.Rows[rowIndex + 2].Cells[songay + 4].Value = "S";
            sheet.Rows[rowIndex + 2].Cells[songay + 4].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 2, songay + 4);

            sheet.Rows[rowIndex + 2].Cells[songay + 5].Value = "T";
            sheet.Rows[rowIndex + 2].Cells[songay + 5].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 2, songay + 5);

            sheet.Rows[rowIndex + 2].Cells[songay + 6].Value = "K";
            sheet.Rows[rowIndex + 2].Cells[songay + 6].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 2, songay + 6);

            int index = 3;
            rowIndex += 2;
            foreach (DTO_NgayChamCong date in listNgay)
            {
                sheet.Rows[rowIndex].Cells[index].Value = date.Ngay;
                //if (date.T7CN)
                //    BackgroundColor(sheet, rowIndex, index, index, Color.LightGray);
                index++;
            }
            SetCellFormat(sheet, rowIndex, 3, songay + 2, true, false, false, true, true);
            //SetCellFormat(sheet, rowIndex+1, 2, 1 + songay, true, false, false,false, true);
            rowIndex++;

            //tính tổng cộng
            int sumM = 0;
            int sumS = 0;
            int sumT = 0;
            int sumK = 0;

            //Lấy STT
            index = 1;
            foreach (DTO_QuanLyChamCong_ThongTinChamCongThang ccThang in result)
            {
                sheet.Rows[rowIndex].Cells[0].Value = index;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = ccThang.MaNhanSu;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = ccThang.HoTen;
                sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 2);

                int idx = 3;
                SetCellFormat(sheet, rowIndex, idx, songay + 6, false, false, false, true, true);

                //tính tổng hợp
                int totalM = 0;
                int totalS = 0;
                int totalT = 0;
                int totalK = 0;
                foreach (DTO_NgayChamCong date in listNgay)
                {
                    //sheet.Rows[rowIndex].Cells[idx].Value = XuLyChuoi(cc.MaHinhThucNghi);
                    sheet.Rows[rowIndex].Cells[idx].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    //CellBorder(sheet, rowIndex, idx);
                    foreach (DTO_QuanLyChamCong_ChamCongNgay cc in ccThang.ChiTietChamCong)
                    {
                        if (date.Ngay == cc.Ngay.Day)
                        {
                            sheet.Rows[rowIndex].Cells[idx].Value = XuLyChuoi(cc.MaHinhThucNghi);
                            sheet.Rows[rowIndex].Cells[idx].CellFormat.Font.Height = 180;
                            if (date.T7CN)
                                BackgroundColor(sheet, rowIndex, idx, idx, Color.LightGray);

                            if (cc.MaHinhThucNghi == "K")
                            {
                                totalK++;
                            }
                            if (cc.MaHinhThucNghi.Contains("KV") || cc.MaHinhThucNghi.Contains("KR"))
                            {
                                totalT++;
                            }
                            if (cc.MaHinhThucNghi.Contains("M"))
                            {
                                totalM += int.Parse(GetThoiGianVaoMuon(cc.MaHinhThucNghi));
                            }
                            if (cc.MaHinhThucNghi.Contains("S"))
                            {
                                totalS += int.Parse(GetThoiGianRaSom(cc.MaHinhThucNghi));
                            }
                        }
                    }
                    idx++;
                }
                sheet.Rows[rowIndex].Cells[songay + 3].Value = totalM;
                CellBorder(sheet, rowIndex, songay + 3);

                sheet.Rows[rowIndex].Cells[songay + 4].Value = totalS;
                CellBorder(sheet, rowIndex, songay + 4);

                sheet.Rows[rowIndex].Cells[songay + 5].Value = totalT;
                CellBorder(sheet, rowIndex, songay + 5);

                sheet.Rows[rowIndex].Cells[songay + 6].Value = totalK;
                CellBorder(sheet, rowIndex, songay + 6);

                sumM += totalM;
                sumS += totalS;
                sumT += totalT;
                sumK += totalK;
                rowIndex++;
                index++;
            }
            WorksheetMergedCellsRegion mergedTongCong = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, songay + 2);
            mergedTongCong.Value = "Tổng cộng";
            SetRegionBorder(mergedTongCong, false, false, true, true);
            mergedTongCong.CellFormat.Alignment = HorizontalCellAlignment.Right;

            sheet.Rows[rowIndex].Cells[songay + 3].Value = sumM;
            sheet.Rows[rowIndex].Cells[songay + 4].Value = sumS;
            sheet.Rows[rowIndex].Cells[songay + 5].Value = sumT;
            sheet.Rows[rowIndex].Cells[songay + 6].Value = sumK;
            CellBorder(sheet, rowIndex, songay + 3);
            CellBorder(sheet, rowIndex, songay + 4);
            CellBorder(sheet, rowIndex, songay + 5);
            CellBorder(sheet, rowIndex, songay + 6);
            sheet.Rows[rowIndex].Cells[songay + 3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[songay + 4].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[songay + 5].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[songay + 6].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "1. Ghi chú phần CHI TIẾT: ";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold= ExcelDefaultableBoolean.True;

            //DateTime cur = DateTime.Now;
            //sheet.Rows[rowIndex].Cells[35].Value = "Ngày " + cur.Day.ToString() +" tháng " + cur.Month.ToString() + " năm " + cur.Year.ToString();
            //sheet.Rows[rowIndex].Cells[35].CellFormat.Alignment = HorizontalCellAlignment.Center;
            //sheet.Rows[rowIndex].Cells[35].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
            //sheet.Rows[rowIndex].Cells[35].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;

            rowIndex++;
            

            sheet.Rows[rowIndex].Cells[1].Value = "(1) M: Thời gian (số phút) chấm vân tay đầu giờ làm việc muộn so với 7g30p;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(2) S: Thời gian (số phút) chấm vân tay cuối giờ làm việc sớm so với 17g00;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(3) KV: Không chấm vân tay đầu giờ làm việc (7g30p) trong ngày;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(4) KR: Không chấm vân tay cuối giờ làm việc (17g00) trong ngày;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(5) K: Không chấm vân tay cả đầu giờ và cuối giờ làm việc trong ngày";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(6) Trường hợp nhân viên có chấm vân tay trong ngày nghỉ thì báo cáo chỉ thể hiện giờ chấm vân tay vào/ra";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "2. Ghi chú phần TỔNG HỢP: ";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(1) M: Tổng thời gian (số phút) chấm vân tay đầu giờ làm việc muộn trong tháng;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(2) S: Tổng thời gian (số phút) chấm vân tay cuối giờ làm việc sớm trong tháng;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(3) T: Tổng số ngày thiếu chấm vân tay đầu giờ hoặc cuối giờ làm việc trong tháng;";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "(4) K: Tổng số ngày không chấm vân tay cả đầu giờ và cuối giờ làm việc trong tháng";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            sheet.Rows[rowIndex].Cells[1].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;

            DateTime cur = DateTime.Now;
            sheet.Rows[rowIndex].Cells[21].Value = "Ngày " + cur.Day.ToString() + " tháng " + cur.Month.ToString() + " năm " + cur.Year.ToString();
            sheet.Rows[rowIndex].Cells[21].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[21].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex++;

            sheet.Rows[rowIndex].Cells[4].Value = "Lập bảng";
            sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[4].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            sheet.Rows[rowIndex].Cells[21].Value = DonVi.TenBoPhan;
            sheet.Rows[rowIndex].Cells[21].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[21].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

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

        public string XuLyChuoi(string input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (i == input.Length - 1 && input[i] == ';')
                {
                    //nếu cuối chuỗi mà có dấu ; thì không xuống dòng
                }
                else
                {
                    if (input[i] == ' ')
                    {
                        //loại bỏ khoảng trắng
                    }
                    else
                    {
                        if (input[i] == ';')
                        {
                            result += '\n'.ToString();
                        }
                        else
                        {
                            result += input[i].ToString();
                        }
                    }
                }
            }
            return result;
        }
        public string GetThoiGianRaSom(string input)
        {
            string result = "";
            bool flag = false;
            for(int i = 0; i < input.Length; i++)
            {
                if (flag)
                {
                    if (input[i] == ';')
                        return result;
                    else if (Char.IsDigit(input[i]))
                        result = result + input[i];
                }
                if (input[i] == 'S')
                {
                    flag = true;
                }
            }
            return result;
        }
        public string GetThoiGianVaoMuon(string input)
        {
            string result = "";
            bool flag = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (flag)
                {
                    if (input[i] == ';')
                        return result;
                    else if (Char.IsDigit(input[i]))
                        result = result + input[i];
                }
                else if (input[i] == 'M')
                {
                    flag = true;
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