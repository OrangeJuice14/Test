using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using System.Web.Configuration;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class BoPhan_Factory : BaseFactory<Entities, BoPhan>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return BoPhan_Factory.New().CreateAloneObject();
        }
        public static BoPhan_Factory New()
        {
            return new BoPhan_Factory();
        }
        public BoPhan_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public IQueryable<BoPhan> GetAll_GCRecordIsNull()
        {//su dung cho cham cong only
            Guid idThongTinTruong = new Guid(WebConfigurationManager.AppSettings["SchoolGuid"]);
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                          //trường SPKT
                          && o.BoPhanCha == idThongTinTruong
                         orderby o.STT ascending, o.TenBoPhan ascending
                         select o;
            return result;
        }
        public BoPhan GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<BoPhan> GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(Guid webUserId)
        {//su dung cho cham cong only
            Guid idThongTinTruong = new Guid(WebConfigurationManager.AppSettings["SchoolGuid"]);
            //
            var result = (from o in this.Context.BoPhans
                          where o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                          //trường SPKT
                          && o.BoPhanCha == idThongTinTruong
                          && o.GCRecord == null
                          orderby o.STT ascending, o.TenBoPhan ascending
                          select o);
            return result;
        }

        public List<BoPhan> GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_All(Guid webUserId)
        {//su dung cho cham cong only

            Guid idThongTinTruong = new Guid(WebConfigurationManager.AppSettings["SchoolGuid"]);
            //
            var result = (from o in this.Context.BoPhans
                          where o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                          && o.BoPhanCha == idThongTinTruong
                          && o.GCRecord == null
                          orderby o.STT ascending, o.TenBoPhan ascending
                          select o).ToList();
            return result;
        }
        private bool GetBoPhanInCC_ChiTietChamCongNhanVien(int thang, int nam, Guid idBoPhan)
        {
            var result = (from x in this.Context.CC_ChiTietChamCongNhanVien
                          where x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                && x.BoPhan.Value == idBoPhan
                          select true).FirstOrDefault();
              return result;
        }
        public IEnumerable<DTO_KiemTraPhongBanXetABC> KiemTraPhongBanXetABC_Find(int thang, int nam, Boolean? daXetXongABC)
        {
            Boolean tatCaTrangThaiXetABC = daXetXongABC == null ? true : false;
            Guid idThongTinTruong = new Guid(WebConfigurationManager.AppSettings["SchoolGuid"]);
            //
            IEnumerable<DTO_KiemTraPhongBanXetABC> query = null;
            //
            List<Guid> boPhanChuaXet_List = (from o in this.Context.BoPhans
                                             join x in this.Context.CC_ChiTietChamCongNhanVien on o.Oid equals x.BoPhan
                                             where  (x.TrangThai ?? false) == false
                                                    &&  o.BoPhanCha == idThongTinTruong
                                                    && o.GCRecord == null
                                                    && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                             select o.Oid).Distinct().ToList();
            //
            List<Guid> boPhandaXet_List = (from o in this.Context.BoPhans
                                           join x in this.Context.CC_ChiTietChamCongNhanVien on o.Oid equals x.BoPhan
                                           where   o.BoPhanCha == idThongTinTruong 
                                                   && o.GCRecord == null
                                                   && x.TrangThai == true
                                                   && !boPhanChuaXet_List.Contains(o.Oid)
                                                   && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                   && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                           select o.Oid).Distinct().ToList();
            //
            if (!tatCaTrangThaiXetABC)
            {
                if (daXetXongABC.Value)
                {
                    query = (from o in this.Context.BoPhans
                             where o.BoPhanCha == idThongTinTruong
                                   && o.GCRecord == null
                                   && boPhandaXet_List.Contains(o.Oid)
                             select new DTO_KiemTraPhongBanXetABC() { TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Đã xét" });
                }
                //
                else if (!daXetXongABC.Value)
                {
                    query = (from o in this.Context.BoPhans
                             where o.BoPhanCha == idThongTinTruong
                                   && o.GCRecord == null
                                   && !boPhandaXet_List.Contains(o.Oid)
                             select new DTO_KiemTraPhongBanXetABC() { TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Chưa xét" });
                }
            }
            else
            {
               var queryDaXet = (from o in this.Context.BoPhans
                         where o.BoPhanCha == idThongTinTruong
                               && o.GCRecord == null
                                   && boPhandaXet_List.Contains(o.Oid)
                                 select new DTO_KiemTraPhongBanXetABC() { TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Đã xét" });

                var queryChuaXet = (from o in this.Context.BoPhans
                                  where o.BoPhanCha == idThongTinTruong
                                        && o.GCRecord == null
                                        && !boPhandaXet_List.Contains(o.Oid)
                                    select new DTO_KiemTraPhongBanXetABC() { TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Chưa xét" });
                //
                query = queryDaXet.Union(queryChuaXet);
            }
            //
            return query;
        }
        #endregion
    }//end class
}
