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
    public class ChiTietChamCongNgoaiGio_Factory : BaseFactory<Entities, ChiTietChamCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return ChiTietChamCongNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static ChiTietChamCongNgoaiGio_Factory New()
        {
            return new ChiTietChamCongNgoaiGio_Factory();
        }
        public ChiTietChamCongNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public bool CheckExist(Guid kytinhluong,Guid nhanvienid)
        {
            return this.ObjectSet.Any(x => x.BangChamCongNgoaiGio1.KyTinhLuong == kytinhluong);
        }
        public List<Guid?> GetListDistinctByMaBoPhan(Guid boPhanID, DateTime tungay, DateTime denngay)
        {
            List<Guid?> result = new List<Guid?>();
            result = (from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                      where o.IDBoPhan == boPhanID
                      && (o.Ngay >= tungay && o.Ngay <= denngay)
                      select o.IDNhanVien).Distinct().ToList();
            return result;
        }
        public ChiTietChamCongNgoaiGio GetByNhanVien(Guid quanLyChamCong, Guid nhanVien)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == nhanVien
                                && o.BangChamCongNgoaiGio == quanLyChamCong
                          select o).SingleOrDefault();
            return result;
        }
        public Guid? GetBoPhanByIdNhanvien(Guid nhanVien)
        {
            return (from o in this.Context.NhanViens
                    where o.Oid == nhanVien
                    select o.BoPhan).SingleOrDefault()
                    ;
        }
        public bool CheckChot(int ngay, int thang, int nam, Guid bophanId)
        {
            bool result = false;
            DateTime date = new DateTime(nam, thang, ngay);
            BangChamCongNgoaiGio bcc = (from o in this.Context.BangChamCongNgoaiGios
                                        where o.KyTinhLuong1.TuNgay <= date
                                        && o.KyTinhLuong1.DenNgay >= date
                                        select o
                                      ).FirstOrDefault();
            if (bcc!=null)
            {
                result = this.ObjectSet.Any(c => c.BangChamCongNgoaiGio == bcc.Oid && c.BoPhan == bophanId);
            }
            return result;
        }
        public bool CheckChotByKy(Guid bophanId, Guid kyTinhLuong)
        {
            bool result = false;
            BangChamCongNgoaiGio bcc = (from o in this.Context.BangChamCongNgoaiGios
                                        where o.KyTinhLuong== kyTinhLuong
                                        select o
                                      ).FirstOrDefault();
            if (bcc != null)
            {
                result = this.ObjectSet.Any(c => c.BangChamCongNgoaiGio == bcc.Oid && c.BoPhan==bophanId);
            }
            return result;
        }
        #endregion
    }//end class
}
