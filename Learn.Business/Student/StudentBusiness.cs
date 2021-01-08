using Learn.Common;
using Learn.Interface;
using Learn.Models.Business;
using Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Learn.Models.Entity;

namespace Learn.Business.Student
{
    public class StudentBusiness : IStudent
    {
        MongodbService<Learn.Models.Entity.Student> service = new MongodbService<Learn.Models.Entity.Student>("student");

        WhereHelper<Models.Entity.Student> where = new WhereHelper<Models.Entity.Student>();

        public IEnumerable<Models.Entity.Student> GetStudents(StudentSearch search)
        {
            return service.GetList(c => string.IsNullOrEmpty(search.Name) || c.FirstName == search.Name);
        }

        public async Task<IEnumerable<Models.Entity.Student>> GetStudentsAsync(StudentSearch search)
        {
            //Expression<Func<Models.Entity.Student, bool>> expression = c =>
            //     string.IsNullOrEmpty(search.Name)
            //     || c.FirstName == search.Name;
            Expression<Func<Models.Entity.Student, bool>> expression = null;
            if (null != search)
            {
                Models.Entity.Student student = new Models.Entity.Student();
                where.Equal("FirstName", search.Name, "and");
                where.LessThanOrEqual(nameof(student.Birthday), search.Birthday, "and");
                expression = where.GetExpression();
            }
            return await service.GetListAsync(expression);
        }

        public async Task<IEnumerable<Models.Entity.Student>> GetPageStudentsAsync(int pageIndex, int pageSize, Dictionary<string, string> sortDic, StudentSearch search)
        {
            Expression<Func<Models.Entity.Student, bool>> expression = null;
            if (null != search)
            {
                Models.Entity.Student student = new Models.Entity.Student();
                where.Equal("FirstName", search.Name, "and");
                where.LessThanOrEqual(nameof(student.Birthday), search.Birthday, "and");
                expression = where.GetExpression();
            }
            return await service.GetPageListAsync(pageIndex, pageSize, sortDic, expression);
        }

        public void AddMany(IEnumerable<Learn.Models.Entity.Student> students)
        {
            service.AddMany(students);
        }

        public async Task AddManyAsync(IEnumerable<Learn.Models.Entity.Student> students)
        {
            await service.AddManyAsync(students);
        }

    }
}
