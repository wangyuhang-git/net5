using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    public interface IManagePostAtt
    {

        Task<IEnumerable<ManagePostAtt>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic);
    }
}
