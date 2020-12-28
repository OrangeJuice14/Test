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
    public class CC_QuanLyChamCongNhanVien_Factory : BaseFactory<Entities, CC_QuanLyChamCongNhanVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyChamCongNhanVien_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyChamCongNhanVien_Factory New()
        {
            return new CC_QuanLyChamCongNhanVien_Factory();
        }
        public CC_QuanLyChamCongNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_QuanLyChamCongNhanVien GetByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Where(x => (x.KyTinhLuong1.Thang ?? 0) == thang && (x.KyTinhLuong1.Nam ?? 0) == nam).SingleOrDefault();
        }
        public bool ExistsByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Any(x => (x.KyTinhLuong1.Thang??0) == thang && (x.KyTinhLuong1.Nam??0) == nam);
        }

        public bool CheckKhoaQLCCNV(int thang, int nam)
        {
            CC_QuanLyChamCongNhanVien qlcc = (from o in this.ObjectSet
                                            where o.KyTinhLuong1.Thang == thang && o.KyTinhLuong1.Nam == nam
                                            select o).SingleOrDefault();
            bool result = false;
            if (qlcc.KhoaChamCong==true)
            {
                result = true;
            }
            return result;
        }

        public CC_QuanLyChamCongNhanVien GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }
        public string QuanLyChamCong_GetDepartmentOfStaff(Guid idNhanVien)
        {
            var result = "";
            var temp = (from o in this.Context.NhanViens
                          where o.Oid == idNhanVien
                        select o).SingleOrDefault();
            result =temp!=null ? temp.BoPhan1.Oid.ToString() :"";
            return result;
        }
        public IQueryable<CC_QuanLyChamCongNhanVien> GetListByIdList(List<Guid> idList)
        {
            var result = (from o in this.ObjectSet
                          where idList.Any(x => x == o.Oid)
                          select o);
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_QuanLyChamCongNhanVien item in deleteList)
            {
                var chiTietList = context.CC_ChiTietChamCongNhanVien.Where(x => x.CC_QuanLyChamCongNhanVien == item.Oid).ToArray();
                //
                CC_ChiTietChamCongNhanVien_Factory.FullDelete(context, chiTietList);
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
