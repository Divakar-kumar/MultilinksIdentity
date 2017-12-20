using System;

namespace Multilinks.ApiService.Infrastructure
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class SearchableDateTimeAttribute : SearchableAttribute
   {
      public SearchableDateTimeAttribute()
      {
         ExpressionProvider = new DateTimeSearchExpressionProvider();
      }
   }
}
