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
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public bool QuanLyNghiPhep_CheckExists(String publicKey, String token, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyNghiPhep_Factory.New();
                bool daTonTai = factory.CheckExistChiTietNghiPhep(nam);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhep_Find(String publicKey, String token, int nam, Guid? bophan, string maNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyNghiPhep_Factory factory = new CC_QuanLyNghiPhep_Factory();
                IEnumerable<DTO_QuanLyNghiPhep_Find> list = null;
                list = factory.QuanLyNghiPhep_Find(nam, bophan, maNhanSu);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyNghiPhep_Find_Json(String publicKey, String token, int nam, Guid? bophan, string maNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyNghiPhep_Find> list = QuanLyNghiPhep_Find(publicKey, token, nam, bophan, maNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public decimal QuanLyNghiPhep_NgayPhepConLai(String publicKey, String token, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyNghiPhep_Factory factory = new CC_QuanLyNghiPhep_Factory();
                decimal result = factory.QuanLyNghiPhep_NgayPhepConLai(nam, idNhanVien);
                return result;
            }
            else
            {
                return 0;
            }
        }
        public String QuanLyNghiPhep_NgayPhepConLai_Json(String publicKey, String token, int nam, Guid idNhanVien)
        {//DANG SD
            decimal soNgayPhepConLai = QuanLyNghiPhep_NgayPhepConLai(publicKey, token, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(soNgayPhepConLai);
            return json;
        }
        public bool QuanLyNghiPhep_Create(String publicKey, String token, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyNghiPhep_Factory.New();
                bool daTonTaiBangCon = false;
                if (daTonTaiBangCon)
                {
                    throw new Exception(string.Format("Không thể chốt vì đã tồn tại bảng chốt năm {1} của bộ phận", nam));
                }
                else
                {
                    //try
                    //{
                    //lay quan ly cu neu co
                    QuanLyNghiPhep quanLy = factory.GetByNam(nam);
                    if (quanLy == null)
                    {
                        //tao quan ly moi
                        quanLy = factory.CreateManagedObject();
                        quanLy.Oid = Guid.NewGuid();
                        quanLy.Nam = nam;
                    }
                    factory.SaveChanges();
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                TimeSpan.FromSeconds(180)))
                    {
                        factory.Context.spd_WebChamCong_QuanLyNghiPhep_TaoMoiChiTietNghiPhep(nam);
                        transaction.Complete();
                    }
                    return true;


                    //}
                    //catch (Exception ex)
                    //{
                    //    return false;
                    //}
                }

            }
            else
            {
                return false;
            }
        }
        public DTO_QuanLyNghiPhep_Find GetThongTinNghiPhepDTO(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyNghiPhep_Factory factory = new CC_QuanLyNghiPhep_Factory();
                DTO_QuanLyNghiPhep_Find result = null;
                result = factory.GetThongTinNghiPhepDTO(oid);
                return result;
            }
            else
            {
                return null;
            }
        }
        public String GetThongTinNghiPhepDTO_Json(String publicKey, String token, Guid oid)
        {//DANG SD
            DTO_QuanLyNghiPhep_Find obj = GetThongTinNghiPhepDTO(publicKey, token, oid);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public bool ThongTinNghiPhep_Save(String publicKey, String token, Guid Oid, decimal tongSoNgayPhep, decimal soNgayPhepCongThem, decimal daNghi, decimal conLai, string ghiChu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_QuanLyNghiPhep_Factory.New();
                factory.ThongTinNghiPhep_Save(Oid, tongSoNgayPhep, soNgayPhepCongThem, daNghi, conLai, ghiChu);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
