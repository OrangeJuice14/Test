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
    public class CC_Attachments_Factory : BaseFactory<Entities, CC_Attachments>
    {
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_Attachments_Factory.New().CreateAloneObject();
        }
        public static CC_Attachments_Factory New()
        {
            return new CC_Attachments_Factory();
        }
        public CC_Attachments_Factory()
            : base(Database.NewEntities())
        {

        }

        public CC_Attachments GetAttachment(Guid oid,string filename)
        {
            var result = (from o in this.ObjectSet
                          where o.KhaiBaoCongTac == oid && o.FileName.Equals(filename)
                          select o).FirstOrDefault();
            return result;
        }
        public IQueryable<CC_Attachments> GetAttachmentList_By(Guid oidKhaiBaoCongTac)
        {
            var result = (from o in this.ObjectSet
                          where o.KhaiBaoCongTac == oidKhaiBaoCongTac
                          select o);
            return result;
        }

    }//end class
}
