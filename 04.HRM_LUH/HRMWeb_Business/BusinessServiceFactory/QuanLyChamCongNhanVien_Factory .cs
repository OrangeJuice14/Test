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
    public class QuanLyChamCongNhanVien_Factory : BaseFactory<Entities, QuanLyChamCongNhanVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return QuanLyChamCongNhanVien_Factory.New().CreateAloneObject();
        }
        public static QuanLyChamCongNhanVien_Factory New()
        {
            return new QuanLyChamCongNhanVien_Factory();
        }
        public QuanLyChamCongNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public QuanLyChamCongNhanVien GetByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Where(x => (x.KyTinhLuong1.Thang ?? 0) == thang && (x.KyTinhLuong1.Nam ?? 0) == nam).SingleOrDefault();
        }
        public bool ExistsByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Any(x => (x.KyTinhLuong1.Thang??0) == thang && (x.KyTinhLuong1.Nam??0) == nam);
        }

        public bool ExistsByThangNamBoPhanID(int thang, int nam, Guid boPhanID)
        {
            return this.ObjectSet.Any(x => (x.KyTinhLuong1.Thang ?? 0) == thang && (x.KyTinhLuong1.Nam ?? 0) == nam
                                    && x.ChiTietChamCongNhanViens.Any(y => y.BoPhan == boPhanID)
                            );
        }
        public IQueryable<QuanLyChamCongNhanVien> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         select o;
            return result;
        }

        public QuanLyChamCongNhanVien GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<QuanLyChamCongNhanVien> GetListByIdList(List<Guid> idList)
        {
            var result = (from o in this.ObjectSet
                          where idList.Any(x => x == o.Oid)
                          select o);
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (QuanLyChamCongNhanVien item in deleteList)
            {
                ChiTietChamCongNhanVien_Factory.FullDelete(context, item.ChiTietChamCongNhanViens.ToArray<Object>());
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
