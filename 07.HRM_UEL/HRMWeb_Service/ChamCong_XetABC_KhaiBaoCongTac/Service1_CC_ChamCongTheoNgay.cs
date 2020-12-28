using System;
using System.Collections;
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
        //private static void GetSetChild(CC_ChamCongTheoNgay obj)
        //{
        //    obj.MaNhanSu = obj.HoSo.MaQuanLy;
        //    obj.HoTen = obj.HoSo.HoTen;
        //    obj.TenPhongBan = obj.BoPhan.TenBoPhan;
        //}

    


        #region GET LIST

        //GetList lấy danh sách bộ phận//////////////////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgay(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetAll();
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgay_Json(String publicKey, String token)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgay(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //GetList By_HoSoNhanVienId ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_HoSoNhanVienId(String publicKey, String token, Guid hoSoNhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_HoSoId(hoSoNhanVienId);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();

                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_HoSoNhanVienId_Json(String publicKey, String token, Guid hoSoNhanVienId)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_HoSoNhanVienId(publicKey, token, hoSoNhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //GetList By_Ngay ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Ngay(String publicKey, String token, DateTime ngay)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_Date(ngay);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();

                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_Ngay_Json(String publicKey, String token, DateTime ngay)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_Ngay(publicKey, token, ngay);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        //GetList By_Ngay_HoSoId ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Ngay_HoSoNhanVienId(String publicKey, String token, DateTime ngay, Guid hoSoNhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_Date_HoSoId(ngay, hoSoNhanVienId);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();

                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_Ngay_HoSoNhanVienId_Json(String publicKey, String token, DateTime ngay, Guid hoSoNhanVienId)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_Ngay_HoSoNhanVienId(publicKey, token, ngay, hoSoNhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //GetList By_NgayThangNam_HoSoId ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_ThangNam_HoSoNhanVienId(String publicKey, String token, int thang, int nam, Guid hoSoNhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_ThangNam_HoSoId(thang, nam, hoSoNhanVienId);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();

                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_ThangNam_HoSoNhanVienId_Json(String publicKey, String token, int thang, int nam, Guid hoSoNhanVienId)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_ThangNam_HoSoNhanVienId(publicKey, token, thang, nam, hoSoNhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //GetList By_BoPhanId ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_BoPhanId(String publicKey, String token, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_BoPhanId(boPhanId);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_BoPhanId_Json(String publicKey, String token, Guid boPhanId)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_BoPhanId(publicKey, token, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        //GetList By_BoPhanId ////////////////////////////////////////////
        public IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Date_HoSoId_BoPhanId(String publicKey, String token, DateTime ngay, Guid hoSoNhanVienId, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<CC_ChamCongTheoNgay> tmpList = factory.GetBy_Date_HoSoId_BoPhanId(ngay, hoSoNhanVienId, boPhanId);
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CC_ChamCongTheoNgayBy_Date_HoSoId_BoPhanId_Json(String publicKey, String token, DateTime ngay, Guid hoSoNhanVienId, Guid boPhanId)
        {
            IEnumerable<DTO_CC_ChamCongTheoNgay> list = GetList_CC_ChamCongTheoNgayBy_Date_HoSoId_BoPhanId(publicKey, token, ngay, hoSoNhanVienId, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        #endregion



        #region GET SINGLE OBJECT
        //Get obj ////////////////////////////////////////////
        public DTO_CC_ChamCongTheoNgay Get_CC_ChamCongTheoNgayBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                var tmpObj = factory.GetByID(id);
                //GetSetChild(tmpObj);
                DTO_CC_ChamCongTheoNgay obj = tmpObj.Map<DTO_CC_ChamCongTheoNgay>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String Get_CC_ChamCongTheoNgayBy_Id_Json(String publicKey, String token, Guid id)
        {

            DTO_CC_ChamCongTheoNgay obj = Get_CC_ChamCongTheoNgayBy_Id(publicKey, token, id);

            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion



    }
}
