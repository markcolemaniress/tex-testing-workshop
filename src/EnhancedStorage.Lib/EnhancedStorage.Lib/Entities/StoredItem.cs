using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStorage.Lib.Entities
{
    public class StoredItem
    {
        public Guid ItemId { get; set; }

        public object Data { get; internal set; }
    }
}
