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
using NHibernate.Linq;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebMenu_Factory : BaseFactory<Entities, HRMWeb_Business.Model.WebMenu>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebMenu_Factory.New().CreateAloneObject();
        }
        public static WebMenu_Factory New()
        {
            return new WebMenu_Factory();
        }
        public WebMenu_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public HRMWeb_Business.Model.WebMenu GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public override IQueryable<HRMWeb_Business.Model.WebMenu> GetAll()
        {
            var result = (from o in this.ObjectSet
                          where o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<HRMWeb_Business.Model.WebMenu> GetChildWebMenuList(Guid parentId)
        {
            var result = (from o in this.ObjectSet
                          where o.ParentId == parentId && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListBy_WebUserId(Guid webUserId,Guid congTy)
        {
            Guid idYerSin = BoPhanConst.YerSinGuid;
            //
            if (congTy.Equals(idYerSin))
            {
                bool giangVien = false;
                WebUser currentUser = this.Context.WebUsers.Where(x=>x.Oid == webUserId).SingleOrDefault();
                if (currentUser != null)
                {
                    if (currentUser.ThongTinNhanVien1.LoaiNhanSu.Equals(LoaiNhanSuConst.GiangVienId)
                        || (currentUser.ThongTinNhanVien1.GiangDay != null && currentUser.ThongTinNhanVien1.GiangDay.Value))
                    {
                        giangVien = true;
                    }
                }
                //
                if (giangVien) // Giảng viên thì link uis nhân viên thì không
                {
                    var result = (from o in this.ObjectSet
                                  where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                                        && o.Active == true
                                  orderby o.Global_idx ascending, o.Local_idx ascending
                                  select o);

                    //
                    return result;
                }
                else
                {
                    var result = (from o in this.ObjectSet
                                  where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                                        && o.Active == true
                                        && !o.Name.Equals("Thông tin đào tạo")
                                  orderby o.Global_idx ascending, o.Local_idx ascending
                                  select o);

                    //
                    return result;
                }
            }
            else
            {
                //Nếu khác yersin thì bỏ menu uis đi

                var result = (from o in this.ObjectSet
                              where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                                    && o.Active == true
                                    && !o.Name.Equals("Thông tin đào tạo")
                              orderby o.Global_idx ascending, o.Local_idx ascending
                              select o);
                //
                return result;
            }
        }

        public IQueryable<String> GetURLListBy_WebUserId(Guid webUserId)
        {
            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                                && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o.Url);
            return result;
        }
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListTop2LevelDeepBy_WebUserId(Guid webUserId,Guid congTy)
        {
            var result = (from o in GetListBy_WebUserId(webUserId, congTy)
                          where (o.ParentId == Guid.Empty || o.WebMenu2.ParentId == Guid.Empty || o.ParentId == new Guid("00000000-0000-0000-0000-000000000001"))
                                 && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetChildMenuList_ByWebUserId_AndMenuId(Guid webUserId, Guid menuId,Guid congTy)
        {
            var result = (from o in GetListBy_WebUserId(webUserId, congTy)
                          where o.ParentId == menuId && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetChildMenuList_ByMenuId(Guid menuId)
        {
            var result = (from o in this.ObjectSet
                          where o.ParentId == menuId && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListBy_WebGroupId(Guid webGroupId)
        {
            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.Oid == webGroupId) && o.Active == true
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        #endregion
    }//end class
}
