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
using HRMWeb_Business.Predefined;

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

        private static void GetSetChild_WithBoPhanList(WebUser webUser)
        {
            //lay bo phan ma user duoc xu ly cham cong
            {
                if (webUser.HoSo != null)
                {
                    webUser.SoHieuCongChuc = webUser.HoSo.MaNhanVien;
                    webUser.HoVaTen = webUser.HoSo.HoTen;
                    webUser.Email = webUser.HoSo.Email;
                    try
                    {
                        //
                        webUser.TenBoPhan = webUser.HoSo.NhanVien.BoPhan1.TenBoPhan;
                    }
                    catch (Exception){  }
                }
                //
                BoPhan_Factory boPhanFactory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> tmpList = boPhanFactory.BoPhan_LayTatCaBoPhan().Map<DTO_BoPhan>();
                //
                foreach (var item in tmpList)
                {
                    var check = (from o in webUser.WebUser_BoPhan
                                 where (o.BoPhan.Oid == item.Oid)
                                 select true).FirstOrDefault();
                    if (check)
                    {
                        item.Chon = true;
                    }
                    else
                    {
                        item.Chon = false;
                    }

                }
                //var danhSachTruong = tmpList.Where(q => q.LoaiBoPhan == LoaiBoPhanConst.Truong);
                //foreach (var item in danhSachTruong)
                //{
                //    var listChild = tmpList.Where(q => q.BoPhanCha == item.Oid).Select(q => q);
                //    foreach (var child in listChild)
                //    {
                //        if (item.items == null) item.items = new List<DTO_BoPhan>();
                //        DTO_BoPhan newChild = new DTO_BoPhan();
                //        newChild = child;
                //        item.items.Add(newChild);
                //    }
                //    //if (listChild != null)
                //    //{
                //    //    if (item.items == null) item.items = new List<DTO_BoPhan>();
                //    //    item.items.AddRange(listChild);
                //    //}
                //}
                webUser.DanhSachDTO_BoPhan = tmpList.BuildTree();
            }
        }
        public IList<DTO_BoPhan> BuildTree(IEnumerable<DTO_BoPhan> source)
        {
            var groups = source.GroupBy(i => i.BoPhanCha);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        private void AddChildren(DTO_BoPhan node, IDictionary<Guid, List<DTO_BoPhan>> source)
        {
            if (source.ContainsKey(node.Oid))
            {
                node.items = source[node.Oid];
                for (int i = 0; i < node.items.Count; i++)
                    AddChildren(node.items[i], source);
            }
            else
            {
                node.items = new List<DTO_BoPhan>();
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
                    webUser.Password = newPassword.Trim();
                    webUser.LastPasswordChange = DateTime.Now;
                    webUser.UserChangePassword = idWebUser;
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
                   //
                var obj = factory.GetDTO_WebUser_ByUsernameAndPassword(username, password);
                //
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public DTO_WebUser CheckForLogin_WebUser_LDap(String publicKey, String token, string username)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                //
                var obj = factory.GetDTO_WebUser_ByUsername(username);
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
                //
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

        public IEnumerable<DTO_WebUser> GetList_WebUserQuanTri(String publicKey, String token, Guid webgroupid,Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                IEnumerable<DTO_WebUser> list = factory.GetAllDTO_WebUser_GCRecordIsNull_UserQuanTriToanQuyen(congTy, webgroupid);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_WebUserQuanTri_Json(String publicKey, String token, Guid webgroupid,Guid congTy)
        {//DANG SD
            IEnumerable<DTO_WebUser> list = GetList_WebUserQuanTri(publicKey, token, webgroupid,congTy);
            String json = string.Empty;
            if (list.Count()>0)
                json = JsonConvert.SerializeObject(list);
            return json;
        }

        /////////////khac quan tri////////////////////////////////////

        public IEnumerable<DTO_WebUser> GetList_WebUserKhacQuanTri(String publicKey, String token, Guid webgroupid, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                IEnumerable<DTO_WebUser> list = factory.GetAllDTO_WebUser_GCRecordIsNull_UserKhacQuanTriToanQuyen(congTy, webgroupid);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_WebUserKhacQuanTri_Json(String publicKey, String token,Guid webgroupid, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_WebUser> list = GetList_WebUserKhacQuanTri(publicKey, token, webgroupid,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // Save////////////////////////////////////////////
        public bool Save_WebUser(String publicKey, String token, DTO_WebUser obj, string currentUserId)
        {
            try
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
                            //
                            var newDBObject = factory.CreateManagedObject();
                            newDBObject.CopyPropertiesFrom(obj);
                            newDBObject.Oid = Guid.NewGuid();
                            if (newDBObject.ThongTinNhanVien1 != null && newDBObject.BoPhanId == null)
                                newDBObject.BoPhanId = newDBObject.ThongTinNhanVien1.NhanVien.BoPhan;
                            if (newDBObject.ThongTinNhanVien1 != null && newDBObject.CongTyId == null)
                                newDBObject.CongTyId = newDBObject.HoSo.NhanVien.CongTy;
                            objForSave = newDBObject;
                        }
                        else
                        {
                            //cap nhat
                            objFromDB.CopyIncludedPropertiesFrom(obj, "UserName", "Password", "HoatDong", "WebGroupID", "DepartmentId", "AgentObjectTypeId", "EmailHDQT", "EmailHT", "EmailTP");//.CopyPropertiesFrom(obj);
                            objForSave = objFromDB;
                            if (obj.Password != objForSave.Password)
                            {
                                objForSave.LastPasswordChange = DateTime.Now;
                                if (!string.IsNullOrEmpty(currentUserId))
                                    objForSave.UserChangePassword = new Guid(currentUserId);
                            }
                            //
                        }

                        //Phân quyền đơn vị
                        {
                            // Xóa các bộ phận đã có trước đó rồi phân quyền lại (chỉ xóa các dòng có ChucVuChinh)
                            List<WebUser_BoPhan> tempList = objForSave.WebUser_BoPhan.ToList();
                            foreach (var item in tempList)
                            {
                                if (item.ChucVuChinh == true)
                                    objForSave.WebUser_BoPhan.Remove(item);
                            }
                            //
                            Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;
                            Guid idAdmin = WebGroupConst.AdminId;
                            Guid idTruongPhong = WebGroupConst.TruongPhongID;
                            Guid idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID;
                            Guid idHieuTruong = WebGroupConst.HieuTruongID;
                            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
                            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
                            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
                            //1. Tài khoản cá nhân or Quản trị toàn quyền thì không phân quyền bộ phận

                            if (objForSave.WebGroupID != idCaNhan
                                && objForSave.WebGroupID != idAdmin)
                            {

                                if (obj.DanhSachDTO_BoPhan != null && obj.DanhSachDTO_BoPhan.ToList().Count > 0)
                                {
                                    var emailCapTren = HoSo_Factory.New().GetByID(objForSave.HoSo.Oid).Email;
                                    foreach (var dtoBoPhan in obj.DanhSachDTO_BoPhan)
                                    {
                                        if (dtoBoPhan.Chon) // Thêm mới
                                        {
                                            //
                                            if (objForSave.WebUser_BoPhan.All(x => x.BoPhanID != dtoBoPhan.Oid))
                                            {
                                                var wubp = new WebUser_BoPhan();
                                                wubp.Oid = Guid.NewGuid();
                                                wubp.IDWebUser = objForSave.Oid;
                                                wubp.BoPhanID = dtoBoPhan.Oid;
                                                wubp.ChucVuChinh = true;
                                                objForSave.WebUser_BoPhan.Add(wubp);

                                                //cập nhật email cấp trên cho tất cả nhân viên trong đơn vị mình quản lý
                                                List<WebUser> listUserOfDepartment = factory.GetByBoPhanId(dtoBoPhan.Oid).ToList();
                                                var usersKhac = listUserOfDepartment.Where(q => q.ThongTinNhanVien != objForSave.ThongTinNhanVien);
                                                foreach (var user in usersKhac)
                                                {
                                                    if (objForSave.WebGroupID == idTruongPhong || objForSave.WebGroupID == idTruongPhongUQ)
                                                        user.EmailTP = emailCapTren;
                                                    else if (objForSave.WebGroupID == idHieuTruong || objForSave.WebGroupID == idHieuTruongUQ)
                                                        user.EmailHT = emailCapTren;
                                                    else if (objForSave.WebGroupID == idHoiDongQuanTri || objForSave.WebGroupID == idHoiDongQuanTriUQ)
                                                        user.EmailHDQT = emailCapTren;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ///Lưu dữ liệu
                        try
                        {
                            factory.SaveChanges();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_WebUser/Save_WebUser", ex);
                throw ex;
            }
            //
        }
        public bool Save_WebUser_Json(String publicKey, String token, string jsonObject, string currentUserId)
        {
            //
            var obj = JsonConvert.DeserializeObject<DTO_WebUser>(jsonObject);
            return Save_WebUser(publicKey, token, obj, currentUserId);
        }

        // ////////////////////////////////////////////
        public DTO_WebUser Get_WebUserBy_Id(String publicKey, String token, Guid id)
        {//
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory factory = WebUser_Factory.New();
                DTO_WebUser tmpObj = factory.GetDTO_WebUser_ByID(id);
                return tmpObj;
            }
            else
            {
                return null;
            }
        }
        public DTO_WebUser Get_WebUser_ById(String publicKey, String token, Guid id)
        {
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
