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

        public bool ChamCongNhanh(String publicKey, String token, int ngay, int thang, int nam, Guid? idHinhThucNghi, Guid? idBoPhan, Guid? idLoaiNhanSu, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_ChamCongTheoNgay_Factory.New();
                //
                try
                {
                    Boolean daChamCong = true;
                    idHinhThucNghi = idHinhThucNghi == Guid.Empty ? null : idHinhThucNghi;
                    idLoaiNhanSu = idLoaiNhanSu == Guid.Empty ? null : idLoaiNhanSu;
                    idBoPhan = idBoPhan == Guid.Empty ? null : idBoPhan;
                    //
                    factory.Context.spd_WebChamCong_ChamCongHangLoatDonVi(ngay, thang, nam, idHinhThucNghi, daChamCong, idBoPhan, idLoaiNhanSu, webUserId);
                    //
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }

            return false;
        }
        public bool CapNhatChamCongDonVi(String publicKey, String token, int thang, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_ChamCongTheoNgay_Factory.New();
                try
                {
                    factory.Context.spd_WebChamCong_CapNhatBoPhanChamCongTheoNgay(thang, nam);

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //
            return false;
        }


    }
}