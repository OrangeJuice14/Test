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
    public class CC_ChiTietChamCongNhanVien_Factory : BaseFactory<Entities, CC_ChiTietChamCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChiTietChamCongNhanVien_Factory.New().CreateAloneObject();
        }
        public static CC_ChiTietChamCongNhanVien_Factory New()
        {
            return new CC_ChiTietChamCongNhanVien_Factory();
        }
        public CC_ChiTietChamCongNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public bool CheckChot(int thang, int nam, Guid bophanId)
        {
            return this.ObjectSet.Any(x => (x.BoPhan==bophanId || x.BoPhan1.BoPhanCha == bophanId) && x.CC_QuanLyChamCong.CC_KyChamCong.Thang==thang && x.CC_QuanLyChamCong.CC_KyChamCong.Nam==nam);
        }
        public IQueryable<CC_ChiTietChamCong> GetListByIdList(List<Guid> idList)
        {
            var result = from o in this.ObjectSet
                         where idList.Any(x => x == o.Oid)
                         select o;
            return result;
        }
        public IQueryable<CC_ChiTietChamCong> GetByHoSoNhanVienID(Guid hoSoNhanVienId)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == hoSoNhanVienId
                          select o);
            return result;
        }

        public CC_QuanLyChamCong GetByThangNam(int thang,int nam)
        {
            CC_QuanLyChamCong result = (from o in this.Context.CC_QuanLyChamCong
                                                where o.CC_KyChamCong.Thang == thang
                                                    && o.CC_KyChamCong.Nam == nam  select o).SingleOrDefault();
            return result;
        }

        public IQueryable<CC_ChiTietChamCong> GetAll(Guid hoSoNhanVienId)
        {
            IQueryable<CC_ChiTietChamCong> result = (from o in this.ObjectSet
                                                              select o);
            return result;
        }

        public IQueryable<CC_ChiTietChamCong> GetByBoPhanId(Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhan == boPhanId
                          select o);
            return result;
        }
        public CC_ChiTietChamCong GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }

        public bool ExistsByThangNamBoPhanID(int thang, int nam, Guid boPhanID,Guid congTy)
        {
            return this.ObjectSet.Any(x => x.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang && x.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                      && ( x.BoPhan == boPhanID || boPhanID == Guid.Empty)
                                      && x.CC_QuanLyChamCong.CongTy == congTy);
        }

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChiTietChamCong item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
