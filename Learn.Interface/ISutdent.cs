using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    public interface ISutdent
    {
        Task<IEnumerable<Learn.Models.Entity.Student>> GetStudentsAsync(string name = "");
    }
}
