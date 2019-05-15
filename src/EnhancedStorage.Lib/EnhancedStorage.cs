using EnhancedStorage.Lib.Entities;
using EnhancedStorage.Lib.Exceptions;
using Exchange.EntLib.DataStorage;
using Exchange.EntLib.DataStorage.Entities;
using Exchange.EntLib.DataStorage.Exceptions;
using Exchange.EntLib.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStorage.Lib
{
    public class EnhancedStorage
    {
        public RetrievedItem Retrieve(Guid itemId, bool logRequest = false)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ValidateItemId(itemId);

            if (logRequest)
            {
                LogAccessRequest(itemId);
            }

            BasicDataItem dataItem = RetrieveItem(itemId);

            var retrievedItem = MapDataItemToRetrievedItem(dataItem);
            retrievedItem.RetrievalTime = sw.ElapsedMilliseconds;

            return retrievedItem;
        }

        private void LogAccessRequest(Guid itemId)
        {
            LogEntry logEntry = new LogEntry()
            {
                CreatedBy = "EnhancedStorageLib",
                EventId = 361,
                Message = $"Access to storage item {itemId} requested at {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.",
                Title = "EnhancedStorageLib Access",
                Severity = TraceEventType.Warning
            };

            Logger.Write(logEntry);
        }

        private static BasicDataItem RetrieveItem(Guid itemId)
        {
            BasicDataItem dataItem;
            try
            {
                dataItem = DataStorageLibrary.GetData(itemId);
            }
            catch (DataItemNotFoundException)
            {
                throw new StoredItemNotFoundException();
            }

            return dataItem;
        }

        private static RetrievedItem MapDataItemToRetrievedItem(BasicDataItem dataItem)
        {
            return new RetrievedItem()
            {
                ItemId = dataItem.DataItemId,
                Data = dataItem.Data
            };
        }

        private static void ValidateItemId(Guid itemId)
        {
            if (itemId.Equals(Guid.Empty))
            {
                throw new ArgumentException(nameof(itemId));
            }
        }
    }
}
