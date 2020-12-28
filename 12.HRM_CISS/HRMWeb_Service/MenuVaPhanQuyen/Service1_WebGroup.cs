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
using Newtonsoft.Json;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        //private static void GetSetChild(WebGroup webGroup)
        //{
        //    //lay user cua group
        //    webGroup.DanhSachDTO_WebUser = webGroup.WebUsers.Map<DTO_WebUser>().ToList();
        //    //lay menu cua group
        //    List<DTO_WebMenu> tmpListWebMenu = new List<DTO_WebMenu>();
        //    foreach (var role in webGroup.WebMenu_Role)
        //    {
        //        tmpListWebMenu.Add(role.WebMenu.Map<DTO_WebMenu>());
        //    }
        //    webGroup.DanhSachDTO_WebMenu = tmpListWebMenu;
        //}

        // /////////////////////////////////////////////
        public IEnumerable<DTO_WebGroup> WebGroup_GetList(String publicKey, String token, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebGroup_Factory factory = WebGroup_Factory.New();
                //
                if (congTy.Equals(BoPhanConst.CoporationGuid)) // Tổng công ty thì lấy hết nhóm
                {
                    var tmpList = factory.GetAll().ToList();
                    //
                    IEnumerable<DTO_WebGroup> list = tmpList.Map<DTO_WebGroup>().ToList();

                    return list;
                }
                else// Quản trị trường
                {
                    //
                    var tmpList = factory.GetAll().Where(x=> !x.Oid.Equals(WebGroupConst.HoiDongQuanTriID)
                                                         && !x.Oid.Equals(WebGroupConst.HoiDongQuanTriUyQuyenID)
                                                         && !x.Oid.Equals(WebGroupConst.QuanTriKhoiID)
                                                         && !x.Oid.Equals(WebGroupConst.AdminId)).ToList();
                    //
                    IEnumerable<DTO_WebGroup> list = tmpList.Map<DTO_WebGroup>().ToList();

                    return list;
                }
            }
            else
            {
                return null;
            }
        }

        public String WebGroup_GetList_Json(String publicKey, String token, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_WebGroup> list = WebGroup_GetList(publicKey, token,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        public DTO_WebGroup WebGroup_GetBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebGroup_Factory factory = WebGroup_Factory.New();
                var tmpObj = factory.GetByID(id);
                //GetSetChild(tmpObj);
                DTO_WebGroup obj = tmpObj.Map<DTO_WebGroup>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String WebGroup_GetBy_Id_Json(String publicKey, String token, Guid id)
        {
            DTO_WebGroup obj = WebGroup_GetBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}
