using System;

namespace Multilinks.ApiService.Infrastructure
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class SearchableDecimalAttribute : SearchableAttribute
   {
      public SearchableDecimalAttribute()
      {
         ExpressionProvider = new DecimalToIntSearchExpressionProvider();
      }
   }
}
