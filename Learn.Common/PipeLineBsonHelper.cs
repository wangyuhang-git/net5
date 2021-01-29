using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Common
{
    public class PipeLineBsonHelper
    {

        public void AddMatchList(Dictionary<string, string> searchDic, List<string> pipeLinelist)
        {
            if (null != searchDic && searchDic.Count > 0)
            {
                string match = string.Empty;
                foreach (var item in searchDic)
                {
                    string parmeName = string.Empty; //要查询的字段
                    string parmeValue = item.Value;//要查询的值

                    if (item.Key.StartsWith("s_0_"))//模糊查询
                    {
                        parmeName = item.Key.Replace("s_0_", "");//获取字段名字
                        string[] parmeArr = parmeName.Split(new char[] { '|' });//多个字段模糊查询 用 or
                        if (parmeArr.Length > 0)
                        {
                            int i = 0;
                            match = "{$match:{$or:[";
                            foreach (string str in parmeArr)
                            {
                                match += "{'" + str + "':{$regex:'" + parmeValue + "'}}";
                                if (i < parmeArr.Length - 1)
                                {
                                    match += ",";
                                }
                                i++;
                            }
                            match += "]}}";
                            pipeLinelist.Add(match);
                        }
                    }
                    else if (item.Key.StartsWith("s_1_"))//精确查询
                    {
                        parmeName = item.Key.Replace("s_1_", "");//获取字段名字
                        match = "{ $match : {'" + parmeName + "':'" + item.Value + "'}}";
                        pipeLinelist.Add(match);
                    }
                    else if (item.Key.StartsWith("s_3_"))//时间段查询
                    {
                        parmeName = item.Key.Replace("s_3_", "");//获取字段名字
                        string[] valueArr = item.Value.Split(new char[] { ',' });
                        if (valueArr.Length == 2)
                        {
                            DateTime dateTime;
                            if (!string.IsNullOrEmpty(valueArr[0]))
                            {
                                if (DateTime.TryParse(valueArr[0], out dateTime))
                                {
                                    match = "{ $match : {'" + parmeName + "':{$gte:new Date('" + dateTime + "')}}}";
                                    pipeLinelist.Add(match);
                                }
                                else
                                {
                                    throw new ArgumentException("根据时间段查询条件的开始日期格式不正确！");
                                }
                            }
                            if (!string.IsNullOrEmpty(valueArr[1]))
                            {
                                if (DateTime.TryParse(valueArr[1], out dateTime))
                                {
                                    match = "{ $match : {'" + parmeName + "':{$lte:new Date('" + dateTime + "')}}}";
                                    pipeLinelist.Add(match);
                                }
                                else
                                {
                                    throw new ArgumentException("根据时间段查询条件的结束日期格式不正确！");
                                }
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException("根据时间段查询条件的格式不正确！");
                        }
                    }
                }
            }
        }

        public void DeleteMatch(List<string> pipeLinelist, string key)
        {
            if (pipeLinelist.Contains(key))
            {
                pipeLinelist.Remove(key);
            }
        }

        public void AddSortList(Dictionary<string, string> sortDic, List<string> pipeLinelist)
        {
            if (null != sortDic && sortDic.Count > 0)
            {
                string sort = string.Empty;
                foreach (var item in sortDic)
                {
                    if (item.Value == "d")
                    {
                        sort = "{ $sort:{'" + item.Key + "':-1}}";
                        pipeLinelist.Add(sort);
                    }
                    else
                    {
                        sort = "{ $sort:{'" + item.Key + "':1}}";
                        pipeLinelist.Add(sort);
                    }
                }
            }
        }
    }
}
