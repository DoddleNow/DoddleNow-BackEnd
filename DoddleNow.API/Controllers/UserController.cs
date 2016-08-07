using DataAccessLayer;
using DoddleNow.API.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///All account related functions
    ///</summary>
    [RoutePrefix("api/v1/users")]
    public class UserController : ApiController
    {
        private AuthRepository _repo = null;

        ///<summary>
        /// Account related functions
        ///</summary>
        public UserController()
        {
            _repo = new AuthRepository();
        }


        ///<summary>
        ///Users
        ///</summary>
        /// <remarks>Gets all users </remarks>
        /// <response code="200">An array of users</response>
        /// <response code="0">Unexpected error</response>
        [Authorize(Roles = "1,2")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok(Users.GetAllUsers(null, null));
        }

        ///<summary>
        ///Create new user
        ///</summary>
        [AllowAnonymous]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (user.Password == null)
            {
                return BadRequest("Password is required on POST.");
            }

            IdentityResult result = await _repo.RegisterUser(user);
           
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            else
            {
                //add additional user info to database
                DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
                da.UpdateUser(user.Id, user.RoleID, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            }

            return Ok();
        }

        ///<summary>
        ///Get user with id = id 
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{userId}")]
        [HttpGet]
        public IHttpActionResult GetUser(Guid userId)
        {
            return Ok(Users.GetUser(userId));
        }

        ///<summary>
        ///Update user with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{userId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(Guid userId, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Id = userId;

            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateUser(user.Id, user.RoleID, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            
            return Ok();
        }

        ///<summary>
        ///Delete user with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{userId}")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string userId)
        {
            Users.DeleteUser(userId);
            return Ok();
        }

        

        /////<summary>
        /////Gets all users by role 
        /////</summary>
        //[Authorize(Roles = "1,2")]
        //[Route("role/{roleId}")]
        //public IHttpActionResult GetUsersByRole(int roleId)
        //{
        //    return Ok(Users.GetAllUsers(roleId, null));
        //}

        

        /////<summary>
        /////Gets all users by role and client GUID
        /////</summary>
        //[Authorize(Roles = "1,2")]
        //[Route("role/{roleID}/client/{clientGUID}")]
        //public IHttpActionResult GetUsersByRoleClient(int roleId, Guid clientGUID)
        //{
        //    return Ok(Users.GetAllUsers(roleId, clientGUID));
        //}

        ///<summary>
        ///
        ///</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

    #region Helpers

    ///<summary>
    /// Users
    ///</summary>
    public class Users
    {
        ///<summary>
        ///Get all clients across DoddleNow
        ///</summary>
        public static List<usp_GetUsersResult> GetAllUsers(int? roleId, Guid? clientGUID)
        {
            DataAccess da = new DataAccess();
            return da.GetUsers(roleId, clientGUID).ToList();
        }

        public static usp_GetUserResult GetUser(Guid userId)
        {
            DataAccess da = new DataAccess();
            return da.GetUser(userId);
        }

        ///<summary>
        ///Delete user
        ///</summary>
        public static void DeleteUser(string userId)
        {
            DataAccess da = new DataAccess();
            da.DeleteUser(userId);
        }
    }
    #endregion
}
