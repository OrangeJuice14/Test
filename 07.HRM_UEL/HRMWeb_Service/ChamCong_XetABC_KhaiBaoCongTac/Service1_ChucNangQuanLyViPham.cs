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

        public IEnumerable<DTO_QuanLyViPham_Find> QuanLyViPham_Find(String publicKey, String token, int ngay, int thang, int nam, Guid boPhanId, Guid webUserId, Guid? nhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyViPham_Factory factory = new CC_QuanLyViPham_Factory();
                IEnumerable<DTO_QuanLyViPham_Find> list = null;
                list = factory.FindForQuanLyViPham(ngay, thang, nam, boPhanId, webUserId, nhanVienId);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyViPham_Find_Json(String publicKey, String token, int ngay, int thang, int nam, Guid boPhanId, Guid webUserId, Guid? nhanVienId)
        {//DANG SD
            IEnumerable<DTO_QuanLyViPham_Find> list = QuanLyViPham_Find(publicKey, token, ngay, thang, nam, boPhanId, webUserId, nhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyViPham_ThongKe_Find(String publicKey, String token, DateTime? tuNgay, DateTime? denNgay, Guid boPhanId, Guid webUserId, Guid? nhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyViPham_Factory factory = new CC_QuanLyViPham_Factory();
                IEnumerable<DTO_QuanLyViPham_Find> list = null;
                list = factory.FindForQuanLyViPham_ThongKe(tuNgay, denNgay, boPhanId, webUserId, nhanVienId);
                return JsonConvert.SerializeObject(list);
            }
            else
            {
                return null;
            }
        }
        public bool QuanLyViPham_ThayDoiTrangThaiList(String publicKey, String token, List<DTO_QuanLyViPham_Find> list, Guid webUserId, int trangThai)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    var webgroup = WebUser_Factory.New().GetByID(webUserId)?.WebGroupID;
                    CC_QuanLyViPham_Factory factory = CC_QuanLyViPham_Factory.New();
                    foreach (var item in list)
                    {
                        var objFromDB = factory.GetById(item.Oid);
                        if (objFromDB != null)
                        {
                            if (webgroup == new Guid("00000000-0000-0000-0000-000000000004") || webgroup == new Guid("00000000-0000-0000-0000-000000000005"))
                            { //lãnh đạo đơn vị
                                objFromDB.TrangThai_TP = trangThai;
                            }
                            else
                            {
                                objFromDB.TrangThai_HD = trangThai;
                            }
                        }
                    }
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_ChucNangQuanLyViPham/QuanLyViPham_ThayDoiTrangThaiList", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyViPham_UpdateGiaiTrinh(String publicKey, String token, Guid quanLyViPhamOid, string giaiTrinh)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    CC_QuanLyViPham_Factory factory = CC_QuanLyViPham_Factory.New();
                    var objFromDB = factory.GetById(quanLyViPhamOid);
                    if (objFromDB != null)
                    {
                        objFromDB.GiaiTrinh = giaiTrinh;
                    }
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_ChucNangQuanLyViPham/QuanLyViPham_UpdateGiaiTrinh", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyViPham_SaveFile(String publicKey, String token, string filePath, Guid fileId, string fileName, string fileExtension, int fileSize, Guid quanLyViPhamOid, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //var currentUserId = WebUser_Factory.New().get
                CC_QuanLyViPham_Factory factory = CC_QuanLyViPham_Factory.New();
                CC_QuanLyViPham objFromDB = factory.GetById(quanLyViPhamOid);
                if (objFromDB != null)
                {
                    try
                    {
                        objFromDB.CC_QuanLyViPham_File.Add(new CC_QuanLyViPham_File
                        {
                            Oid = fileId,
                            CC_QuanLyViPham = quanLyViPhamOid,
                            FilePath = filePath,
                            FileName = fileName,
                            FileExtension = fileExtension,
                            FileSize = fileSize,
                            DateCreated = DateTime.Now,
                            Uploader = webUserId
                        });

                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HRMWebApp.Helpers.Helper.ErrorLog("Service1_ChucNangQuanLyViPham/QuanLyViPham_SaveFile", ex);
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyViPham_RemoveFile(String publicKey, String token, Guid quanLyViPhamFileOid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyViPham_Factory factory = CC_QuanLyViPham_Factory.New();
                CC_QuanLyViPham_File objFromDB = factory.GetFileById(quanLyViPhamFileOid);
                if (objFromDB != null)
                {
                    try
                    {
                        objFromDB.GCRecord = 1;

                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HRMWebApp.Helpers.Helper.ErrorLog("Service1_ChucNangQuanLyViPham/QuanLyViPham_RemoveFile", ex);
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
