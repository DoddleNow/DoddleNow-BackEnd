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
        public async Task<IHttpActionResult> AddClient(ClientModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid newVal = Clients.AddClient(clientModel).Value;
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
        public async Task<IHttpActionResult> UpdateClient(Guid clientId, ClientModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            clientModel.ClientGUID = clientId;

            Clients.UpdateClient(clientModel);

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
        [Route("{parentId}/clients")]
        [HttpPost]
        public IHttpActionResult AddSubClient(Guid parentId, ClientModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            clientModel.ParentGUID = parentId;

            Guid newVal = Clients.AddClient(clientModel).Value;
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
        public async Task<IHttpActionResult> AddClientUser(Guid clientId, UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userModel.ClientGUID = clientId;

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            else
            {
                //add additional user info to database
                DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
                da.UpdateUser(userModel.ID, userModel.RoleID, userModel.EMail, userModel.FirstName, userModel.LastName, userModel.Phone, userModel.Title, userModel.Department, userModel.ClientGUID);
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
            var users = Users.GetAllUsers(null, clientId).Where(v => v.ID == userId.ToString()).FirstOrDefault();
            return Ok(users);
        }


        ///<summary>
        ///Update client user by userId
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/users/{userId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientUser(Guid clientId, Guid userId, UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userModel.ClientGUID = clientId;
            userModel.ID = userId;
            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateUser(userModel.ID, userModel.RoleID, userModel.EMail, userModel.FirstName, userModel.LastName, userModel.Phone, userModel.Title, userModel.Department, userModel.ClientGUID);

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
        public IHttpActionResult AddJob(Guid clientId, JobModel jobModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            jobModel.ClientGUID = clientId;

            Guid newVal = Clients.AddJob(jobModel).Value;
            return Ok(newVal);
        }

        ///<summary>
        ///Update job user by clientId and jobId
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{clientId}/jobs/{jobId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateClientJob(Guid clientId, Guid jobId, JobModel jobModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            jobModel.ClientGUID = clientId;
            jobModel.ID = jobId;
            //add additional user info to database
            DataAccessLayer.DataAccess da = new DataAccessLayer.DataAccess();
            da.UpdateJob(jobModel.ID, jobModel.ClientGUID, jobModel.Name, jobModel.Description, jobModel.StartDate.HasValue ? jobModel.StartDate : null, jobModel.EndDate.HasValue ? jobModel.EndDate : null);

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
        public IHttpActionResult AddJobSpecialty(Guid clientId, Guid jobId, SpecialtyModel specialtyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check that job is valid for client
            if (Clients.GetJobs(clientId, jobId).Count > 0)
            {
                //add specialty
                int specialtyId = Specialties.AddSpecialty(specialtyModel);
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
        public IHttpActionResult UpdateJobSpecialty(Guid clientId, Guid jobId, int specialtyId, SpecialtyModel specialtyModel)
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
                    specialtyModel.ID = specialtyId;
                    //allow specialty update
                    Specialties.UpdateSpecialty(specialtyModel);
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
        public IHttpActionResult AddSkillsChecklists(Guid clientId, Guid jobId, SkillsChecklistModel skillsChecklistModel)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                Guid id = SkillsChecklists.AddSkillsChecklist(skillsChecklistModel);
                Jobs.AddJobSkillsChecklist(jobId, id);
            }   

            return Ok();
        }



        ///<summary>
        ///Get Skill check list id for job id , client id
        ///</summary>
        [Route("{clientId}/jobs/{jobId}/scl/{skillsChecklistId}")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklists(Guid clientId, Guid jobId, Guid skillsChecklistId)
        {
            usp_GetSkillsChecklistsResult spec = new usp_GetSkillsChecklistsResult();
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    spec = SkillsChecklists.GetSkillsChecklist(skillsChecklistId);
                }
            }
                
            return Ok(spec);
        }


        ///<summary>
        ///Update Get Skill check list id for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{skillsChecklistId}")]
        [HttpPost]
        public IHttpActionResult UpdateSkillsChecklist(Guid clientId, Guid jobId, Guid skillsChecklistId, SkillsChecklistModel skillsChecklistModel)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    skillsChecklistModel.SkillsChecklistGUID = skillsChecklistId;
                    SkillsChecklists.UpdateSkillsChecklist(skillsChecklistModel);
                }
            }
            return Ok();
        }


        ///<summary>
        ///Delete Get Skill check list id for job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{skillsChecklistId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklist(Guid clientId, Guid jobId, Guid skillsChecklistId)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    SkillsChecklists.DeleteSkillsChecklist(skillsChecklistId);
                }
            }   

            return Ok();
        }

        /////////////////////////////////////////////

        ///<summary>
        ///Get all questions for scl id, job id , client id
        ///</summary>
        [Route("{clientId}/jobs/{jobId}/scl/{skillsChecklistId}/questions")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklistQuestions(Guid clientId, Guid jobId, Guid skillsChecklistId)
        {
            List<usp_GetQuestionsResult> questions = new List<usp_GetQuestionsResult>();
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    questions = SkillsChecklists.GetSkillsChecklistQuestions(skillsChecklistId);
                }
            }
                return Ok(questions);
        }


        ///<summary>
        ///Create new question for scl id, job id , client id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{clientId}/jobs/{jobId}/scl/{skillsChecklistId}/questions")]
        [HttpPost]
        public IHttpActionResult AddSkillsChecklistQuestion(Guid clientId, Guid jobId, Guid skillsChecklistId, QuestionModel questionModel)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    int id = SkillsChecklists.AddQuestion(skillsChecklistId, questionModel);
                }
            }
            return Ok();
        }

        ///<summary>
        ///Delete all questions for scl with id = id, job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{skillsChecklistId}/questions")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid clientId, Guid jobId, Guid skillsChecklistId)
        {
            if (Clients.GetJobs(clientId, jobId).Count() > 0)
            {
                if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == skillsChecklistId).Count() > 0)
                {
                    SkillsChecklists.DeleteSkillsChecklistQuestions(skillsChecklistId, null);
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
        public static List<usp_GetClientsResult> GetAllClients()
        {
            DataAccess da = new DataAccess();
            return da.GetClients().ToList();
        }

        ///<summary>
        ///Get all jobs for client with id = id
        ///</summary>
        public static List<usp_GetJobsResult> GetJobs(Guid clientId, Guid? jobId)
        {
            DataAccess da = new DataAccess();
            return da.GetJobs(clientId, jobId).ToList();
        }

        /// <summary>
        /// Get all subs for a client
        /// </summary>
        /// <param name="clientGuid"></param>
        /// <returns></returns>
        public static List<usp_GetSubClientsResult> GetSubClients(Guid clientGuid)
        {
            DataAccess da = new DataAccess();
            return da.GetSubClients(clientGuid).ToList();

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
        public static usp_GetClientsResult GetClient(Guid clientGuid)
        {
            DataAccess da = new DataAccess();
            usp_GetClientsResult client = da.GetClient(clientGuid);

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
        /// <param name="clientModel"></param>
        public static void UpdateClient(ClientModel clientModel)
        {
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateClient(clientModel.ClientGUID, clientModel.Name, clientModel.Description, clientModel.Address1, clientModel.Address2, clientModel.City,
                    clientModel.State, clientModel.ZIP, clientModel.ParentGUID);
        }

        /// <summary>
        /// Adds Client
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        public static Guid? AddClient(ClientModel clientModel)
        {
            DataAccess da = new DataAccess();
            return da.AddClient(clientModel.Name, clientModel.Description, clientModel.Address1, clientModel.Address2, clientModel.City,
                    clientModel.State, clientModel.ZIP, clientModel.ParentGUID).Value;
        }

        /// <summary>
        /// Adds Job
        /// </summary>
        /// <param name="jobModel"></param>
        /// <returns></returns>
        public static Guid? AddJob(JobModel jobModel)
        {
            DataAccess da = new DataAccess();
            return da.AddJob(jobModel.ClientGUID, jobModel.Name, jobModel.Description, jobModel.StartDate, jobModel.EndDate).Value;
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
        /// <param name="jobModel"></param>
        public static void UpdateJob(JobModel jobModel)
        {
            //add additional user info to database
            DataAccess da = new DataAccess();
            da.UpdateJob(jobModel.ID, jobModel.ClientGUID, jobModel.Name, jobModel.Description, jobModel.StartDate, jobModel.EndDate);
        }
    }
    #endregion
}
