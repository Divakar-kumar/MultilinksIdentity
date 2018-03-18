/*************************************************************************************************
 *    Class:      NewEndpointForm
 * 
 *    Summary:    This is the form used in the post request to create a new
 *                endpoint. For more information about endpoints refer to EndpointEntity class.
 *             
 *    Property:   1. A create new endpoint request can be posted from the client.
 *                2. The CID will be the ApplicationUserId of the logged in user.
 * 
 *************************************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class NewEndpointForm
   {
      [Required]
      [Display(Name = "creatorId", Description = "Creator Id")]
      public Guid CreatorId { get; set; }

      [Required]
      [Display(Name = "name", Description = "Name of endpoint")]
      public string Name { get; set; }

      [Required]
      [Display(Name = "description", Description = "Short description of endpoint")]
      public string Description { get; set; }
   }
}
