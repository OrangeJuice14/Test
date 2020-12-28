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
    /// <summary>
    /// Summary description for PlanDetailExport
    /// </summary>
    public class RatingDetailExport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            Guid planId = context.Request.Params["planId"] != null ? new Guid(context.Request.Params["planId"].ToString()) : Guid.Empty;
            Guid agentObjectId = context.Request.Params["agentObjectId"] != null ? new Guid(context.Request.Params["agentObjectId"].ToString()) : Guid.Empty;
            Guid departmentId = context.Request.Params["departmentId"] != null ? new Guid(context.Request.Params["departmentId"].ToString()) : Guid.Empty;

            int op = context.Request.Params["option"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["option"].ToString());

            int userRole = context.Request.Params["userRole"] == null
                            ? 0
                            : Convert.ToInt32(context.Request.Params["userRole"].ToString());

            switch (op)
            {
                case 1:
                    {
                    }
                    break;
                case 2:
                    {
                        StaffPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 3:
                    {
                        DepartmentPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 4:
                    {
                        SchoolPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 5:
                    {
                        FacultyPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 8:
                    {
                        SubFacultyPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 7:
                    {
                        SubDepartmentPlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                case 10:
                    {
                        PrinciplePlanExportToExcelProcess(planId, agentObjectId, departmentId, userRole, context);
                    }
                    break;
                    //case 6:
                    //    {
                    //        SubjectPlanExportToExcelProcess(planId, agentObjectId, userRole, context);
                    //    }
                    //    break;

            }

        }

        //--4. Kế hoạch trường
        public void SchoolPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged1.Value = "BẢN KẾ HOẠCH MTCL";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged2.Value = "ÁP DỤNG CHO BAN GIÁM HIỆU";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 10);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 11, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[11].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 10, 11, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 10);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[11].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[11].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 10, 11, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 11, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;

            sheet.Columns[0].Width = 50 * 30;
            sheet.Columns[1].Width = 50 * 140;
            sheet.Columns[2].Width = 50 * 150;
            sheet.Columns[3].Width = 50 * 100;
            sheet.Columns[4].Width = 50 * 90;
            sheet.Columns[5].Width = 50 * 90;
            sheet.Columns[6].Width = 50 * 120;
            sheet.Columns[7].Width = 50 * 80;
            sheet.Columns[8].Width = 50 * 80;
            sheet.Columns[9].Width = 50 * 70;
            sheet.Columns[10].Width = 50 * 60;
            sheet.Columns[11].Width = 50 * 60;
            stt = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                #region TargetGroup
                WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                mergedStt.Value = "STT";
                BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                SetRegionBorder(mergedStt, true, false, true, true);

                sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt++ + ":";
                SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
                mergedGroupName.Value = tg.TargetGroupName;
                SetRegionBorder(mergedGroupName, true, false, false, true);
                BackgroundColor(sheet, rowIndex, 1, 11, Color.LightGray);
                rowIndex++;


                sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp thực hiện";
                sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                sheet.Rows[rowIndex].Cells[4].Value = "Chỉ đạo";
                sheet.Rows[rowIndex].Cells[5].Value = "Đơn vị chủ trì";
                sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị phối hợp thực hiện";
                sheet.Rows[rowIndex].Cells[7].Value = "KPIs thực hiện năm học trước";
                sheet.Rows[rowIndex].Cells[8].Value = "KPIs đăng ký thực hiện năm nay";
                sheet.Rows[rowIndex].Cells[9].Value = "Đơn vị tính";
                sheet.Rows[rowIndex].Cells[10].Value = "Thời gian bắt đầu";
                sheet.Rows[rowIndex].Cells[11].Value = "Thời gian kết thúc";
                SetCellFormat(sheet, rowIndex, 1, 11, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 1, 11, Color.LightGray);

                int stt2 = 1;
                rowIndex++;
                int beginIndex = rowIndex;
                foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                {
                    #region PlanDetail
                    int subRowIndex = rowIndex;
                    List<int> subRowIndexs = new List<int>();
                    int maxSubRowIndex = rowIndex;
                    if (pmd.SubDepartmentNames.Count < 1)
                    {
                        SetCellFormat(sheet, subRowIndex++, 6, 6, false, true, false, true, false);
                    }
                    foreach (string strSD in pmd.SubDepartmentNames)
                    {
                        sheet.Rows[subRowIndex].Cells[6].Value = "- " + strSD;
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 6, 6, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 6, 6, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    foreach (MethodDTO me in pmd.Methods)
                    {
                        sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                        sheet.Rows[subRowIndex].Cells[10].Value = me.StartTimeString;
                        sheet.Rows[subRowIndex].Cells[11].Value = me.EndTimeString;
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                        }

                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;


                    foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                    {
                        sheet.Rows[subRowIndex].Cells[8].Value = kpi.Name;
                        sheet.Rows[subRowIndex].Cells[9].Value = kpi.MeasureUnitName;
                        SetCellFormat(sheet, subRowIndex, 8, 8, false, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex++, 9, 9, false, false, false, false, false);
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = subRowIndexs.Max();
                    if (subRowIndex < rowIndex)
                        subRowIndex = rowIndex;
                    WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                    mergedSTT.Value = stt2++;
                    SetRegionBorder(mergedSTT, true, false, false, false);

                    WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                    mergedTargetDetail.Value = pmd.TargetDetail;
                    SetRegionBorder(mergedTargetDetail, false, true, false, false);

                    WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                    mergedBasicResource.Value = pmd.BasicResource;
                    SetRegionBorder(mergedBasicResource, true, false, false, false);

                    WorksheetMergedCellsRegion mergedStaffLeader = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                    mergedStaffLeader.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                    SetRegionBorder(mergedStaffLeader, true, false, false, false);

                    WorksheetMergedCellsRegion mergedLeadDepartmentName = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                    mergedLeadDepartmentName.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                    SetRegionBorder(mergedLeadDepartmentName, true, false, false, false);

                    WorksheetMergedCellsRegion mergedPreviousKPI = sheet.MergedCellsRegions.Add(rowIndex, 7, subRowIndex, 7);
                    mergedPreviousKPI.Value = pmd.PreviousKPI;
                    SetRegionBorder(mergedPreviousKPI, true, false, false, false);

                    rowIndex = subRowIndex + 1;
                    SetCellFormat(sheet, rowIndex, 8, 8, true, false, false, true, false);
                    SetCellFormat(sheet, rowIndex, 9, 9, false, false, false, true, false);
                    SetCellFormat(sheet, rowIndex, 10, 10, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 11, 11, true, true, false, true, false);
                    #endregion
                }
                for (int i = beginIndex; i < rowIndex; i++)
                {
                    SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                    SetCellFormat(sheet, i, 9, 9, false, false, false, false, false);
                    SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                    SetCellFormat(sheet, i, 11, 11, true, false, false, false, false);
                }
                SetCellFormat(sheet, rowIndex - 1, 2, 2, false, false, false, false, true);
                SetCellFormat(sheet, rowIndex - 1, 6, 6, false, false, false, false, true);

                //rowIndex++;
                #endregion
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
       //3. Kế hoạch phòng ban
        public void DepartmentPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged1.Value = "BẢN KẾ HOẠCH HOẠT ĐỘNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged2.Value = "ÁP DỤNG CHO TẬP THỂ PHÒNG/BAN";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[14].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            int stt2 = 1;
            int stt3 = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[14].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[14].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            List<PlanKPIMakingDetailDTO> PlanKPIDetails2 = new List<PlanKPIMakingDetailDTO>();
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                if (tg.TargetGroupDetailTypeId == 1)
                {
                    #region TargetGroup 1
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;
                    sheet.Columns[14].Width = 50 * 60;


                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 15);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);
                    rowIndex++;

                    
                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[8].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[11].Value = "KPIs đăng ký của trường";
                    sheet.Rows[rowIndex].Cells[12].Value = "KPIs đăng ký của đơn vị";
                    sheet.Rows[rowIndex].Cells[13].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[14].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[15].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 15, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[14].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[15].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 14, 14, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 15, 15, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 9, 9, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 9, 9, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.ParentPlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = kpi.Name;
                            SetCellFormat(sheet, subRowIndex++, 11, 11, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[12].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[13].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 13, 13, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged5, true, false, false, false);
                        

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 7, subRowIndex, 7);
                        merged7.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged7, true, false, false, false);

                        WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                        merged10.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged10, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 2)
                {
                    #region TargetGroup 2
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;
                    sheet.Columns[14].Width = 50 * 60;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 12);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs đăng ký năm nay";
                    sheet.Rows[rowIndex].Cells[11].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 12, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in PlanKPIDetails2)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);


                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 9, subRowIndex, 9);
                        merged9.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged9, true, false, false, false);
                        WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                        merged10.Value = pmd.CurrentKPISecond;
                        SetRegionBorder(merged10, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);

                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 3)
                {
                    #region TargetGroup 3
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 120;
                    sheet.Columns[5].Width = 50 * 120;
                    sheet.Columns[6].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 90;
                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 7);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[7].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 7, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[6].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[7].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetailName;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 5, 5, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
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
        //4. Nhân viên
        public void StaffPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged1.Value = "BẢN KẾ HOẠCH HOẠT ĐỘNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged2.Value = "ÁP DỤNG CHO NHÂN VIÊN VĂN PHÒNG";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[14].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            int stt2 = 1;
            int stt3 = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[14].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[14].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            List<PlanKPIMakingDetailDTO> PlanKPIDetails2 = new List<PlanKPIMakingDetailDTO>();
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                if (tg.TargetGroupDetailTypeId == 1)
                {
                    #region TargetGroup 1
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;
                    sheet.Columns[14].Width = 50 * 60;


                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 15);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[7].Value = "KPIs đăng ký";
                    sheet.Rows[rowIndex].Cells[8].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[9].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[10].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 15, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[9].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[10].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[8].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged6, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 2)
                {
                    #region TargetGroup 2
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 8);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 8, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[6].Value = "KPIs đăng ký năm nay";
                    sheet.Rows[rowIndex].Cells[7].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[8].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 8, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 8, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in PlanKPIDetails2)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[7].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[8].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        //subRowIndex = rowIndex;
                        
                       
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion mergedStaffLeader = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        mergedStaffLeader.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(mergedStaffLeader, true, false, false, false);
;

                        WorksheetMergedCellsRegion mergedPreviousKPI = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        mergedPreviousKPI.Value = pmd.PreviousKPI;
                        SetRegionBorder(mergedPreviousKPI, true, false, false, false);
                        WorksheetMergedCellsRegion mergedCurrentKPI = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        mergedCurrentKPI.Value = pmd.CurrentKPISecond;
                        SetRegionBorder(mergedCurrentKPI, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, true);


                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);

                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 3)
                {
                    #region TargetGroup 3
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 6);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 6, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[6].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 6, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 6, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[5].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[6].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetailName;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion mergedStaffLeader = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        mergedStaffLeader.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(mergedStaffLeader, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 5, 5, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
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
        //8. phó khoa
        public void SubFacultyPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged1.Value = "BẢN KẾ HOẠCH HOẠT ĐỘNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged2.Value = "ÁP DỤNG CHO PHÓ TRƯỞNG KHOA/VIỆN/TRƯỜNG";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[14].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            int stt2 = 1;
            int stt3 = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[14].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[14].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            List<PlanKPIMakingDetailDTO> PlanKPIDetails2 = new List<PlanKPIMakingDetailDTO>();
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                if (tg.TargetGroupDetailTypeId == 0)
                {
                    #region TargetGroup 0
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 13);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs đăng ký của đơn vị";
                    sheet.Rows[rowIndex].Cells[11].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[13].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 13, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[13].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[10].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 10, 10, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 11, 11, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 9, subRowIndex, 9);
                        merged9.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged9, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 5)
                {
                    #region TargetGroup 5 Nghiên cứu khoa học
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 13);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs đăng ký của đơn vị";
                    sheet.Rows[rowIndex].Cells[11].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[13].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 13, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[13].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[10].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 10, 10, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 11, 11, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 9, subRowIndex, 9);
                        merged9.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged9, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 1)
                {
                    #region TargetGroup 1
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;
                    sheet.Columns[14].Width = 50 * 60;


                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 15);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[8].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[11].Value = "KPIs đăng ký của trường";
                    sheet.Rows[rowIndex].Cells[12].Value = "KPIs đăng ký của đơn vị";
                    sheet.Rows[rowIndex].Cells[13].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[14].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[15].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 15, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[14].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[15].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 14, 14, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 15, 15, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 9, 9, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 9, 9, false, false, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.ParentPlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = kpi.Name;
                            SetCellFormat(sheet, subRowIndex++, 11, 11, false, false, false, false, false);
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[12].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[13].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 13, 13, false, false, false, false, false);
                        }
                        //subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged5, true, false, false, false);


                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 7, subRowIndex, 7);
                        merged7.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged7, true, false, false, false);

                        WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                        merged10.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged10, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 2)
                {
                    #region TargetGroup 2
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;
                    sheet.Columns[14].Width = 50 * 60;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 12);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs đăng ký năm nay";
                    sheet.Rows[rowIndex].Cells[11].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 12, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in PlanKPIDetails2)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, false, false);
                            }
                        }
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);


                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 9, subRowIndex, 9);
                        merged9.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged9, true, false, false, false);
                        WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                        merged10.Value = pmd.CurrentKPISecond;
                        SetRegionBorder(merged10, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);

                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 3)
                {
                    #region TargetGroup 3
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 120;
                    sheet.Columns[5].Width = 50 * 120;
                    sheet.Columns[6].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 90;
                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 7);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[7].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 7, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[6].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[7].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }

                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetailName;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 5, 5, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
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
        //7. Phó phòng
        public void SubDepartmentPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged1.Value = "BẢN KẾ HOẠCH HOẠT ĐỘNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged2.Value = "ÁP DỤNG CHO PHÓ TRƯỞNG PHÒNG/BAN";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[14].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            int stt2 = 1;
            int stt3 = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[14].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[14].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            List<PlanKPIMakingDetailDTO> PlanKPIDetails2 = new List<PlanKPIMakingDetailDTO>();
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                if (tg.TargetGroupDetailTypeId == 1)
                {
                    #region TargetGroup 1
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;
                    sheet.Columns[13].Width = 50 * 60;



                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 13);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[10].Value = "KPIs đăng ký";
                    sheet.Rows[rowIndex].Cells[11].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[13].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 13, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 13, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[13].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 8, 8, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 8, 8, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[10].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 10, 10, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 11, 11, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.Name : ""; 
                        SetRegionBorder(merged5, true, false, false, false);


                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 7, subRowIndex, 7);
                        merged7.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged7, true, false, false, false);

                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                        merged9.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged9, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 13, 13, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);
                            SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 2)
                {
                    #region TargetGroup 2
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;


                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 11, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[7].Value = "Người thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs đăng ký năm nay";
                    sheet.Rows[rowIndex].Cells[10].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[11].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 11, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 11, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in PlanKPIDetails2)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[10].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[11].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[6].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 6, 6, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 6, 6, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubStaffNames)
                        {
                            sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, false, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex, 7, 7, false, false, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.StaffLeader != null ? pmd.StaffLeader.Name : "";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);


                        WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 8, subRowIndex, 8);
                        merged8.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged8, true, false, false, false);
                        WorksheetMergedCellsRegion merged9 = sheet.MergedCellsRegions.Add(rowIndex, 9, subRowIndex, 9);
                        merged9.Value = pmd.CurrentKPISecond;
                        SetRegionBorder(merged9, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);


                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);

                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 3)
                {
                    #region TargetGroup 3
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 120;
                    sheet.Columns[5].Width = 50 * 120;
                    sheet.Columns[6].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 90;
                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 7);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[7].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 7, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[6].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[7].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetailName;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 5, 5, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
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
        //10. Hiệu trưởng
        public void PrinciplePlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);


            DateTime now = DateTime.Now;
            string className = "";

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;


            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged1.Value = "BẢN KẾ HOẠCH HOẠT ĐỘNG";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;

            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            merged2.Value = "ÁP DỤNG CHO HIỆU TRƯỞNG";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedDonVi.Value = "Đơn vị";
            mergedDonVi2.Value = "Ban giám hiệu";
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedCapTren.Value = "Cấp trên trực tiếp";
            mergedCapTren2.Value = "Hội đồng trường";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;

            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 14);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;

            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[14].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);

            rowIndex++;
            int stt = 1;
            int stt2 = 1;
            int stt3 = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 13);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[14].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[14].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 13, 14, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 14, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            List<PlanKPIMakingDetailDTO> PlanKPIDetails2 = new List<PlanKPIMakingDetailDTO>();
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                if (tg.TargetGroupDetailTypeId == 0)
                {
                    #region TargetGroup 0
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;

                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 12);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs đăng";
                    sheet.Rows[rowIndex].Cells[10].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[11].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 12, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[10].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 9, 9, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 10, 10, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 8, subRowIndex, 8);
                        merged8.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged8, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);

                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);

                        }
                        #endregion
                    }
                    #endregion
                }
                if (tg.TargetGroupDetailTypeId == 5)
                {
                    #region TargetGroup 5 Nghiên cứu khoa học
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 90;
                    sheet.Columns[5].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 120;
                    sheet.Columns[7].Width = 50 * 80;
                    sheet.Columns[8].Width = 50 * 80;
                    sheet.Columns[9].Width = 50 * 70;
                    sheet.Columns[10].Width = 50 * 60;
                    sheet.Columns[11].Width = 50 * 60;
                    sheet.Columns[12].Width = 50 * 60;


                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 12);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/Các bước thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "Trọng số";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Đơn vị chủ trì";
                    sheet.Rows[rowIndex].Cells[7].Value = "Đơn vị phối hợp thực hiện";
                    sheet.Rows[rowIndex].Cells[8].Value = "KPIs thực hiện năm học trước";
                    sheet.Rows[rowIndex].Cells[9].Value = "KPIs đăng";
                    sheet.Rows[rowIndex].Cells[10].Value = "Đơn vị tính";
                    sheet.Rows[rowIndex].Cells[11].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[12].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 12, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 12, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        PlanKPIDetails2.Add(pmd);
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[11].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[12].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (string strSD in pmd.SubDepartmentNames)
                        {
                            sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = rowIndex;
                        foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                        {
                            sheet.Rows[subRowIndex].Cells[9].Value = kpi.Name;
                            sheet.Rows[subRowIndex].Cells[10].Value = kpi.MeasureUnitName;
                            SetCellFormat(sheet, subRowIndex, 9, 9, false, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex++, 10, 10, false, false, false, false, false);
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetail;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.MaxRecord + " %";
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                        merged6.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                        SetRegionBorder(merged6, true, false, false, false);

                        WorksheetMergedCellsRegion merged8 = sheet.MergedCellsRegions.Add(rowIndex, 8, subRowIndex, 8);
                        merged8.Value = pmd.PreviousKPI;
                        SetRegionBorder(merged8, true, false, false, false);


                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 8, 8, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 9, 9, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 10, 10, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 11, 11, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 12, 12, true, false, false, false, true);

                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                            SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                            SetCellFormat(sheet, i, 9, 9, true, false, false, false, false);
                            SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                            SetCellFormat(sheet, i, 11, 11, false, false, false, false, false);
                            SetCellFormat(sheet, i, 12, 12, false, false, false, false, false);

                        }
                        #endregion
                    }
                    #endregion
                }                            
                if (tg.TargetGroupDetailTypeId == 3)
                {
                    #region TargetGroup 3
                    sheet.Columns[0].Width = 50 * 30;
                    sheet.Columns[1].Width = 50 * 140;
                    sheet.Columns[2].Width = 50 * 150;
                    sheet.Columns[3].Width = 50 * 100;
                    sheet.Columns[4].Width = 50 * 120;
                    sheet.Columns[5].Width = 50 * 120;
                    sheet.Columns[6].Width = 50 * 90;
                    sheet.Columns[6].Width = 50 * 90;
                    WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, 0);
                    mergedStt.Value = "STT";
                    BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                    SetRegionBorder(mergedStt, true, false, true, true);

                    sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt3++ + ":";
                    SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                    WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 7);
                    mergedGroupName.Value = tg.TargetGroupName;
                    SetRegionBorder(mergedGroupName, true, false, false, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);
                    rowIndex++;


                    sheet.Rows[rowIndex].Cells[1].Value = "Mục tiêu cụ thể";
                    sheet.Rows[rowIndex].Cells[2].Value = "Phương pháp/kế hoạch thực hiện";
                    sheet.Rows[rowIndex].Cells[3].Value = "Nguồn lực cần có";
                    sheet.Rows[rowIndex].Cells[4].Value = "BGH Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[5].Value = "Chỉ đạo";
                    sheet.Rows[rowIndex].Cells[6].Value = "Thời gian bắt đầu";
                    sheet.Rows[rowIndex].Cells[7].Value = "Thời gian kết thúc";
                    SetCellFormat(sheet, rowIndex, 1, 7, true, false, true, true, true);
                    BackgroundColor(sheet, rowIndex, 1, 7, Color.LightGray);

                    stt2 = 1;
                    rowIndex++;
                    foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                    {
                        #region PlanDetail
                        int subRowIndex = rowIndex;
                        int beginRow = rowIndex;
                        List<int> subRowIndexs = new List<int>();
                        foreach (MethodDTO me in pmd.Methods)
                        {
                            sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                            sheet.Rows[subRowIndex].Cells[6].Value = me.StartTimeString;
                            sheet.Rows[subRowIndex].Cells[7].Value = me.EndTimeString;
                            SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, false);
                            SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, false);
                            if (subRowIndex == rowIndex)
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                            }
                            else
                            {
                                SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                            }
                        }
                        subRowIndex--;
                        subRowIndexs.Add(subRowIndex);
                        subRowIndex = subRowIndexs.Max();
                        if (subRowIndex < rowIndex)
                            subRowIndex = rowIndex;
                        WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                        mergedSTT.Value = stt2++;
                        SetRegionBorder(mergedSTT, true, false, false, false);

                        WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                        mergedTargetDetail.Value = pmd.TargetDetailName;
                        SetRegionBorder(mergedTargetDetail, false, true, false, false);


                        WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                        mergedBasicResource.Value = pmd.BasicResource;
                        SetRegionBorder(mergedBasicResource, true, false, false, false);

                        WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                        merged4.Value = pmd.AdminLeaderName;
                        SetRegionBorder(merged4, true, false, false, false);

                        WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                        merged5.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                        SetRegionBorder(merged5, true, false, false, false);

                        rowIndex = subRowIndex + 1;
                        SetCellFormat(sheet, subRowIndex, 2, 2, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 5, 5, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 6, 6, true, false, false, false, true);
                        SetCellFormat(sheet, subRowIndex, 7, 7, true, false, false, false, true);
                        for (int i = beginRow; i <= subRowIndex; i++)
                        {
                            SetCellFormat(sheet, i, 2, 2, false, false, false, false, false);
                            SetCellFormat(sheet, i, 5, 5, false, false, false, false, false);
                            SetCellFormat(sheet, i, 6, 6, false, false, false, false, false);
                            SetCellFormat(sheet, i, 7, 7, false, false, false, false, false);
                        }
                        #endregion
                    }
                    #endregion
                }
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 5 - Khoa
        public void FacultyPlanExportToExcelProcess(Guid planId, Guid agentObjectId, Guid departmentId, int userRole, HttpContext context)
        {
            PlanDetailMakingDTO detailResult = new PlanDetailMakingDTO();
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            PlanKPIDetailApiController controller = new PlanKPIDetailApiController();
            detailResult = controller.GetList(planId, agentObjectId, Guid.Empty, departmentId, userRole, 0);

            DateTime now = DateTime.Now;

            string checkStatus = context.Request.Params["type"];
            string str = string.Format("KH_{0}_{1}_{2}_{3}_{4}_{5}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond);
            str = str + ".xls";
            Workbook workbook = new Workbook();
            workbook.Styles.NormalStyle.StyleFormat.Font.Name = "Times New Roman";
            //Size 11
            workbook.Styles.NormalStyle.StyleFormat.Font.Height = 220;
            Worksheet sheet = workbook.Worksheets.Add("KeHoach");

            workbook.WindowOptions.SelectedWorksheet = workbook.Worksheets["KeHoach"];
            sheet.PrintOptions.PaperSize = PaperSize.A4;

            //Margin 2 cm
            sheet.PrintOptions.LeftMargin = 0.77;
            sheet.PrintOptions.RightMargin = 0.77;
            sheet.PrintOptions.BottomMargin = 0.77;
            sheet.PrintOptions.TopMargin = 0.77;
            sheet.PrintOptions.HeaderMargin = 0;
            sheet.PrintOptions.FooterMargin = 0;
            sheet.PrintOptions.Orientation = Orientation.Landscape;
            int rowIndex = 0;

            #region BẢN KẾ HOẠCH MTCL NĂM HỌC
            WorksheetMergedCellsRegion merged1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged1.Value = "BẢN KẾ HOẠCH MTCL NĂM HỌC";
            merged1.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged1.CellFormat.Font.Name = "Times New Roman";
            merged1.CellFormat.Font.Height = 320;
            merged1.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex++;
            #endregion
            #region ÁP DỤNG CHO KHOA/VIỆN/TRƯỜNG
            WorksheetMergedCellsRegion merged2 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 11);
            merged2.Value = "ÁP DỤNG CHO KHOA/VIỆN/TRƯỜNG";
            merged2.CellFormat.Alignment = HorizontalCellAlignment.Center;
            merged2.CellFormat.Font.Name = "Times New Roman";
            merged2.CellFormat.Font.Height = 320;
            merged2.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            rowIndex = rowIndex + 2;
            #endregion

            #region Tầm nhìn
            WorksheetMergedCellsRegion mergedTamNhin = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedTamNhin2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedTamNhin.Value = "Tầm nhìn";
            mergedTamNhin2.Value = detailResult.Vision;
            SetRegionBorder(mergedTamNhin, false, false, false, true);
            SetRegionBorder(mergedTamNhin2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Sứ mạng
            WorksheetMergedCellsRegion mergedSuMang = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedSuMang2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedSuMang.Value = "Sứ mạng";
            mergedSuMang2.Value = detailResult.Mission;
            SetRegionBorder(mergedSuMang, false, false, false, true);
            SetRegionBorder(mergedSuMang2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Khoa/Viện/Trường
            WorksheetMergedCellsRegion mergedDonVi = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedDonVi2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedDonVi.Value = "Khoa/Viện/Trường";
            mergedDonVi2.Value = detailResult.StaffDTO.Department.Name;
            SetRegionBorder(mergedDonVi, false, false, false, true);
            SetRegionBorder(mergedDonVi2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Người lập
            WorksheetMergedCellsRegion mergedNguoiLap = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedNguoiLap2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedNguoiLap.Value = "Người lập";
            mergedNguoiLap2.Value = detailResult.StaffDTO.Name;
            SetRegionBorder(mergedNguoiLap, false, false, false, true);
            SetRegionBorder(mergedNguoiLap2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Vị trí
            WorksheetMergedCellsRegion mergedVitri = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedVitri2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedVitri.Value = "Vị trí";
            mergedVitri2.Value = "Trưởng khoa";
            SetRegionBorder(mergedVitri, false, false, false, true);
            SetRegionBorder(mergedVitri2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Cấp trên trực tiếp
            WorksheetMergedCellsRegion mergedCapTren = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedCapTren2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedCapTren.Value = "Cấp trên trực tiếpp";
            mergedCapTren2.Value = "Ban Giám Hiệu";
            SetRegionBorder(mergedCapTren, false, false, false, true);
            SetRegionBorder(mergedCapTren2, false, false, false, true);
            rowIndex++;
            #endregion
            #region Thời gian thực hiện
            WorksheetMergedCellsRegion mergedThoiGianThucHien = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 1);
            WorksheetMergedCellsRegion mergedThoiGianThucHien2 = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 11);
            mergedThoiGianThucHien.Value = "Thời gian thực hiện";
            mergedThoiGianThucHien2.Value = detailResult.StartPlanTime.ToString("dd/MM/yyyy") + " - " + detailResult.EndPlanTime.ToString("dd/MM/yyyy");
            SetRegionBorder(mergedThoiGianThucHien, false, false, false, true);
            SetRegionBorder(mergedThoiGianThucHien2, false, false, false, true);
            rowIndex = rowIndex + 2;
            #endregion

            #region Nhóm mục tiêu(NMT); Tỷ trọng
            WorksheetMergedCellsRegion mergedThoiNMT = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
            mergedThoiNMT.Value = "Nhóm mục tiêu (NMT)";
            BackgroundColor(sheet, rowIndex, 0, 15, Color.LightGray);
            SetRegionBorder(mergedThoiNMT, true, false, false, true);
            sheet.Rows[rowIndex].Cells[15].Value = "Tỷ trọng";
            SetCellFormat(sheet, rowIndex, 14, 15, true, false, true, true, true);          

            rowIndex++;
            int stt = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                WorksheetMergedCellsRegion mergedThoiNMT1 = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 14);
                mergedThoiNMT1.Value = "NMT " + stt++ + ": " + tg.TargetGroupName;
                sheet.Rows[rowIndex].Cells[15].Value = tg.Density + " %";
                sheet.Rows[rowIndex].Cells[15].CellFormat.Alignment = HorizontalCellAlignment.Center;
                SetCellFormat(sheet, rowIndex, 14, 15, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 0, 15, Color.LightGray);
                SetRegionBorder(mergedThoiNMT1, false, false, false, true);
                rowIndex++;
            }

            rowIndex++;
            #endregion

            sheet.Columns[0].Width = 50 * 30;
            sheet.Columns[1].Width = 50 * 140;
            sheet.Columns[2].Width = 50 * 150;
            sheet.Columns[3].Width = 50 * 100;
            sheet.Columns[4].Width = 50 * 90;
            sheet.Columns[5].Width = 50 * 90;
            sheet.Columns[6].Width = 50 * 120;
            sheet.Columns[7].Width = 100 * 80;
            sheet.Columns[8].Width = 50 * 80;
            sheet.Columns[9].Width = 50 * 70;
            sheet.Columns[10].Width = 50 * 60;
            sheet.Columns[11].Width = 50 * 60;
            sheet.Columns[12].Width = 50 * 60;
            sheet.Columns[13].Width = 50 * 60;
            sheet.Columns[14].Width = 50 * 60;
            sheet.Columns[15].Width = 50 * 60;
            stt = 1;
            foreach (TargetGroupPlanMakingDTO tg in detailResult.TargetGroupPlanMakings)
            {
                #region TargetGroup
                //STT
                WorksheetMergedCellsRegion mergedStt = sheet.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 2, 0);
                mergedStt.Value = "STT";
                BackgroundColor(sheet, rowIndex, 0, 1, Color.LightGray);
                SetRegionBorder(mergedStt, true, false, true, true);
                //NMT #
                sheet.Rows[rowIndex].Cells[1].Value = "NMT # " + stt++ + ":";
                SetCellFormat(sheet, rowIndex, 1, 1, true, false, true, true, true);

                WorksheetMergedCellsRegion mergedGroupName = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex, 15);
                mergedGroupName.Value = tg.TargetGroupName;
                SetRegionBorder(mergedGroupName, true, false, false, true);
                BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);
                rowIndex++;                               

                WorksheetMergedCellsRegion mergedMucTieu = sheet.MergedCellsRegions.Add(rowIndex, 1, rowIndex + 1, 1);
                mergedMucTieu.Value = "Mục tiêu đơn vị";
                SetRegionBorder(mergedMucTieu, true, false, false, true);

                WorksheetMergedCellsRegion mergedKeHoach = sheet.MergedCellsRegions.Add(rowIndex, 2, rowIndex + 1, 2);
                mergedKeHoach.Value = "Kế hoạch/Các bước thực hiện";
                SetRegionBorder(mergedKeHoach, true, false, false, true);

                WorksheetMergedCellsRegion merged3 = sheet.MergedCellsRegions.Add(rowIndex, 3, rowIndex + 1, 3);
                merged3.Value = "Nguồn lực cần có";
                SetRegionBorder(merged3, true, false, false, true);

                WorksheetMergedCellsRegion merged4 = sheet.MergedCellsRegions.Add(rowIndex, 4, rowIndex + 1, 4);
                merged4.Value = "BGH chỉ đạo";
                SetRegionBorder(merged4, true, false, false, true);

                WorksheetMergedCellsRegion merged5 = sheet.MergedCellsRegions.Add(rowIndex, 5, rowIndex + 1, 5);
                merged5.Value = "Chỉ đạo";
                SetRegionBorder(merged5, true, false, false, true);

                WorksheetMergedCellsRegion merged6 = sheet.MergedCellsRegions.Add(rowIndex, 6, rowIndex + 1, 6);
                merged6.Value = "Đơn vị chủ trì";
                SetRegionBorder(merged6, true, false, false, true);

                WorksheetMergedCellsRegion merged7 = sheet.MergedCellsRegions.Add(rowIndex, 7, rowIndex + 1, 7);
                merged7.Value = "Đơn vị phối hợp thực hiện";
                SetRegionBorder(merged7, true, false, false, true);

                WorksheetMergedCellsRegion mergedBoMonThucHien = sheet.MergedCellsRegions.Add(rowIndex, 8, rowIndex, 9);
                mergedBoMonThucHien.Value = "Thực hiện";
                SetRegionBorder(mergedBoMonThucHien, true, false, false, true);

                sheet.Rows[rowIndex + 1].Cells[8].Value = "Bộ môn";
                sheet.Rows[rowIndex + 1].Cells[9].Value = "Trưởng/Phó khoa";

                WorksheetMergedCellsRegion merged10 = sheet.MergedCellsRegions.Add(rowIndex, 10, rowIndex + 1, 10);
                merged10.Value = "KPIs thực hiện năm trước";
                SetRegionBorder(merged10, true, false, false, true);

                WorksheetMergedCellsRegion merged11 = sheet.MergedCellsRegions.Add(rowIndex, 11, rowIndex + 1, 11);
                merged11.Value = "KPI đăng ký của trường";
                SetRegionBorder(merged11, true, false, false, true);

                WorksheetMergedCellsRegion merged12 = sheet.MergedCellsRegions.Add(rowIndex, 12, rowIndex + 1, 12);
                merged12.Value = "KPI đăng ký của đơn vị";
                SetRegionBorder(merged12, true, false, false, true);

                WorksheetMergedCellsRegion merged13 = sheet.MergedCellsRegions.Add(rowIndex, 13, rowIndex + 1, 13);
                merged13.Value = "Đơn vị tính";
                SetRegionBorder(merged13, true, false, false, true);

                WorksheetMergedCellsRegion merged14 = sheet.MergedCellsRegions.Add(rowIndex, 14, rowIndex + 1, 14);
                merged14.Value = "Thời gian bắt đầu";
                SetRegionBorder(merged14, true, false, false, true);

                WorksheetMergedCellsRegion merged15 = sheet.MergedCellsRegions.Add(rowIndex, 15, rowIndex + 1, 15);
                merged15.Value = "Thời gian kết thúc";
                SetRegionBorder(merged15, true, false, false, true);


                SetCellFormat(sheet, rowIndex, 1, 15, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex, 1, 15, Color.LightGray);

                SetCellFormat(sheet, rowIndex + 1, 8, 9, true, false, true, true, true);
                BackgroundColor(sheet, rowIndex + 1, 8, 9, Color.LightGray);


                int stt2 = 1;
                rowIndex = rowIndex + 2;//tính gọp dòng ở cột "thực hiện"
                int beginIndex = rowIndex;
                foreach (PlanKPIMakingDetailDTO pmd in tg.PlanKPIDetails)
                {
                    #region PlanDetail
                    int subRowIndex = rowIndex;
                    List<int> subRowIndexs = new List<int>();
                    int maxSubRowIndex = rowIndex;
                    #region 7 -Đơn vị chủ trì
                    if (pmd.SubDepartmentNames.Count < 1)
                    {
                        SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                    }
                    foreach (string strSD in pmd.SubDepartmentNames)
                    {
                        sheet.Rows[subRowIndex].Cells[7].Value = "- " + strSD;
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 7, 7, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    #endregion
                    #region 8 -Bộ môn
                    if (pmd.SubjectNames.Count < 1)
                    {
                        SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, true, false);
                    }
                    foreach (string strSD in pmd.SubjectNames)
                    {
                        sheet.Rows[subRowIndex].Cells[8].Value = "- " + strSD;
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 8, 8, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    #endregion
                    #region 9 -Trướng/Phó Khoa
                    if (pmd.SubStaffNames.Count < 1)
                    {
                        SetCellFormat(sheet, subRowIndex++,9, 9, false, true, false, true, false);
                    }
                    foreach (string strSD in pmd.SubStaffNames)
                    {
                        sheet.Rows[subRowIndex].Cells[9].Value = "- " + strSD;
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 9, 9, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 9, 9, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    #endregion
                    #region 11 -KPI của trường
                    if (pmd.ParentPlanKPIDetail_KPIs.Count < 1)
                    {
                        SetCellFormat(sheet, subRowIndex++, 11,11, false, true, false, true, false);
                    }
                    foreach (var item in pmd.ParentPlanKPIDetail_KPIs)
                    {
                        sheet.Rows[subRowIndex].Cells[11].Value =  item.Name;
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 11, 11, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 11,11, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    #endregion
                    #region 2 -Kế hoạch/Các bước thực hiện; 14 -TG bắt đầu; 15 -TG kết thúc
                    foreach (MethodDTO me in pmd.Methods)
                    {
                        sheet.Rows[subRowIndex].Cells[2].Value = me.Name;
                        sheet.Rows[subRowIndex].Cells[14].Value = me.StartTimeString;
                        sheet.Rows[subRowIndex].Cells[15].Value = me.EndTimeString;
                        SetCellFormat(sheet, subRowIndex, 14, 14, true, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex, 15, 15, true, false, false, false, false);
                        if (subRowIndex == rowIndex)
                        {
                            SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, true, false);
                        }
                        else
                        {
                            SetCellFormat(sheet, subRowIndex++, 2, 2, false, true, false, false, false);
                        }
                    }
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = rowIndex;
                    #endregion
                    #region  12 -KPI đăng ký của đơn vị; 13 -Đơn vị tính
                    foreach (PlanKPIDetail_KPIDTO kpi in pmd.PlanKPIDetail_KPIs)
                    {
                        sheet.Rows[subRowIndex].Cells[12].Value = kpi.Name;
                        sheet.Rows[subRowIndex].Cells[13].Value = kpi.MeasureUnitName;
                        SetCellFormat(sheet, subRowIndex, 12, 12, false, false, false, false, false);
                        SetCellFormat(sheet, subRowIndex++, 13, 13, false, false, false, false, false);                        
                    }
                    #endregion
                    subRowIndex--;
                    subRowIndexs.Add(subRowIndex);
                    subRowIndex = subRowIndexs.Max();
                    if (subRowIndex < rowIndex)
                        subRowIndex = rowIndex;

                    //0 -STT
                    WorksheetMergedCellsRegion mergedSTT = sheet.MergedCellsRegions.Add(rowIndex, 0, subRowIndex, 0);
                    mergedSTT.Value = stt2++;
                    SetRegionBorder(mergedSTT, true, false, false, false);
                    //1 -Mục tiêu đơn vị
                    WorksheetMergedCellsRegion mergedTargetDetail = sheet.MergedCellsRegions.Add(rowIndex, 1, subRowIndex, 1);
                    mergedTargetDetail.Value = pmd.TargetDetail;
                    SetRegionBorder(mergedTargetDetail, false, true, false, false);
                    //3 -Nguồn lực cần có
                    WorksheetMergedCellsRegion mergedBasicResource = sheet.MergedCellsRegions.Add(rowIndex, 3, subRowIndex, 3);
                    mergedBasicResource.Value = pmd.BasicResource;
                    SetRegionBorder(mergedBasicResource, true, false, false, false);
                    //4 -BGH chỉ đạo
                    WorksheetMergedCellsRegion mergedAdminLeaderName = sheet.MergedCellsRegions.Add(rowIndex, 4, subRowIndex, 4);
                    mergedAdminLeaderName.Value = pmd.AdminLeaderName; 
                    SetRegionBorder(mergedAdminLeaderName, true, false, false, false);
                    //5 -Chỉ đạo
                    WorksheetMergedCellsRegion mergedLeadDepartmentName = sheet.MergedCellsRegions.Add(rowIndex, 5, subRowIndex, 5);
                    mergedLeadDepartmentName.Value = pmd.StaffLeader != null ? pmd.StaffLeader.StaffProfile.Name : "";
                    SetRegionBorder(mergedLeadDepartmentName, true, false, false, false);
                    //6 -Đơn vị chủ trì
                    WorksheetMergedCellsRegion mergedStaffLeader = sheet.MergedCellsRegions.Add(rowIndex, 6, subRowIndex, 6);
                    mergedStaffLeader.Value = pmd.LeadDepartment != null ? pmd.LeadDepartment.Name : "";
                    SetRegionBorder(mergedStaffLeader, true, false, false, false);
                    //10 -KPIs thực hiện năm học trước
                    WorksheetMergedCellsRegion mergedPreviousKPI = sheet.MergedCellsRegions.Add(rowIndex, 10, subRowIndex, 10);
                    mergedPreviousKPI.Value = pmd.PreviousKPI;
                    SetRegionBorder(mergedPreviousKPI, true, false, false, false);

                    rowIndex = subRowIndex + 1;
                    SetCellFormat(sheet, rowIndex, 8, 8, true, false, false, true, false);
                    SetCellFormat(sheet, rowIndex, 9, 9, false, false, false, true, false);
                    SetCellFormat(sheet, rowIndex, 10, 10, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 11, 11, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 12, 12, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 13, 13, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 14, 14, true, true, false, true, false);
                    SetCellFormat(sheet, rowIndex, 15, 15, true, true, false, true, false);
                    #endregion
                }
                for (int i = beginIndex; i < rowIndex; i++)
                {
                    SetCellFormat(sheet, i, 8, 8, true, false, false, false, false);
                    SetCellFormat(sheet, i, 9, 9, false, false, false, false, false);
                    SetCellFormat(sheet, i, 10, 10, true, false, false, false, false);
                    SetCellFormat(sheet, i, 11, 11, true, false, false, false, false);
                    SetCellFormat(sheet, i, 12,12, true, false, false, false, false);
                    SetCellFormat(sheet, i, 13, 13, false, false, false, false, false);
                    SetCellFormat(sheet, i, 14, 14, true, false, false, false, false);
                    SetCellFormat(sheet, i, 15, 15, true, false, false, false, false);
                }
                SetCellFormat(sheet, rowIndex - 1, 2, 2, false, false, false, false, true);
                SetCellFormat(sheet, rowIndex - 1, 6, 6, false, false, false, false, true);

                //rowIndex++;
                #endregion
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
        #endregion
    }
}