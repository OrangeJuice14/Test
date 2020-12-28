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

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        //lấy danh sách bộ phận

        public IEnumerable<DTO_BoPhan> GetList_BoPhan(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<Department> list = factory.GetAll_GCRecordIsNull();
                List<DTO_BoPhan> listDTO = new List<DTO_BoPhan>();
                foreach (Department bp in list)
                {
                    DTO_BoPhan bpDTO = new DTO_BoPhan();
                    bpDTO.Oid = bp.Oid;
                    bpDTO.BoPhanCha = bp.ParentDepartment;
                    bpDTO.LoaiBoPhan = bp.LoaiBoPhan;
                    bpDTO.MaQuanLy = bp.MaQuanLy;
                    bpDTO.STT = Convert.ToInt32(bp.STT);
                    bpDTO.TenBoPhan = bp.TenBoPhan;
                    bpDTO.TenVietTatThongTinTruong = bp.CompanyInfo1!=null? bp.CompanyInfo1.TenVietTat :"";
                    listDTO.Add(bpDTO);
                }
                return listDTO;
            }
            else
            {
                return null;
            }
        }
        public String GetList_BoPhan_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetList_BoPhan(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        public DTO_BoPhan Get_BoPhanBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                DTO_BoPhan obj = factory.GetByID(id).Map<DTO_BoPhan>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String Get_BoPhanBy_Id_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_BoPhan obj = Get_BoPhanBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        // ////////////////////////////////////////////
        public IEnumerable<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserId(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<Department> list = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId);
                List<DTO_BoPhan> listDTO = new List<DTO_BoPhan>();
                foreach (Department bp in list)
                {
                    DTO_BoPhan bpDTO = new DTO_BoPhan();
                    bpDTO.Oid = bp.Oid;
                    bpDTO.BoPhanCha = bp.ParentDepartment;
                    bpDTO.LoaiBoPhan = bp.LoaiBoPhan;
                    bpDTO.MaQuanLy = bp.MaQuanLy;
                    bpDTO.STT = Convert.ToInt32(bp.STT);
                    bpDTO.TenBoPhan = bp.TenBoPhan;
                    bpDTO.TenVietTatThongTinTruong = "";
                    if (bp.CompanyInfo1 != null && bp.CompanyInfo1.TenVietTat != null)
                    {
                        bpDTO.TenVietTatThongTinTruong = bp.CompanyInfo1.TenVietTat + " - ";
                    }
                    listDTO.Add(bpDTO);
                }
                return listDTO;
            }
            else
            {
                return null;
            }
        }

        public String GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(String publicKey, String token, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetList_BoPhan_DuocPhanQuyenChoWebUserId(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
