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
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public String QuanLyNhacViec_Find_Json(String publicKey, String token, Guid id, Guid congTy)
        {
            DTO_QuanLyNhacViec obj = QuanLyNhacViec_Find(publicKey, token, id,congTy);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_QuanLyNhacViec QuanLyNhacViec_Find(String publicKey, String token, Guid userID, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                DTO_QuanLyNhacViec obj = new DTO_QuanLyNhacViec();

                //Quản lý công tác
                CC_KhaiBaoCongTac_Factory facKBCongTac = new CC_KhaiBaoCongTac_Factory();
                List<CC_KhaiBaoCongTac> congTacList = facKBCongTac.KhaiBaoCongTacChuaDuyet_ByUser(userID, congTy).ToList();
                //
                if(congTacList != null)
                   obj.SoLuongCongTac = congTacList.Count;

                //Quản lý ngày nghỉ
                CC_ChamCongNgayNghi_Factory facCCNgayNghi = new CC_ChamCongNgayNghi_Factory();
                List<CC_ChamCongNgayNghi> ngayNghiList = facCCNgayNghi.ChamCongNgayNghiChuaDuyet_ByUser(userID, congTy).ToList();
                //
                if(ngayNghiList != null)
                obj.SoLuongDonNghi = ngayNghiList.Count;

                //Quản lý ngoài giờ
                CC_ChamCongNgoaiGio_Factory facCCNgoaiGio = new CC_ChamCongNgoaiGio_Factory();
                List<CC_ChamCongNgoaiGio> ngoaiGioList = facCCNgoaiGio.ChamCongNgoaiGioChuaDuyet_ByUser(userID, congTy).ToList();
                //
                if (ngoaiGioList != null)
                    obj.SoLuongNgoaiGio = ngoaiGioList.Count;
                //
                return obj;
            }
            else
            {
                return null;
            }
        }
    }
}
