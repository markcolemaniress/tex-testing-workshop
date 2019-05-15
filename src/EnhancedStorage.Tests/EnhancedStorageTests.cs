using EnhancedStorage.Lib.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

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
            var scenario = new Scenario().WithMissingStorageItem();
            var target = scenario.Build();

            Should.Throw<StoredItemNotFoundException>(() => target.Retrieve(Guid.NewGuid()));
        }

        [TestMethod]
        public void Retrieve_ItemIsFound_ReturnsRetrievedItem()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            var scenario = new Scenario().WithStorageItem(DATA);
            var target = scenario.Build();

            // Act
            var retrievedItem = target.Retrieve(itemId);

            // Assert
            retrievedItem.ShouldNotBeNull();
            retrievedItem.Data.ShouldBe(DATA);
            retrievedItem.RetrievalTime.ShouldBeGreaterThan(0);
            scenario.VerifyStoredItem(itemId);
        }

        [TestMethod]
        public void Retrieve_LoggingNotSpecified_DoesNotLogRequest()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            var scenario = new Scenario().WithStorageItem(DATA)
                                        .WithLogging();
            var target = scenario.Build();

            // Act
            var retrievedItem = target.Retrieve(itemId);

            // Assert
            retrievedItem.Data.ShouldBe(DATA);
            scenario.VerifyStoredItem(itemId);
            scenario.VerifyNotLogged();
        }

        [TestMethod]
        public void Retrieve_LoggingNotRequired_DoesNotLogRequest()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            var scenario = new Scenario().WithStorageItem(DATA)
                                        .WithLogging();
            var target = scenario.Build();

            // Act
            var retrievedItem = target.Retrieve(itemId, false);

            // Assert
            retrievedItem.Data.ShouldNotBeNull();
            retrievedItem.Data.ShouldBe(DATA);
            scenario.VerifyStoredItem(itemId);
            scenario.VerifyNotLogged();
        }

        [TestMethod]
        public void Retrieve_LoggingRequired_LogsRequest()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            const string DATA = "My data";

            var scenario = new Scenario().WithStorageItem(DATA)
                                        .WithLogging();
            var target = scenario.Build();

            // Act
            var retrievedItem = target.Retrieve(itemId, true);

            // Assert
            retrievedItem.Data.ShouldNotBeNull();
            retrievedItem.Data.ShouldBe(DATA);
            scenario.VerifyStoredItem(itemId);
            scenario.VerifyLog();
        }
    }
}
