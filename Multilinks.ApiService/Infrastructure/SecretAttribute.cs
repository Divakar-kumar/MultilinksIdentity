using System;

namespace Multilinks.ApiService.Infrastructure
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class SecretAttribute : Attribute
   {
   }
}
