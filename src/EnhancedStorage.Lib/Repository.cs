using EnhancedStorage.Lib.Exceptions;
using EnhancedStorage.Lib.Interfaces;
using Exchange.EntLib.DataStorage;
using Exchange.EntLib.DataStorage.Entities;
using Exchange.EntLib.DataStorage.Exceptions;
using Exchange.EntLib.Logging;
using System;

namespace EnhancedStorage.Lib
{
    internal class Repository : IRepository
    {
        public object GetData(Guid dataItemId)
        {
            try
            {
                return DataStorageLibrary.GetData(dataItemId).Data;
            }
            catch (DataItemNotFoundException)
            {
                throw new StoredItemNotFoundException();
            }
        }

        public void WriteToLog(LogEntry logEntry)
        {
            Logger.Write(logEntry);
        }
    }
}
