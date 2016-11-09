using DataAccessLayer;
using System;
using DoddleNow.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///myjobs Controller.  Used to get jobs and their details for an HP
    ///</summary>
    [RoutePrefix("api/v1/myjobs")]
    public class MyJobController : ApiController
    {
        ///<summary>
        ///Get all jobs
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetMyJobs()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            return Ok(MyJobs.GetHPJobs(Guid.Parse(userId), null));
        }
        
    }

    #region Helpers

    ///<summary>
    /// MyJobs
    ///</summary>
    public class MyJobs
    {
        ///<summary>
        ///Get HP jobs
        ///</summary>
        public static List<HPJob> GetHPJobs(Guid userId, Guid? jobId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetHPJobsResult> items;

            if (!jobId.HasValue)
                items = da.GetHPJobs(userId.ToString());
            else
                items = da.GetHPJobs(userId.ToString(), jobId.Value);

            List<HPJob> jobs = new List<HPJob>();

            for(int i=0; i< items.Count; ++i)
            {
                jobs.Add(new HPJob { ClientId = items[i].ClientId.Value, ClientInterested = items[i].CLIENT_INTEREST,
                    JobDescription = items[i].DESCRIPTION, EndDate = items[i].EndDate, JobId = items[i].JobID, JobName = items[i].NAME,
                    Starred = items[i].STARRED, StartDate = items[i].StartDate, ClientAddress=items[i].ClientAddress, ClientAddress2=items[i].ClientAddress2,
                 ClientCity=items[i].ClientCity, ClientName=items[i].ClientName, ClientState=items[i].ClientState, ClientZip=items[i].ClientZIP, Specialities=items[i].Specialties});
            }
            return jobs;
        }

        
    }
    #endregion
}
