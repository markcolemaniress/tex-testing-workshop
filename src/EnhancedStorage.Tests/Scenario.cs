using EnhancedStorage.Lib.Exceptions;
using EnhancedStorage.Lib.Interfaces;
using Exchange.EntLib.Logging;
using Exchange.TestHelpers.ObjectBuilder;
using Moq;
using System;
using System.Threading;

namespace EnhancedStorage.Tests
{
    internal class Scenario : Scenario<Lib.EnhancedStorage, Scenario>
    {
        internal Scenario WithMissingStorageItem()
        {
            this.SetupMock<IRepository>(mock =>
            {
                mock.Setup(m => m.GetData(It.IsAny<Guid>()))
                    .Throws(new StoredItemNotFoundException());
            });

            return this;
        }

        internal Scenario WithStorageItem(string data)
        {
            this.SetupMock<IRepository>(mock =>
            {
                mock.Setup(m => m.GetData(It.IsAny<Guid>()))
                    .Returns(() =>
                            {
                                Thread.Sleep(2);
                                return data;
                            });

            });

            return this;
        }

        internal Scenario WithLogging()
        {
            this.SetupMock<IRepository>(mock =>
            {
                mock.Setup(m => m.WriteToLog(It.IsAny<LogEntry>()));
            });

            return this;
        }

        internal void VerifyStoredItem(Guid itemId)
        {
            this.GetMock<IRepository>()
                .Verify(m => m.GetData(It.Is<Guid>(g => g == itemId)), 
                Times.Once());
        }

        internal void VerifyLog(string title = "EnhancedStorageLib Access")
        {
            this.GetMock<IRepository>()
                .Verify(m => m.WriteToLog(It.Is<LogEntry>(l => l.Title == title)), 
                Times.Once());
        }

        internal void VerifyNotLogged()
        {
            this.GetMock<IRepository>()
                .Verify(m => m.WriteToLog(It.IsAny<LogEntry>()), 
                Times.Never());
        }
    }
}
