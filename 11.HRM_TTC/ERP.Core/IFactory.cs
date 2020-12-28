using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Core
{
    public interface IFactory
    {
        Boolean IsDirty { get; }
        BaseEntityObject CreateManagedBaseEntityObject();
        void RefreshAll(RefreshMode refreshMode);
        void SaveChanges(int secondsTimeOut = 180);
    }
}
