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
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        //tao danh sach theo thang cho tat ca nhan vien lam hanh chinh
        public bool DoDuLieuChamCongThang(String publicKey, String token, int thang, int nam, Guid idHinhThucNghi, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChamCongTheoNgay_Factory.New();
                var factoryKy = CC_KyChamCong_Factory.New();
                CC_KyChamCong kyCC = factoryKy.GetByMonthAndYear(thang, nam);
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                if (daTonTai)
                {
                    throw new Exception("Không thể lập danh sách vì đã tồn tại trước đó");
                }
                else
                {
                    //try
                    //{
                        Boolean daChamCong = (idHinhThucNghi != HinhThucNghiConst.KhongXacDinhId && idHinhThucNghi != HinhThucNghiConst.NghiKhongPhepRoId);

                    factory.Context.spd_WebChamCong_TaoDanhSach_CC_ChamCongTheoNgay_ChoCaThang(thang, nam,
                        idHinhThucNghi, webUserId, daChamCong);
                    // Cập nhật trạng thái nghỉ cho nhân viên
                    List<CC_ChamCongNgayNghi> listCC = factory.DSNhanVienCoTrongCCNgayNghiTheoThangNam(thang, nam).ToList();
                    foreach (CC_ChamCongNgayNghi cc in listCC)
                    {
                        DateTime tuNgay = cc.TuNgay.Value;
                        DateTime denNgay = cc.DenNgay.Value;
                        if (tuNgay <= kyCC.TuNgay && denNgay >= kyCC.TuNgay)
                        {
                            tuNgay = kyCC.TuNgay;
                        }
                        else if (tuNgay <= kyCC.TuNgay && denNgay >= kyCC.DenNgay)
                        {
                            tuNgay = kyCC.TuNgay;
                            denNgay = kyCC.DenNgay;
                        }
                        else if (tuNgay >= kyCC.TuNgay && denNgay >= kyCC.DenNgay)
                        {
                            denNgay = kyCC.DenNgay;
                        }
                        DateTime day = tuNgay;
                        while (day <= denNgay)
                        {
                            CC_ChamCongTheoNgay cctn = factory.CreateAloneObject();
                            cctn = factory.GetByDate(day, cc.IDNhanVien);
                            if (cctn != null)
                            {
                                int thu = (int)day.DayOfWeek;
                                switch (thu)
                                {
                                    //Chủ nhật
                                    case 0:
                                        {
                                            cctn.IDHinhThucNghi = HinhThucNghiConst.KhongXacDinhId;
                                        }
                                        break;
                                    ////Thứ 7
                                    //case 6:
                                    //    {
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.DiCongTacId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.DiCTNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiKhongPhepRoId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiRoNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiLeId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiLeNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiHocTapId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiHocNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiHuongLuongId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiHLNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiThaiSanId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiTSNuaNgayId;
                                    //        }
                                    //        if (cc.IDHinhThucNghi == HinhThucNghiConst.NghiPhepId)
                                    //        {
                                    //            cctn.IDHinhThucNghi = HinhThucNghiConst.NghiNuaNgayId;
                                    //        }
                                    //    }
                                    //    break;
                                    default:
                                        {
                                            cctn.IDHinhThucNghi = cc.IDHinhThucNghi;
                                        }
                                        break;
                                }
                                factory.SaveChanges();
                            }
                            day = day.AddDays(1);
                        }

                    }
                    // Cập nhật trạng thái đi công tác cho nhân viên
                    List<CC_KhaiBaoCongTac> listCT = factory.DSNhanVienKhaiBaoCongTacTheoThangNam(thang, nam).ToList();
                    foreach (CC_KhaiBaoCongTac cc in listCT)
                    {
                        DateTime tuNgay = cc.TuNgay.Value;
                        DateTime denNgay = cc.DenNgay.Value;
                        if (tuNgay <= kyCC.TuNgay && denNgay >= kyCC.TuNgay)
                        {
                            tuNgay = kyCC.TuNgay;
                        }
                        else if (tuNgay <= kyCC.TuNgay && denNgay >= kyCC.DenNgay)
                        {
                            tuNgay = kyCC.TuNgay;
                            denNgay = kyCC.DenNgay;
                        }
                        else if (tuNgay >= kyCC.TuNgay && denNgay >= kyCC.DenNgay)
                        {
                            denNgay = kyCC.DenNgay;
                        }
                        DateTime day = tuNgay;
                        while (day <= denNgay)
                        {
                            CC_ChamCongTheoNgay cctn = factory.CreateAloneObject();
                            cctn = factory.GetByDate(day, cc.IDNhanVien);
                            if (cctn != null)
                            {
                                int thu = (int)day.DayOfWeek;
                                switch (thu)
                                {
                                    //Chủ nhật
                                    case 0:
                                        {
                                            cctn.IDHinhThucNghi = HinhThucNghiConst.KhongXacDinhId;
                                        }
                                        break;
                                    //Thứ 7
                                    //case 6:
                                    //    {
                                    //        cctn.IDHinhThucNghi = HinhThucNghiConst.DiCTNuaNgayId;
                                    //    }
                                    //    break;
                                    default:
                                        {
                                            cctn.IDHinhThucNghi = HinhThucNghiConst.DiCongTacId;
                                        }
                                        break;
                                }
                                factory.SaveChanges();
                            }
                            day = day.AddDays(1);
                        }

                    }
                    return true;
                //}
                //    catch (Exception ex)
                //{
                //    throw new Exception("Xác thực không hợp lệ.");
                //}
            }

                return false;
            }
            else
            {
                return false;
            }
        }
        public bool DoDuLieuChamCongThang_BoSung(String publicKey, String token, int thang, int nam, Guid idHinhThucNghi, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<HoSo> dsNhanVienChuaCoCC = factory.List_NVMoi_ChuaCo_CCTheoNgay(thang, nam);
                foreach (HoSo hs in dsNhanVienChuaCoCC.ToList())
                {
                    try
                    {
                        //Boolean daChamCong = (idHinhThucNghi != HinhThucNghiConst.KhongXacDinhId && idHinhThucNghi != HinhThucNghiConst.NghiKhongPhepRoId);

                        //Sửa lại daChamCong = false --ngày 18/07/2016
                        Boolean daChamCong = false;

                        factory.Context.spd_WebChamCong_BoSung_CC_ChamCongTheoNgay_ChoCaThang(thang, nam,
                            idHinhThucNghi, webUserId, daChamCong, hs.Oid);
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("Xác thực không hợp lệ.");
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool DoDuLieuChamCongThang_CheckExists(String publicKey, String token, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChamCongTheoNgay_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                return daTonTai;

            }
            else
            {
                throw new Exception("Xác thực không hợp lệ.");
            }
        }


    }
}
