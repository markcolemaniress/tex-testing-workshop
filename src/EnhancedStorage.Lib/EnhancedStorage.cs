using EnhancedStorage.Lib.Entities;
using EnhancedStorage.Lib.Exceptions;
using EnhancedStorage.Lib.Interfaces;
using Exchange.EntLib.DataStorage.Entities;
using Exchange.EntLib.DataStorage.Exceptions;
using Exchange.EntLib.Logging;
using System;
using System.Diagnostics;

namespace EnhancedStorage.Lib
{
    public class EnhancedStorage
    {
        private readonly IRepository repository;

        public EnhancedStorage(IRepository repository = null)
        {
            this.repository = (repository == null ? new Repository() : repository);
        }

        public RetrievedItem Retrieve(Guid itemId, bool logRequest = false)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ValidateItemId(itemId);

            if (logRequest)
            {
                LogAccessRequest(itemId);
            }

            return new RetrievedItem()
            {
                Data = RetrieveItem(itemId),
                RetrievalTime = sw.ElapsedMilliseconds
            };
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

            this.repository.WriteToLog(logEntry);
        }

        private object RetrieveItem(Guid itemId)
        {
            return this.repository.GetData(itemId);
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
