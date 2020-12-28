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
using HRMWeb_Business.Predefined;

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
            //
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
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc= o.HoSo != null ? o.HoSo.MaNhanVien : ""
                              ,
                              HoVaTen = o.HoSo!= null ?o.HoSo.HoTen : ""
                              ,
                              Email = o.HoSo!= null? o.HoSo.Email : ""
                              ,
                              TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                              ,
                              LoaiTaiKhoan = o.WebGroup != null  ? o.WebGroup.Name : ""
                              , 
                              EmailHDQT = o.EmailHDQT
                             ,
                              EmailHT = o.EmailHT
                             ,
                              EmailTP = o.EmailTP
                              ,
                              CongTyId = o.CongTyId

                          }).SingleOrDefault();
            return result;
        }

        public DTO_WebUser GetDTO_WebUser_ByUsername(string username)
        {
            string lowerUsername = username.Trim().ToLower();
            var result = (from o in this.ObjectSet
                          where o.UserName.ToLower() == lowerUsername
                          && (o.HoatDong ?? false) && o.GCRecord == null
                          select new DTO_WebUser()
                          {
                              Oid = o.Oid
                              ,
                              ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                              Password = o.Password
                              ,
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc = o.HoSo!= null ? o.HoSo.MaNhanVien : ""
                              ,
                              HoVaTen = o.HoSo != null ?  o.HoSo.HoTen : ""
                              ,
                              Email = o.HoSo != null ?  o.HoSo.Email : ""
                              ,
                              TenBoPhan = o.ThongTinNhanVien1 != null ?  o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                              ,
                              LoaiTaiKhoan = o.WebGroup !=null ? o.WebGroup.Name : ""
                             ,
                              EmailHDQT = o.EmailHDQT
                             ,
                              EmailHT = o.EmailHT
                             ,
                              EmailTP = o.EmailTP
                              ,
                              CongTyId = o.CongTyId

                          }).FirstOrDefault();
            return result;
        }

        public DTO_WebUser GetDTO_WebUser_ByEmail(string email)
        {
            string lowerEmail = email.Trim().ToLower();
            if (!String.IsNullOrWhiteSpace(lowerEmail))
            {
                DTO_WebUser result=null;

                //
                var temp = (from o in this.ObjectSet
                              where (o.ThongTinNhanVien1.EmailNoiBo.ToLower() == lowerEmail)
                                     && o.HoSo.GCRecord == null
                              select o).ToList();
                List<DTO_WebUser> resultList = new List<DTO_WebUser>();
                foreach (WebUser o in temp)
                {
                    DTO_WebUser wd = new DTO_WebUser();
                    wd.Oid = o.Oid;
                    wd.ThongTinNhanVien = o.ThongTinNhanVien;
                    wd.Password = o.Password;
                    wd.HoatDong = o.HoatDong;
                    wd.WebGroupID = o.WebGroupID;
                    wd.UserName = o.UserName;
                    wd.SoHieuCongChuc = o.HoSo !=null ? o.HoSo.MaNhanVien : "";
                    wd.HoVaTen = o.HoSo != null ?  o.HoSo.HoTen : "";
                    wd.Email = o.HoSo != null ?  o.HoSo.Email : "";
                    wd.TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : "";
                    wd.LoaiTaiKhoan = o.WebGroup!=null ? o.WebGroup.Name : "";
                    wd.EmailHT = o.EmailHT;
                    wd.EmailHDQT = o.EmailHDQT;
                    wd.EmailTP = o.EmailTP;
                    wd.CongTyId = o.CongTyId;
                    //
                    resultList.Add(wd);
                }
                
                if (resultList.Count > 1)
                {  
                    result = resultList.Where(x => x.WebGroupID == new Guid("00000000-0000-0000-0000-000000000008")).FirstOrDefault();
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

        public DTO_WebUser GetDTO_WebUser_ByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid && o.GCRecord == null
                          select new DTO_WebUser()
                          {
                              Oid = o.Oid
                              ,
                              ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                              Password = o.Password
                              ,
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc = o.HoSo != null ? o.HoSo.MaNhanVien : ""
                              ,
                              HoVaTen = o.HoSo!= null ? o.HoSo.HoTen : ""
                              ,
                              Email = o.HoSo != null ?  o.HoSo.Email : ""
                              ,
                              TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                              ,
                              LoaiTaiKhoan = o.WebGroup != null ? o.WebGroup.Name : ""
                             ,
                              EmailHT = o.EmailHT
                             ,
                              EmailHDQT = o.EmailHDQT
                             ,
                              EmailTP = o.EmailTP
                              ,
                              CongTyId = o.CongTyId

                          }).SingleOrDefault();
            return result;
        }
        public WebUser GetByID(Guid oid)
        {
            var result = this.ObjectSet.SingleOrDefault(x => x.Oid == oid);
            return result;
        }
        public IEnumerable<WebUser> GetByBoPhanId(Guid boPhanId)
        {
            var result = this.ObjectSet.Where(x => x.BoPhanId == boPhanId);
            return result;
        }
        public DTO_WebUser GetDTO_WebUser_ById(Guid oid)
        {//
            var result = (from o in this.ObjectSet
                          where  o.Oid == oid
                          orderby o.UserName ascending
                          select new DTO_WebUser()
                          {
                              Oid = o.Oid
                              ,
                              ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                              Password = o.Password
                              ,
                              HoatDong = o.HoatDong
                              ,
                              WebGroupID = o.WebGroupID
                              ,
                              UserName = o.UserName
                              ,
                              SoHieuCongChuc = o.HoSo != null ? o.HoSo.MaNhanVien : ""
                             ,
                              HoVaTen = o.HoSo != null ?  o.HoSo.HoTen : ""
                             ,
                              Email = o.HoSo != null ?  o.HoSo.Email : ""
                             ,
                              TenBoPhan = o.ThongTinNhanVien1 != null ?  o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                              ,
                              BoPhanId = o.BoPhanId
                             ,
                              EmailHDQT = o.EmailHDQT
                             ,
                              EmailHT = o.EmailHT
                             ,
                              EmailTP = o.EmailTP
                              ,
                              CongTyId = o.CongTyId

                          }).SingleOrDefault();
            return result;
        }

        public IQueryable<WebUser> GetAll_GCRecordIsNull()
        {//
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
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
                             ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                             Password = o.Password
                              ,
                             HoatDong = o.HoatDong
                              ,
                             WebGroupID = o.WebGroupID
                              ,
                             UserName = o.UserName
                              ,
                             SoHieuCongChuc = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien : ""
                            ,
                             HoVaTen = o.HoSo != null ? o.HoSo.HoTen : ""
                            ,
                             Email = o.HoSo != null ? o.HoSo.Email : ""
                            ,
                             TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                             ,
                             LoaiTaiKhoan = o.WebGroup != null ? o.WebGroup.Name : ""
                             ,
                             EmailHDQT = o.EmailHDQT
                             ,
                             EmailHT = o.EmailHT
                             ,
                             EmailTP = o.EmailTP
                              ,
                             CongTyId = o.CongTyId
                         };
            return result;
        }

        public IQueryable<DTO_WebUser> GetAllDTO_WebUser_GCRecordIsNull_UserQuanTriToanQuyen(Guid congTy, Guid webgroupid)
        {
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null && o.WebGroupID != idCaNhan
                               && ( (o.CongTyId == congTy && !webgroupid.Equals(idAdmin) )
                                    || webgroupid.Equals(idAdmin))
                         orderby o.UserName ascending
                         select new DTO_WebUser()
                         {
                             Oid = o.Oid
                              ,
                             ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                             Password = o.Password
                              ,
                             HoatDong = o.HoatDong
                              ,
                             WebGroupID = o.WebGroupID
                              ,
                             UserName = o.UserName
                              ,
                             SoHieuCongChuc = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien : ""
                            ,
                             HoVaTen = o.HoSo != null ? o.HoSo.HoTen : ""
                            ,
                             Email = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.EmailNoiBo : ""
                            ,
                             TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                             ,
                             LoaiTaiKhoan = o.WebGroup != null ? o.WebGroup.Name : ""
                             ,
                             EmailHT = o.EmailHT
                             ,
                             EmailHDQT = o.EmailHDQT
                             ,
                             EmailTP = o.EmailTP
                              ,
                             CongTyId = o.CongTyId
                         };
            return result;
        }
        public IQueryable<DTO_WebUser> GetAllDTO_WebUser_GCRecordIsNull_UserKhacQuanTriToanQuyen(Guid congTy, Guid webgroupid)
        {
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null && o.WebGroupID == idCaNhan
                               && ((o.CongTyId == congTy && !webgroupid.Equals(idAdmin))
                                    || webgroupid.Equals(idAdmin))
                         orderby o.UserName ascending
                         select new DTO_WebUser()
                         {
                             Oid = o.Oid
                              ,
                             ThongTinNhanVien = o.ThongTinNhanVien
                              ,
                             Password = o.Password
                              ,
                             HoatDong = o.HoatDong
                              ,
                             WebGroupID = o.WebGroupID
                              ,
                             UserName = o.UserName
                              ,
                             SoHieuCongChuc = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien : ""
                            ,
                             HoVaTen = o.HoSo != null ? o.HoSo.HoTen : ""
                            ,
                             Email = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.EmailNoiBo : ""
                            ,
                             TenBoPhan = o.ThongTinNhanVien1 != null ? o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan : ""
                             ,
                             LoaiTaiKhoan = o.WebGroup != null ? o.WebGroup.Name : ""
                             ,
                             EmailHT = o.EmailHT
                             ,
                             EmailHDQT = o.EmailHDQT
                             ,
                             EmailTP = o.EmailTP
                              ,
                             CongTyId = o.CongTyId
                         };
            return result;
        }
        #endregion
    }//end class
}
