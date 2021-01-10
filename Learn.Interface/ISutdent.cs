using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    public interface IStudent
    {
        /// <summary>
        /// 获取列表[同步]
        /// </summary>
        /// <param name="studentSearch"></param>
        /// <returns></returns>
        IEnumerable<Student> GetStudents(StudentSearch studentSearch);

        /// <summary>
        /// 获取列表[异步]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IEnumerable<Student>> GetStudentsAsync(StudentSearch studentSearch);

        Task<BaseResultModel<Student>> GetPageStudentsAsync(int pageIndex,int pageSize,Dictionary<string,string> sortDic, StudentSearch studentSearch);

        void AddMany(IEnumerable<Student> students);

        Task AddManyAsync(IEnumerable<Student> students);

    }
}
