using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Transactions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ERP_Core
{
    public class EntityTracking
    {
        public string BusinessCode { get; set; }
        public string Message { get; set; }
        public string EntityKeys { get; set; }
        public string CurrentPropertyValues { get; set; }
        public string OldPropertyValues { get; set; }
        public string NewPropertyValues { get; set; }
        public string LogType { get; set; }
        public string EntitySet { get; set; }//tuong ung voi bang du lieu
        public string ObjectSet { get; set; }
        public string UserNetworkIp { get; set; }
        public EntityTracking()
        {
            BusinessCode = null;
            Message = null;
            EntityKeys = null;
            CurrentPropertyValues = null;
            OldPropertyValues = null;
            NewPropertyValues = null;
            LogType = null;
            EntitySet = null;
            ObjectSet = null;
            UserNetworkIp = null;
        }
    }
    public class BaseObjectContext : ObjectContext
    {
        //public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Guid UserId
        {
            get;
            set;
        }

        public Boolean IsDirty
        {
            get { return CheckIsDirty_Helper(); }
        }

        public static Guid CompanyId
        {
            get;
            set;
        }

        public static DateTime WorkingDate
        {
            get;
            set;
        }
        static BaseObjectContext()
        {

        }
        public BaseObjectContext(EntityConnection connection)
            : base(connection)
        {

        }
        public BaseObjectContext(string connectionString)
            : base(connectionString)
        {

        }
        protected BaseObjectContext(string connectionString, string defaultContainerName)
            : base(connectionString, defaultContainerName)
        {

        }
        protected BaseObjectContext(EntityConnection connection, string defaultContainerName)
            : base(connection, defaultContainerName)
        {

        }

        public void ENABLE_BROKER()
        {
            String dbName = DatabaseName();
            base.ExecuteStoreCommand(String.Format("ALTER DATABASE {0} SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE", dbName));
        }

        public void DISABLE_BROKER()
        {
            String dbName = DatabaseName();
            base.ExecuteStoreCommand(String.Format("ALTER DATABASE {0} SET DISABLE_BROKER WITH ROLLBACK IMMEDIATE", dbName));
        }
        public Boolean Detect_IS_BROKER_ENABLED()
        {
            String dbName = DatabaseName();
            Boolean kq = base.ExecuteStoreQuery<Boolean>(String.Format("SELECT is_broker_enabled FROM sys.databases WHERE name = '{0}'", dbName)).SingleOrDefault();
            return kq;
        }
        public string AdoConnectionString()
        {
            return ((EntityConnection)this.Connection).StoreConnection.ConnectionString;
        }

        /// This works regardless of how the connection string names the database ("initial catalog", "database", etc.).
        public string DatabaseName()
        {
            return new SqlConnectionStringBuilder(AdoConnectionString()).InitialCatalog;
        }

        public static readonly List<string> TestConnectionStrings = new List<string>();
        public virtual void RefreshAll(RefreshMode refreshMode)
        {
            // Get all objects in statemanager with entityKey 
            // (context.Refresh will throw an exception otherwise) 
            IEnumerable<Object> refreshableObjects = (from entry in this.ObjectStateManager.GetObjectStateEntries(
                                                       EntityState.Added
                                                      | EntityState.Deleted
                                                      | EntityState.Modified
                                                      | EntityState.Unchanged)
                                                      where entry.EntityKey != null
                                                      select entry.Entity);

            base.Refresh(refreshMode, refreshableObjects);


        }
        //
        // Summary:
        //     Persists all updates to the data source and resets change tracking in the
        //     object context.
        //
        // Returns:
        //     The number of objects in an System.Data.EntityState.Added, System.Data.EntityState.Modified,
        //     or System.Data.EntityState.Deleted state when System.Data.Objects.ObjectContext.SaveChanges()
        //     was called.
        //
        // Exceptions:
        //   System.Data.OptimisticConcurrencyException:
        //     An optimistic concurrency violation has occurred in the data source.
        public new int SaveChanges(int secondsTimeOut = 360)//1
        {
            return SaveChanges_Helper(1, true, SaveOptions.None, secondsTimeOut);
        }

        public new int SaveChangesWithoutTransactionScope()//1
        {
            return SaveChanges_HelperWithoutTransactionScope(1, true, SaveOptions.None);
        }

        //
        // Summary:
        //     Persists all updates to the data source and optionally resets change tracking
        //     in the object context.
        //
        // Parameters:
        //   acceptChangesDuringSave:
        //     This parameter is needed for client-side transaction support. If true, the
        //     change tracking on all objects is reset after System.Data.Objects.ObjectContext.SaveChanges(System.Boolean)
        //     finishes. If false, you must call the System.Data.Objects.ObjectContext.AcceptAllChanges()
        //     method after System.Data.Objects.ObjectContext.SaveChanges(System.Boolean).
        //
        // Returns:
        //     The number of objects in an System.Data.EntityState.Added, System.Data.EntityState.Modified,
        //     or System.Data.EntityState.Deleted state when System.Data.Objects.ObjectContext.SaveChanges()
        //     was called.
        //
        // Exceptions:
        //   System.Data.OptimisticConcurrencyException:
        //     An optimistic concurrency violation has occurred.
        public new int SaveChanges(bool acceptChangesDuringSave, int secondsTimeOut = 360)//2
        {
            return SaveChanges_Helper(2, acceptChangesDuringSave, SaveOptions.None, secondsTimeOut);
        }


        //
        public int SaveChangesWithoutTrackingLog(int secondsTimeOut = 360)//2
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                   TimeSpan.FromSeconds(secondsTimeOut)))
            {
                int num = 0;
                num = base.SaveChanges();
                //hoàn tất giao tác
                transaction.Complete();
                return num;
            }
        }

        public async Task<int> SaveChangesAsyncWithoutTrackingLog(int secondsTimeOut = 360)//2
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                   TimeSpan.FromSeconds(secondsTimeOut)))
            {
                int num = 0;
                num = await base.SaveChangesAsync();
                //hoàn tất giao tác
                transaction.Complete();
                return num;
            }
        }



        //
        // Summary:
        //     Persists all updates to the data source with the specified System.Data.Objects.SaveOptions.
        //
        // Parameters:
        //   options:
        //     A System.Data.Objects.SaveOptions value that determines the behavior of the
        //     operation.
        //
        // Returns:
        //     The number of objects in an System.Data.EntityState.Added, System.Data.EntityState.Modified,
        //     or System.Data.EntityState.Deleted state when System.Data.Objects.ObjectContext.SaveChanges()
        //     was called.
        //
        // Exceptions:
        //   System.Data.OptimisticConcurrencyException:
        //     An optimistic concurrency violation has occurred.
        //public new int SaveChanges(SaveOptions options)//3
        //{
        //    return SaveChanges_Helper(1, true, options);
        //}

        //#region PropertyInfo
        //private static PropertyInfo GetPropertyInfo(Type classType, String propertyName)
        //{
        //    PropertyInfo info = classType.GetProperty(propertyName);
        //    return info;
        //}
        //private static PropertyInfo[] GetPropertiesInfo(Type classType)
        //{
        //    PropertyInfo[] infos = classType.GetProperties();
        //    return infos;
        //}
        //private static PropertyInfo GetPropertyInfo(String fullClassTypeName, String propertyName)
        //{
        //    Type type = Type.GetType(fullClassTypeName);
        //    return GetPropertyInfo(type, propertyName);
        //}
        //private static PropertyInfo[] GetPropertiesInfo(String fullClassTypeName)
        //{
        //    Type type = Type.GetType(fullClassTypeName);
        //    return GetPropertiesInfo(type);
        //}
        //#endregion
        //#region GetPropertyValue
        //private static object GetPropertyValue(object objectToGet, string propertyName)
        //{
        //    Type objectType = objectToGet.GetType();
        //    PropertyInfo propertyInfo = objectType.GetProperty(propertyName);
        //    object propertyValue = null;
        //    if ((propertyInfo != null) && (propertyInfo.CanRead))
        //    {
        //        propertyValue = propertyInfo.GetValue(objectToGet, null);
        //    }
        //    return propertyValue;
        //}
        //private static TResult GetPropertyValue<TResult>(object objectToGet, string propertyName)
        //{
        //    Type objectType = objectToGet.GetType();
        //    PropertyInfo info = objectType.GetProperty(propertyName);
        //    object value = null;
        //    if ((info != null) && (info.CanRead))
        //    {
        //        value = info.GetValue(objectToGet, null);
        //    }
        //    return (TResult)value;
        //}
        //private static void SetProperty(object objectToSet, string propertyName, object propertyValue)
        //{
        //    Type objectType = objectToSet.GetType();
        //    PropertyInfo propertyInfo = objectType.GetProperty(propertyName);

        //    if ((propertyInfo != null) && (propertyInfo.CanWrite))
        //        propertyInfo.SetValue(objectToSet, propertyValue, null);
        //}
        //#endregion


        //private static PropertyInfo GetPublicInstancePropertyInfoInObjectContext<TObjectContext>(String propertyName)
        //{
        //    Type type = typeof(TObjectContext);
        //    //Type type = Type.GetType(fullClassTypeName);
        //    PropertyInfo info = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        //    return info;
        //}

        public static string SerializeObjectToXmlString(object obj, System.Text.Encoding encoding)
        {
            try
            {
                String xmlizedString = null;
                var memoryStream = new MemoryStream();
                var xs = new XmlSerializer(obj.GetType());
                var xmlTextWriter = new XmlTextWriter(memoryStream, encoding);

                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                byte[] bytes = memoryStream.ToArray();
                xmlizedString = encoding.GetString(bytes, 0, bytes.Length);//ByteArrayUtil.BytesToString(memoryStream.ToArray(), encoding);
                return xmlizedString;
            }
            catch (Exception ex)
            {
            }

            return string.Empty;
        }

        private int SaveChanges_Helper(int saveType, bool acceptChangesDuringSave, SaveOptions options, int secondsTimeOut = 180)
        {
            int num = 0;
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                    TimeSpan.FromSeconds(secondsTimeOut)))
            {
                num = SaveChanges_HelperWithoutTransactionScope(saveType, acceptChangesDuringSave, options);


                //hoàn tất giao tác
                transaction.Complete();
            }
            return num;
        }

        private int SaveChanges_HelperWithoutTransactionScope(int saveType, bool acceptChangesDuringSave, SaveOptions options)
        {
            int num = 0;
            //lưu thay đổi
            if (saveType == 1)
                num = base.SaveChanges();
            else if (saveType == 2)
                num = base.SaveChanges(acceptChangesDuringSave: acceptChangesDuringSave);
            else if (saveType == 3)
                num = base.SaveChanges(options);
            //This persists the object to the DB.  After this call, if it's successful, it will process the item.
            return num;
        }

        private bool CheckIsDirty_Helper()
        {

            IEnumerable<ObjectStateEntry> addedEntries = base.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            if (addedEntries.Any())
                return true;
            IEnumerable<ObjectStateEntry> deletedEntries = base.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted);
            if (deletedEntries.Any())
                return true;
            IEnumerable<ObjectStateEntry> modifiedEntries = base.ObjectStateManager.GetObjectStateEntries(EntityState.Modified);
            //duyệt modifiedEntries để lấy giá trị trước khi thay đổi
            foreach (ObjectStateEntry entry in modifiedEntries)
            {
                //lấy danh sách tên property đã bị thay đổi giá trị
                IEnumerable<string> modifiedProperties = entry.GetModifiedProperties();
                //duyệt qua từng property name có giá trị bị thay đổi
                if ((from propertyName in modifiedProperties
                     let oldValue = entry.OriginalValues[propertyName]
                     let newValue = entry.CurrentValues[propertyName]
                     where Object.Equals(oldValue, newValue) == false
                     select oldValue).Any())
                {
                    return true;
                }
            }
            return false;
        }//end fn
        //private static void WriteTracking(EntityTrackingTool trackingLog, EntityTracking item)
        //{
        //    trackingLog.EntityTracking(item.BusinessCode, item.Message, UserId, item.EntityKeys, item.CurrentPropertyValues, item.OldPropertyValues, item.NewPropertyValues, item.LogType, item.EntitySet, item.ObjectSet, item.UserNetworkIp);


        //}
    }
}
