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
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVien(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                IEnumerable<DTO_ChiTietChamCongNhanVien> list = factory.GetAll_GCRecordIsNull().Map<DTO_ChiTietChamCongNhanVien>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_ChiTietChamCongNhanVien_Json(String publicKey, String token)
        {
            IEnumerable<DTO_ChiTietChamCongNhanVien> list = GetList_ChiTietChamCongNhanVien(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // ////////////////////////////////////////////
        public IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVienBy_HoSoNhanVienId(String publicKey, String token, Guid hoSoNhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                IEnumerable<DTO_ChiTietChamCongNhanVien> list = factory.GetByHoSoNhanVienID(hoSoNhanVienId).Map<DTO_ChiTietChamCongNhanVien>().ToList();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_ChiTietChamCongNhanVienBy_HoSoNhanVienId_Json(String publicKey, String token, Guid hoSoNhanVienId)
        {
            IEnumerable<DTO_ChiTietChamCongNhanVien> list = GetList_ChiTietChamCongNhanVienBy_HoSoNhanVienId(publicKey, token, hoSoNhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        public IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVienBy_BoPhanId(String publicKey, String token, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                IEnumerable<DTO_ChiTietChamCongNhanVien> list = factory.GetByBoPhanId(boPhanId).Map<DTO_ChiTietChamCongNhanVien>().ToList();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_ChiTietChamCongNhanVienBy_BoPhanId_Json(String publicKey, String token, Guid boPhanId)
        {
            IEnumerable<DTO_ChiTietChamCongNhanVien> list = GetList_ChiTietChamCongNhanVienBy_BoPhanId(publicKey, token, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        public DTO_ChiTietChamCongNhanVien Get_ChiTietChamCongNhanVienBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                DTO_ChiTietChamCongNhanVien obj = factory.GetById(id).Map<DTO_ChiTietChamCongNhanVien>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String GetList_ChiTietChamCongNhanVienBy_Id_Json(String publicKey, String token, Guid id)
        {
            DTO_ChiTietChamCongNhanVien obj = Get_ChiTietChamCongNhanVienBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        // Save////////////////////////////////////////////
        public bool Save_ChiTietChamCongNhanVien(String publicKey, String token, DTO_ChiTietChamCongNhanVien obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                    ChiTietChamCongNhanVien objFromDB = factory.GetById(obj.Oid);
                    if (objFromDB == null)
                    {
                        //them moi
                        //map sang entity
                        var newDBObject = factory.CreateManagedObject();
                        newDBObject.CopyPropertiesFrom(obj);
                    }
                    else
                    {
                        //cap nhat
                        objFromDB.CopyPropertiesFrom(obj);
                    }
                    //////////////
                    try
                    {
                        factory.SaveChanges();
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
        }
        public bool Save_ChiTietChamCongNhanVien_Json(String publicKey, String token, string jsonObject)
        {
            //chuyen jsonObject thanh object
            DTO_ChiTietChamCongNhanVien obj = JsonConvert.DeserializeObject<DTO_ChiTietChamCongNhanVien>(jsonObject);
            return Save_ChiTietChamCongNhanVien(publicKey, token, obj);
        }
        // XOA////////////////////////////////////////////
        public bool Delete_ChiTietChamCongNhanVienBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                ChiTietChamCongNhanVien obj = factory.GetById(id);
                if (obj != null)
                {
                    try
                    {
                        ChiTietChamCongNhanVien_Factory.FullDelete(factory.Context, obj);
                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool DeleteList_ChiTietChamCongNhanVienBy_IdList(String publicKey, String token, List<Guid> idList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                Object[] objList = factory.GetListByIdList(idList).ToArray<Object>();
                if (objList != null)
                {
                    try
                    {
                        ChiTietChamCongNhanVien_Factory.FullDelete(factory.Context, objList);
                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
