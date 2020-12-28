using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Web.Configuration;
using System.Transactions;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public String QuanLyChamCongNgoaiGio_Find_Json(String publicKey, String token, Guid? boPhanId, Guid kytinhluong)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCongNgoaiGio_Find> list = QuanLyChamCongNgoaiGio_Find(publicKey, token, boPhanId, kytinhluong);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String ChamCongNgoaiGioTheoNgay_Find_Json(String publicKey, String token, Guid? boPhanId, string kytinhluong, string webGroupId)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCongNgoaiGio_Find> list = ChamCongNgoaiGioTheoNgay_Find(publicKey, token, boPhanId, kytinhluong, webGroupId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String ChamCongNgoaiGioTheoNgayThayDoi_GetByOid_Json(String publicKey, String token,Guid Oid)
        {//DANG SD
            DTO_ChamCongNgoaiGioTheoNgayThayDoi list = ChamCongNgoaiGioTheoNgayThayDoi_GetByOid(publicKey, token, Oid);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_QuanLyChamCongNgoaiGio_Find> QuanLyChamCongNgoaiGio_Find(String publicKey, String token, Guid? boPhanId, Guid kytinhluong)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                QuanLyChamCongNgoaiGio_Factory factory = QuanLyChamCongNgoaiGio_Factory.New();
                IEnumerable<DTO_QuanLyChamCongNgoaiGio_Find> list = null;
                list = factory.QuanLyChamCongNgoaiGio_Find(boPhanId, kytinhluong);
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanLyChamCongNgoaiGio_Find> ChamCongNgoaiGioTheoNgay_Find(String publicKey, String token, Guid? boPhanId, string kytinhluong, string webGroupId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                QuanLyChamCongNgoaiGio_Factory factory = QuanLyChamCongNgoaiGio_Factory.New();
                ChamCongNgoaiGioTheoNgayThayDoi_Factory ngFact = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();
                List<DTO_QuanLyChamCongNgoaiGio_Find> list = null;
                KyTinhLuong kyTinhLuong = KyTinhLuong_Factory.New().GetKyTinhLuong_GCRecordIsNull_ById(new Guid(kytinhluong));
                var tuNgay = kyTinhLuong.TuNgay.HasValue ? kyTinhLuong.TuNgay.Value : DateTime.Now;
                var denNgay = kyTinhLuong.DenNgay.HasValue ? kyTinhLuong.DenNgay.Value : DateTime.Now;
                //Nếu user là admin thì lấy list bình thường
                if (webGroupId.ToLower() == WebConfigurationManager.AppSettings["AdminGroupId"].ToLower())
                {
                    list = factory.ChamCongNgoaiGioTheoNgay_Find(boPhanId, tuNgay, denNgay).ToList();
                }
                //Nếu là thư ký hoặc trưởng phòng thì lấy list của đơn vị
                else
                {
                    list = factory.ChamCongNgoaiGioTheoNgayDonVi_Find(boPhanId, tuNgay, denNgay).ToList();
                }
                foreach (DTO_QuanLyChamCongNgoaiGio_Find item in list)
                {
                    bool check = ngFact.CheckExist(item.Oid);
                    if (check)
                        item.DaChinhSua = 1;
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        public DTO_ChamCongNgoaiGioTheoNgayThayDoi ChamCongNgoaiGioTheoNgayThayDoi_GetByOid(String publicKey, String token, Guid Oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChamCongNgoaiGioTheoNgayThayDoi_Factory factory = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();
                DTO_ChamCongNgoaiGioTheoNgayThayDoi list = null;
                list = factory.GetByID(Oid).Map<DTO_ChamCongNgoaiGioTheoNgayThayDoi>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool ChamCongNgoaiGio_CheckExists(String publicKey, String token, Guid kytinhluong, Guid nhanvienid)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChiTietChamCongNgoaiGio_Factory.New();
                bool daTonTai = factory.CheckExist(kytinhluong, nhanvienid);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChamCongNgoaiGioTheoNgay_CheckExists(String publicKey, String token, DateTime ngay, Guid nhanvienid)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                bool daTonTai = factory.CheckExist(ngay, nhanvienid);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChamCongNgoaiGioTheoNgay_CheckNgay(String publicKey, String token, Guid kytinhluong, DateTime ngay)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                bool daTonTai = factory.CheckNgay(kytinhluong, ngay);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public DTO_QuanLyChamCongNgoaiGio_Find QuanLyChamCongNgoaiGio_TaoMoi(String publicKey, String token, Guid nhanVienID, Guid kytinhluong, decimal SoCongNgoaiGio, decimal SoCongNgoaiGioSau23Gio,
            decimal SoCongNgoaiGioT7CN, decimal SoCongNgoaiGioT7CNSau23Gio, decimal SoCongNgoaiGioLe, decimal SoCongNgoaiGioLeSau23Gio)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                QuanLyChamCongNgoaiGio_Factory qlfac = new QuanLyChamCongNgoaiGio_Factory();
                BangChamCongNgoaiGio bcc = qlfac.GetByKyTinhLuong(kytinhluong);
                if (bcc == null)
                {
                    try
                    {
                        bcc = qlfac.CreateManagedObject();
                        bcc.Oid = Guid.NewGuid();
                        bcc.KyTinhLuong = kytinhluong;
                        bcc.NgayLap = DateTime.Now;
                        bcc.ThongTinTruong = new Guid(WebConfigurationManager.AppSettings["ThongTinTruong"]);
                        qlfac.SaveChanges();
                    }
                    catch (Exception e)
                    {

                    }
                }
                HoSo_Factory hsfac = new HoSo_Factory();
                HoSo nv = hsfac.GetByID(nhanVienID);
                ChiTietChamCongNgoaiGio_Factory factory = ChiTietChamCongNgoaiGio_Factory.New();
                ChiTietChamCongNgoaiGio newObj = factory.CreateManagedObject();
                if (nv != null)
                {
                    newObj.Oid = Guid.NewGuid();
                    newObj.BangChamCongNgoaiGio = bcc.Oid;
                    newObj.BoPhan = nv.NhanVien.BoPhan;
                    newObj.ThongTinNhanVien = nv.Oid;
                    newObj.SoCongNgoaiGio = SoCongNgoaiGio;
                    newObj.SoCongNgoaiGioSau23Gio = SoCongNgoaiGioSau23Gio;
                    newObj.SoCongNgoaiGio1 = SoCongNgoaiGioT7CN;
                    newObj.SoCongNgoaiGio1Sau23Gio = SoCongNgoaiGioT7CNSau23Gio;
                    newObj.SoCongNgoaiGio2 = SoCongNgoaiGioLe;
                    newObj.SoCongNgoaiGio2Sau23Gio = SoCongNgoaiGioLeSau23Gio;
                    factory.SaveChanges();
                }
                var returnObj = newObj.Map<DTO_QuanLyChamCongNgoaiGio_Find>();
                return returnObj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool SaveChamCongNgoaiGioTheoNgayThayDoi(CC_ChamCongNgoaiGioTheoNgay objFromDB, DTO_ChamCongNgoaiGioTheoNgay obj)
        {
            ChamCongNgoaiGioTheoNgayThayDoi_Factory ccFactory = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();
            CC_ChamCongNgoaiGioTheoNgayThayDoi ccObjSave = ccFactory.CreateManagedObject();
            ccObjSave.Oid = objFromDB.Oid;
            ccObjSave.SoCongNgoaiGio = obj.SoCongNgoaiGio;
            ccObjSave.SoCongNgoaiGioDonVi = objFromDB.SoCongNgoaiGio;
            ccObjSave.SoCongNgoaiGioLe = obj.SoCongNgoaiGioLe;
            ccObjSave.SoCongNgoaiGioLeDonVi = objFromDB.SoCongNgoaiGioLe;
            ccObjSave.SoCongNgoaiGioLeSau23Gio = obj.SoCongNgoaiGioLeSau23Gio;
            ccObjSave.SoCongNgoaiGioLeSau23GioDonVi = objFromDB.SoCongNgoaiGioLeSau23Gio;
            ccObjSave.SoCongNgoaiGioSau23Gio = obj.SoCongNgoaiGioSau23Gio;
            ccObjSave.SoCongNgoaiGioSau23GioDonVi = objFromDB.SoCongNgoaiGioSau23Gio;
            ccObjSave.SoCongNgoaiGioT7CN = obj.SoCongNgoaiGioT7CN;
            ccObjSave.SoCongNgoaiGioT7CNDonVi = objFromDB.SoCongNgoaiGioT7CN;
            ccObjSave.SoCongNgoaiGioT7CNSau23Gio = obj.SoCongNgoaiGioT7CNSau23Gio;
            ccObjSave.SoCongNgoaiGioT7CNSau23GioDonVi = objFromDB.SoCongNgoaiGioT7CNSau23Gio;
            ccObjSave.IDUserDonVi = objFromDB.IDUser;
            ccFactory.SaveChanges();
            return true;
        }
        public DTO_QuanLyChamCongNgoaiGio_Find ChamCongNgoaiGioTheoNgay_TaoMoi(String publicKey, String token, Guid nhanVienID, Guid kytinhluong, DateTime ngay, decimal? SoCongNgoaiGio, decimal? SoCongNgoaiGioSau23Gio,
           decimal? SoCongNgoaiGioT7CN, decimal? SoCongNgoaiGioT7CNSau23Gio, decimal? SoCongNgoaiGioLe, decimal? SoCongNgoaiGioLeSau23Gio, int GioTuGio, int PhutTuGio, int GioDenGio, int PhutDenGio, string idUser)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                SoCongNgoaiGio = SoCongNgoaiGio ?? 0;
                SoCongNgoaiGioSau23Gio = SoCongNgoaiGioSau23Gio ?? 0;
                SoCongNgoaiGioT7CN = SoCongNgoaiGioT7CN ?? 0;
                SoCongNgoaiGioT7CNSau23Gio = SoCongNgoaiGioT7CNSau23Gio ?? 0;
                SoCongNgoaiGioLe = SoCongNgoaiGioLe ?? 0;
                SoCongNgoaiGioLeSau23Gio = SoCongNgoaiGioLeSau23Gio ?? 0;
                QuanLyChamCongNgoaiGio_Factory qlfac = new QuanLyChamCongNgoaiGio_Factory();
                BangChamCongNgoaiGio bcc = qlfac.GetByKyTinhLuong(kytinhluong);
                if (bcc == null)
                {
                    try
                    {
                        bcc = qlfac.CreateManagedObject();
                        bcc.Oid = Guid.NewGuid();
                        bcc.KyTinhLuong = kytinhluong;
                        bcc.NgayLap = DateTime.Now;
                        bcc.ThongTinTruong = new Guid(WebConfigurationManager.AppSettings["ThongTinTruong"]);
                        qlfac.SaveChanges();
                    }
                    catch (Exception e)
                    {

                    }
                }
                HoSo_Factory hsfac = new HoSo_Factory();
                HoSo nv = hsfac.GetByID(nhanVienID);
                ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                CC_ChamCongNgoaiGioTheoNgay newObj = factory.CreateManagedObject();
                if (nv != null && idUser!=null && idUser!="")
                {
                    newObj.Oid = Guid.NewGuid();
                    newObj.IDBoPhan = nv.NhanVien.BoPhan;
                    newObj.IDNhanVien = nv.Oid;
                    newObj.Ngay = ngay;
                    newObj.SoCongNgoaiGio = SoCongNgoaiGio;
                    newObj.SoCongNgoaiGioSau23Gio = SoCongNgoaiGioSau23Gio;
                    newObj.SoCongNgoaiGioT7CN = SoCongNgoaiGioT7CN;
                    newObj.SoCongNgoaiGioT7CNSau23Gio = SoCongNgoaiGioT7CNSau23Gio;
                    newObj.SoCongNgoaiGioLe = SoCongNgoaiGioLe;
                    newObj.SoCongNgoaiGioLeSau23Gio = SoCongNgoaiGioLeSau23Gio;
                    newObj.TuGio = ParseTimeString(GioTuGio, PhutTuGio);
                    newObj.DenGio = ParseTimeString(GioDenGio, PhutDenGio);
                    newObj.IDUser = new Guid(idUser);
                    factory.SaveChanges();
                }
                var returnObj = newObj.Map<DTO_QuanLyChamCongNgoaiGio_Find>();
                return returnObj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChamCongNgoaiGioTheoNgay_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgoaiGioTheoNgay> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgoaiGioTheoNgay>>(jsonObjectList);
            return ChamCongNgoaiGioTheoNgay_DeleteList(publicKey, token, objList);
        }
        public bool ChamCongNgoaiGioTheoNgay_DeleteList(String publicKey, String token, List<DTO_ChamCongNgoaiGioTheoNgay> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                ChamCongNgoaiGioTheoNgayThayDoi_Factory ccFactory = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();

                foreach (var obj in objList)
                {
                    var objFromDB = factory.GetByID(obj.Oid);
                    factory.DeleteObject(objFromDB);
                }

                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                return false;
            }
            return false;
        }
        public bool ChamCongNgoaiGioTheoNgay_Save_Json(String publicKey, String token, string jsonObjectList, string idUser)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgoaiGioTheoNgay> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgoaiGioTheoNgay>>(jsonObjectList);
            return ChamCongNgoaiGioTheoNgay_Save(publicKey, token, objList, idUser);
        }
        //public bool ChamCongNgoaiGioTheoNgay_Save(String publicKey, String token, List<DTO_ChamCongNgoaiGioTheoNgay> objList, string idUser)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
        //        ChamCongNgoaiGioTheoNgayThayDoi_Factory ccFactory = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();
        //        WebGroup_Factory groupFactory = WebGroup_Factory.New();
        //        Guid userId = new Guid(idUser);
        //        string webGroupId = groupFactory.GetWebGroupIdByUserId(userId);
        //        foreach (var obj in objList)
        //        {
        //            if (obj != null)
        //            {

        //                CC_ChamCongNgoaiGioTheoNgay objFromDB = factory.GetByID(obj.Oid);
        //                //Nếu có cc và có sự thay đổi
        //                if (objFromDB != null &&
        //                    (obj.SoCongNgoaiGio != objFromDB.SoCongNgoaiGio
        //                    || obj.SoCongNgoaiGioSau23Gio != objFromDB.SoCongNgoaiGioSau23Gio
        //                    || obj.SoCongNgoaiGioT7CN != objFromDB.SoCongNgoaiGioT7CN
        //                    || obj.SoCongNgoaiGioT7CNSau23Gio != objFromDB.SoCongNgoaiGioT7CNSau23Gio
        //                    || obj.SoCongNgoaiGioLe != objFromDB.SoCongNgoaiGioLe
        //                    || obj.SoCongNgoaiGioLeSau23Gio != objFromDB.SoCongNgoaiGioLeSau23Gio
        //                    ))
        //                {
        //                    //Lấy webgroup của user đã sửa cc
        //                    string webGroupObj = objFromDB.IDUser != null ? groupFactory.GetWebGroupIdByUserId(objFromDB.IDUser.Value) : "";

        //                    //Nếu người sửa là user thư ký
        //                    if (webGroupId == WebConfigurationManager.AppSettings["ThuKyGroupId"])
        //                    {
        //                        //Kiểm tra đã được admin sửa chưa
        //                        bool exist = ccFactory.CheckExist(objFromDB.Oid);

        //                        //Nếu chưa được admin sửa thì update thay đổi
        //                        if (exist == false)
        //                        {
        //                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });
        //                            objFromDB.IDUser = userId;
        //                        }
        //                        //Nếu cc đã đc sửa bởi admin
        //                        else
        //                        {
        //                            //Không sửa
        //                        }
        //                    }
        //                    //Nếu người sửa là admin
        //                    else if (webGroupId == WebConfigurationManager.AppSettings["AdminGroupId"])
        //                    {
        //                        //Nếu admin chưa chấm
        //                        if (webGroupObj != WebConfigurationManager.AppSettings["AdminGroupId"])
        //                        {
        //                            CC_ChamCongNgoaiGioTheoNgayThayDoi ccObj = ccFactory.GetByID(objFromDB.Oid);

        //                            //Nếu đã có thay đổi thì cập nhật thay đổi
        //                            if (ccObj != null)
        //                            {
        //                                //Nếu đã chấm rồi sửa lại trùng với đơn vị chấm thì xóa
        //                                if (obj.SoCongNgoaiGio == ccObj.SoCongNgoaiGioDonVi
        //                                    && obj.SoCongNgoaiGioSau23Gio == ccObj.SoCongNgoaiGioSau23GioDonVi
        //                                    && obj.SoCongNgoaiGioT7CN == ccObj.SoCongNgoaiGioT7CNDonVi
        //                                    && obj.SoCongNgoaiGioT7CNSau23Gio == ccObj.SoCongNgoaiGioT7CNSau23GioDonVi
        //                                    && obj.SoCongNgoaiGioLe == ccObj.SoCongNgoaiGioLeDonVi
        //                                    && obj.SoCongNgoaiGioLeSau23Gio == ccObj.SoCongNgoaiGioLeSau23GioDonVi)
        //                                {
        //                                    //Cập nhật lại người sửa là user đơn vị
        //                                    objFromDB.IDUser = ccObj.IDUserDonVi;
        //                                    //objFromDB.TuGio
        //                                    ccFactory.DeleteObject(ccObj);
        //                                    ccFactory.SaveChanges();
        //                                }
        //                                else
        //                                {
        //                                    ccObj.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });
        //                                    ccFactory.SaveChanges();
        //                                }
        //                                objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });

        //                            }

        //                            //Nếu chưa có thì thêm mới
        //                            else
        //                            {
        //                                SaveChamCongNgoaiGioTheoNgayThayDoi(objFromDB, obj);
        //                                //Cập nhật thay đổi
        //                                objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });
        //                                objFromDB.IDUser = userId;
        //                            }


        //                        }
        //                        //Nếu cc đã đc sửa bở admin
        //                        else
        //                        {
        //                            CC_ChamCongNgoaiGioTheoNgayThayDoi ccObj = ccFactory.GetByID(objFromDB.Oid);

        //                            //Nếu đã có thay đổi thì cập nhật thay đổi
        //                            if (ccObj != null)
        //                            {
        //                                bool check = obj.SoCongNgoaiGio != ccObj.SoCongNgoaiGioDonVi ? true : false;
        //                                //Nếu đã chấm rồi sửa lại trùng với đơn vị chấm thì xóa
        //                                if (obj.SoCongNgoaiGio == ccObj.SoCongNgoaiGioDonVi
        //                                   && obj.SoCongNgoaiGioSau23Gio == ccObj.SoCongNgoaiGioSau23GioDonVi
        //                                   && obj.SoCongNgoaiGioT7CN == ccObj.SoCongNgoaiGioT7CNDonVi
        //                                   && obj.SoCongNgoaiGioT7CNSau23Gio == ccObj.SoCongNgoaiGioT7CNSau23GioDonVi
        //                                   && obj.SoCongNgoaiGioLe == ccObj.SoCongNgoaiGioLeDonVi
        //                                   && obj.SoCongNgoaiGioLeSau23Gio == ccObj.SoCongNgoaiGioLeSau23GioDonVi)
        //                                {
        //                                    //Cập nhật lại người sửa là user đơn vị
        //                                    objFromDB.IDUser = ccObj.IDUserDonVi;
        //                                    ccFactory.DeleteObject(ccObj);
        //                                    ccFactory.SaveChanges();
        //                                }
        //                                else
        //                                {
        //                                    ccObj.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });
        //                                    ccFactory.SaveChanges();                                     
        //                                }
        //                                //Cập nhật thay đổi
        //                                objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "SoCongNgoaiGio", "SoCongNgoaiGioSau23Gio", "SoCongNgoaiGioT7CN", "SoCongNgoaiGioT7CNSau23Gio", "SoCongNgoaiGioLe", "SoCongNgoaiGioLeSau23Gio" });

        //                            }                         
        //                        }
        //                    }                           
        //                }
        //            }

        //        }
        //        //////////////
        //        try
        //        {
        //            factory.SaveChanges();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //        return false;
        //    }
        //    return false;
        //}
        public bool ChamCongNgoaiGioTheoNgay_Save(String publicKey, String token, List<DTO_ChamCongNgoaiGioTheoNgay> objList, string idUser)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                ChamCongNgoaiGioTheoNgayThayDoi_Factory ccFactory = ChamCongNgoaiGioTheoNgayThayDoi_Factory.New();
                WebGroup_Factory groupFactory = WebGroup_Factory.New();
                Guid userId = new Guid(idUser);
                string webGroupId = groupFactory.GetWebGroupIdByUserId(userId);
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {
                        CC_ChamCongNgoaiGioTheoNgay objFromDB = factory.GetByID(obj.Oid);
                        //Nếu có cc và có sự thay đổi
                        if (objFromDB != null &&
                            (obj.SoCongNgoaiGio != objFromDB.SoCongNgoaiGio
                            || obj.SoCongNgoaiGioSau23Gio != objFromDB.SoCongNgoaiGioSau23Gio
                            || obj.SoCongNgoaiGioT7CN != objFromDB.SoCongNgoaiGioT7CN
                            || obj.SoCongNgoaiGioT7CNSau23Gio != objFromDB.SoCongNgoaiGioT7CNSau23Gio
                            || obj.SoCongNgoaiGioLe != objFromDB.SoCongNgoaiGioLe
                            || obj.SoCongNgoaiGioLeSau23Gio != objFromDB.SoCongNgoaiGioLeSau23Gio
                            ))
                        {
                            objFromDB.SoCongNgoaiGio = obj.SoCongNgoaiGio;
                            objFromDB.SoCongNgoaiGioSau23Gio = obj.SoCongNgoaiGioSau23Gio;
                            objFromDB.SoCongNgoaiGioT7CN = obj.SoCongNgoaiGioT7CN;
                            objFromDB.SoCongNgoaiGioT7CNSau23Gio = obj.SoCongNgoaiGioT7CNSau23Gio;
                            objFromDB.SoCongNgoaiGioLe = obj.SoCongNgoaiGioLe;
                            objFromDB.SoCongNgoaiGioLeSau23Gio = obj.SoCongNgoaiGioLeSau23Gio;
                            objFromDB.TuGio = obj.TuGio.ToString();
                            objFromDB.DenGio = obj.DenGio.ToString();
                            //factory.SaveChanges();
                            
                        }
                    }

                }
                //////////////
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                return false;
            }
            return false;
        }
        public bool ChamCongNgoaiGioTheoNgay_Save2(String publicKey, String token, string Oid, decimal SoCongNgoaiGio, decimal SoCongNgoaiGioSau23Gio, decimal SoCongNgoaiGioT7CN, decimal SoCongNgoaiGioT7CNSau23Gio, decimal SoCongNgoaiGioLe, decimal SoCongNgoaiGioLeSau23Gio, int GioTuGio, int PhutTuGio, int GioDenGio, int PhutDenGio)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Guid id = Guid.Empty;
                if (Oid != "" && Oid != null)
                {
                    id = new Guid(Oid);
                }
                ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                CC_ChamCongNgoaiGioTheoNgay objFromDB = factory.GetByID(id);
                if (objFromDB != null)
                {
                    objFromDB.SoCongNgoaiGio = SoCongNgoaiGio;
                    objFromDB.SoCongNgoaiGioSau23Gio = SoCongNgoaiGioSau23Gio;
                    objFromDB.SoCongNgoaiGioT7CN = SoCongNgoaiGioT7CN;
                    objFromDB.SoCongNgoaiGioT7CNSau23Gio = SoCongNgoaiGioT7CNSau23Gio;
                    objFromDB.SoCongNgoaiGioLe = SoCongNgoaiGioLe;
                    objFromDB.SoCongNgoaiGioLeSau23Gio = SoCongNgoaiGioLeSau23Gio;
                    objFromDB.TuGio = ParseTimeString(GioTuGio, PhutTuGio);
                    objFromDB.DenGio = ParseTimeString(GioDenGio, PhutDenGio);
                    objFromDB.DaSuaTrenWeb = true;
                }
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public DTO_ChamCongNgoaiGioTheoNgay ChamCongNgoaiGioTheoNgay_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChamCongNgoaiGioTheoNgay_Factory factory = ChamCongNgoaiGioTheoNgay_Factory.New();
                CC_ChamCongNgoaiGioTheoNgay objFromDB = factory.GetByID(id);
                DTO_ChamCongNgoaiGioTheoNgay obj = new DTO_ChamCongNgoaiGioTheoNgay();
                if (objFromDB != null)
                {
                    HoSo hs = factory.Context.HoSoes.Where(h => h.Oid == objFromDB.IDNhanVien).SingleOrDefault();
                    obj.TenNhanVien = hs.HoTen;
                    obj.TenBoPhan = hs.NhanVien.BoPhan1.TenBoPhan;
                    obj.Ngay = objFromDB.Ngay;
                    obj.SoCongNgoaiGio = objFromDB.SoCongNgoaiGio;
                    obj.SoCongNgoaiGioSau23Gio = objFromDB.SoCongNgoaiGioSau23Gio;
                    obj.SoCongNgoaiGioT7CN = objFromDB.SoCongNgoaiGioT7CN;
                    obj.SoCongNgoaiGioT7CNSau23Gio = objFromDB.SoCongNgoaiGioT7CNSau23Gio;
                    obj.SoCongNgoaiGioLe = objFromDB.SoCongNgoaiGioLe;
                    obj.SoCongNgoaiGioLeSau23Gio = objFromDB.SoCongNgoaiGioLeSau23Gio;
                    obj.GioTuGio = ParseHourDTO(objFromDB.TuGio);
                    obj.PhutTuGio = ParseMinuteDTO(objFromDB.TuGio);
                    obj.GioDenGio = ParseHourDTO(objFromDB.DenGio);
                    obj.PhutDenGio = ParseMinuteDTO(objFromDB.DenGio);
                    return obj;
                }
            }
            return null;
        }
        public String ChamCongNgoaiGioTheoNgay_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgoaiGioTheoNgay obj = ChamCongNgoaiGioTheoNgay_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public bool ChotChamCongNgoaiGio_Create(String publicKey, String token, Guid boPhanID, Guid kyTinhLuong, string idUser)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNgoaiGio_Factory.New();
                KyTinhLuong kyTinhLuongObj = KyTinhLuong_Factory.New().GetKyTinhLuong_GCRecordIsNull_ById(kyTinhLuong);
                int thang = kyTinhLuongObj.Thang.HasValue ? kyTinhLuongObj.Thang.Value : DateTime.Today.Month;
                int nam = kyTinhLuongObj.Nam.HasValue ? kyTinhLuongObj.Nam.Value : DateTime.Today.Year;
                bool daTonTaiBangCon = false; factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
                if (daTonTaiBangCon)
                {
                    throw new Exception(string.Format("Không thể chốt vì đã tồn tại bảng chốt của tháng {0} năm {1} của bộ phận", thang, nam));
                }
                else
                {
                    //try
                    //{
                    //tim ky tinh luong thich hop
                    //KyTinhLuong kyTinhLuong = KyTinhLuong_Factory.New().GetObjByThangNam_GCRecordIsNull(thang, nam);
                    CC_KyChamCong kyChamCong = CC_KyChamCong_Factory.New().GetByKyTinhLuong(kyTinhLuong);
                    if (kyTinhLuong == null)
                        throw new Exception(string.Format("Kỳ tính lương tháng {0} năm {1} không tìm thấy", thang, nam));
                    else
                        if (kyChamCong == null)
                        throw new Exception(string.Format("Kỳ chấm công tháng {0} năm {1} không tìm thấy", thang, nam));
                    else
                    {
                        //lay quan ly cu neu co
                        BangChamCongNgoaiGio quanLy = factory.GetByThangNam(thang, nam);
                        if (quanLy == null)
                        {
                            //tao quan ly moi
                            quanLy = factory.CreateManagedObject();
                            quanLy.Oid = Guid.NewGuid();
                            //quanLy.KyTinhLuong = kyTinhLuong.Oid;
                            quanLy.KyTinhLuong = kyTinhLuong;
                            quanLy.KyChamCong = kyChamCong.Oid;
                            quanLy.NgayLap = DateTime.Today;
                            quanLy.ThongTinTruong = new Guid(WebConfigurationManager.AppSettings["ThongTinTruong"]);
                        }
                        var ctccngFactory = ChiTietChamCongNgoaiGio_Factory.New();

                        //Chốt tất cả đơn vị
                        //if (boPhanID == Guid.Empty)
                        //{
                        //    var facbp = BoPhan_Factory.New();
                        //    var bophanlist = facbp.GetAll_GCRecordIsNull();
                        //    foreach (BoPhan bp in bophanlist)
                        //    {
                        //        List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(bp.Oid, kyChamCong.DenNgay).ToList();
                        //        foreach (var hoSo in hsListTheoBoPhan)
                        //        {
                        //            ChiTietChamCongNhanVien chiTiet = ctccnvFactory.GetChiTietChamCongNhanVienByNhanVien(quanLy.Oid, hoSo.Oid);
                        //            if (chiTiet == null)
                        //            {
                        //                chiTiet = ctccnvFactory.CreateAloneObject();
                        //                chiTiet.Oid = Guid.NewGuid();
                        //                chiTiet.ThongTinNhanVien = hoSo.Oid;
                        //                chiTiet.BoPhan = hoSo.NhanVien.BoPhan;
                        //                chiTiet.BoPhanTheoBangCong = hoSo.NhanVien.BoPhan1.TenBoPhan;
                        //                chiTiet.TrangThai = false;
                        //                chiTiet.DanhGia = "A";
                        //                chiTiet.Khoa = false;

                        //                quanLy.ChiTietChamCongNhanViens.Add(chiTiet);
                        //            }

                        //        }
                        //        using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                        //            TimeSpan.FromSeconds(180)))
                        //        {
                        //            factory.SaveChanges();

                        //            factory.Context.spd_WebChamCong_ChotChamCongThang_CapNhatSoNgayCongNgayNghi(thang, nam,
                        //                quanLy.Oid, boPhanID);
                        //            transaction.Complete();
                        //        }

                        //    }
                        //}
                        ////Chốt 1 đơn vị
                        //else
                        {
                            List<Guid?> hsListTheoBoPhan = ctccngFactory.GetListDistinctByMaBoPhan(boPhanID,kyChamCong.TuNgay,kyChamCong.DenNgay).ToList();
                            foreach (Guid id in hsListTheoBoPhan)
                            {
                                ChiTietChamCongNgoaiGio chiTiet = ctccngFactory.GetByNhanVien(quanLy.Oid, id);
                                if (chiTiet == null)
                                {
                                    chiTiet = ctccngFactory.CreateAloneObject();
                                    chiTiet.Oid = Guid.NewGuid();
                                    chiTiet.ThongTinNhanVien = id;
                                    chiTiet.BoPhan = boPhanID;
                                    quanLy.ChiTietChamCongNgoaiGios.Add(chiTiet);
                                }

                            }
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                TimeSpan.FromSeconds(180)))
                            {
                                factory.SaveChanges();

                                WebGroup_Factory groupFactory = WebGroup_Factory.New();
                                Guid userId = new Guid(idUser);
                                string webGroupId = groupFactory.GetWebGroupIdByUserId(userId);
                                if (webGroupId == WebConfigurationManager.AppSettings["AdminGroupId"])
                                {
                                    factory.Context.spd_WebChamCong_ChotChamCongNgoaiGio(kyChamCong.Thang, kyChamCong.Nam,
                                   quanLy.Oid, boPhanID);
                                }
                                else
                                {
                                    factory.Context.spd_WebChamCong_ChotChamCongNgoaiGio_DonViChot(kyChamCong.Thang, kyChamCong.Nam,
                                   quanLy.Oid, boPhanID);
                                }
                                   
                                transaction.Complete();
                            }
                        }

                        return true;
                    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    return false;
                    //}
                }

                return false;
            }
            else
            {
                return false;
            }
        }
        public bool ChotChamCongNgoaiGio_Delete(String publicKey, String token, Guid boPhanID, Guid kyTinhLuong)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNgoaiGio_Factory.New();
                bool daTonTai = factory.ExistsByKyTinhLuongBoPhanID(kyTinhLuong);
                if (daTonTai)
                {
                    //tien hanh xoa bang chot
                    //try
                    //{

                    if (boPhanID == Guid.Empty)
                    {
                        var facbp = BoPhan_Factory.New();
                        var bophanlist = facbp.GetAll_GCRecordIsNull();
                        foreach (BoPhan bp in bophanlist)
                        {
                            factory.Context.spd_WebChamCong_ChotChamNgoaiGio_Delete(bp.Oid,kyTinhLuong);
                        }
                    }
                    else
                    {
                        factory.Context.spd_WebChamCong_ChotChamNgoaiGio_Delete(boPhanID, kyTinhLuong);
                    }


                    return true;
                    //}
                    //catch (Exception)
                    //{
                    //    throw new Exception(string.Format("Có lỗi khi xóa bảng chốt của tháng {0} năm {1}", thang, nam));
                    //}

                }
                else
                {
                    return false;
                    //throw new Exception(string.Format("Không thể xóa chốt vì không tồn tại bảng chốt của tháng {0} năm {1}");
                }

            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool QuanLyChamCongNgoaiGio_CheckChot(String publicKey, String token, Guid boPhanID,int ngay, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChiTietChamCongNgoaiGio_Factory.New();
                bool daTonTai = factory.CheckChot(ngay,thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool QuanLyChamCongNgoaiGio_CheckChotByKy(String publicKey, String token, Guid boPhanID, Guid kyTinhLuong)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChiTietChamCongNgoaiGio_Factory.New();
                bool daTonTai = factory.CheckChotByKy(boPhanID, kyTinhLuong);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String QuanLyChamCongNgoaiGio_ChamCongThang_Json(String publicKey, String token, Guid PhongBan, Guid KyTinhLuong)
        {
            IEnumerable<DTO_QuanLyChamCongNgoaiGio_ChamCongThang> list = QuanLyChamCongNgoaiGio_ChamCongThang(publicKey, token, PhongBan, KyTinhLuong);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_QuanLyChamCongNgoaiGio_ChamCongThang> QuanLyChamCongNgoaiGio_ChamCongThang(String publicKey, String token, Guid PhongBan, Guid KyTinhLuong)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                QuanLyChamCongNgoaiGio_Factory factory = QuanLyChamCongNgoaiGio_Factory.New();
                IEnumerable<DTO_QuanLyChamCongNgoaiGio_ChamCongThang> obj = factory.QuanLyChamCongNgoaiGio_ChamCongThang(PhongBan, KyTinhLuong);
                return obj;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
    }
}
