/*************************************************************************************************
 *    Class:      ApplicationRole (Entity)
 * 
 *    Summary:    The role the UserEntity can be in. ApplicationRole is used to describe
 *                different users like administrator.
 *             
 *    Property:   - An UserEntity will have an Id (AUID), AUID is unique within the system.
 *                - An UserEntity will have a Firstname
 *                - An UserEntity will have a Lastname
 *                - An UserEntity will have an Email address (exists in IdentityUser)
 *                - An UserEntity will have a StartDate
 *                - An UserEntity will have an EndDate
 *************************************************************************************************/

using Microsoft.AspNetCore.Identity;
using System;

namespace Multilinks.DataService.Entities
{
   public class UserRoleEntity : IdentityRole<string>
   {
      public UserRoleEntity()
         : base()
      {
      }

      public UserRoleEntity(string roleName)
         : base(roleName)
      {
      }
   }
}
