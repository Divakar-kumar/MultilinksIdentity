using System;
using System.Linq.Expressions;

namespace Multilinks.ApiService.Infrastructure
{
   public class BooleanSearchExpressionProvider : SearchExpressionProvider
   {
      public override ConstantExpression GetValue(string input)
      {
         if(!bool.TryParse(input, out var val))
            throw new ArgumentException("Invalid search value");

         return Expression.Constant(val);
      }

      public override Expression GetComparison(MemberExpression left, string op, ConstantExpression right)
      {
         switch(op.ToLower())
         {
            case "eq": return Expression.Equal(left, right);

            // If nothing matches, fall back to base impl
            default: return base.GetComparison(left, op, right);
         }
      }
   }
}

