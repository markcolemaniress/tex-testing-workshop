using System;
using EnhancedStorage.Lib.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Should.Throw<StoredItemNotFoundException>(() => new Lib.EnhancedStorage().Retrieve(Guid.NewGuid()));
        }

        [TestMethod]
        public void Retrieve_ItemIsFound_ReturnsRetrievedItem()
        {
            Guid itemId = new Guid("8BC1A026-DA1E-490C-ABDF-01AB694BC706");

            var retrievedItem = new Lib.EnhancedStorage().Retrieve(itemId);

            retrievedItem.ShouldNotBeNull();
            retrievedItem.Data.ShouldNotBeNull();
            retrievedItem.RetrievalTime.ShouldBeGreaterThan(1000000);
        }
    }
}
