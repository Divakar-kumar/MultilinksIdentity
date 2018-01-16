using Microsoft.AspNetCore.Mvc.Filters;
using Multilinks.ApiService.Filters;
using System;

namespace Multilinks.ApiService.Infrastructure
{
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class EtagAttribute : Attribute, IFilterFactory
   {
      public bool IsReusable => true;

      public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
          => new EtagHeaderFilter();
   }
}
