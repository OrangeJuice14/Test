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
    public class CC_QuanLyChamCongNhanVien_Factory : BaseFactory<Entities, CC_QuanLyChamCong>
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
        public CC_QuanLyChamCong GetByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Where(x => x.CC_KyChamCong.Thang == thang && x.CC_KyChamCong.Nam == nam && x.GCRecord == null).SingleOrDefault();
        }
        public CC_QuanLyChamCong GetByKyChamCong(Guid oidKyChamCong,Guid congTy)
        {
            return this.ObjectSet.Where(x => x.CC_KyChamCong.Oid == oidKyChamCong && x.GCRecord == null && x.CongTy == congTy).SingleOrDefault();
        }

        public bool CheckKhoaQLCCNV(int thang, int nam,Guid congTy)
        {
            CC_QuanLyChamCong qlcc = (from o in this.ObjectSet
                                            where o.CC_KyChamCong.Thang == thang && o.CC_KyChamCong.Nam == nam
                                                  && o.CongTy == congTy
                                                  && o.GCRecord == null
                                            select o).FirstOrDefault();
            bool result = false;
            if (qlcc!= null && qlcc.KhoaChamCong==true)
            {
                result = true;
            }
            return result;
        }

        public CC_QuanLyChamCong GetById(Guid id)
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
        public IQueryable<CC_QuanLyChamCong> GetListByIdList(List<Guid> idList)
        {
            var result = (from o in this.ObjectSet
                          where idList.Any(x => x == o.Oid)
                          select o);
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_QuanLyChamCong item in deleteList)
            {
                var chiTietList = context.CC_ChiTietChamCong.Where(x => x.QuanLyChamCong == item.Oid).ToArray();
                //
                CC_ChiTietChamCongNhanVien_Factory.FullDelete(context, chiTietList);
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
