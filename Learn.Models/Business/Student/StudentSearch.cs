using Learn.Models.Common;
using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    [Serializable]
    public class StudentSearch : BaseSearchModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

    }
    
}
