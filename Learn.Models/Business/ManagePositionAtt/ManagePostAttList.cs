using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    [Serializable]
    public class ManagePostAttList<T> where T: ManagePostAtt
    {
        public List<T> HistoryAtts { get; set; }
    }
}
