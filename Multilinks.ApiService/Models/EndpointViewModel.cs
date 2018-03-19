/*************************************************************************************************
 *    Class:      Endpoint
 * 
 *    Summary:    This is the view model version of EndpointIdentity. It will contains properties
 *                related to EndpointIdentity that is useful to the client.
 *                The EndpointId is used to retreived the info from the data store.
 * 
 *************************************************************************************************/

using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Multilinks.ApiService.Models
{
   public class EndpointViewModel : Resource, IEtaggable
   {
      public Guid EndpointId { get; set; }

      public Guid CreatorId { get; set; }

      [Sortable(Default = true)]
      [Searchable]
      public string Name { get; set; }

      public string Description { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
