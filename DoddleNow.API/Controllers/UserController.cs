using DataAccessLayer;
using DoddleNow.API.Models;
using DoddleNow.API.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}")]
        [HttpGet]
        public IHttpActionResult GetUsers(int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc")
        {
            var users = Users.GetAllUsers(null, null).AsQueryable();

            //only allow orderby on these
            if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("LASTNAME") || orderBy.ToUpper().Contains("FIRSTNAME") || orderBy.ToUpper().Contains("CLIENTNAME") || orderBy.ToUpper().Contains("ROLENAME")))
            {
                orderBy = string.Empty;
            }

            var totalCount = users.Count();
            var totalPages = Math.Ceiling((double)totalCount / perPage);

            if (QueryHelper.PropertyExists<usp_GetUsersResult>(orderBy))
            {
                var orderByExpression = QueryHelper.GetPropertyExpression<usp_GetUsersResult>(orderBy);
                if (sort.ToUpper() == "ASC" || sort == string.Empty)
                    users = users.OrderBy(orderByExpression);
                else 
                    users = users.OrderByDescending(orderByExpression);
            }
            else
            {
                users = users.OrderBy(c => c.LastName);
            }

            var usrs = users.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .ToList();

            var result = new
            {
                totalCount = totalCount,
                totalPages = totalPages,
                currentPage = page,
                data = usrs
            };

            return Ok(result);

            
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
                da.UpdateUser(user.Id, user.RoleID.Value, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
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

            usp_GetUserResult orig = Users.GetUser(userId);

            if(orig != null)
            {
                user.RoleID = user.RoleID == null ? Convert.ToInt32(orig.RoleId) : user.RoleID;
                user.EMail = user.EMail == null ? orig.Email : user.EMail;
                user.FirstName = user.FirstName == null ? orig.FirstName : user.FirstName;
                user.LastName = user.LastName == null ? orig.LastName : user.LastName;
                user.Phone = user.Phone == null ? orig.Phone : user.Phone;
                user.Title = user.Title == null ? orig.Title : user.Title;
                user.Department = user.Department == null ? orig.Department : user.Department;
            }

            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateUser(user.Id, user.RoleID.Value, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            
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
