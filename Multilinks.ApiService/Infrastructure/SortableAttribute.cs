using System;

namespace Multilinks.ApiService.Infrastructure
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class SortableAttribute : Attribute
   {
      public string EntityProperty { get; set; }

      public bool Default { get; set; }
   }
}
