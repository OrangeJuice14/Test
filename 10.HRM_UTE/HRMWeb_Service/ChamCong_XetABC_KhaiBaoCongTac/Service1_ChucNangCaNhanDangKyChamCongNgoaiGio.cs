using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Web;
using ERP_Core.Common;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {


        public IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> CaNhanDangKyChamCongNgoaiGio_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                var tmpList = factory.CaNhanDangKyChamCongNgoaiGio_Find(thang, nam, idNhanVien).ToList();
                IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_DangKyChamCongNgoaiGio>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> QuanLyDangKyChamCongNgoaiGio_Find(String publicKey, String token, int? ngay, int thang, int nam, Guid IDBoPhan, byte? trangthai)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                var tmpList = factory.QuanLyDangKyChamCongNgoaiGio_Find(ngay, thang, nam, IDBoPhan, trangthai).ToList();
                IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_DangKyChamCongNgoaiGio>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String CaNhanDangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> list = CaNhanDangKyChamCongNgoaiGio_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyDangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, int? ngay, int thang, int nam, Guid IDBoPhan, byte? trangthai)
        {//DANG SD
            IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> list = QuanLyDangKyChamCongNgoaiGio_Find(publicKey, token, ngay, thang, nam, IDBoPhan, trangthai);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool Save_DangKyChamCongNgoaiGio_Json(String publicKey, String token, string jsonObject)
        {//DANG SD
            //chuyen jsonObject thanh object
            var obj = JsonConvert.DeserializeObject<DTO_CC_DangKyChamCongNgoaiGio>(jsonObject);
            return Save_DangKyChamCongNgoaiGio(publicKey, token, obj);
        }

        public bool Save_DangKyChamCongNgoaiGio(String publicKey, String token, DTO_CC_DangKyChamCongNgoaiGio obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    try
                    {
                        var factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                        var nv = (from o in factory.Context.ThongTinNhanViens
                                  where o.Oid == new Guid(obj.ThongTinNhanVien)
                                  select o).SingleOrDefault();
                        DTO_CC_CauHinhChamCong cauHinhChamCong = (CC_CauHinhChamCong_Factory.New()).GetCauHinhCauCong();
                        if (cauHinhChamCong == null) return false;
                        //var lydo = (from o in factory.Context.CC_LyDoDangKyChamCongNgoaiGio
                        //          where o.Oid == new Guid(obj.LyDo)
                        //          select o).SingleOrDefault();
                        List<int> ListThu = new List<int>();
                        foreach (DTO_Thu t in obj.DanhSachDTO_Thu)
                        {
                            if (t.Chon == true)
                            {
                                ListThu.Add(t.Id);
                            }
                        }
                        DateTime date = obj.TuNgay;
                        while (date <= obj.DenNgay)
                        {
                            byte Thu = Convert.ToByte(date.DayOfWeek);
                            foreach (int t in ListThu)
                            {
                                if (Thu == t)
                                {
                                    CC_DangKyChamCongNgoaiGio objForSave = null;
                                    var newDBObject = factory.CreateManagedObject();
                                    newDBObject.Oid = Guid.NewGuid();
                                    newDBObject.IDNhanVien = new Guid(obj.ThongTinNhanVien);
                                    newDBObject.ThongTinNhanVien = nv;
                                    newDBObject.Ngay = date;
                                    newDBObject.LyDo = obj.LyDo;
                                    newDBObject.IDBoPhan = nv.NhanVien.BoPhan1.Oid;
                                    newDBObject.Duyet = 0;
                                    newDBObject.SoPhutDangKy = (ParseTime(obj.GioKetThuc, obj.PhutKetThuc) - ParseTime(obj.GioBatDau, obj.PhutBatDau)) * 60;
                                    newDBObject.TuGio = ParseTimeString(obj.GioBatDau, obj.PhutBatDau);
                                    newDBObject.DenGio = ParseTimeString(obj.GioKetThuc, obj.PhutKetThuc);
                                    newDBObject.DonGiaNgoaiGio = cauHinhChamCong.DonGiaNgoaiGio;
                                    //Tiến hành trừ giờ nghỉ trưa nếu t7 or cn
                                    if (Thu == 6 || Thu == 0)
                                    {
                                        //
                                        if( (obj.GioBatDau <= 13)
                                            && newDBObject.SoPhutDangKy > 90)
                                        {
                                            newDBObject.SoPhutDangKy = newDBObject.SoPhutDangKy - 90 > 0 ? newDBObject.SoPhutDangKy - 90 : 0;
                                        }
                                    }
                                    objForSave = newDBObject;
                                    factory.SaveChanges();

                                }
                            }
                            date = date.AddDays(1.0);
                        }
                        return true;

                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
            //
        }
        public bool DangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {

                            CC_DangKyChamCongNgoaiGio stupidObj = new CC_DangKyChamCongNgoaiGio() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_DangKyChamCongNgoaiGio_Factory.FullDelete(factory.Context, stupidObj);
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return false;
            }
            return false;
        }

        public bool CaNhanDangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList)
        {//ca nhan chi duoc phep xoa nhung dong dang cho xet
            CC_DangKyChamCongNgoaiGio_Factory tmpFactory = CC_DangKyChamCongNgoaiGio_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                TimeSpan.FromSeconds(360)))
            {
                bool daXoaDuocItNhat1Dong = false;
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    if (objFromDB.Duyet == 0)
                    {
                        bool xoaDuocDongHienTai = DangKyChamCongNgoaiGio_DeleteList(publicKey, token,
                              new List<DTO_CC_DangKyChamCongNgoaiGio>() { obj.Map<DTO_CC_DangKyChamCongNgoaiGio>() });
                        if (daXoaDuocItNhat1Dong == false && xoaDuocDongHienTai == true)
                        {
                            daXoaDuocItNhat1Dong = true;
                        }
                    }
                }
                transaction.Complete();
            }
            return false;
        }
        public bool QuanLyDangKyChamCongNgoaiGio_DuyetList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList, byte trangthai, bool isadmin)
        {
            CC_DangKyChamCongNgoaiGio_Factory tmpFactory = CC_DangKyChamCongNgoaiGio_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                TimeSpan.FromSeconds(360)))
            {
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    if (!isadmin)
                    {
                        objFromDB.Duyet = trangthai;
                    }
                    if (isadmin)
                    {
                        objFromDB.DuyetAdmin = trangthai;
                    }
                    objFromDB.SoPhutThucTe = 0;
                    objFromDB.TrangThai = string.Empty;
                    objFromDB.DaChamCong = false;
                    tmpFactory.SaveChangesWithoutTransactionScope();
                }
                transaction.Complete();
            }
            return false;
        }
        public bool CaNhanDangKyChamCongNgoaiGio_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_DangKyChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_DangKyChamCongNgoaiGio>>(jsonObjectList);
            return CaNhanDangKyChamCongNgoaiGio_DeleteList(publicKey, token, objList);
        }
        public bool QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(String publicKey, String token, string jsonObjectList, byte trangthai, bool isadmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_DangKyChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_DangKyChamCongNgoaiGio>>(jsonObjectList);
            return QuanLyDangKyChamCongNgoaiGio_DuyetList(publicKey, token, objList, trangthai, isadmin);
        }
        public string ChamCongNgoaiGio_XuatMauDangKy(string publicKey, string token, int thang, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                //
                List<DTO_CC_DangKyChamCongNgoaiGio> objList = factory.GetDangKyChamCongNgoaiGioByThangNam(thang, nam);
                //
                try
                {
                    var pathfull = string.Empty;
                    if (objList.Count > 0)
                    {
                        //
                        pathfull = ExportDangKyNgoaiGioToExcel(objList, thang, nam);
                    }
                    return pathfull;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public string ChamCongNgoaiGio_XuatMauTongHop(string publicKey, string token, int thang, int nam,Guid bophan)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                //
                List<DTO_CC_DangKyChamCongNgoaiGio > objList = factory.Context.spd_WebChamCong_BangTongHopCongNgoaiGio(thang, nam,bophan).Map<DTO_CC_DangKyChamCongNgoaiGio>().ToList();
                //
                try
                {
                    var pathfull = string.Empty;
                    if (objList.Count  > 0)
                    {
                        //
                        pathfull = ExportTongHopNgoaiGioToExcel(objList, thang, nam);
                    }
                    return pathfull;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        private string ExportDangKyNgoaiGioToExcel(List<DTO_CC_DangKyChamCongNgoaiGio> objList, int thang, int nam)
        {
            string path = string.Empty;
            try
            {
                //
                Excel.Application excelApp = new Excel.Application();
                string fileExelMau = "/Temp/Excel/MauDangKyNgoaiGio.xls";
                fileExelMau = HttpContext.Current.Server.MapPath(fileExelMau);
                //
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(fileExelMau, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
                if (excelWorkSheet == null)
                    return string.Empty;

                //Phần cố định
                if (DateTime.Now.Month < 10)
                {
                    excelWorkSheet.Cells[3, 4] = String.Format("TP. Hồ Chí Minh, ngày {0}  tháng 0{1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
                }
                else
                {
                    excelWorkSheet.Cells[3, 4] = String.Format("TP. Hồ Chí Minh, ngày {0}  tháng {1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
                }
                //
                excelWorkSheet.Cells[6, 1] = "Tháng " + thang.ToString() + " năm " + nam.ToString();
                excelWorkSheet.Cells[7, 2] = objList[0].HoTen;
                excelWorkSheet.Cells[8, 2] = objList[0].BoPhan;
                //

                //Phần danh sách động
                decimal tongTien = 0;
                for (int j = 0; j < objList.Count; j++)
                {
                    //
                    excelWorkSheet.Cells[j + 11, 1] = String.Format("{0:dd/MM/yyyy}", objList[j].Ngay);
                    excelWorkSheet.Cells[j + 11, 2] = objList[j].LyDo;
                    excelWorkSheet.Cells[j + 11, 3] = objList[j].ThoiGianBatDau;
                    excelWorkSheet.Cells[j + 11, 4] = objList[j].ThoiGianKetThuc;
                    excelWorkSheet.Cells[j + 11, 5] = String.Format("{0:0}", objList[j].SoPhutDangKy);
                    excelWorkSheet.Cells[j + 11, 6] = String.Format("{0:0}", objList[j].SoPhutThucTe);
                    excelWorkSheet.Cells[j + 11, 7] = String.Format("{0:0}", objList[j].ThanhTien);
                    tongTien += objList[j].ThanhTien;
                }
                //Footer
                excelWorkSheet.Cells[objList.Count + 12, 1] = "Bằng chữ: " + HamDungChung.DocTien(tongTien);
                //
                excelWorkSheet.Cells[objList.Count + 14, 2] = "Người kiểm tra";
                excelWorkSheet.Cells[objList.Count + 14, 2].Font.Bold = true;
                //
                excelWorkSheet.Cells[objList.Count + 14, 4] = "Người duyệt";
                excelWorkSheet.Cells[objList.Count + 14, 4].Font.Bold = true;
                //
                excelWorkSheet.Cells[objList.Count + 14, 6] = "Người làm thêm giờ";
                excelWorkSheet.Cells[objList.Count + 14, 6].Font.Bold = true;
                //
                excelWorkSheet.Cells[objList.Count + 15, 2] = "P. Tổ chức Cán bộ";

                excelWorkSheet.Cells[objList.Count + 15, 4] = "(Trưởng đơn vị)";

                //
                string folder = @"\ExportExcel\";
                folder = HttpContext.Current.Server.MapPath(folder);
                if (!System.IO.Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);
                //
                path = @"\ExportExcel" + string.Format(@"\MauDangKyNgoaiGio_{0}_{1}_{2}_{3}.xls", DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Minute.ToString());
                //
                excelWorkBook.SaveAs(HttpContext.Current.Server.MapPath(path), Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
                //
                excelWorkBook.Close();
                excelApp.Quit();
                //
            }
            catch { path = "Loi"; }
            //
            return path;
        }

        private string ExportTongHopNgoaiGioToExcel(List<DTO_CC_DangKyChamCongNgoaiGio> objList, int thang, int nam)
        {
            //
            Excel.Application excelApp = new Excel.Application();
            string fileExelMau = "/Temp/Excel/MauTongHopNgoaiGio.xls";
            fileExelMau = HttpContext.Current.Server.MapPath(fileExelMau);
            //
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(fileExelMau, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets["Sheet1"];
            if (excelWorkSheet == null)
                return string.Empty;

            //Phần cố định
            excelWorkSheet.Cells[8, 2] = objList[0].BoPhan;
            excelWorkSheet.Cells[6, 1] = "Tháng " + thang.ToString() + " năm " + nam.ToString();

            //Phần danh sách động
            decimal tongTien = 0;
            for (int j = 0; j < objList.Count; j++)
            {
                //
                excelWorkSheet.Cells[j + 11, 1] = j;  
                excelWorkSheet.Cells[j + 11, 2] = objList[j].HoTen;
                excelWorkSheet.Cells[j + 11, 3] = String.Format("{0:0}", objList[j].SoCongNgoaiGio);
                excelWorkSheet.Cells[j + 11, 4] = String.Format("{0:0}", objList[j].DonGia);
                excelWorkSheet.Cells[j + 11, 5] = String.Format("{0:0}", objList[j].ThanhTien);
                tongTien += objList[j].ThanhTien;
            }
            //Footer
            excelWorkSheet.Cells[objList.Count + 12, 1] = "Bằng chữ: " + HamDungChung.DocTien(tongTien);

            //
            if (DateTime.Now.Month < 10)
            {
                excelWorkSheet.Cells[objList.Count + 14, 6] = String.Format("Ngày {0}  tháng 0{1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
            }
            else
            {
                excelWorkSheet.Cells[objList.Count + 14, 6] = String.Format("Ngày {0}  tháng {1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
            }
            //
            excelWorkSheet.Cells[objList.Count + 15, 6] = "Trưởng đơn vị";
            excelWorkSheet.Cells[objList.Count + 15, 6].Font.Bold = true;
            //

            //
            string folder = @"\ExportExcel\";
            folder = HttpContext.Current.Server.MapPath(folder);
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            //
            string path = @"\ExportExcel" + string.Format(@"\MauTongHopNgoaiGio_{0}_{1}_{2}_{3}.xls", DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Minute.ToString());
            //
            excelWorkBook.SaveAs(HttpContext.Current.Server.MapPath(path), Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
            //
            excelWorkBook.Close();
            excelApp.Quit();
            //
            return path;
        }
    }
}
