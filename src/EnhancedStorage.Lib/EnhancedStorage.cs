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
        public RetrievedItem Retrieve(Guid itemId)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ValidateItemId(itemId);

            BasicDataItem dataItem = RetrieveItem(itemId);

            var retrievedItem = MapDataItemToRetrievedItem(dataItem);
            retrievedItem.RetrievalTime = sw.ElapsedMilliseconds;

            return retrievedItem;
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
