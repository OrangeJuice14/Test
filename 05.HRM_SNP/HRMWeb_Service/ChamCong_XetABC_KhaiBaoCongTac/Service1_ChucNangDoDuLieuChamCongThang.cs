﻿using System;
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
                        return true;
                    //}
                    //catch (Exception ex)
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