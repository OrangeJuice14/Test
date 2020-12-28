using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace HRMWebApp.KPI.DB
{
    public static class SessionManager
    {
        private static readonly ISessionFactory SessionFactory;

        static SessionManager()
        {
            Configuration configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(SessionManager).Assembly);
            SessionFactory = configuration.BuildSessionFactory();
        }
        public static ISession Session
        {
            get
            {
                return SessionFactory.OpenSession();
            }
        }

        public static void DoWork(Action<ISession> work)
        {
            using (ISession session = SessionManager.Session)
            {
                using (ITransaction trans = session.BeginTransaction())
                {
                    work.Invoke(session);
                    trans.Commit();
                }
            }
        }
        public static void DoWorkNoTransaction(Action<ISession> work)
        {
            using (ISession session = SessionManager.Session)
            {
                work.Invoke(session);
            }
        }
    }
}
