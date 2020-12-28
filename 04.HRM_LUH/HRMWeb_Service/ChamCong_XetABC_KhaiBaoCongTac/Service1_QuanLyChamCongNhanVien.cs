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
        //private static void GetSetChild(QuanLyChamCongNhanVien obj)
        //{
        //    //lay chi tiet cham cong nhan vien

        //    List<DTO_ChiTietChamCongNhanVien> tmpList = new List<DTO_ChiTietChamCongNhanVien>();
        //    foreach (var chiTiet in obj.ChiTietChamCongNhanViens)
        //    {
                
        //        tmpList.Add(chiTiet.Map<DTO_ChiTietChamCongNhanVien>());
        //    }
        //    obj.DanhSachDTO_ChiTietChamCongNhanVien = tmpList;

        //}

        //public IEnumerable<DTO_QuanLyChamCongNhanVien> GetList_QuanLyChamCongNhanVien(String publicKey, String token)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        var factory = QuanLyChamCongNhanVien_Factory.New();
        //        //IEnumerable<DTO_QuanLyChamCongNhanVien> list = factory.GetAll_GCRecordIsNull().Map<DTO_QuanLyChamCongNhanVien>().ToList();
        //        //return list;

        //        var tmpList = factory.GetAll_GCRecordIsNull();
        //        foreach (QuanLyChamCongNhanVien obj in tmpList)
        //        {
        //            GetSetChild(obj);
        //        }
        //        IEnumerable<DTO_QuanLyChamCongNhanVien> list = tmpList.Map<DTO_QuanLyChamCongNhanVien>();

        //        return list;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public String GetList_QuanLyChamCongNhanVien_Json(String publicKey, String token)
        //{
        //    IEnumerable<DTO_QuanLyChamCongNhanVien> list = GetList_QuanLyChamCongNhanVien(publicKey, token);
        //    String json = JsonConvert.SerializeObject(list);
        //    return json;
        //}


        // ////////////////////////////////////////////
        //public DTO_QuanLyChamCongNhanVien Get_QuanLyChamCongNhanVienBy_Id(String publicKey, String token, Guid id)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        var factory = QuanLyChamCongNhanVien_Factory.New();
        //        var tmpObj = factory.GetById(id);
        //        GetSetChild(tmpObj);
        //        DTO_QuanLyChamCongNhanVien obj = tmpObj.Map<DTO_QuanLyChamCongNhanVien>();
                
        //        return obj;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public String Get_QuanLyChamCongNhanVienBy_Id_Json(String publicKey, String token, Guid id)
        //{
        //    DTO_QuanLyChamCongNhanVien obj = Get_QuanLyChamCongNhanVienBy_Id(publicKey, token, id);
        //    String json = JsonConvert.SerializeObject(obj);
        //    return json;
        //}

        // Save////////////////////////////////////////////
        //public bool Save_QuanLyChamCongNhanVien(String publicKey, String token, DTO_QuanLyChamCongNhanVien obj)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        if (obj != null)
        //        {
        //            QuanLyChamCongNhanVien_Factory factory = QuanLyChamCongNhanVien_Factory.New();
        //            QuanLyChamCongNhanVien objFromDB = factory.GetById(obj.Oid);
        //            QuanLyChamCongNhanVien objForSave = null;
        //            if (objFromDB == null)
        //            {
        //                //them moi
        //                //map sang entity
        //                var newDBObject = factory.CreateManagedObject();
        //                newDBObject.CopyPropertiesFrom(obj);
        //                //phát sinh mã mới nếu chưa có
        //                if (newDBObject.Oid == Guid.Empty)
        //                    newDBObject.Oid = Guid.NewGuid();
        //                objForSave = newDBObject;
        //            }
        //            else
        //            {
        //                //cap nhat
        //                objFromDB.CopyPropertiesFrom(obj);
        //                objForSave = objFromDB;
        //                //xoa nhung ChiTietChamCongNhanVien bi loai bo
        //                {
        //                    List<ChiTietChamCongNhanVien> danhSachChiTietChamCongNhanVien_CanGoBoKhoi = new List<ChiTietChamCongNhanVien>();
        //                    foreach (var chiTiet in objForSave.ChiTietChamCongNhanViens)
        //                    {
        //                        if (obj.DanhSachDTO_ChiTietChamCongNhanVien.All(x => x.Oid != chiTiet.Oid))
        //                        {
        //                            //them vao danh sach cho xoa
        //                            danhSachChiTietChamCongNhanVien_CanGoBoKhoi.Add(chiTiet);
        //                        }
        //                    }
        //                    ChiTietChamCongNhanVien_Factory.FullDelete(factory.Context,
        //                        danhSachChiTietChamCongNhanVien_CanGoBoKhoi.ToArray<Object>());
        //                }
        //            }
        //            //cap nhat cac doi tuong long ben trong
        //            {
        //                //them ChiTietChamCongNhanVien
        //                foreach (var dtoChiTiet in obj.DanhSachDTO_ChiTietChamCongNhanVien)
        //                {
        //                    //phát sinh mã mới nếu chưa có
        //                    if (dtoChiTiet.Oid == Guid.Empty)
        //                    {
        //                        dtoChiTiet.Oid = Guid.NewGuid();
        //                    }
        //                    //kiem tra da ton tai chua
        //                    //neu ko ton tai thi them vao
        //                    if (objForSave.ChiTietChamCongNhanViens.All(x => x.Oid != dtoChiTiet.Oid))
        //                    {
        //                        var chiTiet = new ChiTietChamCongNhanVien();
        //                        chiTiet.CopyPropertiesFrom(dtoChiTiet);
        //                        objForSave.ChiTietChamCongNhanViens.Add(chiTiet);

        //                    }
        //                }

        //            }
        //            ///luu lai///////////
        //            try
        //            {
        //                factory.SaveChanges();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }
        //        return false;
        //    }
        //    return false;
        //    //
        //}
        //public bool Save_QuanLyChamCongNhanVien_Json(String publicKey, String token, string jsonObject)
        //{
        //    //chuyen jsonObject thanh object
        //    DTO_QuanLyChamCongNhanVien obj = JsonConvert.DeserializeObject<DTO_QuanLyChamCongNhanVien>(jsonObject);
        //    return Save_QuanLyChamCongNhanVien(publicKey, token, obj);
        //}

        // XOA////////////////////////////////////////////
        //public bool Delete_QuanLyChamCongNhanVienBy_Id(String publicKey, String token, Guid id)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        var factory = QuanLyChamCongNhanVien_Factory.New();
        //        QuanLyChamCongNhanVien obj = factory.GetById(id);
        //        if (obj != null)
        //        {
        //            try
        //            {
        //                QuanLyChamCongNhanVien_Factory.FullDelete(factory.Context, obj);
        //                factory.SaveChanges();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}

        //public bool DeleteList_QuanLyChamCongNhanVienBy_IdList(String publicKey, String token, List<Guid> idList)
        //{
        //    if (Helper.TrustTest(publicKey, token))
        //    {
        //        QuanLyChamCongNhanVien_Factory factory = QuanLyChamCongNhanVien_Factory.New();
        //        Object[] objList = factory.GetListByIdList(idList).ToArray<Object>();
        //        if (objList != null)
        //        {
        //            try
        //            {
        //                QuanLyChamCongNhanVien_Factory.FullDelete(factory.Context, objList);
        //                factory.SaveChanges();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
    }
}
