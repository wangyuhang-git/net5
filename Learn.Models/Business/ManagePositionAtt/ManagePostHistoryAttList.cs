using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    [Serializable]
    public class ManagePostHistoryAttList
    {
        public List<ManagePostHistoryAtt> HistoryAtts { get; set; }
    }
}
