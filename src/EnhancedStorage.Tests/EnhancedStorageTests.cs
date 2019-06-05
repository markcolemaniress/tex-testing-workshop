using System;
using System.Threading;
using EnhancedStorage.Lib.Exceptions;
using EnhancedStorage.Lib.Interfaces;
using Exchange.EntLib.DataStorage.Exceptions;
using Exchange.EntLib.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace EnhancedStorage.Tests
{
    [TestClass]
    public class EnhancedStorageTests
    {
        [TestMethod]
        public void Retrieve_ItemIdIsEmpty_ThrowsArgumentException()
        {
            Should.Throw<ArgumentException>(() => new Lib.EnhancedStorage().Retrieve(Guid.Empty));
        }

        [TestMethod]
        public void Retrieve_ItemNotFound_ThrowsStoredItemNotFoundException()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetData(It.IsAny<Guid>()))
                    .Throws(new StoredItemNotFoundException());

            Should.Throw<StoredItemNotFoundException>(
                () => new Lib.EnhancedStorage(mockRepo.Object).Retrieve(Guid.NewGuid()));
        }

        [TestMethod]
        public void Retrieve_ItemIsFound_ReturnsRetrievedItem()
        {
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetData(It.IsAny<Guid>())).Returns(
                () => 
                {
                    Thread.Sleep(2);
                    return DATA;
                });

            var retrievedItem = new Lib.EnhancedStorage(mockRepo.Object).Retrieve(itemId);

            retrievedItem.ShouldNotBeNull();
            retrievedItem.Data.ShouldBe(DATA);
            retrievedItem.RetrievalTime.ShouldBeGreaterThan(0);

            mockRepo.Verify(m => m.GetData(It.Is<Guid>(g => g.Equals(itemId))), Times.Once());
        }

        [TestMethod]
        public void Retrieve_LoggingNotSpecified_DoesNotLogRequest()
        {
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetData(It.IsAny<Guid>())).Returns(DATA);
            mockRepo.Setup(m => m.WriteToLog(It.IsAny<LogEntry>()));

            var retrievedItem = new Lib.EnhancedStorage(mockRepo.Object).Retrieve(itemId);

            retrievedItem.Data.ShouldBe(DATA);

            mockRepo.Verify(m => m.GetData(It.Is<Guid>(g => g.Equals(itemId))), Times.Once());
            mockRepo.Verify(m => m.WriteToLog(It.IsAny<LogEntry>()), Times.Never());
        }

        [TestMethod]
        public void Retrieve_LoggingNotRequired_DoesNotLogRequest()
        {
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetData(It.IsAny<Guid>())).Returns(DATA);
            mockRepo.Setup(m => m.WriteToLog(It.IsAny<LogEntry>()));

            var retrievedItem = new Lib.EnhancedStorage(mockRepo.Object).Retrieve(itemId, false);

            retrievedItem.Data.ShouldNotBeNull();

            retrievedItem.Data.ShouldBe(DATA);

            mockRepo.Verify(m => m.GetData(It.Is<Guid>(g => g.Equals(itemId))), Times.Once());
            mockRepo.Verify(m => m.WriteToLog(It.IsAny<LogEntry>()), Times.Never());
        }

        [TestMethod]
        public void Retrieve_LoggingRequired_LogsRequest()
        {
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetData(It.IsAny<Guid>())).Returns(DATA);
            mockRepo.Setup(m => m.WriteToLog(It.IsAny<LogEntry>()));

            var retrievedItem = new Lib.EnhancedStorage(mockRepo.Object).Retrieve(itemId, true);

            retrievedItem.Data.ShouldNotBeNull();

            mockRepo.Verify(m => m.WriteToLog(It.Is<LogEntry>(l => l.Title == "EnhancedStorageLib Access")), Times.Once());
        }
    }
}
