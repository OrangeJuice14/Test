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
                              SoHieuCongChuc = o.HoSo.MaQuanLy
                              ,
                              HoVaTen = o.HoSo.HoTen
                              ,
                              Email = o.HoSo.Email
                              ,
                              TenBoPhan =
                                  (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.Department != null
                                      ? o.HoSo.NhanVien.Department1.TenBoPhan
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
                var result = (from o in this.ObjectSet
                              where (o.HoSo.Email.ToLower() == lowerEmail || o.AdminEmail.ToLower() == email)
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
                                  SoHieuCongChuc = o.HoSo.MaQuanLy
                                  ,
                                  HoVaTen = o.HoSo.HoTen
                                  ,
                                  Email = o.HoSo.Email
                                  ,
                                  TenBoPhan =
                                      (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.Department != null
                                          ? o.HoSo.NhanVien.Department1.TenBoPhan
                                          : "")
                                  ,
                                  LoaiTaiKhoan = o.WebGroup.Name
                              }).SingleOrDefault();
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
                              SoHieuCongChuc = o.HoSo.MaQuanLy
                             ,
                              HoVaTen = o.HoSo.HoTen
                             ,
                              Email = o.HoSo.Email
                             ,
                              TenBoPhan = (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.Department != null ? o.HoSo.NhanVien.Department1.TenBoPhan : "")
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
                         where //o.HoSo.GCRecord == null
                             o.GCRecord == null
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
                             SoHieuCongChuc = o.HoSo.MaQuanLy
                            ,
                             HoVaTen = o.HoSo.HoTen
                            ,
                             Email = o.HoSo.Email
                            ,
                             TenBoPhan = (o.HoSo != null && o.HoSo.NhanVien != null && o.HoSo.NhanVien.Department != null ? o.HoSo.NhanVien.Department1.TenBoPhan : "")
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
