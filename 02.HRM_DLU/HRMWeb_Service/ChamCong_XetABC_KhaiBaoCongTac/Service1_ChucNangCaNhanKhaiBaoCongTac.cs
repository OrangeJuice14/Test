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

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {


        public IEnumerable<DTO_CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(String publicKey, String token, int thang, int nam, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                var tmpList = factory.CaNhanKhaiBaoCongTac_Find(thang, nam, webUserId).ToList();
                IEnumerable<DTO_CC_KhaiBaoCongTac> objList = tmpList.Map<DTO_CC_KhaiBaoCongTac>();
                foreach (DTO_CC_KhaiBaoCongTac cc in objList)
                {
                    cc.TuNgayString = FormatTime(cc.TuNgay.Value.Hour) + " giờ " + FormatTime(cc.TuNgay.Value.Minute) + " ngày " + FormatTime(cc.TuNgay.Value.Day) + "/" + FormatTime(cc.TuNgay.Value.Month) + "/" + cc.TuNgay.Value.Year.ToString();
                    cc.DenNgayString = FormatTime(cc.DenNgay.Value.Hour) + " giờ " + FormatTime(cc.DenNgay.Value.Minute) + " ngày " + FormatTime(cc.DenNgay.Value.Day) + "/" + FormatTime(cc.DenNgay.Value.Month) + "/" + cc.DenNgay.Value.Year.ToString();
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
                }

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
        public String CaNhanKhaiBaoCongTac_Find_Json(String publicKey, String token, int thang, int nam, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_CC_KhaiBaoCongTac> list = CaNhanKhaiBaoCongTac_Find(publicKey, token, thang, nam, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String CaNhanKhaiBaoCongTac_Report_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_CC_KhaiBaoCongTac list = CaNhanKhaiBaoCongTac_Report(publicKey, token, id);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public bool CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, Guid webUserId,string IDNhanVien)
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
                                                    ( tuNgay <= o.TuNgay && o.TuNgay <= denNgay)
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


        public bool CaNhanKhaiBaoCongTac_KhaiBaoMoi(String publicKey, String token, String noiDung,String diaDiem, DateTime tuNgay, DateTime denNgay,byte buoi, int gioBatDau, int phutBatDau, int gioKetThuc,int phutKetThuc, Guid webUserId,Guid nguoiKy,string IDNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (string.IsNullOrEmpty(IDNhanVien))
                { 
                WebUser_Factory webUser_Factory = WebUser_Factory.New();
                WebUser webUser = webUser_Factory.GetByID(webUserId);
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
                newObj.NgayTao = DateTime.Today;
                newObj.TrangThai = -1;//trạng thái chờ
                newObj.So = factory.LaySoLonNhat()==0 ? 1: factory.LaySoLonNhat()+1;
                newObj.NguoiKy = nguoiKy;
                //
                factory.SaveChanges();

                return true;
                }
                else
                {
                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                    CC_KhaiBaoCongTac newObj = factory.CreateManagedObject();
                    newObj.Oid = Guid.NewGuid();
                    newObj.TuNgay = tuNgay;
                    newObj.DenNgay = denNgay;
                    newObj.Buoi = buoi;
                    newObj.NoiDung = noiDung.Trim();
                    newObj.DiaDiem = diaDiem.Trim();
                    newObj.IDNhanVien = new Guid(IDNhanVien);
                    newObj.IDWebUser = Guid.Empty;
                    newObj.NgayTao = DateTime.Today;
                    newObj.TrangThai = 1;
                    newObj.So = factory.LaySoLonNhat() == 0 ? 1 : factory.LaySoLonNhat() + 1;
                    newObj.NguoiKy = nguoiKy;
                    factory.SaveChanges();
                    //
                    return true;
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
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


    }
}
