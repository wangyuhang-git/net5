﻿using Learn.Interface;
using Learn.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Business.Student
{
    public class StudentBusinessEx : IStudent
    {
        public void AddMany(IEnumerable<Models.Entity.Student> students)
        {
            throw new NotImplementedException();
        }

        public Task AddManyAsync(IEnumerable<Models.Entity.Student> students)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Entity.Student> GetStudents(StudentSearch studentSearch)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Models.Entity.Student>> GetStudentsAsync(StudentSearch studentSearch)
        {
            throw new NotImplementedException();
        }
    }
}
