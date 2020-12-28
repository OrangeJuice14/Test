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


        public IEnumerable<DTO_CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(String publicKey, String token, int thang, int nam, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                var tmpList = factory.CaNhanKhaiBaoCongTac_Find(thang, nam, webUserId).ToList();
                IEnumerable<DTO_CC_KhaiBaoCongTac> objList = tmpList.Map<DTO_CC_KhaiBaoCongTac>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String CaNhanKhaiBaoCongTac_Find_Json(String publicKey, String token, int thang, int nam, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_CC_KhaiBaoCongTac> list = CaNhanKhaiBaoCongTac_Find(publicKey, token, thang, nam, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public bool CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, Guid webUserId)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory webUser_Factory = WebUser_Factory.New();
                WebUser webUser = webUser_Factory.GetByID(webUserId);

                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                bool isNew = true;
                if (khaiBaoCongTacOid != null)
                {
                    var objFromDb = factory.GetByID(khaiBaoCongTacOid.Value);
                    if (objFromDb != null)
                        isNew = false;
                }
                //bat dau kiem tra hop le
                var trungHoacGiaoNgayKhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                         where o.IDNhanVien == webUser.ThongTinNhanVien
                                            && (isNew || o.Oid != khaiBaoCongTacOid)
                                             && (
                                                (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                ||
                                                (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                             )

                                         select true).FirstOrDefault();

                var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                       where o.IDNhanVien == webUser.ThongTinNhanVien
                                                           && (
                                                              (o.TuNgay >= tuNgay && tuNgay <= o.DenNgay)
                                                              ||
                                                              (o.TuNgay >= denNgay && denNgay <= o.DenNgay)
                                                           )
                                                       select true).FirstOrDefault();

                var hopLe = (!trungHoacGiaoNgayKhaiBaoCongTac && !trungHoacGiaoNgay_ChamCongNgayNghi);
                return hopLe;

            }
            return false;
        }


        public DTO_CC_KhaiBaoCongTac CaNhanKhaiBaoCongTac_KhaiBaoMoi(String publicKey, String token, String noiDung, DateTime tuNgay, DateTime denNgay, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                WebUser_Factory webUser_Factory = WebUser_Factory.New();
                WebUser webUser = webUser_Factory.GetByID(webUserId);


                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                CC_KhaiBaoCongTac newObj = factory.CreateManagedObject();
                newObj.Oid = Guid.NewGuid();
                newObj.TuNgay = tuNgay.Date;
                newObj.DenNgay = denNgay.Date;
                newObj.NoiDung = noiDung.Trim();
                newObj.IDNhanVien = webUser.ThongTinNhanVien.Value;
                newObj.IDWebUser = webUserId;
                newObj.NgayTao = DateTime.Today;
                newObj.TrangThai = -1;//trạng thái chờ

                factory.SaveChanges();

                return newObj.Map<DTO_CC_KhaiBaoCongTac>();
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public bool CaNhanKhaiBaoCongTac_DeleteList(String publicKey, String token, List<DTO_CC_KhaiBaoCongTac> objList)
        {//ca nhan chi duoc phep xoa nhung dong dang cho xet
            CC_KhaiBaoCongTac_Factory tmpFactory = CC_KhaiBaoCongTac_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                TimeSpan.FromSeconds(360)))
            {
                bool daXoaDuocItNhat1Dong = false;
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    if (objFromDB.TrangThai == -1)
                    {
                        bool xoaDuocDongHienTai = QuanLyKhaiBaoCongTac_DeleteList(publicKey, token,
                              new List<DTO_QuanLyKhaiBaoCongTac_Find>() { obj.Map<DTO_QuanLyKhaiBaoCongTac_Find>() });
                        if (daXoaDuocItNhat1Dong == false && xoaDuocDongHienTai == true)
                        {
                            daXoaDuocItNhat1Dong = true;
                        }
                    }
                }
                transaction.Complete();
                return daXoaDuocItNhat1Dong;
                return true;
            }
            return false;
        }

        public bool CaNhanKhaiBaoCongTac_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_KhaiBaoCongTac> objList = JsonConvert.DeserializeObject<List<DTO_CC_KhaiBaoCongTac>>(jsonObjectList);
            return CaNhanKhaiBaoCongTac_DeleteList(publicKey, token, objList);
        }


    }
}
