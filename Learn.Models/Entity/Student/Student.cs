using Learn.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Entity
{
    public class Student :BaseModel
    {
        public string StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }
    }
}
