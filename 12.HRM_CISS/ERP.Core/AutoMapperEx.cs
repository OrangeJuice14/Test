using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Core
{
    public static class AutoMapperEx
    {
        public static TDestination Map<TDestination>(this object source)
        {
            var des = default(TDestination);
            if (source != null)
            {
                Mapper.CreateMap(source.GetType(), typeof(TDestination));
                des = (TDestination)Mapper.Map(source, source.GetType(), typeof(TDestination));
            }
            return des;
        }

        public static IEnumerable<TDestination> Map<TDestination>(this IEnumerable<object> source)
        {
            Type sourceType = source.GetType().GetGenericArguments()[0];
            Mapper.CreateMap(sourceType, typeof(TDestination));
            var des = (IEnumerable<TDestination>)Mapper.Map(source, source.GetType(), typeof(IEnumerable<TDestination>));
            return des;
        }

        //public static IEnumerable<TDestination> Map<TDestination>(this IQueryable<object> source)
        //{
        //    Type sourceType = source.GetType().GetGenericArguments()[0];
        //    Mapper.CreateMap(sourceType, typeof(TDestination));
        //    var des = (IEnumerable<TDestination>)Mapper.Map(source, source.GetType(), typeof(IEnumerable<TDestination>));
        //    return des;
        //}
    }
}
