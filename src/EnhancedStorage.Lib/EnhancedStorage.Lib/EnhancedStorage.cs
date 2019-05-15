using EnhancedStorage.Lib.Entities;
using EnhancedStorage.Lib.Exceptions;
using Exchange.EntLib.DataStorage;
using Exchange.EntLib.DataStorage.Entities;
using Exchange.EntLib.DataStorage.Exceptions;
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

            BasicDataItem dataItem;
            try
            {
                dataItem = DataStorageLibrary.GetData(itemId);
            }
            catch (DataItemNotFoundException)
            {
                throw new StoredItemNotFoundException();
            }


            var retrievedItem = MapDataItemToRetrievedItem(dataItem);
            retrievedItem.RetrievalTime = sw.ElapsedMilliseconds;

            return retrievedItem;
        }

        private RetrievedItem MapDataItemToRetrievedItem(BasicDataItem dataItem)
        {
            return new RetrievedItem()
            {
                ItemId = dataItem.DataItemId,
                Data = dataItem.Data
            };
        }

        private void ValidateItemId(Guid itemId)
        {
            if (itemId.Equals(Guid.Empty))
            {
                throw new ArgumentException(nameof(itemId));
            }
        }
    }
}
