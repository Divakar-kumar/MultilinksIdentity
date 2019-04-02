using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Multilinks.ApiService.Models
{
   public class EndpointLinkViewModel : Resource, IEtaggable
   {
      public Guid LinkId { get; set; }

      public string SourceDeviceName { get; set; }

      public string SourceDeviceOwnerName { get; set; }

      public string AssociatedDeviceName { get; set; }

      public string AssociatedDeviceOwnerName { get; set; }

      public bool Confirmed { get; set; }

      public bool IsActive { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
