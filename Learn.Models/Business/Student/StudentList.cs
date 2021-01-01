using Learn.Models.Entity;
using System;
using System.Collections.Generic;

namespace Learn.Models.Business
{
    [Serializable]
    public class StudentList
    {
        public List<Student> Students { get; set; }
    }
}
