using System;
using System.Collections;
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
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public bool Save_KhaiBaoChamCongGiangVien_Json(String publicKey, String token, string jsonObject)
        {//DANG SD
            //chuyen jsonObject thanh object
            var obj = JsonConvert.DeserializeObject<DTO_CC_KhaiBaoChamCongGiangVien>(jsonObject);
            return Save_KhaiBaoChamCongGiangVien(publicKey, token, obj);
        }
        public bool Save_KhaiBaoChamCongGiangVien(String publicKey, String token, DTO_CC_KhaiBaoChamCongGiangVien obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    try
                    {
                        var factory = CC_KhaiBaoChamCongGiangVien_Factory.New();
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
                                    CC_KhaiBaoChamCongGiangVien objForSave = factory.GetKhaiBaoChamCongGiangVienByData(date, obj.Buoi, obj.ThongTinNhanVien);
                                    if (objForSave == null)
                                    {
                                        var newDBObject = factory.CreateManagedObject();
                                        newDBObject.Oid = Guid.NewGuid();
                                        newDBObject.Buoi = obj.Buoi;
                                        newDBObject.IDNhanVien = obj.ThongTinNhanVien;
                                        newDBObject.Ngay = date;
                                        objForSave = newDBObject;

                                        factory.SaveChanges();

                                        factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatKhaiBaoChamCongGiangDay(newDBObject.IDNhanVien, newDBObject.Ngay, newDBObject.Buoi);
                                    }
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
        public bool KhaiBaoChamCong_KiemTraTonTaiTuNgayDenNgay(String publicKey, String token, DateTime tuNgay, DateTime denNgay, Guid nhanVienID)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            bool result = true;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoChamCongGiangVien_Factory factory = CC_KhaiBaoChamCongGiangVien_Factory.New();
                result = factory.Context.CC_KhaiBaoChamCongGiangVien.Any(c => c.Ngay >= tuNgay && c.Ngay <= denNgay && c.IDNhanVien==nhanVienID);               
                return result;
            }
            return result;
        }
        public String KhaiBaoChamCong_Find_Json(String publicKey, String token,int? ngay, int thang, int nam, Guid boPhanId, string maNhanSu)
        {//DANG SD
            IEnumerable<DTO_KhaiBaoChamCong_Find> list = KhaiBaoChamCong_Find(publicKey, token,ngay, thang, nam, boPhanId, maNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_KhaiBaoChamCong_Find> KhaiBaoChamCong_Find(String publicKey, String token,int? ngay, int thang, int nam, Guid boPhanId, string maNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_KhaiBaoChamCongGiangVien_Factory factory = CC_KhaiBaoChamCongGiangVien_Factory.New();
                IEnumerable<DTO_KhaiBaoChamCong_Find> list = null;


                list = factory.FindForKhaiBaoChamCong(ngay, thang, nam, boPhanId,maNhanSu);              
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool KhaiBaoChamCong_DeleteList(String publicKey, String token, List<DTO_KhaiBaoChamCong_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_KhaiBaoChamCongGiangVien_Factory factory = CC_KhaiBaoChamCongGiangVien_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_KhaiBaoChamCongGiangVien stupidObj = new CC_KhaiBaoChamCongGiangVien() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_KhaiBaoChamCongGiangVien_Factory.FullDelete(factory.Context, stupidObj);
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
            return false;
        }

        public bool KhaiBaoChamCong_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            List<DTO_KhaiBaoChamCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_KhaiBaoChamCong_Find>>(jsonObjectList);
            return KhaiBaoChamCong_DeleteList(publicKey, token, objList);
        }
    }
}
