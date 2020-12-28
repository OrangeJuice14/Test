using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Core
{

    public static class PropetiesMapper
    {
        /// <span class="code-SummaryComment"><summary>
        /// Copies all the properties of the "from" object to this object if they exist.
        /// <span class="code-SummaryComment"></summary>
        /// <span class="code-SummaryComment"><param name="to">The object in which the properties are copied</param>
        /// <span class="code-SummaryComment"><param name="from">The object which is used as a source</param>
        /// <span class="code-SummaryComment"><param name="excludedProperties">Exclude these properties from the copy</param>
        public static void CopyPropertiesFrom(this object to, object from, params string[] excludedProperties)
        {
            Type targetType = to.GetType();
            Type sourceType = from.GetType();

            PropertyInfo[] sourceProps = sourceType.GetProperties();
            foreach (var propInfo in sourceProps)
            {
                //filter the properties
                if (excludedProperties != null
                  && excludedProperties.Contains(propInfo.Name))
                    continue;

                //Get the matching property from the target
                PropertyInfo toProp =
                  (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name);

                //If it exists and it's writeable
                if (toProp != null && toProp.CanWrite)
                {
                    //Copy the value from the source to the target
                    Object value = propInfo.GetValue(from, null);
                    toProp.SetValue(to, value, null);
                }
            }
        }

        /// <span class="code-SummaryComment"><summary>
        /// Copies all the properties of the "from" object to this object if they exist.
        /// <span class="code-SummaryComment"></summary>
        /// <span class="code-SummaryComment"><param name="to">The object in which the properties are copied</param>
        /// <span class="code-SummaryComment"><param name="from">The object which is used as a source</param>
        public static void CopyPropertiesFrom(this object to, object from)
        {
            to.CopyPropertiesFrom(from, null);
        }

        /// <span class="code-SummaryComment"><summary>
        /// Copies all the properties of this object to the "to" object
        /// <span class="code-SummaryComment"></summary>
        /// <span class="code-SummaryComment"><param name="to">The object in which the properties are copied</param>
        /// <span class="code-SummaryComment"><param name="from">The object which is used as a source</param>
        public static void CopyPropertiesTo(this object from, object to)
        {
            to.CopyPropertiesFrom(from, null);
        }

        /// <span class="code-SummaryComment"><summary>
        /// Copies all the properties of this object to the "to" object
        /// <span class="code-SummaryComment"></summary>
        /// <span class="code-SummaryComment"><param name="to">The object in which the properties are copied</param>
        /// <span class="code-SummaryComment"><param name="from">The object which is used as a source</param>
        /// <span class="code-SummaryComment"><param name="excludedProperties">Exclude these properties from the copy</param>
        public static void CopyPropertiesTo(this object from, object to, params string[] excludedProperties)
        {
            to.CopyPropertiesFrom(from, excludedProperties);
        }


        public static void CopyIncludedPropertiesFrom(this object to, object from, params string[] includedProperties)
        {
            Type targetType = to.GetType();
            Type sourceType = from.GetType();

            PropertyInfo[] sourceProps = sourceType.GetProperties();
            foreach (var propInfo in sourceProps)
            {
                //filter the properties
                if (includedProperties != null
                    && includedProperties.Contains(propInfo.Name))
                {

                    //Get the matching property from the target
                    PropertyInfo toProp =
                        (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name);

                    //If it exists and it's writeable
                    if (toProp != null && toProp.CanWrite)
                    {
                        //Copy the value from the source to the target
                        Object value = propInfo.GetValue(from, null);
                        toProp.SetValue(to, value, null);
                    }
                }
            }
        }
        public static void CopyIncludedPropertiesTo(this object from, object to, params string[] includedProperties)
        {
            to.CopyIncludedPropertiesFrom(from, includedProperties);
        }

        public static void CopyMappingPropertiesFrom(this object to, object from, params MappingPropertyName[] mappingProperties)
        {
            Type targetType = to.GetType();
            Type sourceType = from.GetType();

            //PropertyInfo[] sourceProps = sourceType.GetProperties();

            foreach (var mappingPropertyName in mappingProperties)
            {
                PropertyInfo sourceProperty = sourceType.GetProperty(mappingPropertyName.Source);
                PropertyInfo targetProperty = targetType.GetProperty(mappingPropertyName.Destination);
                if (sourceProperty != null && targetProperty != null)
                {

                    //If it exists and it's writeable
                    if (targetProperty.CanWrite)
                    {
                        //Copy the value from the source to the target
                        Object value = sourceProperty.GetValue(from, null);
                        targetProperty.SetValue(to, value, null);
                    }
                }
            }
        }

        public static void CopyMappingPropertiesTo(this object from, object to, params MappingPropertyName[] mappingProperties)
        {
            to.CopyMappingPropertiesFrom(from, mappingProperties);
        }

        public class MappingPropertyName
        {
            public String Source;
            public String Destination;

            public MappingPropertyName(String source, String destination)
            {
                Source = source;
                Destination = destination;
            }
        }
    }
}

