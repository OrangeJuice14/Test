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

        public bool WebUsers_XoaUsers(String publicKey, String token, List<DTO_WebUser> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {


                        var objFromDB = factory.GetByID(obj.Oid);
                        objFromDB.UserName += Guid.NewGuid().ToString();
                        objFromDB.GCRecord = 1;
                       

                    }
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
                return false;
            }
            return false;
        }
        public bool WebUsers_XoaUsers_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_WebUser> objList = JsonConvert.DeserializeObject<List<DTO_WebUser>>(jsonObjectList);
            return WebUsers_XoaUsers(publicKey, token, objList);
        }
        public bool WebUsers_KiemTraTrungUsername(String publicKey, String token, Guid? webUserId, String username)
        {//DANG SU DUNG

            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                bool isNew = true;
                if (webUserId != null)
                {
                    var objFromDb = factory.GetByID(webUserId.Value);
                    if (objFromDb != null)
                        isNew = false;
                }
                //bat dau kiem trung
                var trung = (from o in factory.Context.WebUsers
                             where (isNew || o.Oid != webUserId)
                                 && o.UserName.ToLower() == username.ToLower()

                             select true).FirstOrDefault();
                return trung;

            }
            return false;
        }


        //private static void GetSetChild(WebUser webUser)
        //{
        //    //webUser.DTOWebGroup = webUser.WebGroup.Map<DTO_WebGroup>();
        //    //lay bo phan ma user duoc xu ly cham cong
        //    {
        //        if (webUser.HoSo != null)
        //        {
        //            webUser.MaNhanSu = webUser.HoSo.MaQuanLy;
        //            webUser.HoVaTen = webUser.HoSo.HoTen;
        //            webUser.Email = webUser.HoSo.Email;
        //            try
        //            {
        //                webUser.TenBoPhan = webUser.HoSo.NhanVien.BoPhan1.TenBoPhan;
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }

        //        //BoPhan_Factory boPhanFactory = BoPhan_Factory.New();
        //        //IEnumerable<DTO_BoPhan> tmpList = boPhanFactory.GetAll_GCRecordIsNull().Map<DTO_BoPhan>();

        //        //foreach (var webUser_BoPhan in webUser.WebUser_BoPhan)
        //        //{
        //        //    //tmpList.Add(webUser_BoPhan.BoPhan.Map<DTO_BoPhan>());
        //        //    DTO_BoPhan boPhanCanCheck = tmpList.SingleOrDefault(x => x.Oid == webUser_BoPhan.BoPhanID);
        //        //    if (boPhanCanCheck != null)
        //        //        boPhanCanCheck.Chon = true;

        //        //}
        //        //webUser.DanhSachDTO_BoPhan = tmpList;
        //    }
        //}
        private static void GetSetChild_WithBoPhanList(WebUser webUser)
        {
            //webUser.DTOWebGroup = webUser.WebGroup.Map<DTO_WebGroup>();
            //lay bo phan ma user duoc xu ly cham cong
            {
                if (webUser.HoSo != null)
                {
                    webUser.MaNhanSu = webUser.HoSo.MaQuanLy;
                    webUser.HoVaTen = webUser.HoSo.HoTen;
                    webUser.Email = webUser.HoSo.Email;
                    try
                    {
                        webUser.TenBoPhan = webUser.HoSo.NhanVien.BoPhan1.TenBoPhan;
                    }
                    catch (Exception)
                    {
                    }
                }


                BoPhan_Factory boPhanFactory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> tmpList = boPhanFactory.GetAll_GCRecordIsNull().Map<DTO_BoPhan>();

                foreach (var webUser_BoPhan in webUser.WebUser_BoPhan)
                {
                    //tmpList.Add(webUser_BoPhan.BoPhan.Map<DTO_BoPhan>());
                    DTO_BoPhan boPhanCanCheck = tmpList.SingleOrDefault(x => x.Oid == webUser_BoPhan.BoPhanID);
                    if (boPhanCanCheck != null)
                        boPhanCanCheck.Chon = true;

                }
                webUser.DanhSachDTO_BoPhan = tmpList;
            }
        }
        public bool ChangePassword_WebUser(String publicKey, String token, Guid idWebUser, string newPassword)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                var webUser = factory.GetByID(idWebUser);
                if (webUser != null)
                {
                    string pwMd5 = HRMWebApp.Helpers.Helper.getMd5Hash(newPassword.Trim());
                    webUser.Password = pwMd5;
                    factory.SaveChanges();
                    return true;
                }
                else
                {
                    return false; ;
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public DTO_WebUser CheckForLogin_WebUser(String publicKey, String token, string username, string password)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                //var tmpObj = factory.GetByUsernameAndPassword(username, password);
                ////return (webUser != null);
                //DTO_WebUser obj = null;
                //if (tmpObj != null && (tmpObj.HoatDong ?? false) && tmpObj.GCRecord == null)
                //{
                //    GetSetChild(tmpObj);
                //    obj = tmpObj.Map<DTO_WebUser>();
                //}
                //else
                //{
                //    tmpObj = null;
                //}
                var obj = factory.GetDTO_WebUser_ByUsernameAndPassword(username, password);
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String CheckForLogin_WebUserr_Json(String publicKey, String token, string username, string password)
        {
            DTO_WebUser obj = CheckForLogin_WebUser(publicKey, token, username, password);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        //WebUser_GetByEmail //////////////

        public DTO_WebUser WebUser_GetByEmail(String publicKey, String token, string email)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();


                var obj = factory.GetDTO_WebUser_ByEmail(email);
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String WebUser_GetByEmail_Json(String publicKey, String token, string email)
        {//DANG SD
            DTO_WebUser obj = WebUser_GetByEmail(publicKey, token, email);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        
        /// ///////////////////////////////////////

        public IEnumerable<DTO_WebUser> GetList_WebUser(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                //var tmpList = factory.GetAll_GCRecordIsNull();
                //foreach (WebUser webUser in tmpList)
                //{
                //    GetSetChild(webUser);
                //}
                //IEnumerable<DTO_WebUser> list = tmpList.Map<DTO_WebUser>();
                IEnumerable<DTO_WebUser> list = factory.GetAllDTO_WebUser_GCRecordIsNull();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_WebUser_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_WebUser> list = GetList_WebUser(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        /////////////quan tri////////////////////////////////////

        public IEnumerable<DTO_WebUser> GetList_WebUserQuanTri(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                IEnumerable<DTO_WebUser> list = factory.GetAllDTO_WebUser_GCRecordIsNull_UserQuanTriToanQuyen();
                //list = list.Where(p => p.WebGroupID != Guid.Parse("00000000-0000-0000-0000-000000000001")).ToList();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_WebUserQuanTri_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_WebUser> list = GetList_WebUserQuanTri(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        /////////////khac quan tri////////////////////////////////////

        public IEnumerable<DTO_WebUser> GetList_WebUserKhacQuanTri(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                IEnumerable<DTO_WebUser> list = factory.GetAllDTO_WebUser_GCRecordIsNull_UserKhacQuanTriToanQuyen();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_WebUserKhacQuanTri_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_WebUser> list = GetList_WebUserKhacQuanTri(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // Save////////////////////////////////////////////
        public bool Save_WebUser(String publicKey, String token, DTO_WebUser obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    var factory = WebUser_Factory.New();
                    WebUser objFromDB = factory.GetByID(obj.Oid);
                    WebUser objForSave = null;
                    if (objFromDB == null)
                    {
                        //them moi
                        //map sang entity
                        var newDBObject = factory.CreateManagedObject();
                        newDBObject.CopyPropertiesFrom(obj);
                        newDBObject.Oid = Guid.NewGuid();
                        newDBObject.DepartmentId = obj.DepartmentId;
                        objForSave = newDBObject;
                        objForSave.Password = HRMWebApp.Helpers.Helper.getMd5Hash(obj.Password);
                    }
                    else
                    {
                        //cap nhat
                        objFromDB.CopyIncludedPropertiesFrom(obj, "UserName", "HoatDong", "WebGroupID", "DepartmentId", "AgentObjectTypeId");//.CopyPropertiesFrom(obj);
                        objForSave = objFromDB;
                        if (obj.Password != objFromDB.Password)
                            objForSave.Password = HRMWebApp.Helpers.Helper.getMd5Hash(obj.Password);

                        //xoa nhung WebUser_BoPhan bi loai bo
                        /*
                        {
                            List<WebUser_BoPhan> danhSachWebUser_BoPhanCanGoBoKhoi = new List<WebUser_BoPhan>();
                            foreach (var wubp in objForSave.WebUser_BoPhan)
                            {
                                if (obj.DanhSachDTO_BoPhan.All(x => x.Oid != wubp.BoPhanID))
                                {
                                    //them vao danh sach cho xoa
                                    danhSachWebUser_BoPhanCanGoBoKhoi.Add(wubp);
                                }
                            }
                            WebUser_BoPhan_Factory.FullDelete(factory.Context,
                                danhSachWebUser_BoPhanCanGoBoKhoi.ToArray<Object>());
                        }
                         * */
                    }
                    //cap nhat cac doi tuong long ben trong
                    {
                        //them WebUser_BoPhan

                        if (obj.DanhSachDTO_BoPhan != null && obj.DanhSachDTO_BoPhan.ToList().Count > 0)
                            foreach (var dtoBoPhan in obj.DanhSachDTO_BoPhan)
                            {
                                if (dtoBoPhan.Chon)
                                {
                                    //neu ko ton tai thi them vao
                                    if (objForSave.WebUser_BoPhan.All(x => x.BoPhanID != dtoBoPhan.Oid))
                                    {
                                        var wubp = new WebUser_BoPhan();
                                        wubp.BoPhanID = dtoBoPhan.Oid;
                                        //wubp.IDWebUser = objForSave.Oid;
                                        objForSave.WebUser_BoPhan.Add(wubp);

                                    }
                                }
                                else
                                {
                                    WebUser_BoPhan objWebUser_BoPhanTimDuoc = objForSave.WebUser_BoPhan.SingleOrDefault(x => x.BoPhanID == dtoBoPhan.Oid);
                                    if (objWebUser_BoPhanTimDuoc != null)
                                        WebUser_BoPhan_Factory.FullDelete(factory.Context, objWebUser_BoPhanTimDuoc);
                                }
                                /*
                                //kiem tra da ton tai chua
                                //neu ko ton tai thi them vao
                                if (objForSave.WebUser_BoPhan.All(x => x.BoPhanID != dtoBoPhan.Oid))
                                {
                                    var wubp = new WebUser_BoPhan();
                                    wubp.BoPhanID = dtoBoPhan.Oid;
                                    //wubp.IDWebUser = objForSave.Oid;
                                    objForSave.WebUser_BoPhan.Add(wubp);

                                }*/
                            }

                    }
                    //Gán AgentObjectTypeId dựa vào DepartmentId
                    //{
                    //    if (obj.DepartmentId!=null)
                    //    {
                    //        DTO_BoPhan dept = Get_BoPhanBy_Id(publicKey, token, obj.DepartmentId.Value);
                    //        if (dept.LoaiBoPhan==1)
                    //        {
                    //            objForSave.AgentObjectTypeId = 3;
                    //        }
                    //        else if (dept.LoaiBoPhan==4)
                    //        {
                    //            objForSave.AgentObjectTypeId = 5;
                    //        }
                    //    }
                    //    //Guid departmentId = new Guid(obj.DepartmentId!=null? obj.DepartmentId:Guid.Empty);
                    //}
                    ///luu lai///////////
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
            //
        }
        public bool Save_WebUser_Json(String publicKey, String token, string jsonObject)
        {//DANG SD
            //chuyen jsonObject thanh object
            var obj = JsonConvert.DeserializeObject<DTO_WebUser>(jsonObject);
            return Save_WebUser(publicKey, token, obj);


        }

        //public bool Save_WebUserList_Json(String publicKey, String token, string jsonObject)
        //{
        //    //chuyen jsonObject thanh object
        //    var list = JsonConvert.DeserializeObject<List<DTO_WebUser>>(jsonObject);
        //    if(list!=null)
        //    foreach (var obj in list)
        //    {
        //        Save_WebUser(publicKey, token, obj);
        //    }
        //    return true;
        //}
        // ////////////////////////////////////////////
        public DTO_WebUser Get_WebUserBy_Id(String publicKey, String token, Guid id)
        {//DANG SU DUNG
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                WebUser tmpObj = factory.GetByID(id);
                GetSetChild_WithBoPhanList(tmpObj);
                DTO_WebUser obj = tmpObj.Map<DTO_WebUser>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String Get_WebUserBy_Id_Json(String publicKey, String token, Guid id)
        {
            DTO_WebUser obj = Get_WebUserBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}
