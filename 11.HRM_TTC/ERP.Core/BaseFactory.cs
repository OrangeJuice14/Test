using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using System.Reflection;
using System.Linq.Expressions;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
namespace ERP_Core
{
    public class BaseFactory<TObjectContext, TEntity> : IBusinessFactory, IDisposable
        where TObjectContext : BaseObjectContext//ObjectContext
        where TEntity : BaseEntityObject//EntityObject
    {
        public Boolean IsDirty
        {
            get { return Context.IsDirty; }
        }
        public string AdoConnectionString
        {
            get
            {
                return this.Context.AdoConnectionString();
            }
        }


        public string DatabaseName
        {
            get
            {
                return this.Context.DatabaseName();
            }
        }

        private TObjectContext _context = default(TObjectContext);
        public TObjectContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
                _objectSet = this.GetObjectSet();
            }
        }
        public Boolean LazyLoadingEnabled
        {
            get { return Context.ContextOptions.LazyLoadingEnabled; }
            set { Context.ContextOptions.LazyLoadingEnabled = value; }
        }
        private ObjectSet<TEntity> _objectSet = default(ObjectSet<TEntity>);
        public ObjectSet<TEntity> ObjectSet
        {
            get { return _objectSet ?? (_objectSet = this.GetObjectSet()); }
        }
        private String _entitySetName = String.Empty;
        public String EntitySetName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_entitySetName)) _entitySetName = this.GetEntitySetName();
                return _entitySetName;
            }
        }
        private String _entitySetFullName = String.Empty;
        public String EntitySetFullName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_entitySetFullName)) _entitySetFullName = this.GetEntitySetFullName();
                return _entitySetFullName;
            }
        }

        public virtual DateTime SystemDateTime
        {
            get
            {
                DateTime result = (from o in this.ObjectSet
                                   select DateTime.Now).FirstOrDefault();
                return result;
            }
        }
        public virtual DateTime SystemDate
        {
            get
            {
                DateTime result = (from o in this.ObjectSet
                                   select DateTime.Now).FirstOrDefault().Date;
                return result;
            }
        }

        private String _tableName = String.Empty;
        public virtual String TableName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_tableName)) _tableName = this.GetTableName_Helper();
                return _tableName;
            }
        }

        private String _tableNameWithSchema = String.Empty;
        public virtual String TableNameWithSchema
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_tableNameWithSchema)) _tableNameWithSchema = this.GetTableNameWithSchema_Helper();
                return _tableNameWithSchema;
            }
        }

        public BaseFactory(TObjectContext context)
        {
            {
                _context = context;//DataAccess.Database.NewEntities();
                _entitySetName = this.GetEntitySetName();
                _entitySetFullName = this.GetEntitySetFullName();
                _objectSet = GetObjectSet();
            }

        }


        public static void DeleteHelper(IObjectSet<TEntity> objectSet, EntityCollection<TEntity> entityCollection)
        {


            List<Object> objectList = entityCollection.ToList<Object>();

            foreach (Object deleteObject in objectList)
                objectSet.DeleteObject(deleteObject as TEntity);



        }
        public static void DeleteHelper(TObjectContext context, EntityCollection<TEntity> entityCollection)
        {

            //List<Object> objectList = new List<Object>();
            //foreach (var obj in entityCollection)//
            //    objectList.Add(obj);

            List<Object> objectList = entityCollection.ToList<Object>();

            foreach (Object deleteObject in objectList)
                context.DeleteObject(deleteObject);


            //Int32 count = entityCollection.Count();
            //for (int i = 0; i < count; i++)
            //{
            //    Object entity = entityCollection.Skip(0).Take(1).Single();

            //    context.DeleteObject(entity);
            //}

        }
        public static void DeleteHelper(EntityCollection<TEntity> entityCollection)
        {

            if (entityCollection != null && entityCollection.Any())
            {
                using (var context = (entityCollection.FirstOrDefault() as ERP_Core.BaseEntityObject).GetContext() as TObjectContext)
                {
                    DeleteHelper(context, entityCollection);
                }
                //Int32 count = entityCollection.Count();
                //for (int i = 0; i < count; i++)
                //{
                //    Object entity = entityCollection.Skip(0).Take(1).Single();

                //    context.DeleteObject(entity);
                //}
            }
        }
        public static void DeleteHelper(TObjectContext context, params Object[] objectList)
        {
            foreach (var deleteObject in objectList)
                context.DeleteObject(deleteObject);
        }
        public static void DeleteHelper(TObjectContext context, List<Object> objectList)
        {
            foreach (var deleteObject in objectList)
                context.DeleteObject(deleteObject);
        }
        public static void DeleteHelper(params Object[] objectList)
        {
            foreach (var deleteObject in objectList)
            {
                BaseObjectContext context = (objectList[0] as ERP_Core.BaseEntityObject).GetContext();
                context.DeleteObject(deleteObject);
            }
        }
        public static void DeleteHelper(List<Object> objectList)
        {
            foreach (var deleteObject in objectList)
            {
                BaseObjectContext context = (objectList[0] as ERP_Core.BaseEntityObject).GetContext();
                context.DeleteObject(deleteObject);
            }
        }
        public static void DeleteHelper(IQueryable<TEntity> list)
        {
            if (list != null && list.Any())
            {
                var context = (list.FirstOrDefault() as ERP_Core.BaseEntityObject).GetContext();

                var objectList = list.ToList<Object>();

                foreach (var deleteObject in objectList)
                    context.DeleteObject(deleteObject);

                //Int32 count = list.Count();
                //for (int i = 0; i < count; i++)
                //{
                //    TEntity entity = list.Skip(0).Take(1).Single();

                //    context.DeleteObject(entity);
                //}
            }
        }
        public void ENABLE_BROKER()
        {
            this.Context.ENABLE_BROKER();
        }

        public void DISABLE_BROKER()
        {
            this.Context.DISABLE_BROKER();
        }
        public Boolean Detect_IS_BROKER_ENABLED()
        {
            return this.Context.Detect_IS_BROKER_ENABLED();
        }
        public void IDENTITY_INSERT_ON()
        {
            String tableName = GetTableNameWithSchema_Helper();
            Context.ExecuteStoreCommand(String.Format("SET IDENTITY_INSERT {0} ON", tableName));
        }

        public void IDENTITY_INSERT_OFF()
        {
            String tableName = GetTableNameWithSchema_Helper();
            Context.ExecuteStoreCommand(String.Format("SET IDENTITY_INSERT {0} OFF", tableName));
        }

        public void SetIdentify(Int64 index)
        {
            String tableName = GetTableNameWithSchema_Helper();
            Context.ExecuteStoreCommand(String.Format("DBCC CHECKIDENT ('{0}', RESEED, {1})", tableName, index.ToString()));
        }



        private string GetTableNameWithSchema_Helper()
        {
            /*
            string sql = this.Context.CreateObjectSet<TEntity>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
             */
            string sql = this.ObjectSet.ToTraceString();
            var regex = new Regex("FROM (?<table>.*) AS");
            var match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
        private string GetTableName_Helper()
        {

            var sql = this.ObjectSet.ToTraceString();
            var regex = new Regex("FROM [(?<schema>.*)].[(?<table>.*)] AS");
            var match = regex.Match(sql);

            var table = match.Groups["table"].Value;
            return table;
        }

        /// ////////////////////////////////////////////////////
        /*
        private readonly static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();
        private EntitySetBase GetEntitySet(Type type)
        {
            if (_mappingCache.ContainsKey(type))
                return _mappingCache[type];

            type = GetObjectType(type);
            string baseTypeName = type.BaseType.Name;
            string typeName = type.Name;

            ObjectContext octx = this.Context;
            var es = octx.MetadataWorkspace
                            .GetItemCollection(DataSpace.SSpace)
                            .GetItems<EntityContainer>()
                            .SelectMany(c => c.BaseEntitySets
                                            .Where(e => e.Name == typeName
                                            || e.Name == baseTypeName))
                            .FirstOrDefault();

            if (es == null)
                throw new ArgumentException("Entity type not found in GetEntitySet", typeName);

            _mappingCache.Add(type, es);
            return es;
        }

        internal String GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            //if you are using EF6
            return String.Format("[{0}].[{1}]", es.Schema, es.Table);

            //if you have a version prior to EF6
            //return string.Format( "[{0}].[{1}]", 
            //        es.MetadataProperties["Schema"].Value, 
            //        es.MetadataProperties["Table"].Value );
        }

        internal Type GetObjectType(Type type)
        {
            return ObjectContext.GetObjectType(type);
        }


         */

        public virtual Boolean IsDirtyObjectInThisFactory(object entity)
        {
            var result = false;
            ObjectStateEntry entry;
            if (this.Context.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                this.Context.DetectChanges();

                IEnumerable<string> changedNames = entry.GetModifiedProperties();

                result = (changedNames.Any());
            }
            return result;
        }
        public virtual Boolean IsDirtyPropertyOfEntityObjectInThisFactory(object entity, string propertyName)
        {
            Boolean result = false;
            ObjectStateEntry entry;
            if (this.Context.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                this.Context.DetectChanges();

                IEnumerable<string> changedNames = entry.GetModifiedProperties();

                result = changedNames.Any(x => x == propertyName);
            }
            return result;

        }


        public virtual void Attach(TEntity entity)
        {
            this.ObjectSet.Attach(entity);
        }
        public virtual void Attach(List<TEntity> entityList)
        {
            foreach (TEntity entity in entityList)
            {
                this.ObjectSet.Attach(entity);
            }
        }
        public virtual void Attach(IQueryable<TEntity> entityList)
        {
            foreach (TEntity entity in entityList)
            {
                this.Attach(entity);
            }
        }
        public virtual void Attach(IQueryable<ERP_Core.BaseEntityObject> entityList)
        {
            foreach (var entity in entityList)
            {
                this.Context.Attach(entity);
            }
        }
        public virtual void Detach(TEntity entity)
        {
            this.ObjectSet.Detach(entity);
        }
        public virtual void Detach(List<TEntity> entityList)
        {
            foreach (TEntity entity in entityList)
            {
                this.ObjectSet.Detach(entity);
            }

        }
        public virtual void Detach(IQueryable<TEntity> entityList)
        {
            foreach (TEntity entity in entityList)
            {
                this.Detach(entity);
            }

        }

        public virtual void SaveChanges(int secondsTimeOut = 360)
        {

            Context.SaveChanges(secondsTimeOut);
        }
        public virtual void SaveChangesWithoutTransactionScope()
        {
            Context.SaveChangesWithoutTransactionScope();
        }
        public virtual void SaveChangesWithoutTrackingLog(int secondsTimeOut = 360)
        {
            Context.SaveChangesWithoutTrackingLog(secondsTimeOut);
        }
        public virtual async void SaveChangesAsyncWithoutTrackingLog(int secondsTimeOut = 360)
        {
            await Context.SaveChangesAsyncWithoutTrackingLog(secondsTimeOut);
        }


        public virtual IQueryable<TEntity> GetAll()
        {
            
            IQueryable<TEntity> query = (from o in ObjectSet select o);
            return query;
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            List<TEntity> query = await ObjectSet.ToListAsync();
            return query;
        }

        public virtual ParallelQuery<TEntity> ParallelGetAll()
        {
            ParallelQuery<TEntity> query = from o in ObjectSet.AsParallel() select o;
            return query;
        }
        public virtual TEntity GetObjectBySingleKey(String key, Object value)
        {
            var entity = default(TEntity);

            var entityKey = new EntityKey(EntitySetFullName, key, value);
            //try
            //{
            entity = (TEntity)Context.GetObjectByKey(entityKey);
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //}
            return entity;
        }
        ///
        //public virtual IQueryable<TEntity> GetListBySingleKey(String key, Object value)
        //{
        //    IQueryable<TEntity> list;

        //    EntityKey entityKey = new EntityKey(EntitySetFullName, key, value);
        //    try
        //    {

        //        list = from o in ObjectSet 
        //               where o.
        //               select o;//(TEntity)Context.GetObjectByKey(entityKey);
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //    }
        //    return list;
        //}
        /// 
        public virtual TEntity GetObjectBySingleKey(KeyValuePair<string, object> entityKeyValue)
        {
            TEntity entity = default(TEntity);

            var key = new EntityKey(EntitySetFullName, entityKeyValue.Key, entityKeyValue.Value);
            //try
            //{
            entity = (TEntity)Context.GetObjectByKey(key);
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //}
            return entity;
        }
        public virtual TEntity GetObjectByMultiKey(IEnumerable<KeyValuePair<string, object>> entityKeyValues)
        {
            TEntity entity = default(TEntity);

            var key = new EntityKey(EntitySetFullName, entityKeyValues);
            //try
            //{
            entity = (TEntity)Context.GetObjectByKey(key);
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //}
            return entity;
        }

        public virtual TEntity CreateAloneObject()
        {
            var obj = Context.CreateObject<TEntity>();

            return obj;
        }

        public virtual TEntity CreateManagedObject()
        {
            var obj = Context.CreateObject<TEntity>();
            this.AddObject(obj);
            return obj;
        }

        public virtual BaseEntityObject CreateManagedBaseEntityObject()
        {
            return CreateManagedObject() as BaseEntityObject;
        }

        public virtual void AddObject(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            else
                Context.AddObject(EntitySetFullName, entity);

        }


        public virtual void DeleteObject(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            else
                Context.DeleteObject(entity);
        }

        //public virtual void DeleteObject<T>(T entity) where T : EntityObject
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("entity");
        //    else
        //        Context.DeleteObject(entity);
        //}

        //public virtual void DeleteObjectAndSaveChanges(TEntity entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("entity");
        //    else
        //    {
        //        Context.DeleteObject(entity);
        //        this.SaveChanges();
        //    }
        //}

        public virtual void DeleteObjectList(IQueryable<TEntity> list)
        {

            //The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'.
            var count = list.Count();
            for (var i = 0; i < count; i++)
            {
                TEntity entity = list.Skip(0).Take(1).Single();
                ObjectSet.DeleteObject(entity);
            }
        }
        public virtual void DeleteObjectList(List<TEntity> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            else
                foreach (var item in list)
                {
                    ObjectSet.DeleteObject(item);
                }
        }
        //public virtual void DeleteObjectListAndSaveChange(IQueryable<TEntity> list)
        //{
        //    this.DeleteObjectList(list);
        //    this.SaveChanges();
        //}
        //public virtual void DeleteObjectListAndSaveChange(List<TEntity> list)
        //{
        //    this.DeleteObjectList(list);
        //    this.SaveChanges();
        //}
        public virtual void DeleteAll()
        {


            /*cach 1
           
            List<TEntity> list = (from o in ObjectSet
                                        select o).ToList();
            int count = list.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                TEntity entity = list[i];
                ObjectSet.DeleteObject(entity);
            } 
             */

            //cach 2


            var query = from o in ObjectSet
                                        //orderby 1//The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'.
                                        select o;

            var count = query.Count();
            for (int i = 0; i < count; i++)
            {
                var entity = query.Skip(0).Take(1).Single();
                ObjectSet.DeleteObject(entity);
            }
        }

        //public virtual void DeleteAllAndSaveChanges()
        //{
        //    DeleteAll();
        //    SaveChanges();
        //}



        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual TEntity SelectFirst()
        {

            var query = (from o in ObjectSet
                             select o).First();
            return query;
        }
        public virtual async Task<TEntity> SelectFirstAsync()
        {

            var query = await ObjectSet.FirstAsync();
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual TEntity SelectFirstOrDefault()
        {
            var obj = ObjectSet.FirstOrDefault();
            return obj;
        }
        public virtual async Task<TEntity> SelectFirstOrDefaultAsync()
        {
            var obj = await ObjectSet.FirstOrDefaultAsync();
            return obj;
        }
        public virtual TEntity SelectLast()
        {

            var query = ObjectSet.Last();
            return query;
        }

        public virtual TEntity SelectLastOrDefault()
        {

            var query = (from o in ObjectSet
                             select o).LastOrDefault();
            return query;
        }

        public virtual IQueryable<TEntity> SelectTopN(int n)
        {

            var query = (from o in ObjectSet
                                         select o).Take(n);
            return query;
        }
        public virtual ParallelQuery<TEntity> ParallelSelectTopN(int n)
        {

            ParallelQuery<TEntity> query = (from o in ObjectSet.AsParallel()
                                            select o).Take(n);
            return query;
        }
        public virtual IQueryable<TEntity> SelectBottomN(int n)
        {
            ObjectSet<TEntity> objectSet = GetObjectSet();
            var query1 = from o in objectSet
                                         select o;
            int count = query1.Count();
            int skipIndex = count - n;

            if (skipIndex < 0) skipIndex = 0;

            var query2 = (from o in query1 orderby 1 select o).Skip(skipIndex).Take(n);
            return query2;
        }
        public virtual ParallelQuery<TEntity> ParallelSelectBottomN(int n)
        {
            ObjectSet<TEntity> objectSet = GetObjectSet();
            ParallelQuery<TEntity> query1 = from o in objectSet.AsParallel()
                                            select o;
            int count = query1.Count();
            int skipIndex = count - n;

            if (skipIndex < 0) skipIndex = 0;

            ParallelQuery<TEntity> query2 = (from o in query1 orderby 1 select o).Skip(skipIndex).Take(n);
            return query2;
        }
        public virtual IQueryable<TEntity> SelectSkipTake(int skipIndex, int takeCount)
        {
            ObjectSet<TEntity> objectSet = GetObjectSet();
            IQueryable<TEntity> query1 = from o in objectSet
                                         select o;
            if (skipIndex < 0) skipIndex = 0;
            IQueryable<TEntity> query2 = (from o in query1 orderby 1 select o).Skip(skipIndex).Take(takeCount);
            return query2;
        }
        public virtual ParallelQuery<TEntity> ParallelSelectSkipTake(int skipIndex, int takeCount)
        {
            ObjectSet<TEntity> objectSet = GetObjectSet();
            ParallelQuery<TEntity> query1 = from o in objectSet.AsParallel()
                                            select o;
            if (skipIndex < 0) skipIndex = 0;
            ParallelQuery<TEntity> query2 = (from o in query1 orderby 1 select o).Skip(skipIndex).Take(takeCount);
            return query2;
        }
        public virtual void Refresh(RefreshMode refreshMode, IEnumerable collection)
        {
            this.Context.Refresh(refreshMode, collection);
        }
        public virtual void Refresh(RefreshMode refreshMode, object entity)
        {
            this.Context.Refresh(refreshMode, entity);
        }
        public virtual void RefreshAll(RefreshMode refreshMode)
        {
            this.Context.RefreshAll(refreshMode);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, Boolean>> predicate)
        {
            /*su dung: factory.Where(x => x.Age > 18)*/
            return this.ObjectSet.Where(predicate);
        }

        //private ObjectQuery GetObjectQuery(IQueryable query)
        //{
        //    if (query is ObjectQuery)
        //        return query as ObjectQuery;


        //    // Use the DbSet to create the ObjectSet and get the appropriate provider.
        //    IQueryable iqueryable = this.ObjectSet as IQueryable;
        //    IQueryProvider provider = iqueryable.Provider;

        //    // Use the provider and expression to create the ObjectQuery.
        //    return provider.CreateQuery(query.Expression) as ObjectQuery;
        //}
        //public virtual void AutoRefresh(RefreshMode refreshMode, IQueryable refreshableObjects)
        //{
        //    //IEnumerable<Object> refreshableObjects = (from entry in this.Context.ObjectStateManager.GetObjectStateEntries(
        //    //                               EntityState.Added
        //    //                              | EntityState.Deleted
        //    //                              | EntityState.Modified
        //    //                              | EntityState.Unchanged)
        //    //                                          where entry.EntityKey != null
        //    //                                          select entry.Entity);

        //    //this.Context.Refresh(refreshMode, refreshableObjects);
        //    {
        //        EntityConnection ec = (EntityConnection)this.Context.Connection;
        //        SqlConnection sc = (SqlConnection)ec.StoreConnection;
        //        var newConnectionString = sc.ConnectionString;//new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(this.Context.Connection.ConnectionString).ProviderConnectionString;
        //        if (!BaseObjectContext.TestConnectionStrings.Contains(newConnectionString))
        //        {
        //            if (this.Detect_IS_BROKER_ENABLED() == false)
        //                ENABLE_BROKER();
        //            BaseObjectContext.TestConnectionStrings.Add(newConnectionString);
        //            SqlDependency.Start(newConnectionString);
        //        }
        //        string sqlDependencyCookie = "MS.SqlDependencyCookie";
        //        //var oldCookie = CallContext.GetData(sqlDependencyCookie);


        //        ObjectQuery query = this.GetObjectQuery(refreshableObjects);

        //        sc.Open();


        //        SqlCommand command = new SqlCommand(query.ToTraceString(), sc);
        //        ////SqlCommand command =this.Context.GetCom
        //        //// Add all the paramters used in query.
        //        //foreach (ObjectParameter parameter in query.Parameters)
        //        //{
        //        //    command.Parameters.AddWithValue(parameter.Name, parameter.Value);
        //        //}
        //        var dependency = new SqlDependency();
        //        var oldCookie = CallContext.GetData(sqlDependencyCookie);
        //        //using (SqlDataReader reader = command.ExecuteReader())
        //        //{
        //        //}
        //        try
        //        {
        //            CallContext.SetData(sqlDependencyCookie, dependency.Id);
        //            dependency.OnChange += (object sender, SqlNotificationEventArgs e) =>
        //            {
        //                if (e.Info == SqlNotificationInfo.Invalid)
        //                {
        //                    Debug.Print("SqlNotification:  A statement was provided that cannot be notified.");
        //                    return;
        //                }
        //                try
        //                {
        //                    var id = ((SqlDependency)sender).Id;
        //                    //IEnumerable collection;
        //                    //if (collections.TryGetValue(id, out collection))
        //                    //{
        //                    //    collections.Remove(id);
        //                    //    AutoRefresh(collection);
        //                    //    var notifyRefresh = collection as INotifyRefresh;
        //                    //    if (notifyRefresh != null)
        //                    //        System.Windows.Application.Current.Dispatcher.BeginInvoke(
        //                    //         (Action)(notifyRefresh.OnRefresh));
        //                    //}
        //                }
        //                catch (Exception ex)
        //                {
        //                    System.Diagnostics.Debug.Print("Error in OnChange: {0}", ex.Message);
        //                }
        //            };//
        //            //this.Context.Refresh(refreshMode, refreshableObjects);
        //            this.ObjectSet.ToArray();
        //        }
        //        finally
        //        {
        //            CallContext.SetData(sqlDependencyCookie, oldCookie);
        //        }
        //    }
        //}

        #region Helper
        private static PropertyInfo GetPublicInstancePropertyInfoInObjectContext<TObjectContext>(String propertyName)
        {
            Type type = typeof(TObjectContext);
            //Type type = Type.GetType(fullClassTypeName);
            PropertyInfo info = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return info;
        }
        private ObjectSet<TEntity> GetObjectSet()
        {
            //String namespaceAndClassName = Context.ToString();

            PropertyInfo info = GetPublicInstancePropertyInfoInObjectContext<TObjectContext>(EntitySetName);
            var objectSet = (ObjectSet<TEntity>)(info.GetValue(Context, null));
            //ObjectSet<TEntity> objectSet = this.Context.CreateObjectSet<TEntity>();
            return objectSet;
        }
        private string GetEntitySetName()
        {
            Type entityType = typeof(TEntity);

            string entityTypeName = entityType.Name;
            var container = Context.MetadataWorkspace.GetEntityContainer(Context.DefaultContainerName, DataSpace.CSpace);
            string entitySetName = (from meta in container.BaseEntitySets
                                    where meta.ElementType.Name == entityTypeName
                                    select meta.Name).First();

            return entitySetName;
        }
        private string GetEntitySetFullName()
        {
            Type entityType = typeof(TEntity);
            string entityTypeName = entityType.Name;
            var container = Context.MetadataWorkspace.GetEntityContainer(Context.DefaultContainerName, DataSpace.CSpace);
            string entitySetName = (from meta in container.BaseEntitySets
                                    where meta.ElementType.Name == entityTypeName
                                    select meta.Name).First();

            return String.Format("{0}.{1}", container.Name, entitySetName);
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
        /*
         * 
         public T GetByPrimaryKey<T>(int id) where T : class
    {
        return (T)_db.GetObjectByKey(new EntityKey(_db.DefaultContainerName + "." + this.GetEntityName<T>(), GetPrimaryKeyInfo<T>().Name, id));
    }
        string GetEntityName<T>()
    {
            string name = typeof(T).Name;
            if (name.ToLower() == "person")
                return "People";
            else if (name.Substring(name.Length - 1, 1).ToLower() == "y")
                return name.Remove(name.Length - 1, 1) + "ies";
            else if (name.Substring(name.Length - 1, 1).ToLower() == "s")
                return name + "es";
            else
                return name + "s";
    }

    private PropertyInfo GetPrimaryKeyInfo<T>()
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo pI in properties)
        {
            System.Object[] attributes = pI.GetCustomAttributes(true);
            foreach (object attribute in attributes)
            {
                if (attribute is EdmScalarPropertyAttribute)
                {
                    if ((attribute as EdmScalarPropertyAttribute).EntityKeyProperty == true)
                        return pI;
                }
                else if (attribute is ColumnAttribute)
                {

                    if ((attribute as ColumnAttribute).IsPrimaryKey == true)
                        return pI;
                }
            }
        }
        return null;
    } 
         * 
         */

    }
}
