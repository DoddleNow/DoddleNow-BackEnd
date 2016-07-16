using DoddleNow.API.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///
    ///</summary>
    public class BaseApiController : ApiController
    {
        //Code removed from brevity
        private ApplicationRoleManager _AppRoleManager = null;

        ///<summary>
        ///
        ///</summary>
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
    }
}
