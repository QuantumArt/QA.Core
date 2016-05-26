// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QA.Core.Linq
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Получение имени свойства по lambda-выражению
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            // Retrieve member path:
            List<MemberInfo> members = new List<MemberInfo>();
            CollectRelationalMembers(expression, members);

            // Build string path:
            StringBuilder sb = new StringBuilder();
            string separator = "";
            foreach (MemberInfo member in members)
            {
                sb.Append(separator);
                sb.Append(member.Name);
                separator = ".";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Разбор lambda-выражения
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="members"></param>
        private static void CollectRelationalMembers(Expression exp, IList<MemberInfo> members)
        {
            if (exp.NodeType == ExpressionType.Lambda)
            {
                // At root, explore body:
                CollectRelationalMembers(((LambdaExpression)exp).Body, members);
            }
            else if (exp.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression mexp = (MemberExpression)exp;
                CollectRelationalMembers(mexp.Expression, members);
                members.Add((MemberInfo)mexp.Member);
            }
            else if (exp.NodeType == ExpressionType.Call)
            {
                MethodCallExpression cexp = (MethodCallExpression)exp;

                if (cexp.Method.IsStatic == false)
                {
                    members.Add(cexp.Method);
                }
                else
                {
                    foreach (var arg in cexp.Arguments)
                        CollectRelationalMembers(arg, members);
                }
            }
            else if (exp.NodeType == ExpressionType.Parameter)
            {
                // Reached the toplevel:
                return;
            }
            else if (exp.NodeType == ExpressionType.Constant)
            {
                return;
            }
            else
            {
                throw new InvalidOperationException("Invalid type of expression.");
            }
        }
    }
}
