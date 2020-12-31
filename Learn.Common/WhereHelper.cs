using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
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
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
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
    }
}
