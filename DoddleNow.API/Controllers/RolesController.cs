using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///Roles controller.  Used to get role related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        ///<summary>
        ///Get all roles across DoddleNow
        ///</summary>
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(Roles.GetAllRoles());
        }

        ///<summary>
        ///Get roles related to a specific user by UserID
        ///</summary>
        [Authorize(Roles= "1,2")]
        [Route("User/{userId}")]
        public IHttpActionResult GetUserRoles(string userId)
        {
            return Ok(Roles.GetUserRoles(userId));
        }
    }


    #region Helpers

    ///<summary>
    /// Rolese
    ///</summary>
    public class Roles
    {
        ///<summary>
        ///Get all roles across DoddleNow
        ///</summary>
        public static List<usp_GetRolesResult> GetAllRoles()
        {
            DataAccess da = new DataAccess();
            List<usp_GetRolesResult> roles = da.GetRoles(string.Empty);

            return roles;
        }

        ///<summary>
        ///Get roles related to a specific user by UserID
        ///</summary>
        public static List<usp_GetRolesResult> GetUserRoles(string userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetRolesResult> roles = da.GetRoles(userId);

            return roles;
        }
    }
    #endregion
}
