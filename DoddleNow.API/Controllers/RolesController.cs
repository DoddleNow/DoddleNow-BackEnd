using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer;

namespace DoddleNow.API.Controllers
{
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        [AllowAnonymous]
        [Route("All")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(Roles.GetAllRoles());
        }

        [Authorize]
        [Route("User/{userId}")]
        public IHttpActionResult GetUserRoles(string userId)
        {
            return Ok(Roles.GetUserRoles(userId));
        }
    }


    #region Helpers

    public class Roles
    {
        public static List<usp_GetRolesResult> GetAllRoles()
        {
            DataAccess da = new DataAccess();
            List<usp_GetRolesResult> roles = da.GetRoles(string.Empty);

            return roles;
        }

        public static List<usp_GetRolesResult> GetUserRoles(string userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetRolesResult> roles = da.GetRoles(userId);

            return roles;
        }
    }
    #endregion
}
