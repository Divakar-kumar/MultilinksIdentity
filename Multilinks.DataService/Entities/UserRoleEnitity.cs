/*************************************************************************************************
 *    Class:      ApplicationRole (Entity)
 * 
 *    Summary:    The role the ApplicationUser can be in. ApplicationRole is used to describe
 *                different users like administrator.
 *             
 *    Property:   - An ApplicationUser will have an Id (AUID), AUID is unique within the system.
 *                - An ApplicationUser will have a Firstname
 *                - An ApplicationUser will have a Lastname
 *                - An ApplicationUser will have an Email address (exists in IdentityUser)
 *                - An ApplicationUser will have a StartDate
 *                - An ApplicationUser will have an EndDate
 *************************************************************************************************/

using Microsoft.AspNetCore.Identity;

namespace Multilinks.DataService.Entities
{
   public class UserRoleEnitity : IdentityRole
   {
      public UserRoleEnitity()
         : base()
      {
      }

      public UserRoleEnitity(string roleName)
         : base(roleName)
      {
      }
   }
}
