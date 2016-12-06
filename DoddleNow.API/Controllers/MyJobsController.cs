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
using DoddleNow.API.Utility;
using System.Linq.Expressions;

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
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}/{filter:alpha?}")]
        [HttpGet]
        public IHttpActionResult GetMyJobs(int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            List<HPJob> items = MyJobs.GetHPJobs(Guid.Parse(userId), null);

            
            //only allow orderby on these
            if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("JOBNAME") || orderBy.ToUpper().Contains("CLIENTNAME") || orderBy.ToUpper().Contains("STARRED") || orderBy.ToUpper().Contains("CLIENTINTERESTED")
                || orderBy.ToUpper().Contains("APPLIED") || orderBy.ToUpper().Contains("SCLMATCH")))
            {
                orderBy = string.Empty;
            }

            var totalStarred = items.Where(v => v.Starred == true).Count();
            var totalInterested = items.Where(v => v.ClientInterested == true).Count();
            var totalApplied = items.Where(v => v.Applied == true).Count();
            var totalMatches = items.Count();

            if (filter.Length > 0)
            {
                if (filter.ToLower() == "applied")
                {
                    items = items.Where(v => v.Applied == true).ToList();
                }
                else if (filter.ToLower() == "starred")
                {
                    items = items.Where(v => v.Starred == true).ToList();
                }
                else if (filter.ToLower() == "clientinterested")
                {
                    items = items.Where(v => v.ClientInterested == true).ToList();
                }
            }

            //count of items returned after filter and total pages
            var totalCount = items.Count();
            var totalPages = Math.Ceiling((double)totalCount / perPage);


            if (QueryHelper.PropertyExists<HPJob>(orderBy))
            {
                ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.DL>(orderBy);

                //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                if (sort.ToUpper() == "ASC" || sort == string.Empty)
                {
                    if (orderBy.ToUpper() == "JOBNAME")
                        items = items.OrderBy(c => c.Name).ToList();
                    else if (orderBy.ToUpper() == "CLIENTNAME")
                        items = items.OrderBy(c => c.ClientName).ToList();
                    else if (orderBy.ToUpper() == "STARRED")
                        items = items.OrderBy(c => c.Starred).ToList();
                    else if (orderBy.ToUpper() == "CLIENTINTERESTED")
                        items = items.OrderBy(c => c.ClientInterested).ToList();
                    else if (orderBy.ToUpper() == "APPLIED")
                        items = items.OrderBy(c => c.Applied).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        items = items.OrderBy(c => c.SCLMatch).ToList();
                }
                else
                {
                    if (orderBy.ToUpper() == "JOBNAME")
                        items = items.OrderByDescending(c => c.Name).ToList();
                    else if (orderBy.ToUpper() == "CLIENTNAME")
                        items = items.OrderByDescending(c => c.ClientName).ToList();
                    else if (orderBy.ToUpper() == "STARRED")
                        items = items.OrderByDescending(c => c.Starred).ToList();
                    else if (orderBy.ToUpper() == "CLIENTINTERESTED")
                        items = items.OrderByDescending(c => c.ClientInterested).ToList();
                    else if (orderBy.ToUpper() == "APPLIED")
                        items = items.OrderByDescending(c => c.Applied).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        items = items.OrderByDescending(c => c.SCLMatch).ToList();
                }
            }
            else
            {
                items = items.OrderBy(c => c.Starred).OrderBy(c => c.Name).ToList();
            }

            var jobs = items.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .ToList();

            var result = new
            {
                totalCount = totalCount,
                totalPages = totalPages,
                currentPage = page,
                totalStarred = totalStarred,
                totalInterested = totalInterested,
                totalApplied = totalApplied,
                totalMatches = totalMatches,
                data = jobs
            };

            return Ok(result);
        }


        ///<summary>
        ///Get all jobs
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("{jobId}")]
        [HttpGet]
        public IHttpActionResult GetMyJobs(Guid jobId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            HPJob item = MyJobs.GetHPJobs(Guid.Parse(userId), jobId).FirstOrDefault();

            
            return Ok(item);
        }

        ///<summary>
        ///Update job with interest, star job
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("{jobId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateJob(Guid jobId, HPJob job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            DataAccess da = new DataAccess();
            da.UpdateUserJob(userId, jobId, job.Starred, job.Applied);

            return Ok();
        }


        ///<summary>
        ///Get all jobs
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("search")]
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}/{filter:alpha?}")]
        [HttpPost]
        public IHttpActionResult GetHPJobsBySearchParam(HPJobSearchModel searchModel, int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            List<long> ids = new List<long>();
            if (searchModel.SpecialtyIDs != null && searchModel.SpecialtyIDs.Count > 0)
            {
                for (int i = 0; i < searchModel.SpecialtyIDs.Count; ++i)
                {
                    ids.Add(searchModel.SpecialtyIDs[i]);
                }
            }
            
            List<DataAccessLayer.HPJobDL> items = MyJobs.GetHPJobsBySearchParam(userId, searchModel.SearchParam, ids);

            

            //only allow orderby on these
            if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("JOBNAME") || orderBy.ToUpper().Contains("CLIENTNAME") || orderBy.ToUpper().Contains("STARRED") || orderBy.ToUpper().Contains("CLIENTINTERESTED")
                || orderBy.ToUpper().Contains("APPLIED") || orderBy.ToUpper().Contains("SCLMATCH")))
            {
                orderBy = string.Empty;
            }
            
            //global variables
            var totalStarred = items.Where(v => v.Starred == true).Count();
            var totalInterested = items.Where(v => v.ClientInterested == true).Count();
            var totalApplied = items.Where(v => v.Applied == true).Count();
            var totalMatches = items.Count();

            if (filter.Length > 0)
            {
                if (filter.ToLower() == "applied")
                {
                    items = items.Where(v => v.Applied == true).ToList();
                }
                else if (filter.ToLower() == "starred")
                {
                    items = items.Where(v => v.Starred == true).ToList();
                }
                else if (filter.ToLower() == "clientinterested")
                {
                    items = items.Where(v => v.ClientInterested == true).ToList();
                }
            }

            //count of items returned after filter and total pages
            var totalCount = items.Count();
            var totalPages = Math.Ceiling((double)totalCount / perPage);

            if (QueryHelper.PropertyExists<DataAccessLayer.HPJobDL>(orderBy))
            {
                ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.HPJobDL>(orderBy);
                
                //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                if (sort.ToUpper() == "ASC" || sort == string.Empty)
                {
                    if(orderBy.ToUpper() == "JOBNAME")
                        items = items.OrderBy(c => c.JobName).ToList();
                    else if (orderBy.ToUpper() == "CLIENTNAME")
                        items = items.OrderBy(c => c.ClientName).ToList();
                    else if (orderBy.ToUpper() == "STARRED")
                        items = items.OrderBy(c => c.Starred).ToList();
                    else if (orderBy.ToUpper() == "CLIENTINTERESTED")
                        items = items.OrderBy(c => c.ClientInterested).ToList();
                    else if (orderBy.ToUpper() == "APPLIED")
                        items = items.OrderBy(c => c.Applied).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        items = items.OrderBy(c => c.SCLMatch).ToList();
                }
                else
                {
                    if (orderBy.ToUpper() == "JOBNAME")
                        items = items.OrderByDescending(c => c.JobName).ToList();
                    else if (orderBy.ToUpper() == "CLIENTNAME")
                        items = items.OrderByDescending(c => c.ClientName).ToList();
                    else if (orderBy.ToUpper() == "STARRED")
                        items = items.OrderByDescending(c => c.Starred).ToList();
                    else if (orderBy.ToUpper() == "CLIENTINTERESTED")
                        items = items.OrderByDescending(c => c.ClientInterested).ToList();
                    else if (orderBy.ToUpper() == "APPLIED")
                        items = items.OrderByDescending(c => c.Applied).ToList();
                    else if (orderBy.ToUpper() == "SCLMATCH")
                        items = items.OrderByDescending(c => c.SCLMatch).ToList();
                }
            }
            else
            {
                items = items.OrderBy(c => c.Starred).OrderBy(c=>c.JobName).ToList();
            }

            var jobs = items.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .ToList();

            var result = new
            {
                totalCount = totalCount,
                totalPages = totalPages,
                currentPage = page,
                totalStarred = totalStarred,
                totalInterested = totalInterested,
                totalApplied = totalApplied,
                totalMatches = totalMatches,
                data = jobs
            };

            return Ok(result);
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
        public static List<Models.HPJob> GetHPJobs(Guid userId, Guid? jobId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetHPJobsResult> items;

            if (!jobId.HasValue)
                items = da.GetHPJobs(userId.ToString());
            else
                items = da.GetHPJobs(userId.ToString(), jobId.Value);

            List<Models.HPJob> jobs = new List<Models.HPJob>();

            for(int i=0; i< items.Count; ++i)
            {
                List<string> shifts = new List<string>();
                if (items[i].Shifts != null)
                    shifts = Clients.GetShifts(items[i].Shifts);


                jobs.Add(new Models.HPJob
                { ClientId = items[i].ClientId, ClientDescription=items[i].ClientDescription, ClientInterested = items[i].CLIENT_INTEREST, Applied = items[i].Applied == 1 ? true : false, Shifts = shifts,
                    Description = items[i].DESCRIPTION, EndDate = items[i].EndDate, Id = items[i].JobID, Name = items[i].NAME, SCLMatch = items[i].SCLMatch.HasValue ? items[i].SCLMatch.Value : 0,
                    Starred = items[i].STARRED, StartDate = items[i].StartDate, ClientAddress=items[i].ClientAddress, ClientAddress2=items[i].ClientAddress2,
                 ClientCity=items[i].ClientCity, ClientName=items[i].ClientName, ClientState=items[i].ClientState, ClientZip=items[i].ClientZIP, Specialities=items[i].Specialties});
            }
            return jobs;
        }


        ///<summary>
        ///Get HP jobs
        ///</summary>
        public static List<DataAccessLayer.HPJobDL> GetHPJobsBySearchParam(string userId, string searchParam, IEnumerable<long> ids)
        {
            DataAccess da = new DataAccess();
            List<DataAccessLayer.HPJobDL> items = da.GetJobsBySearchParam(userId, searchParam, ids);

            return items;
        }


    }
    #endregion
}
