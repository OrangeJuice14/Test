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
using System.IO;
using System.Diagnostics;
using System.Web;
using HRMWeb_Business.Predefined;
using ERP_Core.Common;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {


        public IEnumerable<DTO_CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(String publicKey, String token, DateTime tungay, DateTime denngay, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                var objList = factory.CaNhanKhaiBaoCongTac_Find(tungay, denngay, webUserId).ToList();
                //
                /*
                foreach (DTO_CC_KhaiBaoCongTac cc in objList)
                {
                    cc.TuNgayString = "Ngày " + FormatTime(cc.TuNgay.Value.Day) + "/" + FormatTime(cc.TuNgay.Value.Month) + "/" + cc.TuNgay.Value.Year.ToString();
                    cc.DenNgayString = "Ngày " + FormatTime(cc.DenNgay.Value.Day) + "/" + FormatTime(cc.DenNgay.Value.Month) + "/" + cc.DenNgay.Value.Year.ToString();

                    switch (cc.Buoi)
                    {
                        case "0":
                            {
                                cc.Buoi = "Cả ngày";
                            }
                            break;
                        case "1":
                            {
                                cc.Buoi = "Buổi sáng";
                            }
                            break;
                        case "2":
                            {
                                cc.Buoi = "Buổi chiều";
                            }
                            break;
                    }
                }*/

                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public DTO_CC_KhaiBaoCongTac CaNhanKhaiBaoCongTac_Report(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                DTO_CC_KhaiBaoCongTac objList = factory.CaNhanKhaiBaoCongTac_Report(id);
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public string FormatTime(int time)
        {
            string result = "";
            result = time < 10 ? "0" + time.ToString() : time.ToString();
            return result;
        }
        public String CaNhanKhaiBaoCongTac_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_CC_KhaiBaoCongTac> list = CaNhanKhaiBaoCongTac_Find(publicKey, token, tungay, denngay, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String CaNhanKhaiBaoCongTac_Report_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_CC_KhaiBaoCongTac list = CaNhanKhaiBaoCongTac_Report(publicKey, token, id);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public bool CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, Guid webUserId, string IDNhanVien)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                if (string.IsNullOrEmpty(IDNhanVien))
                {
                    WebUser_Factory webUser_Factory = WebUser_Factory.New();
                    WebUser webUser = webUser_Factory.GetByID(webUserId);

                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();

                    //bat dau kiem tra hop le
                    var trungHoacGiaoNgayKhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                           where o.IDNhanVien == webUser.ThongTinNhanVien
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

                    var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                              where o.IDNhanVien == webUser.ThongTinNhanVien
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
                    if (trungHoacGiaoNgayKhaiBaoCongTac)
                        hopLe = trungHoacGiaoNgayKhaiBaoCongTac;
                    if (trungHoacGiaoNgay_ChamCongNgayNghi)
                        hopLe = trungHoacGiaoNgay_ChamCongNgayNghi;
                    //
                    return hopLe;
                }
                else
                {
                    Guid id = new Guid(IDNhanVien);
                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();

                    //bat dau kiem tra hop le
                    var trungHoacGiaoNgayKhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                           where o.IDNhanVien == id
                                                                  && (
                                                                      (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                                      ||
                                                                      (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                   )
                                                           select true).FirstOrDefault();

                    var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                              where o.IDNhanVien == id
                                                                      && (
                                                                         (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                                         ||
                                                                         (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                      )
                                                              select true).FirstOrDefault();

                    var hopLe = false;
                    if (trungHoacGiaoNgayKhaiBaoCongTac)
                        hopLe = trungHoacGiaoNgayKhaiBaoCongTac;
                    if (trungHoacGiaoNgay_ChamCongNgayNghi)
                        hopLe = trungHoacGiaoNgay_ChamCongNgayNghi;
                    //
                    return hopLe;
                }
            }
            return false;
        }


        public bool CaNhanKhaiBaoCongTac_KhaiBaoMoi(String publicKey, String token, String noiDung, String diaDiem, DateTime tuNgay, DateTime denNgay, byte buoi, int gioBatDau, int phutBatDau, int gioKetThuc, int phutKetThuc, Guid webUserId, Guid nguoiKy, string IDNhanVien, Guid congTy)
        {
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;
                    Guid idYersin = BoPhanConst.YerSinGuid;
                    //
                    WebUser_Factory webUser_Factory = WebUser_Factory.New();
                    WebUser webUser = webUser_Factory.GetByID(webUserId);

                    if (string.IsNullOrEmpty(IDNhanVien))
                    {
                        //
                        CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                        CC_KhaiBaoCongTac newObj = factory.CreateManagedObject();
                        newObj.Oid = Guid.NewGuid();
                        newObj.TuNgay = tuNgay;
                        newObj.DenNgay = denNgay;
                        newObj.Buoi = buoi;
                        newObj.NoiDung = noiDung.Trim();
                        newObj.DiaDiem = diaDiem.Trim();
                        newObj.IDNhanVien = webUser.ThongTinNhanVien.Value;
                        newObj.IDWebUser = webUserId;
                        newObj.NgayTao = DateTime.Now;
                        //
                        if (webUser.CongTyId == idYersin)
                        {
                            newObj.SoNgay = Helper.GetBusinessDays(tuNgay, denNgay, webUser.CongTyId); // Nghỉ thứ 7
                        }
                        else
                        {
                            newObj.SoNgay = Helper.GetBusinessDays1(tuNgay, denNgay, webUser.CongTyId); // Làm thứ 7
                        }
                        //Nếu là nửa buổi thì chia lại
                        if (newObj.Buoi != 0)
                        {
                            newObj.SoNgay = newObj.SoNgay / 2;
                        }
                        //
                        if (idCaNhan.Equals(webUser.WebGroupID)) // Cá nhân phải đợi duyệt
                        {
                            newObj.TrangThai = -1;//Trạng thái chờ
                        }
                        else
                        {
                            newObj.TrangThai = 1;
                        }
                        newObj.So = factory.LaySoLonNhat() == 0 ? 1 : factory.LaySoLonNhat() + 1;
                        newObj.NguoiKy = null;
                        //
                        factory.SaveChanges();

                        //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                        if (newObj.TrangThai == 1)
                        {
                            //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                            factory.Context.spd_WebChamCong_CapNhatKhaiBaoCongTac(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, true);
                        }

                        //
                        if (idCaNhan.Equals(webUser.WebGroupID))
                        {
                            //Gửi đến trưởng phòng
                            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
                            if (cauHinh != null)
                            {
                                KhaiBaoCongTac_SetDataSendMail(newObj, cauHinh, webUser.EmailTP, webUser.Oid);
                            }
                        }
                        //
                        return true;
                    }
                    else
                    {
                        using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                        {
                            HoSo hoSo = HoSo_Factory.New().GetByID(new Guid(IDNhanVien));
                            CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                            CC_KhaiBaoCongTac newObj = factory.CreateManagedObject();
                            newObj.Oid = Guid.NewGuid();
                            newObj.TuNgay = tuNgay;
                            newObj.DenNgay = denNgay;
                            newObj.Buoi = buoi;
                            newObj.NoiDung = noiDung.Trim();
                            newObj.DiaDiem = diaDiem.Trim();
                            newObj.IDNhanVien = new Guid(IDNhanVien);
                            newObj.IDWebUser = webUserId;
                            newObj.NgayTao = DateTime.Now;
                            newObj.TrangThai = 1;
                            newObj.So = factory.LaySoLonNhat() == 0 ? 1 : factory.LaySoLonNhat() + 1;
                            newObj.NguoiKy = null;
                            //
                            if (hoSo.NhanVien.CongTy == idYersin)
                            {
                                newObj.SoNgay = Helper.GetBusinessDays(tuNgay, denNgay, hoSo.NhanVien.CongTy); // Nghỉ thứ 7
                            }
                            else
                            {
                                newObj.SoNgay = Helper.GetBusinessDays1(tuNgay, denNgay, hoSo.NhanVien.CongTy); // Làm thứ 7
                            }
                            //Nếu là nửa buổi thì chia lại
                            if (newObj.Buoi != 0)
                            {
                                newObj.SoNgay = newObj.SoNgay / 2;
                            }
                            //
                            factory.SaveChanges();
                            //
                            if (newObj.TrangThai == 1)
                            {
                                //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                                factory.Context.spd_WebChamCong_CapNhatKhaiBaoCongTac(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, true);
                            }
                            //
                            transaction.Complete();
                        }
                        //
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Chứng thực không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CaNhanKhaiBaoCongTac_KhaiBaoMoi", ex);
                throw;
            }
        }
        public void KhaiBaoCongTac_SetDataSendMail(CC_KhaiBaoCongTac newObj, DTO_CC_CauHinhChamCong cauHinh, string emailReceiver, Guid user)
        {
            var webMailTemplate = new WebMailTemplate_Factory().GetByCongTyVaLoaiGuiMail(newObj.HoSo.NhanVien.CongTy ?? Guid.Empty, WebMailTemplateTypeConst.DuyetKhaiBaoCongTac);

            string tieudeguimail = "";
            string noidungguimail = "";

            if (webMailTemplate != null && webMailTemplate.TieuDe != null && webMailTemplate.NoiDung != null)
            {
                tieudeguimail = KhaiBaoCongTac_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.TieuDe);
                noidungguimail = KhaiBaoCongTac_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.NoiDung);
            }
            else
            {
                tieudeguimail = "DUYỆT KHAI BÁO CÔNG TÁC";
                noidungguimail = "Họ tên: [" + newObj.HoSo.HoTen.ToUpper() + "] Đơn vị: [" + newObj.HoSo.NhanVien.BoPhan1.TenBoPhan.ToUpper() + "] xin nghỉ: [Đi Công Tác] Từ ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.TuNgay) + "] Đến ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.DenNgay) + "]";
            }

            //Gửi mail
            QuanLyGuiEmail_SendMail(cauHinh, emailReceiver, tieudeguimail, noidungguimail, user);

        }
        public string KhaiBaoCongTac_MailTemplateReplaceText(CC_KhaiBaoCongTac newObj, DTO_CC_CauHinhChamCong cauHinh, string template)
        {
            var result = string.Empty;
            if (template != null)
            {
                var URL = Helper.GetCurrentDomainName();
                result = template.Replace("HoTen", newObj.HoSo.HoTen)
                    .Replace("DonVi", newObj.HoSo.NhanVien.BoPhan1.TenBoPhan)
                    .Replace("NghiTuNgay", String.Format("{0:dd/MM/yyyy}", newObj.TuNgay))
                    .Replace("NghiDenNgay", String.Format("{0:dd/MM/yyyy}", newObj.DenNgay))
                    .Replace("http://linkduyet/", URL + "/kpi/quanlycongtac?id=" + newObj.Oid);
            }
            return result;
        }

        public bool CaNhanKhaiBaoCongTac_DeleteList(String publicKey, String token, List<DTO_CC_KhaiBaoCongTac> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                {
                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_KhaiBaoCongTac stupidObj = new CC_KhaiBaoCongTac() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_KhaiBaoCongTac_Factory.FullDelete(factory.Context, stupidObj);
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

        public bool CaNhanKhaiBaoCongTac_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_KhaiBaoCongTac> objList = JsonConvert.DeserializeObject<List<DTO_CC_KhaiBaoCongTac>>(jsonObjectList);
            return CaNhanKhaiBaoCongTac_DeleteList(publicKey, token, objList);
        }

        public bool QuanLyKhaiBaoCongTac_DownLoadFile(String publicKey, String token, HttpServerUtilityBase Server, string oidkhaibaocongtac, string fileName)
        {
            bool result = false;
            //
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    CC_Attachments_Factory factory = new CC_Attachments_Factory();
                    CC_Attachments obj = factory.GetAttachment(new Guid(oidkhaibaocongtac), fileName);
                    if (obj != null)
                    {
                        string fullPath = Path.Combine(Server.MapPath("~/Downloads/"), fileName);
                        File.WriteAllBytes(fullPath, obj.Data);
                        //
                        Process.Start(new ProcessStartInfo(fullPath));
                    }
                    //

                    result = true;
                }
                catch (Exception ex) { }
            }
            return result;
        }

        public bool QuanLyKhaiBaoCongTac_DeleteFile(String publicKey, String token, HttpServerUtilityBase Server, string oidkhaibaocongtac, string fileName)
        {
            bool result = false;
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    CC_Attachments_Factory factory = new CC_Attachments_Factory();
                    CC_Attachments obj = factory.GetAttachment(new Guid(oidkhaibaocongtac), fileName);
                    if (obj != null)
                    {
                        factory.DeleteObject(obj);
                        //
                        factory.SaveChanges();
                    }
                    //

                    result = true;
                }
                catch (Exception ex) { }
            }
            return result;
        }
    }
}
