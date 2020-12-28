using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection;
using System.ComponentModel;
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebUser_Factory : BaseFactory<Entities, WebUser>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebUser_Factory.New().CreateAloneObject();
        }
        public static WebUser_Factory New()
        {
            return new WebUser_Factory();
        }
        public WebUser_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public WebUser GetByUsernameAndPassword(string username, string password)
        {
            string lowerUsername = username.Trim().ToLower();
            string lowerPassword = password.Trim().ToLower();
            var result = this.ObjectSet.SingleOrDefault(x => x.UserName.ToLower() == lowerUsername && x.Password.ToLower() == lowerPassword);
            return result;
        }
        public DTO_WebUser GetDTO_WebUser_ByUsernameAndPassword(string username, string password)
        {
            string lowerUsername = username.Trim().ToLower();
            string lowerPassword = password.Trim().ToLower();
            var result = (from o in this.ObjectSet
                          where o.UserName.ToLower() == lowerUsername && o.Password.ToLower() == lowerPassword
                          && (o.HoatDong ?? false) && o.GCRecord == null
                          select new DTO_WebUser()
                          {
                              Oid = o.Oid
                              ,
                              ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                              Password = o.Password
                              ,
                              UserChamCong = o.UserChamCong
                              ,
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc=o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              HoVaTen = o.HoSo.HoTen
                              ,
                              Email = o.HoSo.Email
                              ,
                              TenBoPhan =
                                  (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.BoPhan != null
                                      ? o.HoSo.NhanVien.BoPhan1.TenBoPhan
                                      : "")
                              ,
                              LoaiTaiKhoan = o.WebGroup.Name
                          }).SingleOrDefault();
            return result;
        }

        //email
        
        public DTO_WebUser GetDTO_WebUser_ByEmail(string email)
        {
            string lowerEmail = email.Trim().ToLower();
            if (!String.IsNullOrWhiteSpace(lowerEmail))
            {
                DTO_WebUser result=null;
                //Code của mr Cường
                //var result = (from o in this.ObjectSet
                //              where (o.HoSo.Email.ToLower() == lowerEmail || o.AdminEmail.ToLower() == email)
                //              select new DTO_WebUser()
                //              {
                //                  Oid = o.Oid
                //                  ,
                //                  ThongTinNhanVien = o.ThongTinNhanVien
                //                  ,
                //                  Password = o.Password
                //                  ,
                //                  UserChamCong = o.UserChamCong
                //                  ,
                //                  HoatDong = o.HoatDong
                //                  ,
                //                  WebGroupID = o.WebGroupID
                //                  ,
                //                  UserName = o.UserName
                //                  ,
                //                  SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                //                  ,
                //                  HoVaTen = o.HoSo.HoTen
                //                  ,
                //                  Email = o.HoSo.Email
                //                  ,
                //                  TenBoPhan =
                //                      (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.BoPhan != null
                //                          ? o.HoSo.NhanVien.BoPhan1.TenBoPhan
                //                          : "")
                //                  ,
                //                  LoaiTaiKhoan = o.WebGroup.Name
                //              }).SingleOrDefault();

                //Minh sửa cho trường hợp 1 email có 2 user, mặc định lấy user thường
                var temp = (from o in this.ObjectSet
                              where (o.HoSo.Email.ToLower() == lowerEmail || o.AdminEmail.ToLower() == lowerEmail || o.GiangVienThinhGiang1.NhanVien.HoSo.Email.ToLower() == lowerEmail)
                                    && o.HoSo.GCRecord == null && o.HoSo1.GCRecord == null
                              select o).ToList();
                List<DTO_WebUser> resultList = new List<DTO_WebUser>();
                foreach (WebUser o in temp)
                {
                    DTO_WebUser wd = new DTO_WebUser();
                    wd.Oid = o.Oid;
                    wd.ThongTinNhanVien = o.ThongTinNhanVien;
                    wd.Password = o.Password;
                    wd.UserChamCong = o.UserChamCong;
                    wd.HoatDong = o.HoatDong;
                    wd.WebGroupID = o.WebGroupID;
                    wd.UserName = o.UserName;
                    wd.SoHieuCongChuc = (o.HoSo!=null && o.HoSo.NhanVien.ThongTinNhanVien!=null) ? o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc:"";
                    wd.HoVaTen = o.HoSo!=null ? o.HoSo.HoTen : o.GiangVienThinhGiang1.NhanVien.HoSo.HoTen;
                    wd.Email = o.HoSo!=null ? o.HoSo.Email : o.GiangVienThinhGiang1.NhanVien.HoSo.Email;
                    wd.TenBoPhan =(o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.BoPhan != null
                                          ? o.HoSo.NhanVien.BoPhan1.TenBoPhan
                                          : "");
                    wd.LoaiTaiKhoan = o.WebGroup.Name;
                    resultList.Add(wd);
                }
                
                if (resultList.Count > 1)
                {  
                    result = resultList.Where(x => x.WebGroupID == new Guid("53D57298-1933-4E4B-B4C8-98AFED036E21")).FirstOrDefault();
                }
                else
                {
                    result = resultList.FirstOrDefault();
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        

        public WebUser GetByID(Guid oid)
        {
            //var result = (from o in this.ObjectSet
            //              where o.CC_ChamCongTheoNgayOid == oid
            //              select o).SingleOrDefault();
            var result = this.ObjectSet.SingleOrDefault(x => x.Oid == oid);
            return result;
        }
        public DTO_WebUser GetDTO_WebUser_ById(Guid oid)
        {//
            var result = (from o in this.ObjectSet
                          where //o.HoSo.GCRecord == null
                              o.Oid == oid
                          orderby o.UserName ascending
                          select new DTO_WebUser()
                          {
                              Oid = o.Oid
                              ,
                              ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                              Password = o.Password
                              ,
                              UserChamCong = o.UserChamCong
                              ,
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                             ,
                              HoVaTen = o.HoSo.HoTen
                             ,
                              Email = o.HoSo.Email
                             ,
                              TenBoPhan = (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.BoPhan != null ? o.HoSo.NhanVien.BoPhan1.TenBoPhan : "")
                          }).SingleOrDefault();
            return result;
        }
        //public IQueryable<WebUser> GetAllUserChamCong()
        //{//su dung cho cham cong only
        //    var result = from o in this.ObjectSet
        //                 where o.UserChamCong==true
        //                    && o.HoSo.GCRecord == null
        //                 orderby o.UserName ascending
        //                 select o;
        //    return result;
        //}




        public IQueryable<WebUser> GetAll_GCRecordIsNull()
        {//
            var result = from o in this.ObjectSet
                         where //o.HoSo.GCRecord == null
                             o.GCRecord == null
                         orderby o.UserName ascending
                         select o;
            return result;
        }
        public IQueryable<DTO_WebUser> GetAllDTO_WebUser_GCRecordIsNull()
        {//
            var result = from o in this.ObjectSet
                         where   o.GCRecord == null
                         orderby o.UserName ascending
                         select new DTO_WebUser()
                         {
                             Oid = o.Oid
                              ,
                             ThongTinNhanVien = o.ThongTinNhanVien == null ? o.GiangVienThinhGiang : o.ThongTinNhanVien
                              ,
                             Password = o.Password
                              ,
                             UserChamCong = o.UserChamCong
                              ,
                             HoatDong = o.HoatDong
                              ,
                             WebGroupID = o.WebGroupID
                              ,
                             UserName = o.UserName
                              ,
                             SoHieuCongChuc = o.ThongTinNhanVien == null ? o.HoSo1.MaQuanLy : o.HoSo.MaQuanLy
                            ,
                             HoVaTen = o.HoSo.HoTen == null ? o.HoSo1.HoTen : o.HoSo.HoTen
                            ,
                             Email = o.HoSo.Email == null ? o.HoSo1.Email : o.HoSo.Email
                            ,
                             TenBoPhan = o.HoSo == null ? (o.HoSo1 != null && o.HoSo1.NhanVien != null && o.HoSo1.NhanVien.BoPhan != null ? o.HoSo1.NhanVien.BoPhan1.TenBoPhan : "") : (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.BoPhan != null ? o.HoSo.NhanVien.BoPhan1.TenBoPhan : "")
                             ,
                             LoaiTaiKhoan = o.WebGroup.Name
                         };
            return result;
        }

        public IQueryable<DTO_WebUser> GetAllDTO_WebUser_GCRecordIsNull_UserQuanTriToanQuyen()
        {//
            IQueryable<DTO_WebUser> tmplist = GetAllDTO_WebUser_GCRecordIsNull();

            Guid groupThuong = new Guid("53D57298-1933-4E4B-B4C8-98AFED036E21");

            IQueryable<DTO_WebUser> list = from o in tmplist
                                           where o.WebGroupID != groupThuong
                                           orderby o.UserName ascending
                                           select o;
            return list;
        }
        public IQueryable<DTO_WebUser> GetAllDTO_WebUser_GCRecordIsNull_UserKhacQuanTriToanQuyen()
        {//
            IQueryable<DTO_WebUser> tmplist = GetAllDTO_WebUser_GCRecordIsNull();

            Guid groupThuong = new Guid("53D57298-1933-4E4B-B4C8-98AFED036E21");

            IQueryable<DTO_WebUser> list = from o in tmplist
                                           where o.WebGroupID == groupThuong
                                           orderby o.UserName ascending
                                           select o;
            return list;
        }

        //public static void FullDelete(Entities context, params Object[] deleteList)
        //{
        //    //foreach (AppL item in deleteList)
        //    //{
        //    //    context.DeleteObject(item);
        //    //}
        //}
        #endregion
    }//end class
}
