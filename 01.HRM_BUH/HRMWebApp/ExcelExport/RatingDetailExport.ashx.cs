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
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.ExcelExport
{
    /// <summary>
    /// Summary description for PlanDetailExport
    /// </summary>
    public class RatingDetailExport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating)
            Guid evaluationId = context.Request.Params["evaluationId"] != null ? new Guid(context.Request.Params["evaluationId"].ToString()) : Guid.Empty;
            Guid staffId = context.Request.Params["staffId"] != null ? new Guid(context.Request.Params["staffId"].ToString()) : Guid.Empty;
            Guid supervisorId = context.Request.Params["supervisorId"] != null ? new Guid(context.Request.Params["supervisorId"].ToString()) : Guid.Empty;
            Guid departmentId = context.Request.Params["departmentId"] != null ? new Guid(context.Request.Params["departmentId"].ToString()) : Guid.Empty;
            int op = context.Request.Params["option"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["option"].ToString());
            switch (op)
            {
                case 1:
                    {
                        StaffPlanExportToExcelProcess(evaluationId, staffId, supervisorId, departmentId, 0, context);
                    }
                    break;
                case 2:
                    {
                        ThanhTichExportToExcelProcess(evaluationId, staffId, context);
                    }
                    break;
            }
        }
        public void StaffPlanExportToExcelProcess(Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating, HttpContext context)
        {
            ABC_RatingDTO detailResult = new ABC_RatingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            ABC_RatingDetailApiController controller = new ABC_RatingDetailApiController();
            detailResult = controller.GetRatingDetail(evaluationId, staffId, supervisorId, departmentId, isAdminRating);

            //nếu chưa đánh giá thì điểm = 0
            if (detailResult.IsRated == false)
            {
                foreach (var tg in detailResult.ABC_RatingGroupDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.StaffRecord = 0;
                    }
                }
                foreach (var tg in detailResult.ABC_RatingGroupPropertyDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.StaffRecord = 0;
                    }
                }
                foreach (var tg in detailResult.ABC_RatingGroupSpecialDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.StaffRecord = 0;
                    }
                }
            }
            if (detailResult.IsSupervisorRated == false)
            {
                foreach (var tg in detailResult.ABC_RatingGroupDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.SupervisorRecord = 0;
                    }
                }
                foreach (var tg in detailResult.ABC_RatingGroupPropertyDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.SupervisorRecord = 0;
                    }
                }
                foreach (var tg in detailResult.ABC_RatingGroupSpecialDTOs)
                {
                    foreach (var rd in tg.ABC_RatingDetailDTOs)
                    {
                        rd.SupervisorRecord = 0;
                    }
                }
            }


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", detailResult.StaffName, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            str = RemoveWhitespace(str);
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 240;
            workbook.Styles.NormalStyle.StyleFormat.WrapText = ExcelDefaultableBoolean.True;
            Worksheet sheet = workbook.Worksheets.Add("DanhGia");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["DanhGia"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.38;
            sheet.PrintOptions.RightMargin = 0.38;
            sheet.PrintOptions.BottomMargin = 0.38;
            sheet.PrintOptions.TopMargin = 0.38;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Portrait;

            sheet.Columns[0].Width = 1500;
            sheet.Columns[1].Width = 15000;
            sheet.Columns[2].Width = 3000;
            sheet.Columns[3].Width = 3000;


            int rowIndex = 0;

            sheet.Rows[rowIndex].Cells[0].Value = "TRƯỜNG ĐẠI HỌC NGÂN HÀNG TP.HCM";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            sheet.Rows[rowIndex].Cells[3].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "            " + detailResult.DepartmentName.ToUpper();
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            sheet.Rows[rowIndex].Cells[3].Value = "Độc lập - Tự do - Hạnh phúc                  ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;

            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "                   ----------------------";
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            sheet.Rows[rowIndex].Cells[3].Value = "----------------------                          ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;

            rowIndex++;
            rowIndex++;

            if (detailResult.StaffType == 1)
            {
                WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 3);
                merged2.Value = "PHIẾU CHẤM ĐIỂM ĐÁNH GIÁ VÀ PHÂN LOẠI GIẢNG VIÊN";
                merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
                rowIndex++;
            }
            else if (detailResult.StaffType == 2)
            {
                WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 3);
                merged2.Value = "BẢNG ĐÁNH GIÁ XẾP LOẠI";
                merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
                rowIndex++;

                WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 3);
                merged3.Value = "CÔNG CHỨC, VIÊN CHỨC VÀ NGƯỜI LAO ĐỘNG";
                merged3.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                merged3.CellFormat.Alignment = HorizontalCellAlignment.Center;
                rowIndex++;
            }


            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 3);
            if (detailResult.EvaluationBoardType == 1)
                merged4.Value = "NĂM " + detailResult.Year.ToString();
            else if (detailResult.EvaluationBoardType == 2)
                merged4.Value = "6 THÁNG NĂM " + detailResult.Year.ToString();
            else
                merged4.Value = "THÁNG " + detailResult.Month.ToString() + "/NĂM " + detailResult.Year.ToString();

            merged4.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged4.CellFormat.Alignment = HorizontalCellAlignment.Center;
            rowIndex++;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[1].Value = "Họ và tên: " + detailResult.StaffName;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "Chức vụ: " + detailResult.StaffPosition;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            rowIndex++;
            sheet.Rows[rowIndex].Cells[1].Value = "Đơn vị công tác: " + detailResult.DepartmentName;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            rowIndex++;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "I";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 0);

            sheet.Rows[rowIndex].Cells[1].Value = "TIÊU CHÍ ĐÁNH GIÁ";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 1);

            sheet.Rows[rowIndex].Cells[2].Value = "Cá nhân đánh giá";
            sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 2);

            if (detailResult.IsAdmin == 1 || detailResult.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))
            {
                sheet.Rows[rowIndex].Cells[3].Value = "Hiệu trưởng đánh giá";
            }
            else sheet.Rows[rowIndex].Cells[3].Value = "Trưởng đơn vị đánh giá";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 3);
            rowIndex++;
            //Lấy STT

            decimal totalStaffRecord = 0;
            decimal totalSupervisorRecord = 0;

            foreach (ABC_RatingGroupDTO tg in detailResult.ABC_RatingGroupDTOs)
            {
                decimal totalRatingGroupStaffRecord = 0;
                decimal totalRatingGroupSupervisorRecord = 0;
                int idx = rowIndex;

                sheet.Rows[rowIndex].Cells[0].Value = tg.OrderNumber;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 0);

                sheet.Rows[rowIndex].Cells[1].Value = tg.Name;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                CellBorder(sheet, rowIndex, 1);

                sheet.Rows[rowIndex].Cells[2].Value = "";
                sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 2);

                sheet.Rows[rowIndex].Cells[3].Value = "";
                sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 3);
                rowIndex++;

                foreach (ABC_RatingDetailDTO rd in tg.ABC_RatingDetailDTOs)
                {
                    sheet.Rows[rowIndex].Cells[0].Value = rd.OrderNumber;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 0);

                    sheet.Rows[rowIndex].Cells[1].Value = rd.Name;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                    CellBorder(sheet, rowIndex, 1);

                    sheet.Rows[rowIndex].Cells[2].Value = rd.StaffRecord;
                    sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 2);

                    sheet.Rows[rowIndex].Cells[3].Value = rd.SupervisorRecord;
                    sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 3);

                    totalRatingGroupStaffRecord += rd.StaffRecord;
                    totalRatingGroupSupervisorRecord += rd.SupervisorRecord;

                    totalStaffRecord += rd.StaffRecord;
                    totalSupervisorRecord += rd.SupervisorRecord;
                    rowIndex++;
                }
                sheet.Rows[idx].Cells[2].Value = totalRatingGroupStaffRecord;
                sheet.Rows[idx].Cells[3].Value = totalRatingGroupSupervisorRecord;
            }
            sheet.Rows[rowIndex].Cells[0].Value = "II";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 0);

            if (detailResult.StaffType == 1)
            {
                sheet.Rows[rowIndex].Cells[1].Value = "ĐÁNH GIÁ ĐỐI VỚI GIẢNG VIÊN";
                sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 1);
            }
            else if (detailResult.StaffType == 2)
            {
                sheet.Rows[rowIndex].Cells[1].Value = "ĐÁNH GIÁ ĐỐI VỚI VIÊN CHỨC QUẢN LÝ, NGƯỜI LAO ĐỘNG";
                sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                CellBorder(sheet, rowIndex, 1);
            }

            sheet.Rows[rowIndex].Cells[2].Value = "";
            sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 2);

            sheet.Rows[rowIndex].Cells[3].Value = "";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 3);
            int idx2 = rowIndex;
            rowIndex++;
            foreach (ABC_RatingGroupDTO tgs in detailResult.ABC_RatingGroupSpecialDTOs)
            {
                decimal totalRatingGroupStaffRecord = 0;
                decimal totalRatingGroupSupervisorRecord = 0;
                foreach (ABC_RatingDetailDTO rd in tgs.ABC_RatingDetailDTOs)
                {
                    sheet.Rows[rowIndex].Cells[0].Value = rd.OrderNumber;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 0);

                    sheet.Rows[rowIndex].Cells[1].Value = rd.Name;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 1);

                    sheet.Rows[rowIndex].Cells[2].Value = rd.StaffRecord;
                    sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 2);

                    sheet.Rows[rowIndex].Cells[3].Value = rd.SupervisorRecord;
                    sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 3);

                    totalRatingGroupStaffRecord += rd.StaffRecord;
                    totalRatingGroupSupervisorRecord += rd.SupervisorRecord;

                    totalStaffRecord += rd.StaffRecord;
                    totalSupervisorRecord += rd.SupervisorRecord;

                    rowIndex++;
                }
                sheet.Rows[idx2].Cells[2].Value = totalRatingGroupStaffRecord;
                sheet.Rows[idx2].Cells[3].Value = totalRatingGroupSupervisorRecord;
            }
            foreach (ABC_RatingGroupDTO tgp in detailResult.ABC_RatingGroupPropertyDTOs)
            {
                if (!(tgp.IsNotVisibleInEvaluationBoardType == detailResult.EvaluationBoardType))
                {
                    decimal totalRatingGroupStaffRecord = 0;
                    decimal totalRatingGroupSupervisorRecord = 0;
                    string totalRatingGroupStaffRecordString = "";
                    string totalRatingGroupSupervisorRecordString = "";
                    int idx = rowIndex;

                    sheet.Rows[rowIndex].Cells[0].Value = "";
                    CellBorder(sheet, rowIndex, 0);

                    sheet.Rows[rowIndex].Cells[1].Value = tgp.Name;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 1);

                    sheet.Rows[rowIndex].Cells[2].Value = "";
                    sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 2);

                    sheet.Rows[rowIndex].Cells[3].Value = "";
                    sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    CellBorder(sheet, rowIndex, 3);
                    rowIndex++;

                    foreach (ABC_RatingDetailDTO rd in tgp.ABC_RatingDetailDTOs)
                    {
                        sheet.Rows[rowIndex].Cells[0].Value = rd.OrderNumber;
                        sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                        sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, 0);

                        sheet.Rows[rowIndex].Cells[1].Value = rd.Name;
                        sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Justify;
                        sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                        CellBorder(sheet, rowIndex, 1);

                        if (rd.ABC_CriterionDetailType != 2)
                        {
                            sheet.Rows[rowIndex].Cells[2].Value = rd.StaffRecord;
                            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            CellBorder(sheet, rowIndex, 2);

                            sheet.Rows[rowIndex].Cells[3].Value = rd.SupervisorRecord;
                            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            CellBorder(sheet, rowIndex, 3);
                        }
                        if (rd.ABC_CriterionDetailType == 2)
                        {
                            sheet.Rows[rowIndex].Cells[2].Value = rd.StaffRecord == 1 ? "+" : "";
                            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
                            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            CellBorder(sheet, rowIndex, 2);

                            sheet.Rows[rowIndex].Cells[3].Value = rd.SupervisorRecord == 1 ? "+" : "";
                            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            CellBorder(sheet, rowIndex, 3);
                        }
                        if (rd.ABC_CriterionDetailType == 0)
                        {
                            totalRatingGroupStaffRecord += rd.StaffRecord;
                            totalRatingGroupSupervisorRecord += rd.SupervisorRecord;
                            totalRatingGroupStaffRecordString = Math.Round(totalRatingGroupStaffRecord, 0).ToString();
                            totalRatingGroupSupervisorRecordString = Math.Round(totalRatingGroupSupervisorRecord, 0).ToString();

                            totalStaffRecord += rd.StaffRecord;
                            totalSupervisorRecord += rd.SupervisorRecord;
                        }
                        else if (rd.ABC_CriterionDetailType == 1)
                        {
                            totalRatingGroupStaffRecord -= rd.StaffRecord;
                            totalRatingGroupSupervisorRecord -= rd.SupervisorRecord;
                            totalRatingGroupStaffRecordString = Math.Round(totalRatingGroupStaffRecord, 0).ToString();
                            totalRatingGroupSupervisorRecordString = Math.Round(totalRatingGroupSupervisorRecord, 0).ToString();

                            totalStaffRecord -= rd.StaffRecord;
                            totalSupervisorRecord -= rd.SupervisorRecord;
                        }

                        rowIndex++;
                    }
                    sheet.Rows[idx].Cells[2].Value = totalRatingGroupStaffRecordString;
                    sheet.Rows[idx].Cells[3].Value = totalRatingGroupSupervisorRecordString;
                }

            }
            sheet.Rows[rowIndex].Cells[0].Value = "";
            CellBorder(sheet, rowIndex, 0);

            sheet.Rows[rowIndex].Cells[1].Value = "Tổng số điểm";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 1);

            sheet.Rows[rowIndex].Cells[2].Value = totalStaffRecord;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 2);

            sheet.Rows[rowIndex].Cells[3].Value = totalSupervisorRecord;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 3);

            rowIndex += 2;

            sheet.Rows[rowIndex].Cells[3].Value = "TP. Hồ Chí Minh, ngày     tháng     năm 20...";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[3].Value = "Người tự đánh giá                        ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[3].Value = "(Ký và ghi rõ họ tên)                      ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            rowIndex += 5;

            if (detailResult.IsAdmin == 1 || detailResult.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))
            {
                sheet.Rows[rowIndex].Cells[0].Value = "Nhận xét và đánh giá chung của Hiệu trưởng:";
            }
            else sheet.Rows[rowIndex].Cells[0].Value = "Nhận xét và đánh giá chung của Trưởng đơn vị:";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "...............................................................................................................................................................................";
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "...............................................................................................................................................................................";
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            rowIndex++;
            if (detailResult.IsAdmin == 1 || detailResult.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))
            {
                sheet.Rows[rowIndex].Cells[3].Value = "Hiệu trưởng                          ";
            }
            else sheet.Rows[rowIndex].Cells[3].Value = "Trưởng đơn vị                          ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[3].Value = "(Ký và ghi rõ họ tên)                      ";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Italic = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Right;

            string filename = "/Temp/Excel/1.xls";
            workbook.Save(context.Server.MapPath(filename));

            BinaryReader reader = new BinaryReader(new FileStream(context.Server.MapPath(filename), FileMode.Open));
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment; filename=" + str);
            context.Response.BinaryWrite(reader.ReadBytes((int)(new FileInfo(context.Server.MapPath(filename))).Length));
            reader.Close();
            context.Response.Flush();

        }

        public void ThanhTichExportToExcelProcess(Guid evaluationId, Guid staffId, HttpContext context)
        {
            ABC_EvaluationBoardApiController controller_EvaluationBoard = new ABC_EvaluationBoardApiController();
            ABC_EvaluationBoard evaluation = controller_EvaluationBoard.GetObj(evaluationId);
            Dictionary<string, object> resultDic = DataClassHelper.GetThanhTichCaNhan(evaluationId, staffId);

            StaffApiController controller_Staff = new StaffApiController();
            StaffDTO staffDTO = controller_Staff.GetStaffDTOById(staffId);

            DateTime now = DateTime.Now;
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", staffDTO.Name, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            str = RemoveWhitespace(str);
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 240;
            workbook.Styles.NormalStyle.StyleFormat.WrapText = ExcelDefaultableBoolean.True;
            Worksheet sheet = workbook.Worksheets.Add("ThanhTich");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["ThanhTich"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.38;
            sheet.PrintOptions.RightMargin = 0.38;
            sheet.PrintOptions.BottomMargin = 0.38;
            sheet.PrintOptions.TopMargin = 0.38;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Portrait;

            sheet.Columns[0].Width = 1500;
            sheet.Columns[1].Width = 10000;
            sheet.Columns[2].Width = 6000;
            sheet.Columns[3].Width = 5000;
            sheet.Columns[4].Width = 5000;
            sheet.Columns[5].Width = 5000;


            int rowIndex = 0;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 5);
            merged2.Value = "THÀNH TÍCH CÁ NHÂN " + evaluation.Name.ToUpper();
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            rowIndex++;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "1. Thành tích NCKH và sáng kiến (Báo cáo tổng hợp từ Viện nghiên cứu KH&CN)";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            rowIndex++;

            sheet.Rows[rowIndex].Cells[0].Value = "STT";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 0);

            sheet.Rows[rowIndex].Cells[1].Value = "TÊN SÁNG KIẾN";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 1);

            sheet.Rows[rowIndex].Cells[2].Value = "CẤP ĐỘ THAM GIA";
            sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 2);

            sheet.Rows[rowIndex].Cells[3].Value = "THỜI GIAN HOÀN THÀNH";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 3);

            sheet.Rows[rowIndex].Cells[4].Value = "ĐIỂM";
            sheet.Rows[rowIndex].Cells[4].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[4].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 4);
            rowIndex++;

            int stt = 1;
            List<spd_Web_DanhGia_ThanhTichCaNhan_SangKien> listSangKien = (List<spd_Web_DanhGia_ThanhTichCaNhan_SangKien>)resultDic["SangKien"];
            List<spd_Web_DanhGia_ThanhTichCaNhan_NCKH> listNCKH = (List<spd_Web_DanhGia_ThanhTichCaNhan_NCKH>)resultDic["NCKH"];
            List<spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong> listKhenThuong = (List<spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong>)resultDic["KhenThuong"];

            foreach (spd_Web_DanhGia_ThanhTichCaNhan_SangKien obj in listSangKien)
            {
                sheet.Rows[rowIndex].Cells[0].Value = stt;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].Value = obj.TenSangKien;
                sheet.Rows[rowIndex].Cells[2].Value = obj.CapDoThamGia;
                sheet.Rows[rowIndex].Cells[3].Value = obj.ThoiGianHoanThanh;
                sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[4].Value = obj.Diem;
                sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 0, 4, false, false, false, true, true);
                rowIndex++;
                stt++;
            }
            stt = 1;

            rowIndex++;
            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "STT";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[0].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 0);

            sheet.Rows[rowIndex].Cells[1].Value = "TÊN KẾT QUẢ HOẠT ĐỘNG KHCN";
            sheet.Rows[rowIndex].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 1);

            sheet.Rows[rowIndex].Cells[2].Value = "TÊN SẢN PHẨM";
            sheet.Rows[rowIndex].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 2);

            sheet.Rows[rowIndex].Cells[3].Value = "CẤP ĐỘ THAM GIA";
            sheet.Rows[rowIndex].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 3);

            sheet.Rows[rowIndex].Cells[4].Value = "THỜI GIAN HOÀN THÀNH";
            sheet.Rows[rowIndex].Cells[4].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[4].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 4);

            sheet.Rows[rowIndex].Cells[5].Value = "SỐ TIẾT QUY ĐỔI";
            sheet.Rows[rowIndex].Cells[5].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[5].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[5].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 5);

            sheet.Rows[rowIndex].Cells[6].Value = "ĐIỂM";
            sheet.Rows[rowIndex].Cells[6].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[6].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex].Cells[6].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex, 6);
            rowIndex++;
            foreach (spd_Web_DanhGia_ThanhTichCaNhan_NCKH obj in listNCKH)
            {
                sheet.Rows[rowIndex].Cells[0].Value = stt;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].Value = obj.TenHoatDong;
                sheet.Rows[rowIndex].Cells[2].Value = obj.TenNCKH;
                sheet.Rows[rowIndex].Cells[3].Value = obj.CapDoThamGia;
                sheet.Rows[rowIndex].Cells[4].Value = obj.ThoiGianHoanThanh;
                sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[5].Value = obj.SoTietQuyDoi;
                sheet.Rows[rowIndex].Cells[5].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[6].Value = obj.Diem;
                sheet.Rows[rowIndex].Cells[6].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 0, 6, false, false, false, true, true);
                rowIndex++;
                stt++;
            }
            stt = 1;

            rowIndex++;
            rowIndex++;
            sheet.Rows[rowIndex].Cells[0].Value = "2. Thành tích thi đua khen thưởng (Báo cáo tổng hợp từ phòng Tố chức cán bộ)";
            sheet.Rows[rowIndex].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex].Cells[0].CellFormat.WrapText = ExcelDefaultableBoolean.False;
            rowIndex++;

            WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
            merged3.Value = "STT";
            SetRegionBorder(merged3, true, false, true, true);

            WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex, 2);
            merged4.Value = "TÊN HÌNH THỨC THI ĐUA KHEN THƯỞNG";
            SetRegionBorder(merged4, true, false, true, true);

            sheet.Rows[rowIndex + 1].Cells[1].Value = "Tên danh hiệu";
            sheet.Rows[rowIndex + 1].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex + 1].Cells[1].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex + 1].Cells[1].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 1, 1);

            sheet.Rows[rowIndex + 1].Cells[2].Value = "Chi tiết danh hiệu";
            sheet.Rows[rowIndex + 1].Cells[2].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex + 1].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex + 1].Cells[2].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 1, 2);

            WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex, 6);
            merged5.Value = "QUYẾT ĐỊNH";
            SetRegionBorder(merged5, true, false, true, true);

            sheet.Rows[rowIndex + 1].Cells[3].Value = "Số";
            sheet.Rows[rowIndex + 1].Cells[3].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex + 1].Cells[3].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex + 1].Cells[3].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 1, 3);

            sheet.Rows[rowIndex + 1].Cells[4].Value = "Ngày";
            sheet.Rows[rowIndex + 1].Cells[4].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            sheet.Rows[rowIndex + 1].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
            sheet.Rows[rowIndex + 1].Cells[4].CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            CellBorder(sheet, rowIndex + 1, 4);

            WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex + 1, 5, rowIndex + 1, 6);
            merged6.Value = "Đơn vị ra quyết định";
            SetRegionBorder(merged6, true, false, true, true);

            rowIndex++;
            rowIndex++;
            foreach (spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong obj in listKhenThuong)
            {
                sheet.Rows[rowIndex].Cells[0].Value = stt;
                sheet.Rows[rowIndex].Cells[0].CellFormat.Alignment = HorizontalCellAlignment.Center;
                sheet.Rows[rowIndex].Cells[1].Value = obj.TenHinhThucKhenThuong;
                sheet.Rows[rowIndex].Cells[2].Value = obj.LyDo;
                sheet.Rows[rowIndex].Cells[3].Value = obj.SoQuyetDinh;
                sheet.Rows[rowIndex].Cells[4].Value = obj.NgayQuyetDinh;
                sheet.Rows[rowIndex].Cells[4].CellFormat.Alignment = HorizontalCellAlignment.Center;
                //sheet.Rows[rowIndex].Cells[4].Value = obj.CoQuanRaQuyetDinh;
                WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex, 6);
                merged7.Value = obj.CoQuanRaQuyetDinh;
                SetCellFormat(sheet, rowIndex, 0, 6, false, false, false, true, true);
                rowIndex++;
                stt++;
            }

            string filename = "/Temp/Excel/1.xls";
            workbook.Save(context.Server.MapPath(filename));

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
            if (isJustify)
                region.CellFormat.Alignment = HorizontalCellAlignment.Justify;
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