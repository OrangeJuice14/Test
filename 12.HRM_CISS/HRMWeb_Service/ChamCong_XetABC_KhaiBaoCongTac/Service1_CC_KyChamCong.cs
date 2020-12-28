using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
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
        public IEnumerable<DTO_CC_KyChamCong> KyChamCong_GetAll(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                IEnumerable<DTO_CC_KyChamCong> list = factory.GetKyChamCongList_All().Map<DTO_CC_KyChamCong>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_CC_KyChamCong> GetYearKyChamCong(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                //IEnumerable<DTO_CC_KyChamCong> list = factory.GetYearDistinct();
                return null;
            }
            else
            {
                return null;
            }
        }
        
        public String KyChamCong_GetAll_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_CC_KyChamCong> list = KyChamCong_GetAll(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetYearKyChamCong_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_CC_KyChamCong> list = GetYearKyChamCong(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // /////////////////////////////////////////////////////
        public DTO_CC_KyChamCong KyChamCong_ByID(String publicKey, String token,Guid oidKyChamCong)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                DTO_CC_KyChamCong obj = factory.GetKyChamCong_ByID(oidKyChamCong).Map<DTO_CC_KyChamCong>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String KyChamCong_ByID_Json(String publicKey, String token, Guid oidKyChamCong)
        {//DANG SD
            DTO_CC_KyChamCong obj = KyChamCong_ByID(publicKey, token, oidKyChamCong);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public bool KiemTraKhoaSo_KyChamCong(String publicKey, String token, Guid oidKyChamCong)
        {
            CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
            var obj = factory.GetKyChamCong_ByID(oidKyChamCong);
            //
            if (obj != null)
                return obj.KhoaSo ?? false;
            else return false;
        }
        public bool CaChamCong_CheckExists(String publicKey, String token, int thang, int nam, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //
                bool isExists = false;
                var factory = CC_KyChamCong_Factory.New();
                CC_KyChamCong kyChamCong = factory.GetKyChamCong_ByThangNam(thang, nam,congTy);
                if (kyChamCong != null)
                    isExists = true;
                //
                return isExists;

            }
            else
            {
                throw new Exception("Xác thực không hợp lệ.");
            }
        }

        public DTO_CC_KyChamCong KyChamCong_GetTuNgayDenNgay_ByNgay(String publicKey, String token, DateTime ngay, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyChamCong_Factory factory = CC_KyChamCong_Factory.New();
                DTO_CC_KyChamCong result = factory.GetKyChamCong_ByNgay(ngay, congTy)?.Map<DTO_CC_KyChamCong>();
                return result;
            }
            else
            {
                throw new Exception("Xác thực không hợp lệ.");
            }
        }
    }
}
