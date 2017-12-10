/*************************************************************************************************
 *    Class:      ApplicationUser (Entity)
 * 
 *    Summary:    The responsibilities of an ApplicationUser is to create endpoints and to create
 *                links between endpoints. If an endpoint that the ApplicationUser is trying to
 *                link to belongs to another ApplicationUser, then acknowledgement/approval is
 *                required.
 *                An ApplicationUser will use an endpoint they have access to to communicate with
 *                another endpoint.
 *             
 *    Property:   - An ApplicationUser will have an Id (AUID), AUID is unique within the system.
 *                - An ApplicationUser will have a Firstname
 *                - An ApplicationUser will have a Lastname
 *                - An ApplicationUser will have an Email address (exists in IdentityUser)
 *                - An ApplicationUser will have a StartDate
 *                - An ApplicationUser will have an EndDate
 *************************************************************************************************/

using Microsoft.AspNetCore.Identity;
using System;

namespace Multilinks.TokenService.Models
{
   // Add profile data for application users by adding properties to the ApplicationUser class
   public class ApplicationUser : IdentityUser
   {
      public Guid ApplicationUserId { get; set; }

      public string Firstname { get; set; }

      public string Lastname { get; set; }

      public DateTimeOffset StartDate { get; set; }

      public DateTimeOffset EndDate { get; set; }
   }
}
