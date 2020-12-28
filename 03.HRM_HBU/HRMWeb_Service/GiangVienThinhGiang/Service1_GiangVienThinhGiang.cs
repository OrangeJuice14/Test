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
        public IQueryable<DTO_GiangVienThinhGiang> GiangVienThinhGiang_Find(String publicKey, String token, Guid boPhanId, string maNhanSu, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var obj = factory.GetDTOGiangVienThinhGiang_Find(boPhanId, maNhanSu, webUserId);
                //
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String GiangVienThinhGiang_Find_Json(String publicKey, String token, Guid boPhanId, string maNhanSu, Guid webUserId)
        {//DANG SD
            List<DTO_GiangVienThinhGiang> obj = GiangVienThinhGiang_Find(publicKey, token, boPhanId, maNhanSu, webUserId).ToList();
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public String GiangVienThinhGiang_ByOid_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_GiangVienThinhGiang obj = GiangVienThinhGiang_ByOid(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_GiangVienThinhGiang GiangVienThinhGiang_ByOid(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var obj = factory.Get_DTOGiangVienThinhGiang_ByOid(id);
                //
                return obj;
            }
            else
            {
                return null;
            }
        }
        public bool DeleteGiangVienThinhGiang(String publicKey, String token, List<DTO_GiangVienThinhGiang> list)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    //
                    GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                    foreach (var item in list)
                    {
                        var obj = factory.Get_GiangVienThinhGiang_ByOid(item.Oid);
                        if (obj != null)
                        {
                            obj.NhanVien.HoSo.GCRecord = 10000;
                        }
                    }
                    //
                    factory.SaveChanges();
                    //
                    return true;
                }
                catch(Exception ex ) { return false; }
            }
            else
            {
                return false;
            }
        }
        public bool CheckExistsMaQuanLyOfGiangVienThinhGiang(String publicKey, String token, Guid id,string maquanly)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var obj = factory.CheckExistsMaQuanLyOfGiangVienThinhGiang(id, maquanly);
                //
                return obj;
            }
            else
            {
                return false;
            }
        }
        public String GetListTinhTrangALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TinhTrang> list = GetListTinhTrangALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
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
        public String GetListDanTocALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_DanToc> list = GetListDanTocALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTonGiaoALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TonGiao> list = GetListTonGiaoALL(publicKey, token);
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
        public String GetListHocHamALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_HocHam> list = GetListHocHamALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListTrinhDoTinHocALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_TrinhDoTinHoc> list = GetListTrinhDoTinHocALL(publicKey, token);
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
        public String GetListHinhThucDaoTaoALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_HinhThucDaoTao> list = GetListHinhThucDaoTaoALL(publicKey, token);
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
        public String GetListCoQuanThueALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_CoQuanThue> list = GetListCoQuanThueALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetListNganHangALL_Json(String publicKey, String token)
        {
            IEnumerable<DTO_NganHang> list = GetListNganHangALL(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        //  
        public IEnumerable<DTO_DanToc> GetListDanTocALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListDanTocALL();
                //
                DTO_DanToc item = new DTO_DanToc();
                item.TenDanToc = "---Không chọn---";
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
        public IEnumerable<DTO_TinhTrang> GetListTinhTrangALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListTinhTrangALL();
                //
                DTO_TinhTrang item = new DTO_TinhTrang();
                item.TenTinhTrang = "---Không chọn---";
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
        public IEnumerable<DTO_TinhTrangHonNhan> GetListTinhTrangHonNhanALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
        public IEnumerable<DTO_TonGiao> GetListTonGiaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
        public IEnumerable<DTO_TinhThanh> GetListTinhThanhALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
        public IEnumerable<DTO_TrinhDoTinHoc> GetListTrinhDoTinHocALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListTrinhDoTinHocALL();
                //
                DTO_TrinhDoTinHoc item = new DTO_TrinhDoTinHoc();
                item.TenTrinhDoTinHoc = "---Không chọn---";
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
        public IEnumerable<DTO_HocHam> GetListHocHamALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListHocHamALL();
                //
                DTO_HocHam item = new DTO_HocHam();
                item.TenHocHam = "---Không chọn---";
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
        public IEnumerable<DTO_HinhThucDaoTao> GetListHinhThucDaoTaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListHinhThucDaoTaoALL();
                //
                DTO_HinhThucDaoTao item = new DTO_HinhThucDaoTao();
                item.TenHinhThucDaoTao = "---Không chọn---";
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
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
        public IEnumerable<DTO_CoQuanThue> GetListCoQuanThueALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListCoQuanThueALL();
                //
                DTO_CoQuanThue item = new DTO_CoQuanThue();
                item.TenCoQuanThue = "---Không chọn---";
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
        public IEnumerable<DTO_NganHang> GetListNganHangALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                GiangVienThinhGiang_Factory factory = GiangVienThinhGiang_Factory.New();
                var tmpList = factory.GetListNganHangALL();
                //
                DTO_NganHang item = new DTO_NganHang();
                item.TenNganHang = "---Không chọn---";
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

        public bool SaveGiangVienThinhGiang(String publicKey, String token, DTO_GiangVienThinhGiang obj, string type)
        {
            bool result = false;
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,TimeSpan.FromSeconds(360)))
                    {
                        try
                        {
                            GiangVienThinhGiang_Factory factory = new GiangVienThinhGiang_Factory();
                            GiangVienThinhGiang thinhGiang = factory.Get_GiangVienThinhGiang_ByOid(obj.Oid);
                            if (thinhGiang == null)
                            {
                                thinhGiang = factory.CreateManagedObject();
                                thinhGiang.Oid = obj.Oid != null ? obj.Oid : Guid.NewGuid();
                                thinhGiang.TaiBoMon = obj.BoPhan;
                                thinhGiang.DonViCongTac = obj.DonViCongTac;
                                thinhGiang.CMND_ThinhGiang = obj.CMND;
                            }
                            //
                            if (type == "SoYeuLyLich") 
                            {
                                #region 1. Sơ yếu lý lịch

                                //1. Nhân viên
                                if (thinhGiang.NhanVien == null)
                                {
                                    #region 1.1 Thêm mới
                                    NhanVien_Factory factory_NhanVien = new NhanVien_Factory();
                                    NhanVien nhanVien = factory_NhanVien.GetNhanVienByOid(thinhGiang.Oid);
                                    if (nhanVien == null)
                                    {
                                        nhanVien = factory_NhanVien.CreateManagedObject();
                                        nhanVien.Oid = thinhGiang.Oid;
                                        nhanVien.TinhTrang = obj.TinhTrang;
                                        nhanVien.NgayVaoCoQuan = obj.NgayVaoCoQuan;

                                        //2. Hồ sơ
                                        HoSo_Factory factory_HoSo = new HoSo_Factory();
                                        HoSo hoSo = factory_HoSo.GetByID(nhanVien.Oid);
                                        if (hoSo == null)
                                        {
                                            hoSo = factory_HoSo.CreateManagedObject();
                                            hoSo.Oid = thinhGiang.Oid;
                                            hoSo.MaQuanLy = obj.MaQuanLy;
                                            hoSo.Ho = obj.Ho;
                                            hoSo.Ten = obj.Ten;
                                            hoSo.HoTen = obj.Ho + " " + obj.Ten;
                                            hoSo.GioiTinh = obj.GioiTinh == "Nam" ? Convert.ToByte(0) : Convert.ToByte(1);
                                            hoSo.NgaySinh = obj.NgaySinh;
                                            hoSo.Email = obj.Email;
                                            hoSo.DienThoaiDiDong = obj.DienThoaiDiDong;
                                            hoSo.DienThoaiNhaRieng = obj.DienThoaiNhaRieng;
                                            hoSo.CMND = obj.CMND;
                                            hoSo.NgayCap = obj.NgayCap;
                                            hoSo.NoiCap = obj.NoiCap;
                                            if (obj.QuocGia != null)
                                            {
                                                QuocGia qg = factory.GetQuocGiaByOid(obj.QuocGia.Value);
                                                if (qg != null)
                                                    hoSo.QuocTich = qg.Oid;
                                            }
                                            //

                                            #region 1.1.1 Địa chỉ (Nơi sinh, Địa chỉ thường trú, Nơi ở hiện nay)

                                            //////////////////11. Nơi sinh/////////////////////////////
                                            bool daTaoNoiSinh = false;
                                            DiaChi_Factory factory_DiaChi = new DiaChi_Factory();
                                            DiaChi noiSinh = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_NoiSinh != null ? obj.Oid_NoiSinh.Value : Guid.Empty);
                                            if (noiSinh == null)
                                            {
                                                noiSinh = factory_DiaChi.CreateManagedObject();
                                                noiSinh.Oid = Guid.NewGuid();
                                            }

                                            //11.1 Quốc gia
                                            if (obj.QuocGia_NoiSinh != null && obj.QuocGia_NoiSinh != Guid.Empty)
                                            {
                                                QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_NoiSinh.Value);
                                                if (item != null)
                                                    noiSinh.QuocGia = obj.QuocGia_NoiSinh;
                                                //
                                                daTaoNoiSinh = true;
                                            }
                                            else
                                            {
                                                noiSinh.QuocGia = null;
                                            }
                                            //11.2 Tỉnh thành
                                            if (obj.TinhThanh_NoiSinh != null && obj.TinhThanh_NoiSinh != Guid.Empty)
                                            {
                                                TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_NoiSinh.Value);
                                                if (item != null)
                                                    noiSinh.TinhThanh = obj.TinhThanh_NoiSinh;
                                                //
                                                daTaoNoiSinh = true;
                                            }
                                            else
                                            {
                                                noiSinh.TinhThanh = null;
                                            }
                                            //11.3 Quận huyện
                                            if (obj.QuanHuyen_NoiSinh != null && obj.QuanHuyen_NoiSinh != Guid.Empty)
                                            {
                                                QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_NoiSinh.Value);
                                                if (item != null)
                                                    noiSinh.QuanHuyen = obj.QuanHuyen_NoiSinh;
                                                //
                                                daTaoNoiSinh = true;
                                            }
                                            else
                                            {
                                                noiSinh.QuanHuyen = null;
                                            }
                                            //11.4 Xã phường
                                            if (obj.XaPhuong_NoiSinh != null && obj.XaPhuong_NoiSinh != Guid.Empty)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_NoiSinh.Value);
                                                if (item != null)
                                                    noiSinh.XaPhuong = obj.XaPhuong_NoiSinh;
                                                //
                                                daTaoNoiSinh = true;
                                            }
                                            else
                                            {
                                                noiSinh.XaPhuong = null;
                                            }
                                            //11.5 Số nhà
                                            if (!string.IsNullOrEmpty(obj.SoNha_NoiSinh))
                                            {
                                                noiSinh.SoNha = obj.SoNha_NoiSinh;
                                                //
                                                daTaoNoiSinh = true;
                                            }
                                            else
                                            {
                                                noiSinh.SoNha = string.Empty;
                                            }
                                            //
                                            if (daTaoNoiSinh)
                                            {
                                                try
                                                {
                                                    //
                                                    if (!string.IsNullOrEmpty(noiSinh.SoNha))
                                                    {
                                                        noiSinh.FullDiaChi = noiSinh.SoNha;
                                                    }
                                                    if (noiSinh.XaPhuong != null)
                                                    {
                                                        XaPhuong item = factory.GetXaPhuongByOid(noiSinh.XaPhuong.Value);
                                                        if (item != null)
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
                                                    //
                                                    hoSo.NoiSinh = noiSinh.Oid;
                                                }
                                                catch (Exception ex) { throw ex; }
                                            }
                                            //////////////////End Nơi sinh///////////////////////////// 

                                            //////////////////DCTT/////////////////////////////
                                            bool daTaoDCTT = false;
                                            DiaChi dcTT = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_DCTT != null ? obj.Oid_DCTT.Value : Guid.Empty);
                                            if (dcTT == null)
                                            {
                                                dcTT = factory_DiaChi.CreateManagedObject();
                                                dcTT.Oid = Guid.NewGuid();
                                            }

                                            //11.1 Quốc gia
                                            if (obj.QuocGia_DCTT != null && obj.QuocGia_DCTT != Guid.Empty)
                                            {
                                                QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_DCTT.Value);
                                                if (item != null)
                                                    dcTT.QuocGia = obj.QuocGia_DCTT;
                                                //
                                                daTaoDCTT = true;
                                            }
                                            else
                                            {
                                                dcTT.QuocGia = null;
                                            }
                                            //11.2 Tỉnh thành
                                            if (obj.TinhThanh_DCTT != null && obj.TinhThanh_DCTT != Guid.Empty)
                                            {
                                                
                                                TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_DCTT.Value);
                                                if (item != null)
                                                    dcTT.TinhThanh = obj.TinhThanh_DCTT;
                                                //
                                                daTaoDCTT = true;
                                            }
                                            else
                                            {
                                                dcTT.TinhThanh = null;
                                            }
                                            //11.3 Quận huyện
                                            if (obj.QuanHuyen_DCTT != null && obj.QuanHuyen_DCTT != Guid.Empty)
                                            {
                                                
                                                QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_DCTT.Value);
                                                if (item != null)
                                                    dcTT.QuanHuyen = obj.QuanHuyen_DCTT;
                                                //
                                                daTaoDCTT = true;
                                            }
                                            else
                                            {
                                                dcTT.QuanHuyen = null;
                                            }
                                            //11.4 Xã phường
                                            if (obj.XaPhuong_DCTT != null && obj.XaPhuong_DCTT != Guid.Empty)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_DCTT.Value);
                                                if (item != null)
                                                    dcTT.XaPhuong = obj.XaPhuong_DCTT;
                                                //
                                                daTaoDCTT = true;
                                            }
                                            else
                                            {
                                                dcTT.XaPhuong = null;
                                            }
                                            //11.5 Số nhà
                                            if (!string.IsNullOrEmpty(obj.SoNha_DCTT))
                                            {
                                               
                                                dcTT.SoNha = obj.SoNha_DCTT;
                                                //
                                                daTaoDCTT = true;
                                            }
                                            else
                                            {
                                                dcTT.SoNha = string.Empty;
                                            }
                                            //
                                            if (daTaoDCTT)
                                            {
                                                try
                                                {
                                                    //
                                                    if (!string.IsNullOrEmpty(dcTT.SoNha))
                                                    {
                                                        dcTT.FullDiaChi = dcTT.SoNha;
                                                    }
                                                    if (dcTT.XaPhuong != null)
                                                    {
                                                        XaPhuong item = factory.GetXaPhuongByOid(dcTT.XaPhuong.Value);
                                                        if (item != null)
                                                            dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenXaPhuong;
                                                    }
                                                    if (dcTT.QuanHuyen != null)
                                                    {
                                                        QuanHuyen item = factory.GetQuanHuyenByOid(dcTT.QuanHuyen.Value);
                                                        if (item != null)
                                                            dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenQuanHuyen;
                                                    }
                                                    if (dcTT.TinhThanh != null)
                                                    {
                                                        TinhThanh item = factory.GetTinhThanhByOid(dcTT.TinhThanh.Value);
                                                        if (item != null)
                                                            dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenTinhThanh;
                                                    }
                                                    if (dcTT.QuocGia != null)
                                                    {
                                                        QuocGia item = factory.GetQuocGiaByOid(dcTT.QuocGia.Value);
                                                        if (item != null)
                                                            dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenQuocGia;
                                                    }
                                                    //
                                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                                    //
                                                    hoSo.DiaChiThuongTru = dcTT.Oid;
                                                }
                                                catch (Exception ex) { throw ex; }
                                            }
                                            //////////////////End DCTT///////////////////////////// 


                                            //////////////////NOHN/////////////////////////////
                                            bool daTaoNOHN = false;
                                            DiaChi noHN = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_NOHN != null ? obj.Oid_NOHN.Value : Guid.Empty);
                                            if (noHN == null)
                                            {
                                                noHN = factory_DiaChi.CreateManagedObject();
                                                noHN.Oid = Guid.NewGuid();
                                            }

                                            //11.1 Quốc gia
                                            if (obj.QuocGia_NOHN != null && obj.QuocGia_NOHN != Guid.Empty)
                                            {
                                                QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_NOHN.Value);
                                                if (item != null)
                                                    noHN.QuocGia = obj.QuocGia_NOHN;
                                                //
                                                daTaoNOHN = true;
                                            }
                                            else
                                            {
                                                noHN.QuocGia = null;
                                            }
                                            //11.2 Tỉnh thành
                                            if (obj.TinhThanh_NOHN != null && obj.TinhThanh_NOHN != Guid.Empty)
                                            {
                                                TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_NOHN.Value);
                                                if (item != null)
                                                    noHN.TinhThanh = obj.TinhThanh_NOHN;
                                                //
                                                daTaoNOHN = true;
                                            }
                                            else
                                            {
                                                noHN.TinhThanh = null;
                                            }
                                            //11.3 Quận huyện
                                            if (obj.QuanHuyen_NOHN != null && obj.QuanHuyen_NOHN != Guid.Empty)
                                            {
                                                QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_NOHN.Value);
                                                if (item != null)
                                                    noHN.QuanHuyen = obj.QuanHuyen_NOHN;
                                                //
                                                daTaoNOHN = true;
                                            }
                                            else
                                            {
                                                noHN.QuanHuyen = null;
                                            }
                                            //11.4 Xã phường
                                            if (obj.XaPhuong_NOHN != null && obj.XaPhuong_NOHN != Guid.Empty)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_NOHN.Value);
                                                if (item != null)
                                                    noHN.XaPhuong = obj.XaPhuong_NOHN;
                                                //
                                                daTaoNOHN = true;
                                            }
                                            else
                                            {
                                                noHN.XaPhuong = null;
                                            }
                                            //11.5 Số nhà
                                            if (!string.IsNullOrEmpty(obj.SoNha_NOHN))
                                            {
                                                noHN.SoNha = obj.SoNha_NOHN;
                                                //
                                                daTaoNOHN = true;
                                            }
                                            else
                                            {
                                                noHN.SoNha = string.Empty;
                                            }
                                            //
                                            if (daTaoNOHN)
                                            {
                                                try
                                                {
                                                    //
                                                    if (!string.IsNullOrEmpty(noHN.SoNha))
                                                    {
                                                        noHN.FullDiaChi = noHN.SoNha;
                                                    }
                                                    if (noHN.XaPhuong != null)
                                                    {
                                                        XaPhuong item = factory.GetXaPhuongByOid(noHN.XaPhuong.Value);
                                                        if (item != null)
                                                            noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenXaPhuong;
                                                    }
                                                    if (noHN.QuanHuyen != null)
                                                    {
                                                        QuanHuyen item = factory.GetQuanHuyenByOid(noHN.QuanHuyen.Value);
                                                        if (item != null)
                                                            noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenQuanHuyen;
                                                    }
                                                    if (noHN.TinhThanh != null)
                                                    {
                                                        TinhThanh item = factory.GetTinhThanhByOid(noHN.TinhThanh.Value);
                                                        if (item != null)
                                                            noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenTinhThanh;
                                                    }
                                                    if (noHN.QuocGia != null)
                                                    {
                                                        QuocGia item = factory.GetQuocGiaByOid(noHN.QuocGia.Value);
                                                        if (item != null)
                                                            noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenQuocGia;
                                                    }
                                                    //
                                                    factory_DiaChi.SaveChangesWithoutTransactionScope();
                                                    //
                                                    hoSo.NoiOHienNay = noHN.Oid;
                                                }
                                                catch (Exception ex) { throw ex; }
                                            }
                                            //////////////////End DCTT///////////////////////////// 
                                            #endregion
                                        }
                                        //Lưu hồ sơ
                                        factory_HoSo.SaveChanges();
                                    }

                                    //Lưu nhân viên
                                    factory_NhanVien.SaveChanges();

                                    #endregion
                                }
                                else
                                {
                                    #region 1.2 Sửa
                                    thinhGiang.TaiBoMon = obj.BoPhan;
                                    thinhGiang.NhanVien.HoSo.MaQuanLy = obj.MaQuanLy;
                                    thinhGiang.NhanVien.HoSo.Ho = obj.Ho;
                                    thinhGiang.NhanVien.HoSo.Ten = obj.Ten;
                                    thinhGiang.NhanVien.HoSo.HoTen = obj.Ho + " " + obj.Ten;
                                    thinhGiang.NhanVien.HoSo.GioiTinh = obj.GioiTinh == "Nam" ? Convert.ToByte(0) : Convert.ToByte(1);
                                    thinhGiang.NhanVien.HoSo.NgaySinh = obj.NgaySinh;
                                    thinhGiang.NhanVien.NgayVaoCoQuan = obj.NgayVaoCoQuan;
                                    thinhGiang.NhanVien.HoSo.Email = obj.Email;
                                    thinhGiang.NhanVien.HoSo.Email = obj.Email;
                                    thinhGiang.CMND_ThinhGiang = obj.CMND;
                                    thinhGiang.NhanVien.HoSo.CMND = obj.CMND;
                                    thinhGiang.NhanVien.HoSo.NgayCap = obj.NgayCap;
                                    thinhGiang.NhanVien.HoSo.NoiCap = obj.NoiCap;
                                    if (obj.QuocGia != null)
                                    {
                                        QuocGia qg = factory.GetQuocGiaByOid(obj.QuocGia.Value);
                                        if (qg != null)
                                            thinhGiang.NhanVien.HoSo.QuocTich = qg.Oid;
                                    }
                                    thinhGiang.NhanVien.HoSo.DienThoaiNhaRieng = obj.DienThoaiNhaRieng;
                                    thinhGiang.NhanVien.TinhTrang = obj.TinhTrang;
                                    thinhGiang.DonViCongTac = obj.DonViCongTac;
                                    //
                                    #region 1.2.1 Địa chỉ (Nơi sinh, Địa chỉ thường trú, Nơi ở hiện nay)

                                    //////////////////11. Nơi sinh/////////////////////////////
                                    bool daTaoNoiSinh = false;
                                    DiaChi_Factory factory_DiaChi = new DiaChi_Factory();
                                    DiaChi noiSinh = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_NoiSinh != null ? obj.Oid_NoiSinh.Value : Guid.Empty);
                                    //
                                    if (noiSinh == null)
                                    {
                                        noiSinh = factory_DiaChi.CreateManagedObject();
                                        noiSinh.Oid = Guid.NewGuid();
                                    }

                                    //11.1 Quốc gia
                                    if (obj.QuocGia_NoiSinh != null && obj.QuocGia_NoiSinh != Guid.Empty)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_NoiSinh.Value);
                                        if (item != null)
                                            noiSinh.QuocGia = obj.QuocGia_NoiSinh;
                                        //
                                        daTaoNoiSinh = true;
                                    }
                                    else
                                    {
                                        noiSinh.QuocGia = null;
                                    }
                                    //11.2 Tỉnh thành
                                    if (obj.TinhThanh_NoiSinh != null && obj.TinhThanh_NoiSinh != Guid.Empty)
                                    {
                                       
                                        TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_NoiSinh.Value);
                                        if (item != null)
                                            noiSinh.TinhThanh = obj.TinhThanh_NoiSinh;
                                        //
                                        daTaoNoiSinh = true;
                                    }
                                    else
                                    {
                                        noiSinh.TinhThanh = null;
                                    }
                                    //11.3 Quận huyện
                                    if (obj.QuanHuyen_NoiSinh != null && obj.QuanHuyen_NoiSinh != Guid.Empty)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_NoiSinh.Value);
                                        if (item != null)
                                            noiSinh.QuanHuyen = obj.QuanHuyen_NoiSinh;
                                        //
                                        daTaoNoiSinh = true;
                                    }
                                    else
                                    {
                                        noiSinh.QuanHuyen = null;
                                    }
                                    //11.4 Xã phường
                                    if (obj.XaPhuong_NoiSinh != null && obj.XaPhuong_NoiSinh != Guid.Empty)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_NoiSinh.Value);
                                        if (item != null)
                                            noiSinh.XaPhuong = obj.XaPhuong_NoiSinh;
                                        //
                                        daTaoNoiSinh = true;
                                    }
                                    else
                                    {
                                        noiSinh.XaPhuong = null;
                                    }
                                    //11.5 Số nhà
                                    if (!string.IsNullOrEmpty(obj.SoNha_NoiSinh))
                                    {
                                        noiSinh.SoNha = obj.SoNha_NoiSinh;
                                        //
                                        daTaoNoiSinh = true;
                                    }
                                    else
                                    {
                                        noiSinh.SoNha = string.Empty;
                                    }
                                    //
                                    if (daTaoNoiSinh)
                                    {
                                        try
                                        {
                                            //
                                            if (!string.IsNullOrEmpty(noiSinh.SoNha))
                                            {
                                                noiSinh.FullDiaChi = noiSinh.SoNha;
                                            }
                                            if (noiSinh.XaPhuong != null)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(noiSinh.XaPhuong.Value);
                                                if (item != null)
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
                                            //
                                            thinhGiang.NhanVien.HoSo.NoiSinh = noiSinh.Oid;
                                        }
                                        catch (Exception ex) { throw ex; }
                                    }
                                    //////////////////End Nơi sinh///////////////////////////// 

                                    //////////////////DCTT/////////////////////////////
                                    bool daTaoDCTT = false;
                                    DiaChi dcTT = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_DCTT != null ? obj.Oid_DCTT.Value : Guid.Empty);
                                    //
                                    if (dcTT == null)
                                    {
                                        dcTT = factory_DiaChi.CreateManagedObject();
                                        dcTT.Oid = Guid.NewGuid();
                                    }

                                    //11.1 Quốc gia
                                    if (obj.QuocGia_DCTT != null && obj.QuocGia_DCTT != Guid.Empty)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_DCTT.Value);
                                        if (item != null)
                                            dcTT.QuocGia = obj.QuocGia_DCTT;
                                        //
                                        daTaoDCTT = true;
                                    }
                                    else
                                    {
                                        dcTT.QuocGia = null;
                                    }
                                    //11.2 Tỉnh thành
                                    if (obj.TinhThanh_DCTT != null && obj.TinhThanh_DCTT != Guid.Empty)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_DCTT.Value);
                                        if (item != null)
                                            dcTT.TinhThanh = obj.TinhThanh_DCTT;
                                        //
                                        daTaoDCTT = true;
                                    }
                                    else
                                    {
                                        dcTT.TinhThanh = null;
                                    }
                                    //11.3 Quận huyện
                                    if (obj.QuanHuyen_DCTT != null && obj.QuanHuyen_DCTT != Guid.Empty)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_DCTT.Value);
                                        if (item != null)
                                            dcTT.QuanHuyen = obj.QuanHuyen_DCTT;
                                        //
                                        daTaoDCTT = true;
                                    }
                                    else
                                    {
                                        dcTT.QuanHuyen = null;
                                    }
                                    //11.4 Xã phường
                                    if (obj.XaPhuong_DCTT != null && obj.XaPhuong_DCTT != Guid.Empty)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_DCTT.Value);
                                        if (item != null)
                                            dcTT.XaPhuong = obj.XaPhuong_DCTT;
                                        //
                                        daTaoDCTT = true;
                                    }
                                    else
                                    {
                                        dcTT.XaPhuong = null;
                                    }
                                    //11.5 Số nhà
                                    if (!string.IsNullOrEmpty(obj.SoNha_DCTT))
                                    {
                                        dcTT.SoNha = obj.SoNha_DCTT;
                                        //
                                        daTaoDCTT = true;
                                    }
                                    else
                                    {
                                        dcTT.SoNha = string.Empty;
                                    }
                                    //
                                    if (daTaoDCTT)
                                    {
                                        try
                                        {
                                            //
                                            if (!string.IsNullOrEmpty(dcTT.SoNha))
                                            {
                                                dcTT.FullDiaChi = dcTT.SoNha;
                                            }
                                            if (dcTT.XaPhuong != null)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(dcTT.XaPhuong.Value);
                                                if (item != null)
                                                    dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenXaPhuong;
                                            }
                                            if (dcTT.QuanHuyen != null)
                                            {
                                                QuanHuyen item = factory.GetQuanHuyenByOid(dcTT.QuanHuyen.Value);
                                                if (item != null)
                                                    dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenQuanHuyen;
                                            }
                                            if (dcTT.TinhThanh != null)
                                            {
                                                TinhThanh item = factory.GetTinhThanhByOid(dcTT.TinhThanh.Value);
                                                if (item != null)
                                                    dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenTinhThanh;
                                            }
                                            if (dcTT.QuocGia != null)
                                            {
                                                QuocGia item = factory.GetQuocGiaByOid(dcTT.QuocGia.Value);
                                                if (item != null)
                                                    dcTT.FullDiaChi = dcTT.FullDiaChi + "-" + item.TenQuocGia;
                                            }
                                            //
                                            factory_DiaChi.SaveChangesWithoutTransactionScope();
                                            //
                                            thinhGiang.NhanVien.HoSo.DiaChiThuongTru = dcTT.Oid;
                                        }
                                        catch (Exception ex) { throw ex; }
                                    }
                                    //////////////////End DCTT///////////////////////////// 


                                    //////////////////NOHN/////////////////////////////
                                    bool daTaoNOHN = false;
                                    DiaChi noHN = factory_DiaChi.GetDiaChiByOid(obj != null && obj.Oid_NOHN != null ? obj.Oid_NOHN.Value : Guid.Empty);
                                    //
                                    if (noHN == null)
                                    {
                                        noHN = factory_DiaChi.CreateManagedObject();
                                        noHN.Oid = Guid.NewGuid();
                                    }

                                    //11.1 Quốc gia
                                    if (obj.QuocGia_NOHN != null && obj.QuocGia_NOHN != Guid.Empty)
                                    {
                                        QuocGia item = factory.GetQuocGiaByOid(obj.QuocGia_NOHN.Value);
                                        if (item != null)
                                            noHN.QuocGia = obj.QuocGia_NOHN;
                                        //
                                        daTaoNOHN = true;
                                    }
                                    else
                                    {
                                        noHN.QuocGia = null;
                                    }
                                    //11.2 Tỉnh thành
                                    if (obj.TinhThanh_NOHN != null && obj.TinhThanh_NOHN != Guid.Empty)
                                    {
                                        TinhThanh item = factory.GetTinhThanhByOid(obj.TinhThanh_NOHN.Value);
                                        if (item != null)
                                            noHN.TinhThanh = obj.TinhThanh_NOHN;
                                        //
                                        daTaoNOHN = true;
                                    }
                                    else
                                    {
                                        noHN.TinhThanh = null;
                                    }
                                    //11.3 Quận huyện
                                    if (obj.QuanHuyen_NOHN != null && obj.QuanHuyen_NOHN != Guid.Empty)
                                    {
                                        QuanHuyen item = factory.GetQuanHuyenByOid(obj.QuanHuyen_NOHN.Value);
                                        if (item != null)
                                            noHN.QuanHuyen = obj.QuanHuyen_NOHN;
                                        //
                                        daTaoNOHN = true;
                                    }
                                    else
                                    {
                                        noHN.QuanHuyen = null;
                                    }
                                    //11.4 Xã phường
                                    if (obj.XaPhuong_NOHN != null && obj.XaPhuong_NOHN != Guid.Empty)
                                    {
                                        XaPhuong item = factory.GetXaPhuongByOid(obj.XaPhuong_NOHN.Value);
                                        if (item != null)
                                            noHN.XaPhuong = obj.XaPhuong_NOHN;
                                        //
                                        daTaoNOHN = true;
                                    }
                                    else
                                    {
                                        noHN.XaPhuong = null;
                                    }
                                    //11.5 Số nhà
                                    if (!string.IsNullOrEmpty(obj.SoNha_NOHN))
                                    {
                                        noHN.SoNha = obj.SoNha_NOHN;
                                        //
                                        daTaoNOHN = true;
                                    }
                                    else
                                    {
                                        noHN.SoNha = string.Empty;
                                    }
                                    //
                                    if (daTaoNOHN)
                                    {
                                        try
                                        {
                                            //
                                            if (!string.IsNullOrEmpty(noHN.SoNha))
                                            {
                                                noHN.FullDiaChi = noHN.SoNha;
                                            }
                                            if (noHN.XaPhuong != null)
                                            {
                                                XaPhuong item = factory.GetXaPhuongByOid(noHN.XaPhuong.Value);
                                                if (item != null)
                                                    noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenXaPhuong;
                                            }
                                            if (noHN.QuanHuyen != null)
                                            {
                                                QuanHuyen item = factory.GetQuanHuyenByOid(noHN.QuanHuyen.Value);
                                                if (item != null)
                                                    noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenQuanHuyen;
                                            }
                                            if (noHN.TinhThanh != null)
                                            {
                                                TinhThanh item = factory.GetTinhThanhByOid(noHN.TinhThanh.Value);
                                                if (item != null)
                                                    noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenTinhThanh;
                                            }
                                            if (noHN.QuocGia != null)
                                            {
                                                QuocGia item = factory.GetQuocGiaByOid(noHN.QuocGia.Value);
                                                if (item != null)
                                                    noHN.FullDiaChi = noHN.FullDiaChi + "-" + item.TenQuocGia;
                                            }
                                            //
                                            factory_DiaChi.SaveChangesWithoutTransactionScope();
                                            //
                                            thinhGiang.NhanVien.HoSo.NoiOHienNay = noHN.Oid;
                                        }
                                        catch (Exception ex) { throw ex; }
                                    }
                                    //////////////////End DCTT///////////////////////////// 
                                    #endregion

                                    #endregion
                                }

                                #endregion
                            }
                            else if (type == "TrinhDoChuyenMon")
                            {
                                #region 2. Trình độ chuyên môn
                                bool daTaoNVTD = false;
                                NhanVienTrinhDo_Factory factory_NVTD = new NhanVienTrinhDo_Factory();
                                NhanVienTrinhDo nvtd = factory_NVTD.GetNhanVienTrinhDoByOid(obj.Oid_NVTD != null ? obj.Oid_NVTD.Value : Guid.Empty);
                                //
                                if (obj.TrinhDoVanHoa != null && obj.TrinhDoVanHoa != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    TrinhDoVanHoa tdvh = factory.GetTrinhDoVanHoaByOid(obj.TrinhDoVanHoa.Value);
                                    if (tdvh != null)
                                        nvtd.TrinhDoVanHoa = tdvh.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TrinhDoVanHoa = null;
                                }
                                if (obj.TrinhDoTinHoc != null && obj.TrinhDoTinHoc != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    TrinhDoTinHoc tdth = factory.GetTrinhDoTinHocByOid(obj.TrinhDoTinHoc.Value);
                                    if (tdth != null)
                                        nvtd.TrinhDoTinHoc = tdth.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TrinhDoTinHoc = null;
                                }
                                if (obj.HocHam != null && obj.HocHam != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    HocHam hh = factory.GetHocHamByOid(obj.HocHam.Value);
                                    if (hh != null)
                                        nvtd.HocHam = hh.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.HocHam = null;
                                }
                                if (obj.TrinhDoChuyenMon != null && obj.TrinhDoChuyenMon != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    TrinhDoChuyenMon tdcm = factory.GetTrinhDoChuyenMonByOid(obj.TrinhDoChuyenMon.Value);
                                    if (tdcm != null)
                                        nvtd.TrinhDoChuyenMon = tdcm.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TrinhDoChuyenMon = null;
                                }
                                if (obj.ChuyenNganhDaoTao != null && obj.ChuyenNganhDaoTao != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    ChuyenMonDaoTao cndt = factory.GetChuyenMonDaoTaoByOid(obj.ChuyenNganhDaoTao.Value);
                                    if (cndt != null)
                                        nvtd.ChuyenMonDaoTao = cndt.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TruongDaoTao = null;
                                }
                                if (obj.TruongDaoTao != null && obj.TruongDaoTao != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    TruongDaoTao tdt = factory.GetTruongDaoTaoByOid(obj.TruongDaoTao.Value);
                                    if (tdt != null)
                                        nvtd.TruongDaoTao = tdt.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TruongDaoTao = null;
                                }
                                if (obj.HinhThucDaoTao != null && obj.HinhThucDaoTao != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    HinhThucDaoTao htdt = factory.GetHinhThucDaoTaoByOid(obj.HinhThucDaoTao.Value);
                                    if (htdt != null)
                                        nvtd.HinhThucDaoTao = htdt.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.HinhThucDaoTao = null;
                                }
                                if (!string.IsNullOrEmpty(obj.NamTotNghiep.ToString()))
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    nvtd.NamTotNghiep = obj.NamTotNghiep;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.NamTotNghiep = 0;
                                }
                                if (obj.NgoaiNgu != null && obj.NgoaiNgu != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    NgoaiNgu nn = factory.GetNgoaiNguByOid(obj.NgoaiNgu.Value);
                                    if (nn != null)
                                        nvtd.NgoaiNgu = nn.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.NgoaiNgu = null;
                                }
                                if (obj.TrinhDoNgoaiNgu != null && obj.TrinhDoNgoaiNgu != Guid.Empty)
                                {
                                    if (nvtd == null)
                                    {
                                        nvtd = factory_NVTD.CreateManagedObject();
                                        nvtd.Oid = Guid.NewGuid();
                                    }
                                    //
                                    TrinhDoNgoaiNgu tdnn = factory.GetTrinhDoNgoaiNguByOid(obj.TrinhDoNgoaiNgu.Value);
                                    if (tdnn != null)
                                        nvtd.TrinhDoNgoaiNgu = tdnn.Oid;
                                    //
                                    daTaoNVTD = true;
                                }
                                else
                                {
                                    nvtd.TrinhDoNgoaiNgu = null;
                                }
                                //
                                if (daTaoNVTD)
                                {
                                    factory_NVTD.SaveChanges();
                                    //
                                    thinhGiang.NhanVien.NhanVienTrinhDo = nvtd.Oid;
                                }
                                #endregion
                            }
                            else
                            {
                                #region 3. Thông tin lương
                                bool daTaoNVTTL = false;
                                NhanVienThongTinLuong_Factory factory_NVTTL = new NhanVienThongTinLuong_Factory();
                                NhanVienThongTinLuong nvttl = factory_NVTTL.GetNhanVienThongTinLuongByOid(obj.Oid_NVTTL != null ? obj.Oid_NVTTL.Value : Guid.Empty);
                                // 
                                if (nvttl == null)
                                {
                                    nvttl = factory_NVTTL.CreateManagedObject();
                                    nvttl.Oid = Guid.NewGuid();
                                }
                                if (!string.IsNullOrEmpty(obj.MaSoThue))
                                {
                                    nvttl.MaSoThue = obj.MaSoThue;
                                    daTaoNVTTL = true;
                                }
                                else
                                {
                                    nvttl.MaSoThue = string.Empty;
                                }
                                if (obj.CoQuanThue != null && obj.CoQuanThue != Guid.Empty)
                                {
                                    CoQuanThue cqt = factory.GetCoQuanThueByOid(obj.CoQuanThue != null ? obj.CoQuanThue.Value : Guid.Empty);
                                    if (cqt != null)
                                        nvttl.CoQuanThue = cqt.Oid;
                                    daTaoNVTTL = true;
                                }
                                else
                                {
                                    nvttl.CoQuanThue = null;
                                }
                                if (daTaoNVTTL)
                                {
                                    factory_NVTTL.SaveChanges();
                                    //
                                    thinhGiang.NhanVien.NhanVienThongTinLuong = nvttl.Oid;
                                }
                                #endregion
                                
                                #region 4. Tài khoản ngân hàng
                                bool daTaoTKNH = false;
                                TaiKhoanNganHang_Factory factory_TKNH = new TaiKhoanNganHang_Factory();
                                TaiKhoanNganHang tknh = factory_TKNH.GetTaiKhoanNganHangByOid(obj.Oid_TKNH != null ? obj.Oid_TKNH.Value : Guid.Empty);
                                //
                                if (tknh == null)
                                {
                                    tknh = factory_TKNH.CreateManagedObject();
                                    tknh.Oid = Guid.NewGuid();
                                    tknh.NhanVien = obj.Oid;
                                }
                                if (!string.IsNullOrEmpty(obj.SoTaiKhoan))
                                {
                                    tknh.SoTaiKhoan = obj.SoTaiKhoan;
                                    daTaoTKNH = true;
                                }
                                else
                                {
                                    tknh.SoTaiKhoan = string.Empty;
                                }
                                if (obj.NganHang != null && obj.NganHang != Guid.Empty)
                                {
                                    NganHang nh = factory.GetNganHangByOid(obj.NganHang.Value);
                                    if (nh != null)
                                        tknh.NganHang = nh.Oid;
                                    //
                                    daTaoTKNH = true;
                                }
                                else
                                {
                                    tknh.NganHang = null;
                                }
                                if (daTaoTKNH)
                                {
                                    factory_TKNH.SaveChanges();
                                    //
                                }
                                #endregion
                            }
                            //
                            factory.SaveChangesWithoutTrackingLog();
                            transaction.Complete();
                            //
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            result = false;
                        }
                    }
                }
            }
            //
            return result;
        }
    }
}
