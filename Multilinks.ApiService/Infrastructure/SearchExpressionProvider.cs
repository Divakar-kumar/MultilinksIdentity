using System;
using System.Linq.Expressions;

namespace Multilinks.ApiService.Infrastructure
{
   public class SearchExpressionProvider : ISearchExpressionProvider
   {
      public virtual Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right)
      {
         if(!op.Equals("eq", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid operator '{op}'.");

         return Expression.Equal(left, right);
      }

      public virtual ConstantExpression GetValue(string input)
          => Expression.Constant(input);
   }
}
