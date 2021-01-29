﻿using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    /// <summary>
    /// 管理岗位考勤数据[规则内的数据]【作废，改用了泛型接口】
    /// </summary>
    public interface IManagePostAtt<T, S> : IBaseManagePostAtt<T, S>
        where T : BaseModel
        where S : BaseModel
    {
        /// <summary>
        /// 新增多条数据[异步]
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddManyAsync(IEnumerable<T> list);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="sortDic">排序字典值</param>
        /// <param name="searchDic">查询条件字典值</param>
        /// <returns></returns>
        Task<BaseResultModel<T>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<BaseResultModel<T>> GetPageManagePostAtt(ManagePostAttPageSearch search);
        /// <summary>
        /// 获取管理人员考勤统计数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ManagePostAttStatistics> GetManagePostStatistics(ManagePostAttPageSearch search);

    }
}
