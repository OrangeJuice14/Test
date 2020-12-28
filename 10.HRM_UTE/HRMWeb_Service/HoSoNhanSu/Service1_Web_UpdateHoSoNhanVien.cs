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
using System.Transactions;
using ERP_Core.Common;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        
        // ///////////////////////////////////////
        public IEnumerable<DTO_Web_UpdateHoSoNhanVien> GetListByBoPhan_Web_UpdateHoSoNhanVien(String publicKey, String token, Guid idBoPhan)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListByBoPhan(idBoPhan);
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public String GetListByBoPhan_Web_UpdateHoSoNhanVien_Json(String publicKey, String token, Guid idBoPhan)
        {
            IEnumerable<DTO_Web_UpdateHoSoNhanVien> list = GetListByBoPhan_Web_UpdateHoSoNhanVien(publicKey, token, idBoPhan);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ///////////////////////////////////////
        public String GetListByOid_Web_UpdateHoSoNhanVien_Json(String publicKey, String token, Guid oidHoSoNhanVen)
        {
            DTO_Web_UpdateHoSoNhanVien list = GetListByOid_Web_UpdateHoSoNhanVien(publicKey, token, oidHoSoNhanVen);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public DTO_Web_UpdateHoSoNhanVien GetListByOid_Web_UpdateHoSoNhanVien(String publicKey, String token, Guid idHoSoNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListByOId(idHoSoNhanVien);
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public bool GetListByOidNhanVien_Web_UpdateHoSoNhanVien(String publicKey, String token, Guid oidNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetWeb_UpdateHoSoNhanVienByOIdNhanVien(oidNhanVien);
                return tmpList;
            }
            else
            {
                return false;
            }
        }

        public bool ThongTinHoSoNhanVien_Save_Json(String publicKey, String token, string jsonObjectList, Guid userUpdate)
        {//DANG SD
            //chuyen jsonObject thanh object
            DTO_Web_UpdateHoSoNhanVien obj = JsonConvert.DeserializeObject<DTO_Web_UpdateHoSoNhanVien>(jsonObjectList);
            return ThongTinHoSoNhanSu_Save(publicKey, token, obj, userUpdate);
        }

        public bool UpdateHoSoNhanSu_SaveList_Json(String publicKey, String token, string jsonObjectList, Guid userUpdate)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_Web_UpdateHoSoNhanVien> objList = JsonConvert.DeserializeObject<List<DTO_Web_UpdateHoSoNhanVien>>(jsonObjectList);
            return UpdateHoSoNhanSu_SaveList(publicKey, token, objList, userUpdate);
        }

        public bool UpdateHoSoNhanSu_SaveList(String publicKey, String token, List<DTO_Web_UpdateHoSoNhanVien> objList, Guid userUpdate)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj.Oid != Guid.Empty)
                            factory.Context.spd_Web1_UpdateHoSoNhanVien_UTE(obj.Oid,userUpdate);
                    }
                    return true;
                }
                catch { return false; }
            }
            return false;
        }

        public bool UpdateHoSoNhanVien_DeleteList(String publicKey, String token, List<DTO_Web_UpdateHoSoNhanVien> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            Web_UpdateHoSoNhanVien stupidObj = new Web_UpdateHoSoNhanVien() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            factory.FullDelete(factory.Context, stupidObj);
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
            }
            return false;
        }


        public bool ThongTinHoSoNhanSu_Save(String publicKey, String token, DTO_Web_UpdateHoSoNhanVien obj, Guid userUpdate)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    if (obj != null && obj.ThongTinNhanVien != null)
                    {
                        Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                        Web_UpdateHoSoNhanVien newObj = factory.GetHoSoNhanVienByOidNV(obj.ThongTinNhanVien);
                        bool daCapNhatHoSo = false;

                        //Lấy dữ liệu trong HRM
                        var hoSoTrongHRM = factory.GetHoSoNhanVien_InHRM_ByOidNhanVien(obj.ThongTinNhanVien);

                        //So sánh nếu dữ liệu trên form khác với dữ liệu trong HRM thì mới lưu lại

                        //1. Số hiệu công chức
                        if (!string.IsNullOrEmpty(obj.SoHieuCongChuc) && !obj.SoHieuCongChuc.Trim().Equals(hoSoTrongHRM.SoHieuCongChuc))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.SoHieuCongChuc = obj.SoHieuCongChuc.Trim();
                            //
                            
                        }
                        //2. Họ
                        if (!string.IsNullOrEmpty(obj.Ho) && !obj.Ho.Trim().Equals(hoSoTrongHRM.Ho))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.Ho = obj.Ho;
                        }
                        //3. Tên
                        if (!string.IsNullOrEmpty(obj.Ten) && !obj.Ten.Trim().Equals(hoSoTrongHRM.Ten))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.Ten = obj.Ten.Trim();
                        }

                        if (newObj != null)
                        {
                            // Họ Tên
                            if (newObj.Ho == null && newObj.Ten == null)
                            {
                                newObj.HoTen = hoSoTrongHRM.Ho + " " + hoSoTrongHRM.Ten;
                            }
                            else if (newObj.Ho != null && newObj.Ten == null)
                            {
                                newObj.HoTen = newObj.Ho + " " + hoSoTrongHRM.Ten;
                            }
                            else if (newObj.Ho == null && newObj.Ten != null)
                            {
                                newObj.HoTen = hoSoTrongHRM.Ho + " " + newObj.Ten;
                            }
                            else
                            {
                                newObj.HoTen = newObj.Ho + " " + newObj.Ten;
                            }
                        }
                        //4. Tên gọi khác
                        if (!string.IsNullOrEmpty(obj.TenGoiKhac) && !obj.TenGoiKhac.Trim().Equals(hoSoTrongHRM.TenGoiKhac))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TenGoiKhac = obj.TenGoiKhac.Trim();
                        }
                        //5. Ngày sinh
                        if (obj.NgaySinh != DateTime.MinValue && !obj.NgaySinh.Equals(hoSoTrongHRM.NgaySinh))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.NgaySinh = obj.NgaySinh;
                        }
                        //6. Giới tính
                        if (!obj.GioiTinh.Equals(hoSoTrongHRM.GioiTinh))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.GioiTinh = obj.GioiTinh == "Nam" ? false : true;
                        }

                        //7. Email
                        if (!string.IsNullOrEmpty(obj.Email) && !obj.Email.Trim().Equals(hoSoTrongHRM.Email))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.Email = obj.Email.Trim();
                        }
                        //8. Điện thoại di động
                        if (!string.IsNullOrEmpty(obj.DienThoaiDiDong) && !obj.DienThoaiDiDong.Trim().Equals(hoSoTrongHRM.DienThoaiDiDong))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.DienThoaiDiDong = obj.DienThoaiDiDong.Trim();
                        }
                        //9. Dân tộc
                        if (obj.DanToc != Guid.Empty && !obj.DanToc.Equals(hoSoTrongHRM.DanToc))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.DanToc = obj.DanToc;
                        }
                        //10. Tôn giáo
                        if (obj.TonGiao != Guid.Empty && !obj.TonGiao.Equals(hoSoTrongHRM.TonGiao))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TonGiao = obj.TonGiao;
                        }
                        
                        //////////////////11. Nơi sinh/////////////////////////////
                        bool daTaoNoiSinh = false;
                        DiaChi_Factory factory_DiaChi = new DiaChi_Factory();
                        DiaChi noiSinh = factory_DiaChi.GetDiaChiByOid(newObj != null && newObj.NoiSinh != null ? newObj.NoiSinh.Value : Guid.Empty);

                        //11.1 Quốc gia
                        if (obj.NoiSinh.QuocGia != null && obj.NoiSinh.QuocGia != Guid.Empty && ((hoSoTrongHRM.NoiSinh == null || hoSoTrongHRM.NoiSinh.QuocGia == null) || (hoSoTrongHRM.NoiSinh.QuocGia != null && !obj.NoiSinh.QuocGia.Equals(hoSoTrongHRM.NoiSinh.QuocGia))))
                        {
                            if (noiSinh == null)
                            {
                                noiSinh = factory_DiaChi.CreateManagedObject();
                                noiSinh.Oid = Guid.NewGuid();
                            }
                            noiSinh.QuocGia = obj.NoiSinh.QuocGia;
                            //
                            daTaoNoiSinh = true;
                        }
                        //11.2 Tỉnh thành
                        if (obj.NoiSinh.TinhThanh != null && obj.NoiSinh.TinhThanh != Guid.Empty && ((hoSoTrongHRM.NoiSinh == null || hoSoTrongHRM.NoiSinh.TinhThanh == null) || (hoSoTrongHRM.NoiSinh.TinhThanh != null && !obj.NoiSinh.TinhThanh.Equals(hoSoTrongHRM.NoiSinh.TinhThanh))))
                        {
                            if (noiSinh == null)
                            {
                                noiSinh = factory_DiaChi.CreateManagedObject();
                                noiSinh.Oid = Guid.NewGuid();
                            }
                            noiSinh.TinhThanh = obj.NoiSinh.TinhThanh;
                            //
                            daTaoNoiSinh = true;
                        }
                        //11.3 Quận huyện
                        if (obj.NoiSinh.QuanHuyen != null && obj.NoiSinh.QuanHuyen != Guid.Empty && ((hoSoTrongHRM.NoiSinh == null || hoSoTrongHRM.NoiSinh.QuanHuyen == null) || (hoSoTrongHRM.NoiSinh.QuanHuyen != null && !obj.NoiSinh.QuanHuyen.Equals(hoSoTrongHRM.NoiSinh.QuanHuyen))))
                        {
                            if (noiSinh == null)
                            {
                                noiSinh = factory_DiaChi.CreateManagedObject();
                                noiSinh.Oid = Guid.NewGuid();
                            }
                            noiSinh.QuanHuyen = obj.NoiSinh.QuanHuyen;
                            //
                            daTaoNoiSinh = true;
                        }
                        //11.4 Xã phường
                        if (obj.NoiSinh.XaPhuong != null && obj.NoiSinh.XaPhuong != Guid.Empty && ((hoSoTrongHRM.NoiSinh == null || hoSoTrongHRM.NoiSinh.XaPhuong == null) || (hoSoTrongHRM.NoiSinh.XaPhuong != null && !obj.NoiSinh.XaPhuong.Equals(hoSoTrongHRM.NoiSinh.XaPhuong))))
                        {
                            if (noiSinh == null)
                            {
                                noiSinh = factory_DiaChi.CreateManagedObject();
                                noiSinh.Oid = Guid.NewGuid();
                            }
                            noiSinh.XaPhuong = obj.NoiSinh.XaPhuong;
                            //
                            daTaoNoiSinh = true;
                        }
                        //11.5 Số nhà
                        if (!string.IsNullOrEmpty(obj.NoiSinh.SoNha) && !obj.NoiSinh.SoNha.Trim().Equals(hoSoTrongHRM.NoiSinh.SoNha))
                        {
                            if (noiSinh == null)
                            {
                                noiSinh = factory_DiaChi.CreateManagedObject();
                                noiSinh.Oid = Guid.NewGuid();
                            }
                            noiSinh.SoNha = obj.NoiSinh.SoNha;
                            //
                            daTaoNoiSinh = true;
                        }
                        //
                        if (daTaoNoiSinh)
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                            {
                                try
                                {
                                    if (newObj == null)
                                    {
                                        newObj = factory.CreateManagedObject();
                                        newObj.Oid = Guid.NewGuid();
                                        newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                                    }
                                    //
                                    if (!string.IsNullOrEmpty(noiSinh.SoNha))
                                    {
                                        noiSinh.FullDiaChi = noiSinh.SoNha;
                                    }
                                    if (noiSinh.XaPhuong != null)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(noiSinh.XaPhuong.Value);
                                        if(item != null)
                                        noiSinh.FullDiaChi = noiSinh.FullDiaChi + "-" + item.TenXaPhuong;
                                    }
                                    if (noiSinh.QuanHuyen != null)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(noiSinh.QuanHuyen.Value);
                                        if (item != null)
                                            noiSinh.FullDiaChi = noiSinh.FullDiaChi + "-" + item.TenQuanHuyen;
                                    }
                                    if (noiSinh.TinhThanh != null)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(noiSinh.TinhThanh.Value);
                                        if (item != null)
                                            noiSinh.FullDiaChi = noiSinh.FullDiaChi + "-" + item.TenTinhThanh;
                                    }
                                    if (noiSinh.QuocGia != null)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(noiSinh.QuocGia.Value);
                                        if (item != null)
                                            noiSinh.FullDiaChi = noiSinh.FullDiaChi + "-" + item.TenQuocGia;
                                    }
                                    //
                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    //
                                    daCapNhatHoSo = true;
                                    newObj.NoiSinh = noiSinh.Oid;
                                }
                                catch (Exception ex) { throw ex; }
                            }
                        }
                        //////////////////End Nơi sinh///////////////////////////// 

                        //////////////////12. Quê quán/////////////////////////////
                        DiaChi queQuan = factory_DiaChi.GetDiaChiByOid(newObj != null && newObj.QueQuan != null ? newObj.QueQuan.Value : Guid.Empty);
                        bool daTaoQueQuan = false;

                        //12.1 Quốc gia
                        if (obj.QueQuan.QuocGia != null && obj.QueQuan.QuocGia != Guid.Empty && ((hoSoTrongHRM.QueQuan == null || hoSoTrongHRM.QueQuan.QuocGia == null) || (hoSoTrongHRM.QueQuan.QuocGia != null && !obj.QueQuan.QuocGia.Equals(hoSoTrongHRM.QueQuan.QuocGia))))
                        {
                            queQuan = factory_DiaChi.CreateManagedObject();
                            queQuan.Oid = Guid.NewGuid();
                            queQuan.QuocGia = obj.QueQuan.QuocGia;
                            //
                            daTaoQueQuan = true;
                        }
                        //12.2 Tỉnh thành
                        if (obj.QueQuan.TinhThanh != null && obj.QueQuan.TinhThanh != Guid.Empty && ((hoSoTrongHRM.QueQuan == null || hoSoTrongHRM.QueQuan.TinhThanh == null) || (hoSoTrongHRM.QueQuan.TinhThanh != null && !obj.QueQuan.TinhThanh.Equals(hoSoTrongHRM.QueQuan.TinhThanh))))
                        {
                            if (queQuan == null)
                            {
                                queQuan = factory_DiaChi.CreateManagedObject();
                                queQuan.Oid = Guid.NewGuid();
                            }
                            queQuan.TinhThanh = obj.QueQuan.TinhThanh;
                            //
                            daTaoQueQuan = true;
                        }
                        //12.3 Quận huyện
                        if (obj.QueQuan.QuanHuyen != null && obj.QueQuan.QuanHuyen != Guid.Empty && ((hoSoTrongHRM.QueQuan == null || hoSoTrongHRM.QueQuan.QuanHuyen == null) || (hoSoTrongHRM.QueQuan.QuanHuyen != null && !obj.QueQuan.QuanHuyen.Equals(hoSoTrongHRM.QueQuan.QuanHuyen))))
                        {
                            if (queQuan == null)
                            {
                                queQuan = factory_DiaChi.CreateManagedObject();
                                queQuan.Oid = Guid.NewGuid();
                            }
                            queQuan.QuanHuyen = obj.QueQuan.QuanHuyen;
                            //
                            daTaoQueQuan = true;
                        }
                        //12.4 Xã phường
                        if (obj.QueQuan.XaPhuong != null && obj.QueQuan.XaPhuong != Guid.Empty && ((hoSoTrongHRM.QueQuan == null || hoSoTrongHRM.QueQuan.XaPhuong == null) || (hoSoTrongHRM.QueQuan.XaPhuong != null && !obj.QueQuan.XaPhuong.Equals(hoSoTrongHRM.QueQuan.XaPhuong))))
                        {
                            if (queQuan == null)
                            {
                                queQuan = factory_DiaChi.CreateManagedObject();
                                queQuan.Oid = Guid.NewGuid();
                            }
                            queQuan.XaPhuong = obj.QueQuan.XaPhuong;
                            //
                            daTaoQueQuan = true;
                        }
                        //12.5 Số nhà
                        if (!string.IsNullOrEmpty(obj.QueQuan.SoNha) && !obj.QueQuan.SoNha.Trim().Equals(hoSoTrongHRM.QueQuan.SoNha))
                        {
                            if (queQuan == null)
                            {
                                queQuan = factory_DiaChi.CreateManagedObject();
                                queQuan.Oid = Guid.NewGuid();
                            }
                            queQuan.SoNha = obj.QueQuan.SoNha;
                            //
                            daTaoQueQuan = true;
                        }
                        //
                        if (daTaoQueQuan)
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                            {
                                try
                                {
                                    if (newObj == null)
                                    {
                                        newObj = factory.CreateManagedObject();
                                        newObj.Oid = Guid.NewGuid();
                                        newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                                    } 
                                    //
                                    if (!string.IsNullOrEmpty(queQuan.SoNha))
                                    {
                                        queQuan.FullDiaChi = queQuan.SoNha;
                                    }
                                    if (queQuan.XaPhuong != null)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(queQuan.XaPhuong.Value);
                                        if (item != null)
                                            queQuan.FullDiaChi = queQuan.FullDiaChi + "-" + item.TenXaPhuong;
                                    }
                                    if (queQuan.QuanHuyen != null)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(queQuan.QuanHuyen.Value);
                                        if (item != null)
                                            queQuan.FullDiaChi = queQuan.FullDiaChi + "-" + item.TenQuanHuyen;
                                    }
                                    if (queQuan.TinhThanh != null)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(queQuan.TinhThanh.Value);
                                        if (item != null)
                                            queQuan.FullDiaChi = queQuan.FullDiaChi + "-" + item.TenTinhThanh;
                                    }
                                    if (queQuan.QuocGia != null)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(queQuan.QuocGia.Value);
                                        if (item != null)
                                            queQuan.FullDiaChi = queQuan.FullDiaChi + "-" + item.TenQuocGia;
                                    }
                                    //
                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    //
                                    daCapNhatHoSo = true;
                                    newObj.QueQuan = queQuan.Oid;
                                }
                                catch (Exception ex) { throw ex; }
                            }
                            
                        }
                        //////////////////End Quê quán/////////////////////////////

                        //////////////////13. Địa chỉ thường trú/////////////////////////////
                        DiaChi diaChiThuongTru = factory_DiaChi.GetDiaChiByOid(newObj != null && newObj.DiaChiThuongTru != null ? newObj.DiaChiThuongTru.Value : Guid.Empty);
                        bool daTaoDiaChiThuongTru = false;

                        //13.1 Quốc gia
                        if (obj.DiaChiThuongTru.QuocGia != null && obj.DiaChiThuongTru.QuocGia != Guid.Empty && ((hoSoTrongHRM.DiaChiThuongTru == null || hoSoTrongHRM.DiaChiThuongTru.QuocGia == null) || (hoSoTrongHRM.DiaChiThuongTru.QuocGia != null && !obj.DiaChiThuongTru.QuocGia.Equals(hoSoTrongHRM.DiaChiThuongTru.QuocGia))))
                        {
                            if (diaChiThuongTru == null)
                            {
                                diaChiThuongTru = factory_DiaChi.CreateManagedObject();
                                diaChiThuongTru.Oid = Guid.NewGuid();
                            }
                            diaChiThuongTru.QuocGia = obj.DiaChiThuongTru.QuocGia;
                            //
                            daTaoDiaChiThuongTru = true;
                        }
                        //13.2 Tỉnh thành
                        if (obj.DiaChiThuongTru.TinhThanh != null && obj.DiaChiThuongTru.TinhThanh != Guid.Empty && ((hoSoTrongHRM.DiaChiThuongTru == null || hoSoTrongHRM.DiaChiThuongTru.TinhThanh == null) || (hoSoTrongHRM.DiaChiThuongTru.TinhThanh != null && !obj.DiaChiThuongTru.TinhThanh.Equals(hoSoTrongHRM.DiaChiThuongTru.TinhThanh))))
                        {
                            if (diaChiThuongTru == null)
                            {
                                diaChiThuongTru = factory_DiaChi.CreateManagedObject();
                                diaChiThuongTru.Oid = Guid.NewGuid();
                            }
                            diaChiThuongTru.TinhThanh = obj.DiaChiThuongTru.TinhThanh;
                            //
                            daTaoDiaChiThuongTru = true;
                        }
                        //13.3 Quận huyện
                        if (obj.DiaChiThuongTru.QuanHuyen != null && obj.DiaChiThuongTru.QuanHuyen != Guid.Empty && ((hoSoTrongHRM.DiaChiThuongTru == null || hoSoTrongHRM.DiaChiThuongTru.QuanHuyen == null) || (hoSoTrongHRM.DiaChiThuongTru.QuanHuyen != null && !obj.DiaChiThuongTru.QuanHuyen.Equals(hoSoTrongHRM.DiaChiThuongTru.QuanHuyen))))
                        {
                            if (diaChiThuongTru == null)
                            {
                                diaChiThuongTru = factory_DiaChi.CreateManagedObject();
                                diaChiThuongTru.Oid = Guid.NewGuid();
                            }
                            diaChiThuongTru.QuanHuyen = obj.DiaChiThuongTru.QuanHuyen;
                            //
                            daTaoDiaChiThuongTru = true;
                        }
                        //13.4 Xã phường
                        if (obj.DiaChiThuongTru.XaPhuong != null && obj.DiaChiThuongTru.XaPhuong != Guid.Empty && ((hoSoTrongHRM.DiaChiThuongTru == null || hoSoTrongHRM.DiaChiThuongTru.XaPhuong == null) || (hoSoTrongHRM.DiaChiThuongTru.XaPhuong != null && !obj.DiaChiThuongTru.XaPhuong.Equals(hoSoTrongHRM.DiaChiThuongTru.XaPhuong))))
                        {
                            if (diaChiThuongTru == null)
                            {
                                diaChiThuongTru = factory_DiaChi.CreateManagedObject();
                                diaChiThuongTru.Oid = Guid.NewGuid();
                            }
                            diaChiThuongTru.XaPhuong = obj.DiaChiThuongTru.XaPhuong;
                            //
                            daTaoDiaChiThuongTru = true;
                        }
                        //13.5 Số nhà
                        if (!string.IsNullOrEmpty(obj.DiaChiThuongTru.SoNha) && !obj.DiaChiThuongTru.SoNha.Trim().Equals(hoSoTrongHRM.DiaChiThuongTru.SoNha))
                        {
                            if (diaChiThuongTru == null)
                            {
                                diaChiThuongTru = factory_DiaChi.CreateManagedObject();
                                diaChiThuongTru.Oid = Guid.NewGuid();
                            }
                            diaChiThuongTru.SoNha = obj.DiaChiThuongTru.SoNha;
                            //
                            daTaoDiaChiThuongTru = true;
                        }
                        //
                        if (daTaoDiaChiThuongTru)
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                            {
                                try
                                {
                                    if (newObj == null)
                                    {
                                        newObj = factory.CreateManagedObject();
                                        newObj.Oid = Guid.NewGuid();
                                        newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                                    }
                                    //
                                    if (!string.IsNullOrEmpty(diaChiThuongTru.SoNha))
                                    {
                                        diaChiThuongTru.FullDiaChi = diaChiThuongTru.SoNha;
                                    }
                                    if (diaChiThuongTru.XaPhuong != null)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(diaChiThuongTru.XaPhuong.Value);
                                        if (item != null)
                                            diaChiThuongTru.FullDiaChi = diaChiThuongTru.FullDiaChi + "-" + item.TenXaPhuong;
                                    }
                                    if (diaChiThuongTru.QuanHuyen != null)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(diaChiThuongTru.QuanHuyen.Value);
                                        if (item != null)
                                            diaChiThuongTru.FullDiaChi = diaChiThuongTru.FullDiaChi + "-" + item.TenQuanHuyen;
                                    }
                                    if (diaChiThuongTru.TinhThanh != null)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(diaChiThuongTru.TinhThanh.Value);
                                        if (item != null)
                                            diaChiThuongTru.FullDiaChi = diaChiThuongTru.FullDiaChi + "-" + item.TenTinhThanh;
                                    }
                                    if (diaChiThuongTru.QuocGia != null)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(diaChiThuongTru.QuocGia.Value);
                                        if (item != null)
                                            diaChiThuongTru.FullDiaChi = diaChiThuongTru.FullDiaChi + "-" + item.TenQuocGia;
                                    }
                                    //
                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    //
                                    daCapNhatHoSo = true;
                                    newObj.DiaChiThuongTru = diaChiThuongTru.Oid;
                                }
                                catch (Exception ex) { throw ex; }
                            }
                        }
                        //////////////////End Địa chỉ thường trú/////////////////////////////


                        //////////////////14. Nơi ở hiện nay/////////////////////////////
                        DiaChi noiOHienNay = factory_DiaChi.GetDiaChiByOid(newObj != null && newObj.NoiOHienNay != null ? newObj.NoiOHienNay.Value : Guid.Empty);
                        bool daTaoNoiOHienNay = false;

                        //14.1 Quốc gia
                        if (obj.NoiOHienNay.QuocGia != null && obj.NoiOHienNay.QuocGia != Guid.Empty && ((hoSoTrongHRM.NoiOHienNay == null || hoSoTrongHRM.NoiOHienNay.QuocGia == null) || (hoSoTrongHRM.NoiOHienNay.QuocGia != null && !obj.NoiOHienNay.QuocGia.Equals(hoSoTrongHRM.NoiOHienNay.QuocGia))))
                        {
                            if (noiOHienNay == null)
                            {
                                noiOHienNay = factory_DiaChi.CreateManagedObject();
                                noiOHienNay.Oid = Guid.NewGuid();
                            }
                            noiOHienNay.QuocGia = obj.NoiOHienNay.QuocGia;
                            //
                            daTaoNoiOHienNay = true;
                        }
                        //14.2 Tỉnh thành
                        if (obj.NoiOHienNay.TinhThanh != null && obj.NoiOHienNay.TinhThanh != Guid.Empty && ((hoSoTrongHRM.NoiOHienNay == null || hoSoTrongHRM.NoiOHienNay.TinhThanh == null) || (hoSoTrongHRM.NoiOHienNay.TinhThanh != null && !obj.NoiOHienNay.TinhThanh.Equals(hoSoTrongHRM.NoiOHienNay.TinhThanh))))
                        {
                            if (noiOHienNay == null)
                            {
                                noiOHienNay = factory_DiaChi.CreateManagedObject();
                                noiOHienNay.Oid = Guid.NewGuid();
                            }
                            noiOHienNay.TinhThanh = obj.NoiOHienNay.TinhThanh;
                            //
                            daTaoNoiOHienNay = true;
                        }
                        //14.3 Quận huyện
                        if (obj.NoiOHienNay.QuanHuyen != null && obj.NoiOHienNay.QuanHuyen != Guid.Empty && ((hoSoTrongHRM.NoiOHienNay == null || hoSoTrongHRM.NoiOHienNay.QuanHuyen == null) || (hoSoTrongHRM.NoiOHienNay.QuanHuyen != null && !obj.NoiOHienNay.QuanHuyen.Equals(hoSoTrongHRM.NoiOHienNay.QuanHuyen))))
                        {
                            if (noiOHienNay == null)
                            {
                                noiOHienNay = factory_DiaChi.CreateManagedObject();
                                noiOHienNay.Oid = Guid.NewGuid();
                            }
                            noiOHienNay.QuanHuyen = obj.NoiOHienNay.QuanHuyen;
                            //
                            daTaoNoiOHienNay = true;
                        }
                        //14.4 Xã phường
                        if (obj.NoiOHienNay.XaPhuong != null && obj.NoiOHienNay.XaPhuong != Guid.Empty && ((hoSoTrongHRM.NoiOHienNay == null || hoSoTrongHRM.NoiOHienNay.XaPhuong == null) || (hoSoTrongHRM.NoiOHienNay.XaPhuong != null && !obj.NoiOHienNay.XaPhuong.Equals(hoSoTrongHRM.NoiOHienNay.XaPhuong))))
                        {
                            if (noiOHienNay == null)
                            {
                                noiOHienNay = factory_DiaChi.CreateManagedObject();
                                noiOHienNay.Oid = Guid.NewGuid();
                            }
                            noiOHienNay.XaPhuong = obj.NoiOHienNay.XaPhuong;
                            //
                            daTaoNoiOHienNay = true;
                        }
                        //14.5 Số nhà
                        if (!string.IsNullOrEmpty(obj.NoiOHienNay.SoNha) && !obj.NoiOHienNay.SoNha.Trim().Equals(hoSoTrongHRM.NoiOHienNay.SoNha))
                        {
                            if (noiOHienNay == null)
                            {
                                noiOHienNay = factory_DiaChi.CreateManagedObject();
                                noiOHienNay.Oid = Guid.NewGuid();
                            }
                            noiOHienNay.SoNha = obj.NoiOHienNay.SoNha;
                            //
                            daTaoNoiOHienNay = true;
                        }
                        //
                        if (daTaoNoiOHienNay)
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                            {
                                try
                                {
                                    if (newObj == null)
                                    {
                                        newObj = factory.CreateManagedObject();
                                        newObj.Oid = Guid.NewGuid();
                                        newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                                    }
                                    //
                                    if (!string.IsNullOrEmpty(noiOHienNay.SoNha))
                                    {
                                        noiOHienNay.FullDiaChi = noiOHienNay.SoNha;
                                    }
                                    if (noiOHienNay.XaPhuong != null)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(noiOHienNay.XaPhuong.Value);
                                        if (item != null)
                                            noiOHienNay.FullDiaChi = noiOHienNay.FullDiaChi + "-" + item.TenXaPhuong;
                                    }
                                    if (noiOHienNay.QuanHuyen != null)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(noiOHienNay.QuanHuyen.Value);
                                        if (item != null)
                                            noiOHienNay.FullDiaChi = noiOHienNay.FullDiaChi + "-" + item.TenQuanHuyen;
                                    }
                                    if (noiOHienNay.TinhThanh != null)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(noiOHienNay.TinhThanh.Value);
                                        if (item != null)
                                            noiOHienNay.FullDiaChi = noiOHienNay.FullDiaChi + "-" + item.TenTinhThanh;
                                    }
                                    if (noiOHienNay.QuocGia != null)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(noiOHienNay.QuocGia.Value);
                                        if (item != null)
                                            noiOHienNay.FullDiaChi = noiOHienNay.FullDiaChi + "-" + item.TenQuocGia;
                                    }
                                    //
                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    //
                                    daCapNhatHoSo = true;
                                    newObj.NoiOHienNay = noiOHienNay.Oid;
                                }
                                catch (Exception ex) { throw ex; }
                            }
                        }
                        //////////////////End Nơi ở hiện nay/////////////////////////////

                        //15. CMND
                        if (!string.IsNullOrEmpty(obj.CMND) && !obj.CMND.Trim().Equals(hoSoTrongHRM.CMND.Trim()))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.CMND = obj.CMND.Trim();
                        }
                        //16. Ngày cấp
                        if (obj.NgayCap != DateTime.MinValue && !obj.NgayCap.Equals(hoSoTrongHRM.NgayCap))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            ; daCapNhatHoSo = true;
                            newObj.NgayCap = obj.NgayCap;
                        }
                        //17. Nơi cấp
                        if (obj.NoiCap != Guid.Empty && !obj.NoiCap.Equals(hoSoTrongHRM.NoiCap))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.NoiCap = obj.NoiCap;
                        }
                        //18. Tình trạng hôn nhân
                        if (obj.TinhTrangHonNhan != Guid.Empty && !obj.TinhTrangHonNhan.Equals(hoSoTrongHRM.TinhTrangHonNhan))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TinhTrangHonNhan = obj.TinhTrangHonNhan;
                        }
                        //19. Tình độ văn hóa
                        if (obj.TrinhDoVanHoa != Guid.Empty && !obj.TrinhDoVanHoa.Equals(hoSoTrongHRM.TrinhDoVanHoa))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TrinhDoVanHoa = obj.TrinhDoVanHoa;
                        }
                        //20. Tình độ chuyên môn
                        if (obj.TrinhDoChuyenMon != Guid.Empty && !obj.TrinhDoChuyenMon.Equals(hoSoTrongHRM.TrinhDoChuyenMon))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TrinhDoChuyenMon = obj.TrinhDoChuyenMon;
                        }
                        //21. Chuyên ngành đào tạo
                        if (obj.ChuyenNganhDaoTao != Guid.Empty && !obj.ChuyenNganhDaoTao.Equals(hoSoTrongHRM.ChuyenNganhDaoTao))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.ChuyenNganhDaoTao = obj.ChuyenNganhDaoTao;
                        }
                        //22. Trường đào tạo
                        if (obj.TruongDaoTao != Guid.Empty && !obj.TruongDaoTao.Equals(hoSoTrongHRM.TruongDaoTao))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TruongDaoTao = obj.TruongDaoTao;
                        }
                        //23. Năm tốt nghiệp
                        if (obj.NamTotNghiep > 0 && !obj.NamTotNghiep.Equals(hoSoTrongHRM.NamTotNghiep))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.NamTotNghiep = obj.NamTotNghiep;
                        }
                        //24. Ngoại ngữ
                        if (obj.NgoaiNgu != Guid.Empty && !obj.NgoaiNgu.Equals(hoSoTrongHRM.NgoaiNgu))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.NgoaiNgu = obj.NgoaiNgu;
                        }
                        //25. Trình độ ngoại ngữ
                        if (obj.TrinhDoNgoaiNgu != Guid.Empty && !obj.TrinhDoNgoaiNgu.Equals(hoSoTrongHRM.TrinhDoNgoaiNgu))
                        {
                            if (newObj == null)
                            {
                                newObj = factory.CreateManagedObject();
                                newObj.Oid = Guid.NewGuid();
                                newObj.ThongTinNhanVien = obj.ThongTinNhanVien;
                            }
                            daCapNhatHoSo = true;
                            newObj.TrinhDoNgoaiNgu = obj.TrinhDoNgoaiNgu;
                        }

                        if(daCapNhatHoSo)
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required,TimeSpan.FromSeconds(360)))
                            {
                                try
                                {
                                    //26. Ngày cập nhật
                                    newObj.NgayCapNhat = DateTime.Now;
                                    newObj.DaCapNhatHRM = false;
                                    newObj.UserUpdate = userUpdate;
                                    //
                                    factory.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    return true;
                                }
                                catch(Exception ex) { throw ex; }
                            }
                        }
                    }
                }
                catch (Exception ex) { throw ex; }
            }
            return false;
        }

        public String GetListTinhTrangHonNhanALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TinhTrangHonNhan> list = GetListTinhTrangHonNhanALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListQuocGiaALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_QuocGia> list = GetListQuocGiaALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTinhThanhALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TinhThanh> list = GetListTinhThanhALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListQuanHuyenALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_QuanHuyen> list = GetListQuanHuyenALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListXaPhuongALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_XaPhuong> list = GetListXaPhuongALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTrinhDoVanHoaALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TrinhDoVanHoa> list = GetListTrinhDoVanHoaALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTrinhDoChuyenMonALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TrinhDoChuyenMon> list = GetListTrinhDoChuyenMonALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListChuyenNganhDaoTaoALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_ChuyenNganhDaoTao> list = GetListChuyenNganhDaoTaoALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTruongDaoTaoALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TruongDaoTao> list = GetListTruongDaoTaoALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListNgoaiNguALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_NgoaiNgu> list = GetListNgoaiNguALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTrinhDoNgoaiNguALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TrinhDoNgoaiNgu> list = GetListTrinhDoNgoaiNguALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetHoSoNhanVienByOidNhanVien_Json(String publicKey, String token, Guid oidNhanVien)
        {
            DTO_Web_UpdateHoSoNhanVien list = GetHoSoNhanVienByOidNhanVien(publicKey, token, oidNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetListDanTocALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_DanToc> list = GetListDanTocALL(publicKey,token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_DanToc> GetListDanTocALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListDanTocALL();
                //
                DTO_DanToc item = new DTO_DanToc();
                item.TenDanToc = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0,item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public DTO_Web_UpdateHoSoNhanVien GetHoSoNhanVienByOidNhanVien(String publicKey, String token, Guid oidNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetHoSoNhanVienByOidNhanVien(oidNhanVien);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DTO_TinhTrangHonNhan> GetListTinhTrangHonNhanALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTinhTrangHonNhanALL();
                //
                DTO_TinhTrangHonNhan item = new DTO_TinhTrangHonNhan();
                item.TenTinhTrangHonNhan = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DTO_QuocGia> GetListQuocGiaALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListQuocGiaALL();
                //
                DTO_QuocGia item = new DTO_QuocGia();
                item.TenQuocGia = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_TinhThanh> GetListTinhThanhALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTinhThanhALL();
                //
                DTO_TinhThanh item = new DTO_TinhThanh();
                item.TenTinhThanh = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanHuyen> GetListQuanHuyenALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListQuanHuyenALL();
                //
                DTO_QuanHuyen item = new DTO_QuanHuyen();
                item.TenQuanHuyen = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DTO_XaPhuong> GetListXaPhuongALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListXaPhuongALL();
                //
                DTO_XaPhuong item = new DTO_XaPhuong();
                item.TenXaPhuong = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_TrinhDoVanHoa> GetListTrinhDoVanHoaALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTrinhDoVanHoaALL();
                //
                DTO_TrinhDoVanHoa item = new DTO_TrinhDoVanHoa();
                item.TenTrinhDoVanHoa = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_TrinhDoChuyenMon> GetListTrinhDoChuyenMonALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTrinhDoChuyenMonALL();
                //
                DTO_TrinhDoChuyenMon item = new DTO_TrinhDoChuyenMon();
                item.TenTrinhDoChuyenMon = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_ChuyenNganhDaoTao> GetListChuyenNganhDaoTaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListChuyenNganhDaoTaoALL();
                //
                DTO_ChuyenNganhDaoTao item = new DTO_ChuyenNganhDaoTao();
                item.TenChuyenNganhDaoTao = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_TruongDaoTao> GetListTruongDaoTaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTruongDaoTaoALL();
                //
                DTO_TruongDaoTao item = new DTO_TruongDaoTao();
                item.TenTruongDaoTao = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_NgoaiNgu> GetListNgoaiNguALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListNgoaiNguALL();
                //
                DTO_NgoaiNgu item = new DTO_NgoaiNgu();
                item.TenNgoaiNgu = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_TrinhDoNgoaiNgu> GetListTrinhDoNgoaiNguALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTrinhDoNgoaiNguALL();
                //
                DTO_TrinhDoNgoaiNgu item = new DTO_TrinhDoNgoaiNgu();
                item.TenTrinhDoNgoaiNgu = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
        public String GetListTonGiaoALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TonGiao> list = GetListTonGiaoALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_TonGiao> GetListTonGiaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                var tmpList = factory.GetListTonGiaoALL();
                //
                DTO_TonGiao item = new DTO_TonGiao();
                item.TenTonGiao = "---Không chọn---";
                item.Oid = Guid.Empty;
                tmpList.Insert(0, item);
                //
                return tmpList;
            }
            else
            {
                return null;
            }
        }
    }
}
