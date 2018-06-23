/*************************************************************************************************
 *    Class:      UserEntity (Entity)
 * 
 *    Summary:    The responsibilities of an UserEntity is to create endpoints and to create
 *                links between endpoints. If an endpoint that the UserEntity is trying to
 *                link to belongs to another UserEntity, then acknowledgement/approval is
 *                required.
 *                An UserEntity will use an endpoint they have access to to communicate with
 *                another endpoint.
 *             
 *    Property:   - An UserEntity will have an Id (ApplicationUserId), 
 *                  ApplicationUserId is unique within the system.
 *    
 *************************************************************************************************/

using Microsoft.AspNetCore.Identity;
using System;

namespace Multilinks.DataService.Entities
{
   // Add profile data for application users by adding properties to the UserEntity class
   public class UserEntity : IdentityUser<string>
   {
      public Guid ApplicationUserId { get; set; }

      public string Firstname { get; set; }

      public string Lastname { get; set; }

      public DateTimeOffset StartDate { get; set; }

      public DateTimeOffset EndDate { get; set; }

      public bool Deactivated { get; set; }
   }
}
