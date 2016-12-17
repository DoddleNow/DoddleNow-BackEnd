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
using System.Security.Claims;
using System.Web;
using System.Diagnostics;
using DoddleNow.API.Utility;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///All account related functions
    ///</summary>
    [RoutePrefix("api/v1/ha")]
    public class HAController : ApiController
    {
        private AuthRepository _repo = null;

        ///<summary>
        /// Account related functions
        ///</summary>
        public HAController()
        {
            _repo = new AuthRepository();
        }

       
        

        ///<summary>
        ///Get Profile based on signed in user's token
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetProfile()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            return Ok(HAHelper.GetProfile(Guid.Parse(userId)));
        }


        ///<summary>
        ///Get client associated to account with ID.  Must be in network of user's base client
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}")]
        [HttpGet]
        public IHttpActionResult GetClient(Guid clientId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            HA admin = HAHelper.GetProfile(Guid.Parse(userId));
            List<Client> clients = new List<Client>();
            Client selectedClient = new Client();

            if (admin.Overview.ClientId.HasValue )
            {
                clients = Clients.GetSubClients(admin.Overview.ClientId.Value);

                if (admin.Overview.ClientId.Value == clientId)
                    selectedClient = Clients.GetClient(admin.Overview.ClientId.Value);
                else
                    selectedClient = clients.Where(v => v.Id == clientId).FirstOrDefault();
            }

            return Ok(selectedClient);
        }



        ///<summary>
        ///Update client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClient(Guid clientId, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //only allow if current
            if (IsValidClientNetwork(clientId))
            {
                client.Id = clientId;

                Clients.UpdateClient(client);

                return Ok();
            }
            else
                return Ok("Not a valid client");

        }

        private bool IsValidClientNetwork(Guid clientId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            HA admin = HAHelper.GetProfile(Guid.Parse(userId));

            bool validClient = false;

            if (admin.Overview.ClientId.Value != clientId)
            {
                Client c = Clients.GetSubClients(admin.Overview.ClientId.Value).Where(v => v.Id == clientId).FirstOrDefault();
                if (c != null)
                    validClient = true;
            }
            else
                validClient = true;

            return validClient;
        }


        ///<summary>
        ///Get all users for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/staff")]
        [HttpGet]
        public IHttpActionResult GetClientUsers(Guid clientId)
        {
            if(IsValidClientNetwork(clientId))
                return Ok(Users.GetAllUsers(null, clientId));
            else
                return Ok("Not a valid client");
        }


        ///<summary>
        ///Get all users for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/staff/{userId}")]
        [HttpGet]
        public IHttpActionResult GetClientUsers(Guid clientId, Guid userId)
        {
            if (IsValidClientNetwork(clientId))
            {
                var user = Users.GetAllUsers(null, clientId).Where(v => v.Id == userId.ToString()).FirstOrDefault();
                return Ok(user);
            }
            else
                return Ok("Not a valid client");
        }


        ///<summary>
        ///Update client user by userId
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/staff/{userId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientUser(Guid clientId, Guid userId, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IsValidClientNetwork(clientId))
            {
                user.ClientID = clientId;
                user.Id = userId;
                //add additional user info to database
                DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();

                //make sure user exists
                usp_GetUserResult usr = da.GetUser(userId);
                if (usr != null)
                {
                    user.RoleID = user.RoleID == 0 ? Convert.ToInt32(usr.RoleId) : user.RoleID;
                    user.EMail = user.EMail == null ? usr.Email : user.EMail;
                    user.FirstName = user.FirstName == null ? usr.FirstName : user.FirstName;
                    user.LastName = user.LastName == null ? usr.LastName: user.LastName;
                    user.Phone = user.Phone == null ? usr.Phone : user.Phone;
                    user.Title = user.Title == null ? usr.Title : user.Title;
                    user.Department = user.Department == null ? usr.Department : user.Department;

                    //accommodate a partial update
                    da.UpdateUser(user.Id, user.RoleID.Value, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
                }   
                else
                    return Ok("User does not exist");

                return Ok();
            }
            else
                return Ok("Not a valid client");
               
        }


        ///<summary>
        ///Create new user for client with id = id
        ///</summary>
        [AllowAnonymous]
        [Route("{clientId}/staff")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClientUser(Guid clientId, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            user.ClientID = clientId;

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
        ///Get all jobs for client
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs")]
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}/{filter:alpha?}")]
        [HttpGet]
        public IHttpActionResult GetClientJobs(Guid clientId, int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            if (IsValidClientNetwork(clientId))
            {
                var jobs = Clients.GetJobs(clientId, null);

                //only allow orderby on these
                if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("NAME") || orderBy.ToUpper().Contains("NEWAPPLICANTS") || orderBy.ToUpper().Contains("APPLICANTCOUNT") || orderBy.ToUpper().Contains("SCLMATCHPREFERENCE")))
                {
                    orderBy = string.Empty;
                }

                var totalJobs = jobs.Count();
                var totalPastJobs = jobs.Where(v => (v.EndDate != null && v.EndDate.Value < DateTime.Now)).Count();
                var totalActiveJobs = jobs.Where(v => (v.EndDate == null || v.EndDate.Value >= DateTime.Now) && (v.StartDate == null || v.StartDate <= DateTime.Now)).Count();

                if (filter.Length > 0)
                {
                    if (filter.ToLower() == "active")
                    {
                        jobs = jobs.Where(v => (v.EndDate == null || v.EndDate.Value >= DateTime.Now) && (v.StartDate == null || v.StartDate <= DateTime.Now)).ToList();
                    }
                    else if (filter.ToLower() == "past")
                    {
                        jobs = jobs.Where(v => (v.EndDate != null && v.EndDate.Value < DateTime.Now) ).ToList();
                    }
                }

                var totalCount = jobs.Count();
                var totalPages = Math.Ceiling((double)totalCount / perPage);

                if (QueryHelper.PropertyExists<HPJob>(orderBy))
                {
                    ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.DL>(orderBy);

                    //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                    if (sort.ToUpper() == "ASC" || sort == string.Empty)
                    {
                        if (orderBy.ToUpper() == "NAME")
                            jobs = jobs.OrderBy(c => c.Name).ToList();
                        else if (orderBy.ToUpper() == "NEWAPPLICANTS")
                            jobs = jobs.OrderBy(c => c.CandidateCount).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTCOUNT")
                            jobs = jobs.OrderBy(c => c.ApplicantCount).ToList();
                        else if (orderBy.ToUpper() == "SCLMATCHPREFERENCE")
                            jobs = jobs.OrderBy(c => c.SCLMatchPreference).ToList();
                    }
                    else
                    {
                        if (orderBy.ToUpper() == "NAME")
                            jobs = jobs.OrderByDescending(c => c.Name).ToList();
                        else if (orderBy.ToUpper() == "NEWAPPLICANTS")
                            jobs = jobs.OrderByDescending(c => c.CandidateCount).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTCOUNT")
                            jobs = jobs.OrderByDescending(c => c.ApplicantCount).ToList();
                        else if (orderBy.ToUpper() == "SCLMATCHPREFERENCE")
                            jobs = jobs.OrderByDescending(c => c.SCLMatchPreference).ToList();
                    }
                }
                else
                {
                    jobs = jobs.OrderBy(c => c.Starred).OrderBy(c => c.Name).ToList();
                }

                jobs = jobs.Skip((page - 1) * perPage)
                                        .Take(perPage)
                                        .ToList();

                var result = new
                {
                    totalCount = totalCount,
                    totalPages = totalPages,
                    currentPage = page,
                    totalJobs = totalJobs,
                    totalPastJobs = totalPastJobs,
                    totalActiveJobs = totalActiveJobs,
                    data = jobs
                };

                return Ok(result);
            }
            else
                return Ok("Not a valid client");
            
        }

        ///<summary>
        ///Create new job for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs")]
        [HttpPost]
        public IHttpActionResult AddJob(Guid clientId, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IsValidClientNetwork(clientId))
            {
                job.ClientId = clientId;

                Guid newVal = Clients.AddJob(job).Value;
                return Ok(newVal);
            }
            else
                return Ok("Not a valid client");
            
        }

        ///<summary>
        ///Update job by clientId and jobId
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientJob(Guid clientId, Guid jobId, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IsValidClientNetwork(clientId))
            {

                job.ClientId = clientId;
                job.Id = jobId;
                //add additional user info to database
                Jobs.UpdateJob(job);

                return Ok();
            }
            else
                return Ok("Not a valid client");
        }

        ///<summary>
        ///Delete Job
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpDelete]
        public IHttpActionResult DeleteJob(Guid clientId, Guid jobId)
        {
            if (IsValidClientNetwork(clientId))
            {
                var job = Clients.GetJobs(clientId, jobId).FirstOrDefault();

                if (job != null)
                    Clients.DeleteJob(jobId);

                return Ok();
            }
            else
                return Ok("Not a valid client");
        }

        ///<summary>
        ///Get all jobs for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpGet]
        public IHttpActionResult GetClientJob(Guid clientId, Guid jobId)
        {
            if (IsValidClientNetwork(clientId))
            {
                var job = Clients.GetJobs(clientId, jobId).FirstOrDefault();
                return Ok(job);
            }
            else
                return Ok("Not a valid client");
            
        }

        ///<summary>
        ///Get all job candidates for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}/candidates")]
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}/{filter:alpha?}")]
        [HttpGet]
        public IHttpActionResult GetClientJobApplicants(Guid clientId, Guid jobId, int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            if (IsValidClientNetwork(clientId))
            {
                var candidates = Jobs.GetJobCandidates(jobId).ToList();

                //only allow orderby on these
                if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("FIRSTNAME") || orderBy.ToUpper().Contains("LASTNAME") || orderBy.ToUpper().Contains("APPLICANTAPPLIED") || orderBy.ToUpper().Contains("CLIENTINTEREST")
                    || orderBy.ToUpper().Contains("CLIENTSTARRED") || orderBy.ToUpper().Contains("COFFEECONNECT")))
                {
                    orderBy = string.Empty;
                }

                var totalCandidates = candidates.Count();
                var totalStarred = candidates.Where(v => v.ClientStarred == true).Count();
                var totalCoffeeConnected = candidates.Where(v => v.CoffeeConnect == true).Count();

                //TODO: Filters if necessary
                //if (filter.Length > 0)
                //{
                //    if (filter.ToLower() == "applied")
                //    {
                //        items = items.Where(v => v.Applied == true).ToList();
                //    }
                //    else if (filter.ToLower() == "starred")
                //    {
                //        items = items.Where(v => v.Starred == true).ToList();
                //    }
                //    else if (filter.ToLower() == "clientinterested")
                //    {
                //        items = items.Where(v => v.ClientInterested == true).ToList();
                //    }
                //}

                //count of items returned after filter and total pages
                var totalCount = candidates.Count();
                var totalPages = Math.Ceiling((double)totalCount / perPage);

                if (QueryHelper.PropertyExists<HPJob>(orderBy))
                {
                    ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.DL>(orderBy);

                    //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                    if (sort.ToUpper() == "ASC" || sort == string.Empty)
                    {
                        if (orderBy.ToUpper() == "FIRSTNAME")
                            candidates = candidates.OrderBy(c => c.FirstName).ToList();
                        else if (orderBy.ToUpper() == "LASTNAME")
                            candidates = candidates.OrderBy(c => c.LastName).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTAPPLIED")
                            candidates = candidates.OrderBy(c => c.ApplicantApplied).ToList();
                        else if (orderBy.ToUpper() == "CLIENTINTEREST")
                            candidates = candidates.OrderBy(c => c.ClientInterest).ToList();
                        else if (orderBy.ToUpper() == "CLIENTSTARRED")
                            candidates = candidates.OrderBy(c => c.ClientStarred).ToList();
                        else if (orderBy.ToUpper() == "COFFEECONNECT")
                            candidates = candidates.OrderBy(c => c.CoffeeConnect).ToList();
                    }
                    else
                    {
                        if (orderBy.ToUpper() == "FIRSTNAME")
                            candidates = candidates.OrderByDescending(c => c.FirstName).ToList();
                        else if (orderBy.ToUpper() == "LASTNAME")
                            candidates = candidates.OrderByDescending(c => c.LastName).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTAPPLIED")
                            candidates = candidates.OrderByDescending(c => c.ApplicantApplied).ToList();
                        else if (orderBy.ToUpper() == "CLIENTINTEREST")
                            candidates = candidates.OrderByDescending(c => c.ClientInterest).ToList();
                        else if (orderBy.ToUpper() == "CLIENTSTARRED")
                            candidates = candidates.OrderByDescending(c => c.ClientStarred).ToList();
                        else if (orderBy.ToUpper() == "COFFEECONNECT")
                            candidates = candidates.OrderByDescending(c => c.CoffeeConnect).ToList();
                    }
                }
                else
                {
                    candidates = candidates.OrderBy(c => c.ClientStarred).OrderBy(c => c.LastName).ToList();
                }

                candidates = candidates.Skip((page - 1) * perPage)
                                        .Take(perPage)
                                        .ToList();

                var result = new
                {
                    totalCount = totalCount,
                    totalPages = totalPages,
                    currentPage = page,
                    totalCandidates = totalCandidates,
                    totalFavorited = totalStarred,
                    totalCoffeeConnected = totalCoffeeConnected,
                    data = candidates
                };


                return Ok(result);
            }
            else
                return Ok("Not a valid client");

        }

        ///<summary>
        ///Get all job candidates for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/candidates")]
        [Route("{perPage:int}/{page:int}/{orderBy:alpha?}/{filter:alpha?}")]
        [HttpGet]
        public IHttpActionResult GetClientApplicants(Guid clientId, int perPage = 1000, int page = 1, string orderBy = "", string sort = "asc", string filter = "")
        {
            if (IsValidClientNetwork(clientId))
            {
                var candidates = Jobs.GetClientCandidates(clientId).ToList();
                //only allow orderby on these
                if (orderBy.Length > 0 && !(orderBy.ToUpper().Contains("FIRSTNAME") || orderBy.ToUpper().Contains("LASTNAME") || orderBy.ToUpper().Contains("APPLICANTAPPLIED") || orderBy.ToUpper().Contains("CLIENTINTEREST")
                    || orderBy.ToUpper().Contains("CLIENTSTARRED") || orderBy.ToUpper().Contains("COFFEECONNECT")))
                {
                    orderBy = string.Empty;
                }

                var totalCandidates = candidates.Count();
                var totalStarred = candidates.Where(v => v.ClientStarred == true).Count();
                var totalCoffeeConnected = candidates.Where(v => v.CoffeeConnect == true).Count();

                if (filter.Length > 0)
                {
                    if (filter.ToLower() == "favorited")
                    {
                        candidates = candidates.Where(v => v.ClientStarred == true).ToList();
                    }
                    else if (filter.ToLower() == "coffeeconnected")
                    {
                        candidates = candidates.Where(v => v.CoffeeConnect == true).ToList();
                    }
                }

                //count of items returned after filter and total pages
                var totalCount = candidates.Count();
                var totalPages = Math.Ceiling((double)totalCount / perPage);

                if (QueryHelper.PropertyExists<HPJob>(orderBy))
                {
                    ///var orderByExpression = QueryHelper.GetPropertyExpression<DataAccessLayer.DL>(orderBy);

                    //need major refactor.  HPJobDL won't allow the orderByExpression so have to do a nasty if/else
                    if (sort.ToUpper() == "ASC" || sort == string.Empty)
                    {
                        if (orderBy.ToUpper() == "FIRSTNAME")
                            candidates = candidates.OrderBy(c => c.FirstName).ToList();
                        else if (orderBy.ToUpper() == "LASTNAME")
                            candidates = candidates.OrderBy(c => c.LastName).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTAPPLIED")
                            candidates = candidates.OrderBy(c => c.ApplicantApplied).ToList();
                        else if (orderBy.ToUpper() == "CLIENTINTEREST")
                            candidates = candidates.OrderBy(c => c.ClientInterest).ToList();
                        else if (orderBy.ToUpper() == "CLIENTSTARRED")
                            candidates = candidates.OrderBy(c => c.ClientStarred).ToList();
                        else if (orderBy.ToUpper() == "COFFEECONNECT")
                            candidates = candidates.OrderBy(c => c.CoffeeConnect).ToList();
                    }
                    else
                    {
                        if (orderBy.ToUpper() == "FIRSTNAME")
                            candidates = candidates.OrderByDescending(c => c.FirstName).ToList();
                        else if (orderBy.ToUpper() == "LASTNAME")
                            candidates = candidates.OrderByDescending(c => c.LastName).ToList();
                        else if (orderBy.ToUpper() == "APPLICANTAPPLIED")
                            candidates = candidates.OrderByDescending(c => c.ApplicantApplied).ToList();
                        else if (orderBy.ToUpper() == "CLIENTINTEREST")
                            candidates = candidates.OrderByDescending(c => c.ClientInterest).ToList();
                        else if (orderBy.ToUpper() == "CLIENTSTARRED")
                            candidates = candidates.OrderByDescending(c => c.ClientStarred).ToList();
                        else if (orderBy.ToUpper() == "COFFEECONNECT")
                            candidates = candidates.OrderByDescending(c => c.CoffeeConnect).ToList();
                    }
                }
                else
                {
                    candidates = candidates.OrderBy(c => c.ClientStarred).OrderBy(c => c.LastName).ToList();
                }

                candidates = candidates.Skip((page - 1) * perPage)
                                        .Take(perPage)
                                        .ToList();

                var result = new
                {
                    totalCount = totalCount,
                    totalPages = totalPages,
                    currentPage = page,
                    totalCandidates = totalCandidates,
                    totalFavorited = totalStarred,
                    totalCoffeeConnected = totalCoffeeConnected,
                    data = candidates
                };


                return Ok(result);
            }
            else
                return Ok("Not a valid client");

        }

        
        ///<summary>
        ///Get all job candidates for client with id = id
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}/candidates/{candidateId}")]
        [HttpGet]
        public IHttpActionResult GetClientJobCandidate(Guid clientId, Guid jobId, Guid candidateId)
        {
            if (IsValidClientNetwork(clientId))
            {
                var candidate = Jobs.GetJobCandidate(jobId, candidateId.ToString());
                
                    Profile p = Profiles.GetProfile(candidateId);
                    p.CandidateDetails = candidate;
                    return Ok(p);
                
            }
            else
                return Ok("Not a valid client");

        }

      
        ///<summary>
        ///Update properties about candidate (interest)
        ///</summary>
        [Authorize(Roles = "3")]
        [Route("{clientId}/jobs/{jobId}/candidates/{candidateId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientJobCandidate(Guid clientId, Guid jobId, Guid candidateId, Candidate candidate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IsValidClientNetwork(clientId))
            {
                candidate.UserId = candidateId;



                Jobs.UpdateJobCandidate(jobId, candidate);
                return Ok();
            }
            else
                return Ok("Not a valid client");
        }



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
    public class HAHelper
    {
        ///<summary>
        ///Get all clients across DoddleNow
        ///</summary>
        public static List<usp_GetUsersResult> GetAllUsers(int? roleId, Guid? clientGUID)
        {
            DataAccess da = new DataAccess();
            return da.GetUsers(roleId, clientGUID).ToList();
        }

        /// <summary>
        /// Gets overview of profile only
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static HPOverview GetOverview(Guid userId)
        {
            DataAccess da = new DataAccess();
            usp_GetUserResult profile = da.GetUser(userId);

            HPOverview p = new HPOverview()
            {
                CellPhone = profile.CELL_PHONE,
                Department = profile.Department,
                EMail = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ImageUrl = profile.ImageURL,
                PersonalInterests = profile.PERSONAL_INTERESTS,
                PersonalSummary = profile.PERSONAL_SUMMARY,
                Phone = profile.Phone,
                SecondaryEmail = profile.SECONDARY_EMAIL,
                Title = profile.Title,
                UserId = Guid.Parse(profile.Id),
                VideoUrl = profile.VideoUrl
            };

            return p;
        }

        /// <summary>
        /// Gets profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static HA GetProfile(Guid userId)
        {
            DataAccess da = new DataAccess();
            usp_GetUserResult profile = da.GetUser(userId);
            UserController uc = new UserController();
                       

            Client client = new Client();
            List<Client> subs = new List<Client>();
            List<ClientList> allClients = new List<Models.ClientList>();

            if (profile.ClientId.HasValue )
            {
                client = Clients.GetClient(profile.ClientId.Value);
                if(client != null)
                {
                    allClients.Add(new ClientList { ClientId = client.Id, ClientName = client.Name });
                    subs = Clients.GetSubClients(client.Id);
                    if(subs != null && subs.Count > 0)
                    {
                        foreach (Client c in subs)
                        {
                            allClients.Add(new ClientList { ClientId = c.Id, ClientName = c.Name });
                        }
                    }
                }   
            }

            HA currentProfile = new HA();
            currentProfile.Overview = profile;
            currentProfile.ClientList = allClients;
            
            return currentProfile;
        }

        public static List<Address> GetLocations(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetLocationsResult> locations = da.GetLocations(userId);

            List<Address> locs = new List<Address>();
            for (int i = 0; i < locations.Count; ++i)
            {
                locs.Add(new Address { ID = locations[i].ID, Address_1 = locations[i].ADDRESS_1, AddressType = locations[i].ADDRESS_TYPE, AddressTypeId = locations[i].ADDRESS_TYPE_ID, Address_2 = locations[i].ADDRESS_2, City = locations[i].CITY, State = locations[i].STATE, UserId = Guid.Parse(locations[i].UserID), ZIP = locations[i].ZIP});
            }

            return locs;
        }

        public static List<Education> GetEducations(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetEducationsResult> educations = da.GetEducations(userId);

            List<Education> eds = new List<Education>();
            for (int i = 0; i < educations.Count; ++i)
            {
                eds.Add(new Education { UserId = userId, EndDate = educations[i].EndDate, Graduated = educations[i].Graduated, GraduationDate = educations[i].GraduationDate, ID = educations[i].ID, HighestDegreeEarnedID = educations[i].HighestDegreeEarnedID.Value, InstitutionName = educations[i].InstitutionName, Major = educations[i].Major, OtherDegree = educations[i].OtherDegree, StartDate = educations[i].StartDate });
            }

            return eds;
        }

        public static List<Certification> GetCertifications(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetCertificationsResult> certifications = da.GetCertifications(userId);

            List<Certification> certs = new List<Certification>();
            for (int i = 0; i < certifications.Count; ++i)
            {
                certs.Add(new Certification { UserId = userId, ExpirationDate = certifications[i].ExpirationDate, ID = certifications[i].ID, Name=certifications[i].Name, IssuanceDate = certifications[i].IssuanceDate, IssuingBody = certifications[i].IssuingBody });
            }

            return certs;
        }

        public static List<WorkHistory> GetWorkHistories(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetWorkHistoriesResult> wh = da.GetWorkHistories(userId);

            List<WorkHistory> whs = new List<WorkHistory>();
            for (int i = 0; i < wh.Count; ++i)
            {
                List<string> responsibilities = new List<string>();
                //check for job responsibilities
                List<usp_GetWorkHistoryJobResponsibilitiesResult> res = da.GetWorkHistoryJobResponsibilities(wh[i].ID);
                if (res != null && res.Count > 0)
                {
                    for (int y = 0; y < res.Count; ++y)
                    {
                        responsibilities.Add(res[y].Responsibility);
                    }
                }
                whs.Add(new WorkHistory { UserId = userId, CompanyCity = wh[i].CompanyCity, JobResponsibilities = responsibilities, CompanyName = wh[i].CompanyName, CompanyState = wh[i].CompanyState, EndDate = wh[i].EndDate, ID = wh[i].ID, JobTitle = wh[i].JobTitle, StartDate = wh[i].StartDate });

            }

            return whs;
        }

        public static List<Reference> GetReferences(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetReferencesResult> r = da.GetReferences(userId);

            List<Reference> refs = new List<Reference>();
            for (int i = 0; i < r.Count; ++i)
            {
                refs.Add(new Reference { UserId = userId, ContactPhone = r[i].ContactPhone, DirectSupervisor = r[i].DirectSupervisor, ID = r[i].ID, Name = r[i].Name, Title = r[i].Title });
            }

            return refs;
        }

        public static List<Language> GetLanguages(Guid userId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetUserLanguagesResult> r = da.GetUserLanguages(userId.ToString());

            List<Language> langs = new List<Language>();
            for (int i = 0; i < r.Count; ++i)
            {
                langs.Add(new Language { UserId = userId, Description = r[i].Description, ID = r[i].ID});
            }

            return langs;
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
