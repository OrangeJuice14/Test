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
using HRMWeb_Business.Model.DTO.ChucNang.ChamCong;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebUser_BoPhan_Factory : BaseFactory<Entities, WebUser_BoPhan>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebUser_BoPhan_Factory.New().CreateAloneObject();
        }
        public static WebUser_BoPhan_Factory New()
        {
            return new WebUser_BoPhan_Factory();
        }
        public WebUser_BoPhan_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public WebUser_BoPhan GetBy_WebUserId_And_BoPhanId(Guid webUserId, Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.IDWebUser == webUserId && o.BoPhanID == boPhanId
                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (WebUser_BoPhan item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        public IEnumerable<DTO_WebGroup_BoPhan> GetAllWebGroupByUser(Guid webUserId)
        {
            var webUser = (from o in this.Context.WebUsers
                           where o.Oid == webUserId
                           select o).SingleOrDefault();

            List<DTO_WebGroup_BoPhan> result = new List<DTO_WebGroup_BoPhan>();
            var groupChucVuChinh = (from o in this.Context.WebUsers
                                    where o.Oid == webUserId
                                    select o.WebGroupID.Value).SingleOrDefault();
            var boPhanCuaChucVuChinh = (from o in this.Context.WebUser_BoPhan
                                        where o.IDWebUser == webUserId
                                        && o.GCRecord == null
                                        && o.ChucVuChinh == true
                                        select o.BoPhan).ToList();
            List<Guid> boPhanCuaChucVuChinhOid = new List<Guid>();
            foreach (var bp in boPhanCuaChucVuChinh)
            {
                //nếu bộ phận được phân quyền là trường thì lấy luôn tất cả phòng ban trong trường (trường hợp quản trị khối)
                boPhanCuaChucVuChinhOid.Add(bp.Oid);
                if (bp.LoaiBoPhan == Predefined.LoaiBoPhanConst.Truong)
                {
                    boPhanCuaChucVuChinhOid.AddRange(from o in this.Context.BoPhans
                                                     where o.GCRecord == null
                                                     && o.LoaiBoPhan == Predefined.LoaiBoPhanConst.PhongBan
                                                     && (o.NgungHoatDong ?? false) == false
                                                     && (o.KhongTinhLuong ?? false) == false
                                                     && o.CongTy == bp.Oid
                                                     select o.Oid);
                }
            }
            if (webUser.WebGroupID == Predefined.WebGroupConst.QuanTriTruongID)
            {
                IEnumerable<DTO_BoPhan> tmpList = BoPhan_Factory.New().BoPhan_GetLoaiBoPhanByWebGroup(webUser.WebGroupID.Value, webUser.CongTyId.Value).Map<DTO_BoPhan>();
                boPhanCuaChucVuChinhOid = tmpList.Select(q => q.Oid).ToList();
            }
            if (webUser.WebGroupID == Predefined.WebGroupConst.AdminId)
            {
                IEnumerable<DTO_BoPhan> tmpList = BoPhan_Factory.New().GetAll_GCRecordIsNull().Map<DTO_BoPhan>();
                boPhanCuaChucVuChinhOid = tmpList.Select(q => q.Oid).ToList();
            }
            DTO_WebGroup_BoPhan chucVuChinh = new DTO_WebGroup_BoPhan()
            {
                WebGroupId = groupChucVuChinh,
                BoPhanIds = boPhanCuaChucVuChinhOid
            };
            result.Add(chucVuChinh);

            var groupChucVuKiemNhiem = from o in this.Context.WebUser_BoPhan
                                       where o.IDWebUser == webUserId
                                       && o.GCRecord == null
                                       && o.ChucVuChinh == false
                                       group o by o.QuyetDinh
                                       into mygroup
                                       select mygroup.FirstOrDefault();
            foreach (var item in groupChucVuKiemNhiem)
            {
                var boPhanCuaChucVuKiemNhiem = from i in this.Context.WebUser_BoPhan
                                               where i.IDWebUser == webUserId
                                               && i.GCRecord == null
                                               && i.ChucVuChinh == false
                                               && i.QuyetDinh == item.QuyetDinh
                                               select i.BoPhanID.Value;
                DTO_WebGroup_BoPhan chucVuKiemNhiem = new DTO_WebGroup_BoPhan()
                {
                    WebGroupId = item.WebGroup.Value,
                    BoPhanIds = boPhanCuaChucVuKiemNhiem.ToList()
                };
                result.Add(chucVuKiemNhiem);
            }
            return result;
        }
        #endregion
    }//end class
}
