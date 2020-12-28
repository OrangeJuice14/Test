using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Core
{
    public interface IService
    {
        Boolean IsDirty { get; }
        Boolean IsValid { get; }
        Boolean SaveChanges();
        void RefreshAll(RefreshMode refreshMode);

    }
}
