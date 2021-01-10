using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Common
{
    public class BaseResultModel<T> where T : BaseModel
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public long total { get; set; }
        public object rows { get; set; }

        public BaseResultModel()
        {
            this.total = 0;
            this.rows = new List<T>();
            success = false;
            msg = string.Empty;
        }
    }
}
