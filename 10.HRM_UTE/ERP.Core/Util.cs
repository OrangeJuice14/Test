using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace ERP_Core
{
    public class Util
    {
        //private bool IsDirtyPropertyWorking(ObjectContext ctx, object entity, string propertyName)
        //{//chạy chậm
        //    ObjectStateEntry entry;
        //    if (ctx.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
        //    {
        //        ctx.DetectChanges();

        //        IEnumerable<string> changedNames = entry.GetModifiedProperties();

        //        return changedNames.Any(x => x == propertyName);
        //    }
        //    return false;
        //}

        private bool IsDirtyProperty(ObjectContext objectContext, object entity, string propertyName)
        {
            ObjectStateEntry entry;
            if (objectContext.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                int propIndex = this.GetPropertyIndex(entry, propertyName);

                if (propIndex != -1)
                {
                    var oldValue = entry.OriginalValues[propIndex];
                    var newValue = entry.CurrentValues[propIndex];

                    return !Equals(oldValue, newValue);
                }
                else
                {
                    throw new ArgumentException(String.Format("Cannot find original value for property '{0}' in entity entry '{1}'",
                            propertyName,
                            entry.EntitySet.ElementType.FullName));
                }
            }

            return false;
        }


        private int GetPropertyIndex(ObjectStateEntry entry, string propertyName)
        {
            OriginalValueRecord record = entry.GetUpdatableOriginalValues();

            for (int i = 0; i != record.FieldCount; i++)
            {
                FieldMetadata metaData = record.DataRecordInfo.FieldMetadata[i];
                if (metaData.FieldType.Name == propertyName)
                {
                    return metaData.Ordinal;
                }
            }

            return -1;
        }

        public static String ToSQLString(Object linqQuery)
        {
            string result = ((ObjectQuery)linqQuery).ToTraceString();
            
            return result;
        }
    }
}
