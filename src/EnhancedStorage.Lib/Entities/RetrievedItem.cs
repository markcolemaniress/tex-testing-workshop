using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStorage.Lib.Entities
{
    public class RetrievedItem : StoredItem
    {
        public long RetrievalTime { get; set; }
    }
}
