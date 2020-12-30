using Learn.Interface;
using Learn.Models.Business;
using Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Business.Student
{
    public class StudentBusiness : ISutdent
    {
        public async Task<IEnumerable<Models.Entity.Student>> GetStudentsAsync(string name = "")
        {
            MongodbService<Learn.Models.Entity.Student, StudentSearch> service = new MongodbService<Learn.Models.Entity.Student, StudentSearch>("student");
            //return await service.GetAsync();
            return await service.GetListAsync(c => string.IsNullOrEmpty(name) || c.FirstName == name);
        }
    }
}
