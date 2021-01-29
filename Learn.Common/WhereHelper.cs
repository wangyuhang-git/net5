using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Learn.Common
{
    public class WhereHelper<T> where T : class
    {
        private ParameterExpression param;

        private BinaryExpression filter;

        public WhereHelper()
        {
            param = Expression.Parameter(typeof(T), "c");
            //1==1
            Expression left = Expression.Constant(1);
            filter = Expression.Equal(left, left);
        }

        /// <summary>
        /// 格式化成Expression<Func<T, bool>>
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression()
        {
            return Expression.Lambda<Func<T, bool>>(filter, param);
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="value">值</param>
        /// <param name="type">and 还是 or</param>
        public void Equal(string propertyName, object value, string type)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            Expression left = Expression.Property(param, propertyInfo);
            //Expression right = Expression.Constant(value, value.GetType());
            Expression right = null;
            if (propertyInfo.PropertyType == typeof(System.Int32))
            {
                right = Expression.Constant(Convert.ToInt32(value), propertyInfo.PropertyType);
            }
            else
            {
                right = Expression.Constant(value, value.GetType());
            }
            Expression result = Expression.Equal(left, right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);

        }

        /// <summary>
        /// 包含（模糊查询）
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="value">值</param>
        /// <param name="type">and 还是 or</param>
        public void Contains(string propertyName, string value, string type)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            //mondigy on 20210109
            //Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
            //由于netcore中string有多个Contains方法重载，故直接用上面一行代码会报错
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void LessThan(string propertyName, object value, string type)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.LessThan(left, right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void LessThanOrEqual(string propertyName, object value, string type)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.LessThanOrEqual(left, right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void GreaterThan(string propertyName, object value, string type)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.GreaterThan(left, right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);
        }
        /// <summary>
        ///大于等于
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void GreaterThanOrEqual(string propertyName, object value, string type)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.GreaterThanOrEqual(left, right);
            if (type.ToLower() == "and")
                filter = Expression.And(filter, result);
            else
                filter = Expression.Or(filter, result);
        }
        /// <summary>
        /// 根据字典值获取查询条件
        /// </summary>
        /// <param name="searchDic"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> GetExpression(Dictionary<string, string> searchDic)
        {
            Expression<Func<T, bool>> expression = null;
            if (null != searchDic && searchDic.Count > 0)
            {
                foreach (var item in searchDic)
                {
                    string parmeName = string.Empty; //要查询的字段
                    string parmeValue = item.Value;//要查询的值
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        if (item.Key.StartsWith("s_0_"))//模糊查询
                        {
                            parmeName = item.Key.Replace("s_0_", "");//获取字段名字
                            string[] parmeArr = parmeName.Split(new char[] { '|' });//多个字段模糊查询 用 or
                            if (parmeArr.Length > 0)
                            {
                                int _i = 0;
                                foreach (string str in parmeArr)
                                {
                                    if (_i == 0)
                                        this.Contains(str, item.Value, "and");
                                    else
                                        this.Contains(str, item.Value, "or");
                                    _i++;
                                }
                            }
                        }
                        else if (item.Key.StartsWith("s_1_"))//精确查询
                        {
                            parmeName = item.Key.Replace("s_1_", "");//获取字段名字
                            this.Equal(parmeName, item.Value, "and");
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
                                        this.GreaterThanOrEqual(parmeName, dateTime, "and");
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
                                        this.LessThanOrEqual(parmeName, dateTime, "and");
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
                        else if (item.Key.StartsWith("s_4_"))//数字段查询
                        {
                            parmeName = item.Key.Replace("s_4_", "");//获取字段名字
                            string[] valueArr = item.Value.Split(new char[] { ',' });
                            if (valueArr.Length == 2)
                            {
                                double _double;
                                if (!string.IsNullOrEmpty(valueArr[0]))
                                {
                                    if (Double.TryParse(valueArr[0], out _double))
                                    {
                                        this.GreaterThanOrEqual(parmeName, _double, "and");
                                    }
                                    else
                                    {
                                        throw new ArgumentException("根据数字段查询条件的开始数字格式不正确！");
                                    }
                                }
                                if (!string.IsNullOrEmpty(valueArr[1]))
                                {
                                    if (Double.TryParse(valueArr[1], out _double))
                                    {
                                        this.LessThanOrEqual(parmeName, _double, "and");
                                    }
                                    else
                                    {
                                        throw new ArgumentException("根据数字段查询条件的结束数字格式不正确！");
                                    }
                                }
                            }
                            else
                            {
                                throw new ArgumentNullException("根据数字段查询条件的格式不正确！");
                            }
                        }
                    }
                }
                expression = this.GetExpression();
            }
            param = Expression.Parameter(typeof(T), "c");
            //1==1
            Expression left = Expression.Constant(1);
            filter = Expression.Equal(left, left);

            return expression;
        }
    }
}
