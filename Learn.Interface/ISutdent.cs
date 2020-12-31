using Learn.Models.Business;
using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    public interface ISutdent
    {
        IEnumerable<Student> GetStudents(StudentSearch studentSearch);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IEnumerable<Student>> GetStudentsAsync(StudentSearch studentSearch);


    }
}
