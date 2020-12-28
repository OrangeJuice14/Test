using System;
using System.Collections.Generic;
//using System.Data.Common.CommandTrees;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection;
using System.ComponentModel;
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_EmailManager_Factory : BaseFactory<Entities, CC_MailManager>
    {
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_CaChamCong_Factory.New().CreateAloneObject();
        }
        public static CC_EmailManager_Factory New()
        {
            return new CC_EmailManager_Factory();
        }
        public CC_EmailManager_Factory()
            : base(Database.NewEntities())
        {

        }

        public IQueryable<DTO_QuanLyGuiMail> GetDTOQuanLyGuiMail_ByTuNgayDenNgay(DateTime tungay,DateTime denngay,Guid idWebUser)
        {
            //
            WebUser user = (new WebUser_Factory()).GetByID(idWebUser);
            if (user == null) return null;
            Guid IDAdmin = WebGroupConst.AdminId;
            Guid IDQuanTriTruong = WebGroupConst.QuanTriTruongID;

            if (user.WebGroupID.Equals(IDAdmin)) // Nếu admin thì lấy hết
            {
                //
                var result = (from o in this.ObjectSet
                              where o.SendDate >= tungay && o.SendDate <= denngay
                              select new DTO_QuanLyGuiMail
                              {
                                  Oid = o.Oid,
                                  Title = o.Title,
                                  Contents = o.Contents,
                                  ReceiverEmail = o.ReceiverEmail,
                                  SendDate = o.SendDate.Value,
                                  FileName = o.FileName,
                                  SendEmail = o.SendEmail,
                                  SendPass = o.SendPass
                              });
                return result;
            }
            else if (user.WebGroupID.Equals(IDQuanTriTruong)) // Nếu admin thì lấy theo trường
            {
                //
                var result = (from o in this.ObjectSet
                              where o.SendDate >= tungay && o.SendDate <= denngay
                                    && o.WebUser.CongTyId == user.CongTyId
                              select new DTO_QuanLyGuiMail
                              {
                                  Oid = o.Oid,
                                  Title = o.Title,
                                  Contents = o.Contents,
                                  ReceiverEmail = o.ReceiverEmail,
                                  SendDate = o.SendDate.Value,
                                  FileName = o.FileName,
                                  SendEmail = o.SendEmail,
                                  SendPass = o.SendPass
                              });
                return result;
            }
            else
            {
                //
                var result = (from o in this.ObjectSet
                              where o.SendDate >= tungay && o.SendDate <= denngay
                                    && o.IDWebUser == idWebUser 
                              select new DTO_QuanLyGuiMail
                              {
                                  Oid = o.Oid,
                                  Title = o.Title,
                                  Contents = o.Contents,
                                  ReceiverEmail = o.ReceiverEmail,
                                  SendDate = o.SendDate.Value,
                                  FileName = o.FileName,
                                  SendEmail = o.SendEmail,
                                  SendPass = o.SendPass
                              });
                return result;
            }
        }


        public CC_MailManager GetByID(Guid id)
        {
            var result = (from o in this.ObjectSet
                         where o.Oid == id
                         select o).SingleOrDefault();
            return result;
        }

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_MailManager item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
    }//end class
}
