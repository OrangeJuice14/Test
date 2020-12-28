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

        public IEnumerable<DTO_KyChamCong> KyChamCong_Find(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                IEnumerable <DTO_KyChamCong> obj = factory.GetListByYear(nam).Map<DTO_KyChamCong>();
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_KyChamCong> KyChamCong_GetAll(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                IEnumerable<DTO_KyChamCong> obj = factory.GetAll_Order().Map<DTO_KyChamCong>();
                foreach (DTO_KyChamCong ky in obj)
                {
                    ky.TenKyChamCong = "Tháng " + ky.Thang.ToString() +"/" + ky.Nam.ToString();
                }
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public string ParseDateString(DateTime date)
        {
            string result = "";
            String NgayString = "";
            String ThangString = "";
            NgayString = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            ThangString = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            result = NgayString + "/" + ThangString + "/" + date.Year;
            return result;
        }

        public DTO_KyChamCong KyChamCong_FindByDate(String publicKey, String token, int ngay, int thang, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                DTO_KyChamCong obj = factory.GetByDate(ngay,thang,nam).Map<DTO_KyChamCong>();
                obj.TuNgayString = ParseDateString(obj.TuNgay);
                obj.DenNgayString = ParseDateString(obj.DenNgay);
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public DTO_KyChamCong KyChamCong_FindByKyTinhLuong(String publicKey, String token, Guid KyTinhLuong)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                DTO_KyChamCong obj = factory.GetByKyTinhLuong(KyTinhLuong).Map<DTO_KyChamCong>();
                obj.TuNgayString = ParseDateString(obj.TuNgay);
                obj.DenNgayString = ParseDateString(obj.DenNgay);
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String KyChamCong_Find_Json(String publicKey, String token, int nam)
        {
            IEnumerable<DTO_KyChamCong> list = KyChamCong_Find(publicKey, token, nam);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String KyChamCong_GetAll_Json(String publicKey, String token)
        {
            IEnumerable<DTO_KyChamCong> list = KyChamCong_GetAll(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String KyChamCong_FindByDate_Json(String publicKey, String token,int ngay, int thang, int nam)
        {
            DTO_KyChamCong list = KyChamCong_FindByDate(publicKey, token,ngay,thang, nam);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String KyChamCong_FindByKyTinhLuong_Json(String publicKey, String token, Guid KyTinhLuong)
        {
            DTO_KyChamCong list = KyChamCong_FindByKyTinhLuong(publicKey, token, KyTinhLuong);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public DTO_KyChamCong KyChamCong_AddNew(String publicKey, String token, int thang, int nam, DateTime tuNgay, DateTime denNgay)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                CC_KyChamCong newObj = factory.CreateManagedObject();
                newObj.Oid = Guid.NewGuid();
                newObj.TuNgay = tuNgay.Date;
                newObj.DenNgay = denNgay.Date;
                newObj.Thang = thang;
                newObj.Nam = nam;
                factory.SaveChanges();
                return newObj.Map<DTO_KyChamCong>();
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool KyChamCong_CheckExist(String publicKey, String token,int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                var tontai = (from o in factory.Context.CC_KyChamCong
                                                       where o.Thang==thang && o.Nam==nam
                                                       select true).FirstOrDefault();               
                var hopLe = (!tontai);
                return hopLe;
            }
            return false;
        }
        public bool KyChamCong_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, DateTime tuNgay, DateTime denNgay)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                //bat dau kiem tra hop le
                var trungHoacGiaoNgay = (from o in factory.Context.CC_KyChamCong
                                                       where ((o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)||(o.TuNgay <= denNgay && denNgay <= o.DenNgay))
                                                       select true).FirstOrDefault();
                var hopLe = (!trungHoacGiaoNgay);
                return hopLe;
            }
            return false;
        }
        public bool KyChamCong_DeleteList(String publicKey, String token, List<DTO_KyChamCong> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {

                            CC_KyChamCong stupidObj = new CC_KyChamCong() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_KyChamCong_Factory.FullDelete(factory.Context, stupidObj);
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
        public bool KyChamCong_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_KyChamCong> objList = JsonConvert.DeserializeObject<List<DTO_KyChamCong>>(jsonObjectList);
            return KyChamCong_DeleteList(publicKey, token, objList);
        }
        public bool KyChamCong_Check(String publicKey, String token, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_KyChamCong_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                return daTonTai;

            }
            else
            {
                throw new Exception("Xác thực không hợp lệ.");
            }
        }

    }
}
