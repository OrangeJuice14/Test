using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HRMChamCong.Helper
{
    public static class Helper
    {
        public static T Map<T>(this object source)
        {
            Mapper.CreateMap(source.GetType(), typeof(T));
            T des = (T)Mapper.Map(source, source.GetType(), typeof(T));
            return des;
        }

        public static List<T> Map<T>(this IEnumerable<object> source)
        {
            Type sourceType = source.GetType().GetGenericArguments()[0];
            Mapper.CreateMap(sourceType, typeof(T));
            List<T> des = (List<T>)Mapper.Map(source, source.GetType(), typeof(List<T>));
            return des;
        }

        public static T CreateObject<T>()
        {
            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(obj, string.Empty, null);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    propertyInfo.SetValue(obj, DateTime.Now, null);
                }
                else if (propertyInfo.PropertyType == typeof(Guid))
                {
                    propertyInfo.SetValue(obj, Guid.NewGuid(), null);
                }
            }
            return obj;
        }
    }
}