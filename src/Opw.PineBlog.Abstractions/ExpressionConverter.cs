//using System;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Opw.PineBlog
//{
//    public class ExpressionConverter<TTSource, TTarget> : ExpressionVisitor
//    {
//        public Expression<Func<TTarget, bool>> Convert(Expression<Func<TTSource, bool>> expression)
//        {
//            return (Expression<Func<TTarget, bool>>)base.Visit(expression);
//        }

//        protected override Expression VisitLambda<T>(Expression<T> node)
//        {
//            if (node.Parameters.All(p => p.Type != typeof(TTSource)))
//                return node;

//            var substituteParameters = node.Parameters.Select(p =>
//            {
//                return (ParameterExpression)VisitParameter(p);
//            });

//            var substitutedParameters = new ReadOnlyCollection<ParameterExpression>(substituteParameters.ToList());

//            var updatedBody = Visit(node.Body); // which will convert parameters to 'to'
//            return Expression.Lambda<Func<TTarget, bool>>(updatedBody, substitutedParameters);
//        }

//        protected override Expression VisitParameter(ParameterExpression node)
//        {
//            if (node.Type == typeof(TTSource))
//            {
//                return Expression.Parameter(typeof(TTarget), node.Name);
//            }

//            return base.VisitParameter(node);
//        }

//        protected override Expression VisitMember(MemberExpression node)
//        {
//            if (node.Expression.Type == typeof(TTSource))
//            {
//                var memberInfo = typeof(TTarget).GetProperty(node.Member.Name);
//                if (memberInfo != null)
//                {
//                    var parameterExp = VisitParameter((ParameterExpression)node.Expression);
//                    var x = Expression.MakeMemberAccess(parameterExp, memberInfo);
//                    return x;
//                }
//            }

//            return base.VisitMember(node);
//        }
//    }
//}
