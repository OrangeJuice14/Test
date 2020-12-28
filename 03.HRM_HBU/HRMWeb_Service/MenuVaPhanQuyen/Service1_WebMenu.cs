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

        public IEnumerable<DTO_WebMenu> WebMenu_GetList(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<DTO_WebMenu> list = factory.GetAll().Map<DTO_WebMenu>();
                return list;
            }
            else
            {
                return null;
            }
        }

        public String WebMenu_GetList_Json(String publicKey, String token)
        {
            IEnumerable<DTO_WebMenu> list = WebMenu_GetList(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //// ////////////////////////////////////////////
        public IEnumerable<DTO_WebMenu> WebMenu_GetListBy_WebUserId(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<DTO_WebMenu> list = factory.GetListBy_WebUserId(webUserId).Map<DTO_WebMenu>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String WebMenu_GetListBy_WebUserId_Json(String publicKey, String token, Guid webUserId)
        {
            IEnumerable<DTO_WebMenu> list = WebMenu_GetListBy_WebUserId(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }


        public IEnumerable<String> WebMenu_GetURLListBy_WebUserId(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<String> list = factory.GetURLListBy_WebUserId(webUserId);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String WebMenu_GetURLListBy_WebUserId_Json(String publicKey, String token, Guid webUserId)
        {
            IEnumerable<String> list = WebMenu_GetURLListBy_WebUserId(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// ////////////////////////////////////////////////////
        public IEnumerable<DTO_WebMenu> WebMenu_GetListTop2LevelDeepBy_WebUserId(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<DTO_WebMenu> list = factory.GetListTop2LevelDeepBy_WebUserId(webUserId).Map<DTO_WebMenu>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String WebMenu_GetListTop2LevelDeepBy_WebUserId_Json(String publicKey, String token, Guid webUserId)
        {//DANG SU DUNG
            IEnumerable<DTO_WebMenu> list = WebMenu_GetListTop2LevelDeepBy_WebUserId(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// ////////////////////////////////////////////////////
        public IEnumerable<DTO_WebMenu> WebMenu_GetChildMenuListBy_WebUserId_AndMenuId(String publicKey, String token, Guid webUserId, Guid menuId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<DTO_WebMenu> list = factory.GetChildMenuList_ByWebUserId_AndMenuId(webUserId, menuId).Map<DTO_WebMenu>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String WebMenu_GetChildMenuListBy_WebUserId_AndMenuId_Json(String publicKey, String token, Guid webUserId, Guid menuId)
        {//DANG SD
            IEnumerable<DTO_WebMenu> list = WebMenu_GetChildMenuListBy_WebUserId_AndMenuId(publicKey, token, webUserId, menuId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //// ////////////////////////////////////////////
        public IEnumerable<DTO_WebMenu> WebMenu_GetListBy_WebGroupId(String publicKey, String token, Guid webGroupId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                IEnumerable<DTO_WebMenu> list = factory.GetListBy_WebGroupId(webGroupId).Map<DTO_WebMenu>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String WebMenu_GetListBy_WebGroupId_Json(String publicKey, String token, Guid webGroupId)
        {
            IEnumerable<DTO_WebMenu> list = WebMenu_GetListBy_WebGroupId(publicKey, token, webGroupId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //// ////////////////////////////////////////////
        public DTO_WebMenu WebMenu_GetById(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebMenu_Factory factory = WebMenu_Factory.New();
                DTO_WebMenu obj = factory.GetByID(id).Map<DTO_WebMenu>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String WebMenu_GetById_Json(String publicKey, String token, Guid id)
        {
            DTO_WebMenu obj = WebMenu_GetById(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

    }
}
