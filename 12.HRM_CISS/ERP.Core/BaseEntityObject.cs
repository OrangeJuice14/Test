
/*
 *Người viết: Nguyễn Phú Cường 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity.Core.Objects.DataClasses;
//
namespace ERP_Core
{
    public class BaseEntityObject : EntityObject
    {
        public Object OwnerFormAddedObj = null;
        public bool IsNew
        {
            get
            {
                return base.EntityState == System.Data.Entity.EntityState.Added ? true : false;
            }
        }
        public BaseEntityObject()
        {

        }
        //public bool ChildIsDirty
        //{
        //    get
        //    {
        //        Type type = this.GetType();
        //        PropertyInfo[] infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //        foreach (PropertyInfo inf in infos)
        //        {

        //            dynamic obj = inf.GetValue(this, null);
        //            if (obj != null && obj.GetType().BaseType == typeof(ICollection<>))
        //            {
        //                Type childListType = obj.GetType();
        //                IEnumerator<EntityObject> lst = obj.GetEnumerator();

        //                do
        //                {
        //                    if (lst.Current != null && (lst.Current.EntityState == System.Data.Entity.EntityState.Added.EntityState.Added
        //                                        || lst.Current.EntityState == System.Data.Entity.EntityState.Modified))
        //                    {
        //                        return true;
        //                    }
        //                }
        //                while (lst.MoveNext());

        //            }

        //        }
        //        return false;
        //    }
        //}




      
    }
}
