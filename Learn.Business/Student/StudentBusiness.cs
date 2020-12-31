using Learn.Common;
using Learn.Interface;
using Learn.Models.Business;
using Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Business.Student
{
    public class StudentBusiness : ISutdent
    {
        MongodbService<Learn.Models.Entity.Student, StudentSearch> service = new MongodbService<Learn.Models.Entity.Student, StudentSearch>("student");

        WhereHelper<Models.Entity.Student> user = new WhereHelper<Models.Entity.Student>();

        public IEnumerable<Models.Entity.Student> GetStudents(StudentSearch search)
        {
            return service.GetList(c => string.IsNullOrEmpty(search.Name) || $"{c.FirstName}{c.LastName}" == search.Name);
        }

        public async Task<IEnumerable<Models.Entity.Student>> GetStudentsAsync(StudentSearch search)
        {
            //Expression<Func<Models.Entity.Student, bool>> expression = c =>
            //     string.IsNullOrEmpty(search.Name)
            //     || c.FirstName == search.Name;
            Models.Entity.Student student = new Models.Entity.Student();
            user.Equal("FirstName", search.Name, "and");
            user.LessThanOrEqual(nameof(student.Birthday), search.Birthday, "and");
            user.GetExpression();
            return await service.GetListAsync(user.GetExpression());
        }

    }
}
