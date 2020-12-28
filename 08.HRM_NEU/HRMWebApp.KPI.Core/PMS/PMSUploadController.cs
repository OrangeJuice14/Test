using DevExpress.Spreadsheet;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace HRMWebApp.KPI.Core.PMS
{
    public class PMSUploadController : Controller
    {
        public ActionResult Import(IEnumerable<HttpPostedFileBase> files, Guid NamHoc, Guid HocKy, string username)
        {
            int result = 0;

            DateTime dateNow = DateTime.Now;
            string filePath = "~/Files/" + dateNow.Year + "/ImportFile/";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(Server.MapPath(filePath));
            var physicalPathDelete = Path.Combine(Server.MapPath(filePath));

            string[] filesDelete = Directory.GetFiles(physicalPathDelete);
            foreach (string file in filesDelete)
            {
                System.IO.File.Delete(file);
            }

            var fileName = dateNow.ToString("yyyyyMMddHHmmss");// Path.GetFileNameWithoutExtension(file.FileName);
            string extensionFile = "";
            if (files != null && files.Count() > 0)
            {
                HttpPostedFileBase file = files.FirstOrDefault();
                if (file != null)
                {
                    FileInfo finfo = new FileInfo(file.FileName);
                    extensionFile = Path.GetExtension(file.FileName);
                    if (finfo.Extension == ".xlsx" || finfo.Extension == ".xls")
                    {
                        var actualFileName = string.Format("{0}{1}", fileName, extensionFile);
                        var physicalPath = Path.Combine(Server.MapPath(filePath), actualFileName);
                        //Save file xuống server
                        file.SaveAs(physicalPath);

                        result = ImportData(physicalPath, NamHoc, HocKy, username);
                    }
                }
            }
            if (result == 1)
                return Content(filePath.Replace("~", "") + fileName + extensionFile);
            return Content(result.ToString());
        }

        private int ImportData(string filePath, Guid _NamHoc, Guid _HocKy, string _username)
        {
            try
            {
                Workbook file = new Workbook();
                file.LoadDocument(filePath);
                int sheetIndex = 0;
                int rowIndex = 6;
                string sql = string.Empty;
                while (file.Worksheets[sheetIndex].Cells["B" + rowIndex].Value.ToString().Trim() != "")
                {
                    var nhanVien = CheckImport(file, sheetIndex, rowIndex);
                    if (nhanVien != null)
                    {
                        string error = "";
                        string OidBoMonQuanLy = nhanVien.BoMonQuanLy;
                        string MaDV = file.Worksheets[sheetIndex].Cells["B" + rowIndex.ToString()].Value.ToString().Trim(); // check 
                        bool errorMaDV = false;
                        SessionManager.DoWorkNoTransaction(session =>
                        {
                            errorMaDV = session.Query<Department>().SingleOrDefault(e => e.ManageCode == MaDV && e.GCRecord == null) == null;
                        });
                        if (errorMaDV)
                        {
                            if (error != "")
                                error += "\n";
                            error += "B" + rowIndex + ": không tồn tại trong cơ sở dữ liệu.";
                        }

                        string MaMonHoc = file.Worksheets[sheetIndex].Cells["D" + rowIndex.ToString()].Value.ToString().Trim();
                        string TenMonHoc = file.Worksheets[sheetIndex].Cells["E" + rowIndex.ToString()].Value.ToString().Trim();
                        string MaLopHocPhan = file.Worksheets[sheetIndex].Cells["F" + rowIndex.ToString()].Value.ToString().Trim();
                        string TenLopHocPhan = file.Worksheets[sheetIndex].Cells["G" + rowIndex.ToString()].Value.ToString().Trim();
                        string LoaiHocPhan = file.Worksheets[sheetIndex].Cells["H" + rowIndex.ToString()].Value.ToString().Trim();
                        string TenBacDaoTao = file.Worksheets[sheetIndex].Cells["I" + rowIndex.ToString()].Value.ToString().Trim();
                        string TenHeDaoTao = file.Worksheets[sheetIndex].Cells["J" + rowIndex.ToString()].Value.ToString().Trim();
                        string SoTinChi = file.Worksheets[sheetIndex].Cells["K" + rowIndex.ToString()].Value.ToString().Trim(); // check number
                        if (!HRMWebApp.Helpers.Helper.IsNumber(SoTinChi))
                        {
                            if (error != "")
                                error += "\n";
                            error += "K" + rowIndex + ": phải là số.";
                        }

                        string SoTietDungLop = file.Worksheets[sheetIndex].Cells["L" + rowIndex.ToString()].Value.ToString().Trim(); // check number
                        if (!HRMWebApp.Helpers.Helper.IsNumber(SoTietDungLop))
                        {
                            if (error != "")
                                error += "\n";
                            error += "L" + rowIndex + ": phải là số.";
                        }

                        string SoTietHeThong = file.Worksheets[sheetIndex].Cells["M" + rowIndex.ToString()].Value.ToString().Trim(); // check number
                        if (!HRMWebApp.Helpers.Helper.IsNumber(SoTietHeThong))
                        {
                            if (error != "")
                                error += "\n";
                            error += "M" + rowIndex + ": phải là số";
                        }

                        string SoSinhVienDK = file.Worksheets[sheetIndex].Cells["N" + rowIndex.ToString()].Value.ToString().Trim(); // check number
                        if (!HRMWebApp.Helpers.Helper.IsNumber(SoSinhVienDK))
                        {
                            if (error != "")
                                error += "\n";
                            error += "N" + rowIndex + ": phải là số.";
                        }

                        string ThoiGianDiaDiem = file.Worksheets[sheetIndex].Cells["O" + rowIndex.ToString()].Value.ToString().Trim();
                        string CoSoGiangDay = file.Worksheets[sheetIndex].Cells["P" + rowIndex.ToString()].Value.ToString().Trim();
                        string PhongHoc = file.Worksheets[sheetIndex].Cells["Q" + rowIndex.ToString()].Value.ToString().Trim();
                        string ApDungHeSoNgoaiGio = file.Worksheets[sheetIndex].Cells["R" + rowIndex.ToString()].Value.ToString().Trim(); // check
                        if (ApDungHeSoNgoaiGio == "" || (ApDungHeSoNgoaiGio.ToUpper() != "TRUE" && ApDungHeSoNgoaiGio.ToUpper() != "FALSE"))
                        {
                            if (error != "")
                                error += "\n";
                            error += "R" + rowIndex + ": phải là TRUE/FALSE.";
                        }

                        string ApDungHeSoTiengNuocNgoai = file.Worksheets[sheetIndex].Cells["S" + rowIndex.ToString()].Value.ToString().Trim();// check
                        if (ApDungHeSoTiengNuocNgoai == "" || (ApDungHeSoNgoaiGio.ToUpper() != "TRUE" && ApDungHeSoNgoaiGio.ToUpper() != "FALSE"))
                        {
                            if (error != "")
                                error += "\n";
                            error += "S" + rowIndex + ": phải là TRUE/FALSE.";
                        }

                        string KhoaDaoTao = file.Worksheets[sheetIndex].Cells["T" + rowIndex.ToString()].Value.ToString().Trim();

                        string OidNhanVien = nhanVien.NhanVien;
                        if (OidNhanVien == "")
                            OidNhanVien = Guid.Empty.ToString();
                        string GhiChu = file.Worksheets[sheetIndex].Cells["W" + rowIndex.ToString()].Value.ToString().Trim();

                        if (error == "")
                        {
                            sql += " Union All select N'" + OidBoMonQuanLy + "' as OidBoMonQuanLy"

                                + ", N'" + MaMonHoc + "' as MaMonHoc"
                                + ", N'" + TenMonHoc + "' as TenMonHoc"
                                + ", N'" + MaLopHocPhan + "' as MaLopHocPhan"
                                + ", N'" + TenLopHocPhan + "' as TenLopHocPhan"
                                + ", N'" + LoaiHocPhan + "' as LoaiHocPhan"
                                + ", N'" + TenBacDaoTao + "' as TenBacDaoTao"
                                + ", N'" + TenHeDaoTao + "' as TenHeDaoTao"
                                + ", N'" + SoTinChi + "' as SoTinChi"

                                + ", N'" + SoTietDungLop + "' as SoTietDungLop"
                                + ", N'" + SoTietHeThong + "' as SoTietHeThong"

                                + ", N'" + SoSinhVienDK + "' as SoSinhVienDK"
                                + ", N'" + ThoiGianDiaDiem + "' as ThoiGianDiaDiem"
                                + ", N'" + CoSoGiangDay + "' as CoSoGiangDay"

                                + ", N'" + PhongHoc + "' as PhongHoc"

                                + ", N'" + ApDungHeSoNgoaiGio + "' as ApDungHeSoNgoaiGio"
                                + ", N'" + ApDungHeSoTiengNuocNgoai + "' as ApDungHeSoTiengNuocNgoai"

                                + ", N'" + KhoaDaoTao + "' as KhoaDaoTao"

                                + ", N'" + OidNhanVien + "' as OidNhanVien"

                                + ", N'" + GhiChu + "' as GhiChu";
                            file.Worksheets[sheetIndex].Range["X" + rowIndex].SetValue("Thành công");
                        }
                        else
                        {
                            file.Worksheets[sheetIndex].Range["X" + rowIndex].SetValue("Lỗi!");
                            file.Worksheets[sheetIndex].Range["Y" + rowIndex].SetValue(error);
                        }
                    }
                    rowIndex++;
                }
                file.Worksheets[sheetIndex].Range["Z" + rowIndex].SetValue("Kết thúc");
                //Save file xuống server
                file.SaveDocument(filePath);

                //System.IO.File.Delete(filePath);
                if (sql != "")
                {
                    DataClassHelper.spd_PMS_Import_TBK_WEB(_NamHoc, _HocKy, sql.Substring(11), _username);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                HRMWebApp.Helpers.Helper.ErrorLog("PMSUploadController/ImportData", ex);
                throw ex;
            }
        }

        private spd_PMS_KiemTraThongTin_ImportTKB CheckImport(Workbook file, int sheetIndex, int rowIndex)
        {
            return DataClassHelper.spd_PMS_KiemTraThongTin_ImportTKB(
                file.Worksheets[sheetIndex].Cells["V" + rowIndex.ToString()].Value.ToString().Trim(),
                file.Worksheets[sheetIndex].Cells["B" + rowIndex.ToString()].Value.ToString().Trim(),
                file.Worksheets[sheetIndex].Cells["C" + rowIndex.ToString()].Value.ToString().Trim());
        }
        public int DeleteFile(string url)
        {
            int result = 0;
            try
            {

                var physicalPath = Path.Combine(Server.MapPath("~" + url));
                System.IO.File.Delete(physicalPath);
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                throw ex;
            }
            return result;
        }
    }
}
