using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {


        public IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> CaNhanDangKyChamCongNgoaiGio_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                var tmpList = factory.CaNhanDangKyChamCongNgoaiGio_Find(thang, nam, idNhanVien).ToList();
                IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_DangKyChamCongNgoaiGio>();              
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> QuanLyDangKyChamCongNgoaiGio_Find(String publicKey, String token, DateTime? tuNgay, DateTime? denNgay, Guid IDBoPhan,byte? trangthai)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                var tmpList = factory.QuanLyDangKyChamCongNgoaiGio_Find(tuNgay, denNgay, IDBoPhan,trangthai).ToList();
                IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_DangKyChamCongNgoaiGio>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String CaNhanDangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> list = CaNhanDangKyChamCongNgoaiGio_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyDangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, DateTime? tuNgay, DateTime? denNgay, Guid IDBoPhan,byte? trangthai)
        {//DANG SD
            IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> list = QuanLyDangKyChamCongNgoaiGio_Find(publicKey, token, tuNgay, denNgay, IDBoPhan, trangthai);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool Save_DangKyChamCongNgoaiGio_Json(String publicKey, String token, string jsonObject, Guid idwebuser)
        {//DANG SD
            //chuyen jsonObject thanh object
            var obj = JsonConvert.DeserializeObject<DTO_CC_DangKyChamCongNgoaiGio>(jsonObject);
            return Save_DangKyChamCongNgoaiGio(publicKey, token, obj, idwebuser);
        }

        public bool Save_DangKyChamCongNgoaiGio(String publicKey, String token, DTO_CC_DangKyChamCongNgoaiGio obj, Guid idwebuser)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    try
                    {
                        var factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                        var nv = (from o in factory.Context.ThongTinNhanViens
                                  where o.Oid == new Guid(obj.ThongTinNhanVien)
                                  select o).SingleOrDefault();
                        List<int> ListThu = new List<int>();
                        foreach (DTO_Thu t in obj.DanhSachDTO_Thu)
                        {
                            if (t.Chon == true)
                            {
                                ListThu.Add(t.Id);
                            }
                        }
                        DateTime date = obj.TuNgay;
                        while (date <= obj.DenNgay)
                        {
                            byte Thu = Convert.ToByte(date.DayOfWeek);
                            foreach (int t in ListThu)
                            {
                                if (Thu == t)
                                {
                                    CC_DangKyChamCongNgoaiGio objForSave = null;
                                    var newDBObject = factory.CreateManagedObject();
                                    newDBObject.Oid = Guid.NewGuid();
                                    newDBObject.IDNhanVien = new Guid(obj.ThongTinNhanVien);
                                    newDBObject.ThongTinNhanVien = nv;
                                    newDBObject.Ngay = date;
                                    newDBObject.LyDo = obj.LyDo;
                                    newDBObject.IDBoPhan = nv.NhanVien.BoPhan1.Oid;
                                    newDBObject.Duyet = 0;
                                    newDBObject.SoPhutDangKy = (ParseTime(obj.GioKetThuc, obj.PhutKetThuc) - ParseTime(obj.GioBatDau, obj.PhutBatDau))*60;
                                    newDBObject.TuGio = ParseTimeString(obj.GioBatDau, obj.PhutBatDau);
                                    newDBObject.DenGio = ParseTimeString(obj.GioKetThuc, obj.PhutKetThuc);
                                    newDBObject.NgayTao = DateTime.Now;
                                    newDBObject.NguoiTao = idwebuser;
                                    objForSave = newDBObject;
                                    factory.SaveChanges();

                                }
                            }
                            date = date.AddDays(1.0);
                        }
                        return true;

                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
            //
        }
        public bool DangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_DangKyChamCongNgoaiGio_Factory factory = CC_DangKyChamCongNgoaiGio_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {

                            CC_DangKyChamCongNgoaiGio stupidObj = new CC_DangKyChamCongNgoaiGio() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_DangKyChamCongNgoaiGio_Factory.FullDelete(factory.Context, stupidObj);
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return false;
            }
            return false;
        }
       
        public bool CaNhanDangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList)
        {//ca nhan chi duoc phep xoa nhung dong dang cho xet
            CC_DangKyChamCongNgoaiGio_Factory tmpFactory = CC_DangKyChamCongNgoaiGio_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                TimeSpan.FromSeconds(360)))
            {
                bool daXoaDuocItNhat1Dong = false;
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    if (objFromDB.Duyet == 0)
                    {
                        bool xoaDuocDongHienTai = DangKyChamCongNgoaiGio_DeleteList(publicKey, token,
                              new List<DTO_CC_DangKyChamCongNgoaiGio>() { obj.Map<DTO_CC_DangKyChamCongNgoaiGio>() });
                        if (daXoaDuocItNhat1Dong == false && xoaDuocDongHienTai == true)
                        {
                            daXoaDuocItNhat1Dong = true;
                        }
                    }
                }
                transaction.Complete();
                return daXoaDuocItNhat1Dong;
            }
        }
        public bool QuanLyDangKyChamCongNgoaiGio_DuyetList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList,byte trangthai)
        {
            CC_DangKyChamCongNgoaiGio_Factory tmpFactory = CC_DangKyChamCongNgoaiGio_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                TimeSpan.FromSeconds(360)))
            {
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    objFromDB.Duyet = trangthai;
                    tmpFactory.SaveChangesWithoutTransactionScope();
                }
                transaction.Complete();
            }
            return false;
        }
        public bool CaNhanDangKyChamCongNgoaiGio_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_DangKyChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_DangKyChamCongNgoaiGio>>(jsonObjectList);
            return CaNhanDangKyChamCongNgoaiGio_DeleteList(publicKey, token, objList);
        }
        public bool QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(String publicKey, String token, string jsonObjectList, byte trangthai)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_DangKyChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_DangKyChamCongNgoaiGio>>(jsonObjectList);
            return QuanLyDangKyChamCongNgoaiGio_DuyetList(publicKey, token, objList,trangthai);
        }


    }
}
