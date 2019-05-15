using Exchange.EntLib.DataStorage.Entities;
using Exchange.EntLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStorage.Lib.Interfaces
{
    public interface IRepository
    {
        void WriteToLog(LogEntry logEntry);

        object GetData(Guid dataItemId);
    }
}
