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
    ///Client controller.  Used to get client related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/v1/clients")]
    public class ClientController : ApiController
    {

        private AuthRepository _repo = null;

        ///<summary>
        /// Account related functions
        ///</summary>
        public ClientController()
        {
            _repo = new AuthRepository();
        }


        ///<summary>
        ///Get all clients
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllClients()
        {
            return Ok(Clients.GetAllClients());
        }

        ///<summary>
        ///Create new client
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid newVal = Clients.AddClient(client).Value;
            return Ok(newVal);
        }

        ///<summary>
        ///Get client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}")]
        [HttpGet]
        public IHttpActionResult GetClient(Guid clientId)
        {
            return Ok(Clients.GetClient(clientId));
        }



        ///<summary>
        ///Update client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClient(Guid clientId, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            client.Id = clientId;

            Clients.UpdateClient(client);

            return Ok();
        }

        ///<summary>
        ///Delete Client
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{clientId}")]
        [HttpDelete]
        public IHttpActionResult DeleteClient(Guid clientId)
        {
            Clients.DeleteClient(clientId);
            return Ok();
        }



        ///<summary>
        ///Get all sub-clients for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/clients")]
        [HttpGet]
        public IHttpActionResult GetSubClients(Guid clientId)
        {
            return Ok(Clients.GetSubClients(clientId));
        }

        ///<summary>
        ///Create new subclient for client id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/clients")]
        [HttpPost]
        public IHttpActionResult AddSubClient(Guid clientId, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            client.ParentId = clientId;

            Guid newVal = Clients.AddClient(client).Value;
            return Ok(newVal);
        }


        ///<summary>
        ///Get all users for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/users")]
        [HttpGet]
        public IHttpActionResult GetClientUsers(Guid clientId)
        {
            return Ok(Users.GetAllUsers(null, clientId));
        }

        ///<summary>
        ///Create new user for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/users")]
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
                da.UpdateUser(user.Id, user.RoleID, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            }

            return Ok();
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

        ///<summary>
        ///Get specific user by id for client with id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/users/{userId}")]
        [HttpGet]
        public IHttpActionResult GetClientUsers(Guid clientId, Guid userId)
        {
            var users = Users.GetAllUsers(null, clientId).Where(v => v.Id == userId.ToString()).FirstOrDefault();
            return Ok(users);
        }


        ///<summary>
        ///Update client user by userId
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/users/{userId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientUser(Guid clientId, Guid userId, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.ClientID = clientId;
            user.Id = userId;
            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();

            //make sure user exists
            usp_GetUserResult usr = da.GetUser(userId);
            if (usr != null)
                da.UpdateUser(user.Id, user.RoleID, user.EMail, user.FirstName, user.LastName, user.Phone, user.Title, user.Department, user.ClientID);
            else
                return Ok("User does not exist");

            return Ok();
        }

        ///<summary>
        ///Get all jobs for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/users/{userId}")]
        public IHttpActionResult DeleteUser(string clientId, string userId)
        {
            Users.DeleteUser(userId);
            return Ok();
        }

        ///<summary>
        ///Get all jobs for client
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/jobs")]
        [HttpGet]
        public IHttpActionResult GetClientJobs(Guid clientId)
        {
            var jobs = Clients.GetJobs(clientId, null);
            return Ok(jobs);
        }

        ///<summary>
        ///Create new job for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/jobs")]
        [HttpPost]
        public IHttpActionResult AddJob(Guid clientId, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            job.ClientId = clientId;

            Guid newVal = Clients.AddJob(job).Value;
            return Ok(newVal);
        }

        ///<summary>
        ///Update job user by clientId and jobId
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientJob(Guid clientId, Guid jobId, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            job.ClientId = clientId;
            job.Id = jobId;
            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateJob(job.Id, job.ClientId, job.Name, job.Description, job.StartDate.HasValue ? job.StartDate : null, job.EndDate.HasValue ? job.EndDate : null);

            return Ok();
        }

        ///<summary>
        ///Delete Job
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpDelete]
        public IHttpActionResult DeleteJob(Guid clientId, Guid jobId)
        {
            var job = Clients.GetJobs(clientId, jobId).FirstOrDefault();

            if (job != null)
                Clients.DeleteJob(jobId);

            return Ok();
        }

        ///<summary>
        ///Get all jobs for client with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpGet]
        public IHttpActionResult GetClientJob(Guid clientId, Guid jobId)
        {
            var job = Clients.GetJobs(clientId, jobId).FirstOrDefault();
            return Ok(job);
        }

        ///<summary>
        ///Get all specialties for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{clientId}/jobs/{jobId}/specialties")]
        [HttpGet]
        public IHttpActionResult GetSpecialtiesByClientJob(Guid clientId, Guid jobId)
        {
            var specialties = Clients.GetSpecialtiesByClientJob(clientId, jobId);
            return Ok(specialties);
        }


        ///<summary>
        ///Create new  specialty for job id , client id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{clientId}/jobs/{jobId}/specialties")]
        [HttpPost]
        public IHttpActionResult AddJobSpecialty(Guid clientId, Guid jobId, Specialty specialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check that job is valid for client
            if (Clients.GetJobs(clientId, jobId).Count > 0)
            {
                //add specialty
                int specialtyId = Specialties.AddSpecialty(specialty);
                //associate with job
                Clients.AddJobSpecialty(jobId, specialtyId);
            }

            return Ok();
        }

        ///<summary>
        ///Get specialty for job id , client id, specialtyId
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/jobs/{jobId}/specialties/{specialtyId}")]
        [HttpGet]
        public IHttpActionResult GetSpecialtyByClientJob(Guid clientId, Guid jobId, int specialtyId)
        {
            usp_GetSpecialtiesResult specialty = new usp_GetSpecialtiesResult();
            //check that job is valid for client
            if (Clients.GetJobs(clientId, jobId).Count > 0)
            {
                specialty = Clients.GetSpecialtiesByClientJob(clientId, jobId).Where(v => v.ID == specialtyId).FirstOrDefault();
            }

            return Ok(specialty);
        }


        ///<summary>
        ///Create new  specialty for job id , client id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{clientId}/jobs/{jobId}/specialties/{specialtyId}")]
        [HttpPost]
        public IHttpActionResult UpdateJobSpecialty(Guid clientId, Guid jobId, int specialtyId, Specialty specialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check that job is valid for client
            if (Clients.GetJobs(clientId, jobId).Count > 0)
            {
                //check that specialty exists for job
                if (Clients.GetSpecialtiesByClientJob(clientId, jobId).Where(v => v.ID == specialtyId).Count() > 0)
                {
                    specialty.Id = specialtyId;
                    //allow specialty update
                    Specialties.UpdateSpecialty(specialty);
                }

            }

            return Ok();
        }

        ///<summary>
        ///Delete Specialty by job, client, specialty
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{clientId}/jobs/{jobId}/specialties/{specialtyId}")]
        [HttpDelete]
        public IHttpActionResult DeleteJobSpecialty(Guid clientId, Guid jobId, int specialtyId)
        {
            //check that job is valid for client
            if (Clients.GetJobs(clientId, jobId).Count > 0)
            {
                Clients.DeleteJobSpecialty(jobId, specialtyId);
            }

            return Ok();
        }


        #region SkillsChecklists

        ///<summary>
        ///Get all  Skill check lists for job id , client id
        ///</summary>
        [Route("{clientId}/jobs/{jobId}/scl")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklists(Guid clientId, Guid jobId)
        {
            List<usp_GetSkillsChecklistsResult> scls = new List<usp_GetSkillsChecklistsResult>();
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                scls = Jobs.GetJobSkillsChecklists(jobId);
            }
            return Ok(scls);
        }


        ///<summary>
        ///Create new  Skill check list for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl")]
        [HttpPost]
        public IHttpActionResult AddSkillsChecklists(Guid clientId, Guid jobId, SkillsChecklist scl)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                Guid id = SkillsChecklists.AddSkillsChecklist(scl);
                Jobs.AddJobSkillsChecklist(jobId, id);
            }

            return Ok();
        }



        ///<summary>
        ///Get Skill check list id for job id , client id
        ///</summary>
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklists(Guid clientId, Guid jobId, Guid sclId)
        {
            usp_GetSkillsChecklistsResult spec = new usp_GetSkillsChecklistsResult();
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    spec = SkillsChecklists.GetSkillsChecklist(sclId);
                }
            }

            return Ok(spec);
        }


        ///<summary>
        ///Update Get Skill check list id for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}")]
        [HttpPost]
        public IHttpActionResult UpdateSkillsChecklist(Guid clientId, Guid jobId, Guid sclId, SkillsChecklist scl)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    scl.Id = sclId;
                    SkillsChecklists.UpdateSkillsChecklist(scl);
                }
            }
            return Ok();
        }


        ///<summary>
        ///Delete Get Skill check list id for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklist(Guid clientId, Guid jobId, Guid sclId)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    SkillsChecklists.DeleteSkillsChecklist(sclId);
                }
            }

            return Ok();
        }

        /////////////////////////////////////////////

        ///<summary>
        ///Get all questions for scl id, job id , client id
        ///</summary>
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}/questions")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklistQuestions(Guid clientId, Guid jobId, Guid sclId)
        {
            List<usp_GetQuestionsResult> questions = new List<usp_GetQuestionsResult>();
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    questions = SkillsChecklists.GetSkillsChecklistQuestions(sclId);
                }
            }
            return Ok(questions);
        }


        ///<summary>
        ///Create new question for scl id, job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}/questions")]
        [HttpPost]
        public IHttpActionResult AddSkillsChecklistQuestion(Guid clientId, Guid jobId, Guid sclId, Question question)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    int id = SkillsChecklists.AddQuestion(sclId, question);
                }
            }
            return Ok();
        }

        ///<summary>
        ///Delete all questions for scl with id = id, job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{sclId}/questions")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid clientId, Guid jobId, Guid sclId)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
                {
                    SkillsChecklists.DeleteSkillsChecklistQuestions(sclId, null);
                }
            }
            return Ok();
        }


        #endregion

    }

    #region Helpers

    ///<summary>
    /// Clients
    ///</summary>
    public class Clients
    {
        ///<summary>
        ///Get all clients across DoddleNow
        ///</summary>
        public static List<Client> GetAllClients()
        {
            DataAccess da = new DataAccess();
            List<Client> clients = new List<Client>();
            List<usp_GetClientsResult> clientsBase = da.GetClients().ToList();
            foreach (usp_GetClientsResult c in clientsBase)
            {
                clients.Add(new Client { Address1 = c.Address1, Address2 = c.Address2, City = c.CITY,
                    Description = c.DESCRIPTION, Id = c.ID, Name = c.NAME, ParentId = c.ParentId, State = c.STATE, ZIP = c.ZIP,
                 MarketingBullets = GetMarketingBullets(c.ID)});
            }
            return clients;
        }

        private static string[] GetMarketingBullets(Guid clientId)
        {
            DataAccess da = new DataAccess();
            List<MarketingBullet> bullets = new List<MarketingBullet>();
            List<usp_GetMarketingBulletsResult> items = da.GetMarketingBullets(clientId);
            foreach (usp_GetMarketingBulletsResult mb in items)
            {
                bullets.Add(new MarketingBullet { Bullet = mb.BULLET });
            }
            return bullets.Select(v=>v.Bullet).ToArray();
        }

        ///<summary>
        ///Get all jobs for client with id = id
        ///</summary>
        public static List<HPJob> GetJobs(Guid clientId, Guid? jobId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetJobsResult> items = da.GetJobs(clientId, jobId).ToList();
            List<HPJob> final = new List<HPJob>();

            for (int i=0;i<items.Count; ++i)
            {
                List<string> shifts = new List<string>();
                if(items[i].Shifts != null)
                    shifts = GetShifts(items[i].Shifts);

                final.Add(new HPJob
                {
                    Id = items[i].ID,
                    ClientId = items[i].ClientId.Value,
                    ClientName = items[i].ClientName,
                    Active = items[i].Active == 1 ? true : false,
                    ApplicantCount = items[i].ApplicantCount.Value,
                    Name = items[i].NAME,
                    Description = items[i].DESCRIPTION,
                    EndDate = items[i].EndDate,
                    StartDate = items[i].StartDate,
                    NewApplicants = items[i].NewApplicants.Value,
                    SCLMatchPreference = items[i].sclMatchPreference.HasValue ? items[i].sclMatchPreference.Value : 0,
                    Shifts = shifts
                });    
            }
            return final;
        }

        //separates comma delimited string of shifts
        public static List<string> GetShifts(string list)
        {
            List<string> shifts = new List<string>();

            if(list.Length > 0)
                shifts = list.Split(",".ToCharArray()).ToList<string>();

            return shifts;
        }

        /// <summary>
        /// Get all subs for a client
        /// </summary>
        /// <param name="clientGuid"></param>
        /// <returns></returns>
        public static List<Client> GetSubClients(Guid clientGuid)
        {
            DataAccess da = new DataAccess();
            List<usp_GetSubClientsResult> clientsBase = da.GetSubClients(clientGuid).ToList();
            List<Client> clients = new List<Client>();
            foreach (usp_GetSubClientsResult c in clientsBase)
            {
                clients.Add(new Client
                {
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    City = c.CITY,
                    Description = c.DESCRIPTION,
                    Id = c.ID,
                    Name = c.NAME,
                    ParentId = c.ParentId,
                    State = c.STATE,
                    ZIP = c.ZIP,
                    MarketingBullets = GetMarketingBullets(c.ID)
                });
            }
            return clients;
        }


        /// <summary>
        /// Get all specialties for job and client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static List<usp_GetSpecialtiesResult> GetSpecialtiesByClientJob(Guid clientId, Guid jobId)
        {
            DataAccess da = new DataAccess();
            List<usp_GetSpecialtiesResult> specialties = new List<usp_GetSpecialtiesResult>();
            if (Clients.GetJobs(clientId, jobId).Count > 0)  //client is valid
            {
                List<usp_GetJobSpecialtiesResult> jobSpecialties = da.GetJobSpecialties(jobId, null);
                for (int i = 0; i < jobSpecialties.Count; ++i)
                {
                    specialties.Add(Specialties.GetSpecialty(jobSpecialties[i].SPECIALTY_ID));
                }
            }
            return specialties;
        }

        ///<summary>
        ///Get client
        ///</summary>
        public static Client GetClient(Guid clientGuid)
        {
            DataAccess da = new DataAccess();
            usp_GetClientsResult c = da.GetClient(clientGuid);
            Client client = new Client
            {
                Address1 = c.Address1,
                Address2 = c.Address2,
                City = c.CITY,
                Description = c.DESCRIPTION,
                Id = c.ID,
                Name = c.NAME,
                ParentId = c.ParentId,
                State = c.STATE,
                ZIP = c.ZIP,
                MarketingBullets = GetMarketingBullets(c.ID),
                NumOfActiveJobs = c.NumOfActiveJobs.HasValue ? c.NumOfActiveJobs.Value : 0,
                NumOfApplicants = c.NumOfApplicants.HasValue ? c.NumOfApplicants.Value : 0,
                NumOfPastJobs = c.NumOfPastJobs.HasValue ? c.NumOfPastJobs.Value : 0
            };

            return client;
        }

        ///<summary>
        ///Delete client
        ///</summary>
        public static void DeleteClient(Guid clientGuid)
        {
            DataAccess da = new DataAccess();
            da.DeleteClient(clientGuid);
        }

        /// <summary>
        /// Updates specific client by Client GUID
        /// </summary>
        /// <param name="client"></param>
        public static void UpdateClient(Client client)
        {
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateClient(client.Id, client.Name, client.Description, client.Address1, client.Address2, client.City,
                    client.State, client.ZIP, client.ParentId, client.SupplementalDescription, client.URLRoute, client.ProfileTemplateId);
            if (client.MarketingBullets != null && client.MarketingBullets.Length > 0)
            {
                da.DeleteMarketingBullets(client.Id);
                foreach(string mb in client.MarketingBullets)
                {
                    da.AddMarketingBullet(client.Id, mb);
                }
            }
        }

        /// <summary>
        /// Adds Client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Guid? AddClient(Client client)
        {
            DataAccess da = new DataAccess();
            Guid clientId = da.AddClient(client.Name, client.Description, client.Address1, client.Address2, client.City,
                    client.State, client.ZIP, client.ParentId.HasValue ? client.ParentId.Value : new Guid(), client.SupplementalDescription, client.URLRoute, client.ProfileTemplateId).Value;

            if(client.MarketingBullets != null)
            {
                foreach (string b in client.MarketingBullets)
                {
                    da.AddMarketingBullet(clientId, b);
                }
            }
            

            return clientId;
        }

        /// <summary>
        /// Adds Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static Guid? AddJob(Job job)
        {
            DataAccess da = new DataAccess();
            return da.AddJob(job.ClientId, job.Name, job.Description, job.StartDate, job.EndDate).Value;
        }

        /// <summary>
        /// Add job specialty association
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        public static void AddJobSpecialty(Guid jobId, int specialtyId)
        {
            DataAccess da = new DataAccess();
            da.AddJobSpecialty(jobId, specialtyId);
        }

        /// <summary>
        /// Delete job specialty association
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        public static void DeleteJobSpecialty(Guid jobId, int specialtyId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJobSpecialty(jobId, specialtyId);
        }

        ///<summary>
        ///Delete job
        ///</summary>
        public static void DeleteJob(Guid jobId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJob(jobId);
        }

        /// <summary>
        /// Updates specific job by job GUID
        /// </summary>
        /// <param name="job"></param>
        public static void UpdateJob(Job job)
        {
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateJob(job.Id, job.ClientId, job.Name, job.Description, job.StartDate, job.EndDate);
        }
    }
    #endregion
}
