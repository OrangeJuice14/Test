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
using System.Web.Configuration;
using HRMWeb_Business.Predefined;
using Aspose.Words;
using System.IO;
using System.Web;
using System.Diagnostics;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public String ChamCongNgayNghi_Report_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_Report(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                DTO_ChamCongNgayNghi_Find obj = null;
                obj = factory.ChamCongNgayNghi_Report(id);
                obj.TuNgayString = String.Format("{0:dd/MM/yyyy}", obj.TuNgay);
                obj.DenNgayString = String.Format("{0:dd/MM/yyyy}", obj.DenNgay);
                obj.NgaySinhString = String.Format("{0:dd/MM/yyyy}", obj.NgaySinh);
                if (DateTime.Now.Month < 10)
                {
                    obj.NgayBaoCaoString = String.Format("ngày {0}  tháng 0{1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
                }
                else
                {
                    obj.NgayBaoCaoString = String.Format("ngày {0}  tháng {1} năm {2}", DateTime.Now.Date.Day, DateTime.Now.Month, DateTime.Now.Year);
                }
                    return obj;
            }
            else
            {
                return null;
            }
        }

        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                DTO_ChamCongNgayNghi_Find obj = null;
                obj = factory.GetDTO_ChamCongNgayNghi_Find_ByID(id);
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ChamCongNgayNghi_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }


        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find(String publicKey, String token, int thang, int nam, string manhansu, Guid idBoPhan, Guid idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.FindForChamCongNgayNghi(thang, nam, manhansu, idBoPhan, idLoaiNhanSu);
                var ccfact = CC_ChiTietChamCongNhanVien_Factory.New();           
                foreach (DTO_ChamCongNgayNghi_Find cc in list)
                {
                    try
                    {
                        bool daTonTai = ccfact.CheckChot(thang, nam, cc.IDBoPhan ?? Guid.Empty);
                        if (daTonTai)
                        {
                            cc.DaChotChamCong = "Đã chốt chấm công";
                        }
                    }
                   catch (Exception ex) { throw ex; }
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_ChamCongNgayNghi_Find> QuanLyNghiPhep_Find(String publicKey, String token, int thang, int nam, int trangthai, string maNhanSu, Guid idBoPhan, Guid idLoaiNhanSu, string idwebgroup)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.QuanLyNghiPhep_Find(thang, nam, trangthai, maNhanSu, idBoPhan, idLoaiNhanSu, idwebgroup);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String ChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, string manhansu, Guid idBoPhan, Guid idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = ChamCongNgayNghi_Find(publicKey, token, thang, nam, manhansu, idBoPhan, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyNghiPhep_Find_Json(String publicKey, String token, int thang, int nam,int trangthai , string maNhanSu, Guid idBoPhan, Guid idLoaiNhanSu, string idwebgroup)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = QuanLyNghiPhep_Find(publicKey, token, thang, nam, trangthai, maNhanSu, idBoPhan, idLoaiNhanSu, idwebgroup);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyNghiPhep_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = DangKyNghiPhep_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyNghiPhep_SoNgayPhepConLai_Json(String publicKey, String token, int nam, Guid idNhanVien)
        {
            decimal soNgayPhepConLai = DangKyNghiPhep_SoNgayPhepConLai(publicKey, token,nam, idNhanVien);
            String json = JsonConvert.SerializeObject(soNgayPhepConLai);
            return json;
        }
        public decimal DangKyNghiPhep_SoNgayPhepConLai(String publicKey, String token, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                decimal soNgayPhepConLai  = factory.DangKyNghiPhep_SoNgayPhepConLai(nam, idNhanVien);
                return soNgayPhepConLai;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<DTO_ChamCongNgayNghi_Find> DangKyNghiPhep_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.DangKyNghiPhep_Find(thang, nam, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }
        public int GetBusinessDays(DateTime fromDate, DateTime toDate)
        {
            int result = 0;
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday
                    && date.DayOfWeek != DayOfWeek.Saturday)
                    result++;
            }
            return result;
        }
        public bool ChamCongNgayNghi_AcceptList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.CreateAloneObject();
                            objFromDB = factory.GetByID(obj.Oid);
                            if (isAdmin == 1)
                                objFromDB.TrangThaiAdmin = 1;
                            if (isAdmin == 0)
                                objFromDB.TrangThai = 1;
                            if (objFromDB.TrangThai == 1 && objFromDB.TrangThaiAdmin == 1)
                            {
                              //
                              factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                            }
                        }
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
                }
            }
            return false;
        }

        public bool ChamCongNgayNghi_CancelList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, bool isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.CreateAloneObject();
                            objFromDB = factory.GetByID(obj.Oid);
                            if (isAdmin == true)
                                objFromDB.TrangThaiAdmin = 0;
                            if (isAdmin == false)
                                objFromDB.TrangThai = 0;
                            //
                            if (objFromDB.TrangThaiAdmin == 0)
                            {
                                //
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, false);
                            }
                        }
                    }
                    //
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
            }
            return false;
        }
        public bool DangKyNghiPhep_AcceptList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    QuanLyNghiPhep_Factory qlfac = QuanLyNghiPhep_Factory.New();

                    foreach (var obj in objList)
                    {
                        if (obj != null && obj.DaChotChamCong == null)
                        {
                            CC_ChamCongNgayNghi objFromDB  = factory.GetByID(obj.Oid);
                            //
                            if (isAdmin == 1)
                                objFromDB.TrangThaiAdmin = 1;
                            if (isAdmin == 0)
                                objFromDB.TrangThai = 1;
                            if (isAdmin == 2)
                                objFromDB.TrangThaiBGH = 1;
                            if (objFromDB.TrangThaiAdmin == 1 && objFromDB.TrangThaiBGH == 1)
                            {
                                //Cập nhật chấm công theo ngày
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.CC_HinhThucNghi, true);

                                    /*
                                    //Cập nhật vào Chi tiết nghỉ phép
                                    //Nếu chưa có QLNP thì tạo
                                    CC_QuanLyNghiPhep quanLy = qlfac.GetByNam(objFromDB.TuNgay.Value.Year);
                                    if (quanLy == null)
                                    {
                                        //tao quan ly moi
                                        quanLy = qlfac.CreateManagedObject();
                                        quanLy.Oid = Guid.NewGuid();
                                        quanLy.Nam = objFromDB.TuNgay.Value.Year;
                                    }
                                    qlfac.SaveChanges();
                                    //Kiểm tra đã có Chi tiết nghỉ phép chưa
                                    CC_ChiTietNghiPhep ct = qlfac.GetChiTietNghiPhepByNamAndIDNV(objFromDB.TuNgay.Value.Year, objFromDB.IDNhanVien);

                                    //nếu có rồi thì update
                                    if (ct!=null)
                                    {
                                        //factory.Context.spd_WebChamCong_QuanLyNghiPhep_ChamCongNgayNghi_UpdateChiTietNghiPhep(objFromDB.IDNhanVien, quanLy.Oid,ct.Oid, objFromDB.SoNgay,true);
                                    }
                                    //nếu chưa có thì thêm mới
                                    else
                                    {
                                        //factory.Context.spd_WebChamCong_QuanLyNghiPhep_ChamCongNgayNghi_UpdateChiTietNghiPhep(objFromDB.IDNhanVien, quanLy.Oid,null, objFromDB.SoNgay,false);
                                    }
                                    */
                            }
                        }
                    }
                    try
                    {
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch(Exception ex) { throw ex; }
                }
            }
            return false;
        }

        public bool DangKyNghiPhep_CancelList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            //
                            if (isAdmin == 1)
                                objFromDB.TrangThaiAdmin = 0;
                            if (isAdmin == 0)
                                objFromDB.TrangThai = 0;
                            if (isAdmin == 2)
                                objFromDB.TrangThaiBGH = 0;
                            //
                            if (objFromDB.TrangThaiBGH == 0)
                            {
                                //
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, false);
                            }
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
            }
            return false;
        }
        public bool ChamCongNgayNghi_AcceptList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_AcceptList(publicKey, token, objList, isAdmin);
        }
        public bool ChamCongNgayNghi_CancelList_Json(String publicKey, String token, string jsonObjectList, bool isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_CancelList(publicKey, token, objList, isAdmin);
        }
        public bool DangKyNghiPhep_AcceptList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyNghiPhep_AcceptList(publicKey, token, objList, isAdmin);
        }
        public bool DangKyNghiPhep_CancelList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyNghiPhep_CancelList(publicKey, token, objList, isAdmin);
        }
        public String DangKyChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = DangKyChamCongNgayNghi_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.DangKyChamCongNgayNghi_Find(thang, nam, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool ChamCongNgayNghi_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String noiNghiPhep, String tenDonXinNghi, String diaChiLienHe, Guid idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId, int isAdmin)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    HoSo_Factory hoSo_Factory = HoSo_Factory.New();
                    HoSo hoSo = hoSo_Factory.GetByID(nhanVienID);
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    CC_ChamCongNgayNghi newObj = factory.CreateManagedObject();
                    newObj.Oid = Guid.NewGuid();
                    newObj.TuNgay = tuNgay.Date;
                    newObj.DenNgay = denNgay.Date;
                    newObj.DienGiai = noiDung.Trim();
                    newObj.IDBoPhan = hoSo.NhanVien.BoPhan;
                    newObj.IDNhanVien = nhanVienID;
                    newObj.CC_HinhThucNghi = idHinhThucNghi;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Today;
                    newObj.LoaiNghiPhep = loaiDonXinNghi;
                    newObj.NoiNghi = noiNghiPhep;
                    newObj.DiaChiLienHe = diaChiLienHe;
                    newObj.TrangThai = -1;
                    newObj.TrangThaiAdmin = -1;
                    newObj.SoNgay = GetBusinessDays(tuNgay, denNgay);
                    newObj.TenGiayXinPhep = tenDonXinNghi;
                    try
                    {
                        if (isAdmin==1)
                        {
                            newObj.TrangThai = 1;
                            newObj.TrangThaiAdmin = 1;
                            newObj.TrangThaiBGH = 1;
                            factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, idHinhThucNghi, true);
                        }
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }

        public string ChamCongNgayNghi_XuatMauNghiPhep(string publicKey, string token, Guid oid)
        {
            //
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_Report(publicKey, token, oid);
            if (obj == null) return "";
            //
            try
            {
                Document doc = new Document(System.Web.HttpContext.Current.Server.MapPath("/ExportWord/InMauNghiPhep.docx"));
                //
                string[] objInWord = {
                "HoTen",
                "ChucVu",
                "TenPhongBan",
                "NamNghiPhep",
                "TuNgayString",
                "DenNgayString",
                "NoiNghiPhep",
                "DienGiai",
                "DanhXung",
                "NgayBaoCao"
            };
                //
                object[] objBiding =
                {
                obj.HoTen,
                obj.ChucVu,
                obj.TenPhongBan,
                obj.NamNghiPhep,
                obj.TuNgayString,
                obj.DenNgayString,
                obj.NoiNghiPhep,
                obj.DienGiai,
                obj.DanhXung,
                obj.NgayBaoCaoString
            };
                //
                doc.MailMerge.Execute(objInWord, objBiding);

                //
                var timeCurrent = DateTime.Now;
                string foldername = timeCurrent.Day + "_" + timeCurrent.Month + "_" + timeCurrent.Year;
                string fullPath = System.Web.HttpContext.Current.Server.MapPath("/ExportWord/" + foldername + "/");
                string fullname = fullPath + "InMauNghiPhep.docx";
                //
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                //Lưu file
                doc.Save(fullname);

                //Down file
                //Process.Start(new ProcessStartInfo(fullname));

                //
                return "/ExportWord/" + foldername + "/" + "InMauNghiPhep.docx";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public bool DangKyNghiPhep_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String noiNghiPhep, String tenDonXinNghi, String diaChiLienHe, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    Guid idHinhThucNghi = HinhThucNghiConst.NghiPhepId;
                    //
                    HoSo_Factory hoSo_Factory = HoSo_Factory.New();
                    HoSo hoSo = hoSo_Factory.GetByID(nhanVienID);
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    CC_ChamCongNgayNghi newObj = factory.CreateManagedObject();
                    newObj.Oid = Guid.NewGuid();
                    newObj.TuNgay = tuNgay.Date;
                    newObj.DenNgay = denNgay.Date;
                    newObj.DienGiai = noiDung.Trim();
                    newObj.IDBoPhan = hoSo.NhanVien.BoPhan;
                    newObj.IDNhanVien = nhanVienID;
                    newObj.CC_HinhThucNghi = idHinhThucNghi;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Today;
                    newObj.LoaiNghiPhep = loaiDonXinNghi;
                    newObj.NoiNghi = noiNghiPhep;
                    newObj.DiaChiLienHe = diaChiLienHe;
                    newObj.TrangThai = -1;
                    newObj.TrangThaiAdmin = -1;
                    newObj.TrangThaiBGH = -1;
                    newObj.SoNgay = GetBusinessDays(tuNgay, denNgay);
                    newObj.TenGiayXinPhep = tenDonXinNghi;
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }

        public bool DangKyChamCongNgayNghi_DeleteList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj);
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
            }
            return false;
        }
        public bool DangKyChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyChamCongNgayNghi_DeleteList(publicKey, token, objList);
        }
        public bool ChamCongNgayNghi_DeleteList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj); 
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
            }
            return false;
        }

        public bool ChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_DeleteList(publicKey, token, objList);
        }


        public bool ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, Guid nhanVienID)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();

                //bat dau kiem tra hop le
                var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                          where o.IDNhanVien == nhanVienID
                                                                  && (
                                                                     (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                                     ||
                                                                     (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                    ||
                                                                    (tuNgay <= o.TuNgay && o.TuNgay <= denNgay)
                                                                    ||
                                                                    (tuNgay <= o.DenNgay && o.DenNgay <= denNgay)
                                                                  )
                                                          select true).FirstOrDefault();

                var trungHoacGiaoNgay_KhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                        where o.IDNhanVien == nhanVienID
                                                              && (
                                                               (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                               ||
                                                               (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                ||
                                                                (tuNgay <= o.TuNgay && o.TuNgay <= denNgay)
                                                                ||
                                                                (tuNgay <= o.DenNgay && o.DenNgay <= denNgay)
                                                              )

                                                        select true).FirstOrDefault();
                var hopLe = false;
                if (trungHoacGiaoNgay_KhaiBaoCongTac)
                    hopLe = trungHoacGiaoNgay_KhaiBaoCongTac;
                if (trungHoacGiaoNgay_ChamCongNgayNghi)
                    hopLe = trungHoacGiaoNgay_ChamCongNgayNghi;
                //
                return hopLe;

            }
            return false;
        }

        #region SAVE
        //kiem tra duoc phep luu

        // Save////////////////////////////////////////////
        public bool ChamCongNgayNghi_Save(String publicKey, String token, DTO_ChamCongNgayNghi_Find obj)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                    {
                        CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                        if (objFromDB != null)
                        {

                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
                            objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                            objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;
                            //
                            factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi( objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                            try
                            {
                                factory.SaveChangesWithoutTransactionScope();
                                tran.Complete();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool ChamCongNgayNghi_Save_Json(String publicKey, String token, string jsonObject)
        {
            //chuyen jsonObject thanh object
            DTO_ChamCongNgayNghi_Find obj = JsonConvert.DeserializeObject<DTO_ChamCongNgayNghi_Find>(jsonObject);
            return ChamCongNgayNghi_Save(publicKey, token, obj);
        }

        public bool ChamCongNgayNghi_SaveList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            //
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                objFromDB.CopyIncludedPropertiesFrom(obj,
                                    new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
                                objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                                objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                            }
                        }
                    }
                    ////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }
        public bool ChamCongNgayNghi_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_SaveList(publicKey, token, objList);
        }

        public bool QuanLyNghiPhep_LuuTraPhep(String publicKey, String token, Guid oid, DateTime ngaytraphep)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    //
                    try
                    {
                        var factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi quanlynghiphep = factory.GetByID(oid);
                        quanlynghiphep.NgayTraPhep = ngaytraphep;
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }

        public bool QuanLyNghiPhep_HuyTraPhep(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    //
                    try
                    {
                        var factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi quanlynghiphep = factory.GetByID(oid);
                        quanlynghiphep.NgayTraPhep = null;
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }
        #endregion
    }
}
