using System;
using EnhancedStorage.Lib.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnhancedStorage.Tests
{
    [TestClass]
    public class EnhancedStorageTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Retrieve_ItemIdIsEmpty_ThrowsArgumentException()
        {
            new Lib.EnhancedStorage().Retrieve(Guid.Empty);
        }

        [ExpectedException(typeof(StoredItemNotFoundException))]
        [TestMethod]
        public void Retrieve_ItemNotFound_ThrowsStoredItemNotFoundException()
        {
            new Lib.EnhancedStorage().Retrieve(Guid.NewGuid());
        }

        [TestMethod]
        public void Retrieve_ItemIsFound_ReturnsRetrievedItem()
        {
            Guid itemId = new Guid("8BC1A026-DA1E-490C-ABDF-01AB694BC706");

            var retrievedItem = new Lib.EnhancedStorage().Retrieve(itemId);

            Assert.IsNotNull(retrievedItem);
            Assert.IsNotNull(retrievedItem.Data);
            Assert.IsTrue(retrievedItem.RetrievalTime > 0);
        }
    }
}
