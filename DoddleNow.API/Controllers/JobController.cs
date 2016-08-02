using DataAccessLayer;
using System;
using DoddleNow.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///Client controller.  Used to get client related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/v1/jobs")]
    public class JobController : ApiController
    {
        ///<summary>
        ///Get all jobs
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllJobs()
        {
            return Ok(Jobs.GetAllJobs());
        }

        ///<summary>
        ///Get job with id = id 
        ///</summary>
        [Route("{jobId}")]
        [HttpGet]
        public IHttpActionResult GetJob(Guid jobId)
        {
            return Ok(Jobs.GetJob(jobId));
        }

        ///<summary>
        ///Add Job
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Jobs.AddJob(job);

            return Ok();
        }

        ///<summary>
        ///Update JOb with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{jobId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateJob(Guid jobId, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            job.ID = jobId;

            Jobs.UpdateJob(job);

            return Ok();
        }

        ///<summary>
        ///Delete Job
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{jobId}")]
        [HttpDelete]
        public IHttpActionResult DeleteJob(Guid jobId)
        {
            Jobs.DeleteJob(jobId);
            return Ok();
        }

        ///<summary>
        ///Get job specialties for job id 
        ///</summary>
        [Route("{jobId}/specialties")]
        [HttpGet]
        public IHttpActionResult GetJobSpecialties(Guid jobId)
        {
            return Ok(Jobs.GetJobSpecialties(jobId));
        }


        ///<summary>
        ///Create new specialty for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/specialties")]
        [HttpPost]
        public IHttpActionResult AddSpecialty(Guid jobId, Specialty specialty)
        {
            int id = Specialties.AddSpecialty(specialty);
            Jobs.AddJobSpecialty(jobId, id);
            
            return Ok();
        }

        
        ///<summary>
        ///Delete Job Specialties
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/specialties")]
        [HttpDelete]
        public IHttpActionResult DeleteJobSpecialties(Guid jobId)
        {
            Jobs.DeleteJobSpecialties(jobId, null);
            return Ok();
        }



        ///<summary>
        ///Get specialty with id = id for job with id = id
        ///</summary>
        [Route("{jobId}/specialties/{specialtyId}")]
        [HttpGet]
        public IHttpActionResult GetJobSpecialty(Guid jobId, int specialtyId)
        {
            usp_GetSpecialtiesResult spec = new usp_GetSpecialtiesResult();
            if(Jobs.GetJobSpecialties(jobId).Where(v=>v.ID == specialtyId).Count() > 0)
            {
                spec = Specialties.GetSpecialty(specialtyId);
            }
            return Ok(spec);
        }


        ///<summary>
        ///Update specialty id for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/specialties/{specialtyId}")]
        [HttpPost]
        public IHttpActionResult UpdateSpecialty(Guid jobId, int specialtyId, Specialty specialty)
        {
            if (Jobs.GetJobSpecialties(jobId).Where(v => v.ID == specialtyId).Count() > 0)
            {
                specialty.ID = specialtyId;
                Specialties.UpdateSpecialty(specialty);
            }

            return Ok();
        }


        ///<summary>
        ///Delete Job Specialties
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/specialties/{specialtyId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSpecialty(Guid jobId, int specialtyId)
        {
            if (Jobs.GetJobSpecialties(jobId).Where(v => v.ID == specialtyId).Count() > 0)
            {
                Clients.DeleteJobSpecialty(jobId, specialtyId);
            }
            
            return Ok();
        }


        #region SkillsChecklists

        ///<summary>
        ///Get job SkillsChecklists for job id 
        ///</summary>
        [Route("{jobId}/scl")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklists(Guid jobId)
        {
            return Ok(Jobs.GetJobSkillsChecklists(jobId));
        }


        ///<summary>
        ///Create new SkillsChecklists for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl")]
        [HttpPost]
        public IHttpActionResult AddSkillsChecklists(Guid jobId, SkillsChecklist scl)
        {
            Guid id = SkillsChecklists.AddSkillsChecklist(scl);
            Jobs.AddJobSkillsChecklist(jobId, id);

            return Ok();
        }



        ///<summary>
        ///Get SkillsChecklists with id = id for job with id = id
        ///</summary>
        [Route("{jobId}/scl/{sclId}")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklists(Guid jobId, Guid sclId)
        {
            usp_GetSkillsChecklistsResult spec = new usp_GetSkillsChecklistsResult();
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                spec = SkillsChecklists.GetSkillsChecklist(sclId);
            }
            return Ok(spec);
        }


        ///<summary>
        ///Update SkillsChecklists id for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}")]
        [HttpPost]
        public IHttpActionResult UpdateSkillsChecklist(Guid jobId, Guid sclId, SkillsChecklist scl)
        {
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                scl.ID = sclId;
                SkillsChecklists.UpdateSkillsChecklist(scl);
            }

            return Ok();
        }


        ///<summary>
        ///Delete Job SkillsChecklists
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklist(Guid jobId, Guid sclId)
        {
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                SkillsChecklists.DeleteSkillsChecklist(sclId);
            }

            return Ok();
        }

        /////////////////////////////////////////////

        ///<summary>
        ///Get all questions for scl with id = id, job with id = id
        ///</summary>
        [Route("{jobId}/scl/{sclId}/questions")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklistQuestions(Guid jobId, Guid sclId)
        {
            List<usp_GetQuestionsResult> questions = new List<usp_GetQuestionsResult>();
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                questions = SkillsChecklists.GetSkillsChecklistQuestions(sclId);
            }

            return Ok(questions);
        }


        ///<summary>
        ///Create new question for scl with id = id, job with id = id 
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}/questions")]
        [HttpPost]
        public IHttpActionResult AddSkillsChecklistQuestion(Guid jobId, Guid sclId, Question question)
        {
            if(Jobs.GetJobSkillsChecklists(jobId).Where(v=>v.GUID == sclId).Count() > 0)
            {
                int id = SkillsChecklists.AddQuestion(sclId, question);
            }
            
            return Ok();
        }

        ///<summary>
        ///Delete all questions for scl with id = id, job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}/questions")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid jobId, Guid sclId)
        {
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                SkillsChecklists.DeleteSkillsChecklistQuestions(sclId, null);
            }

            return Ok();
        }



        ///<summary>
        ///Get question with id = id for scl with id = id, job with id = id
        ///</summary>
        [Route("{jobId}/scl/{sclId}/questions/{questionId}")]
        [HttpGet]
        public IHttpActionResult GetJobSkillsChecklistQuestion(Guid jobId, Guid sclId, Guid questionId)
        {
            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid

            usp_GetQuestionsResult spec = new usp_GetQuestionsResult();
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                spec = SkillsChecklists.GetSkillsChecklistQuestion(sclId, questionId);
            }
            return Ok(spec);
        }


        ///<summary>
        ///Update SkillsChecklists Question id for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}/questions/{questionId}")]
        [HttpPost]
        public IHttpActionResult UpdateSkillsChecklistQuestion(Guid jobId, Guid sclId, Guid questionId, Question question)
        {
            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                SkillsChecklists.UpdateQuestion(sclId, questionId, question);
            }

            return Ok();
        }


        ///<summary>
        ///Delete Job SkillsChecklist Question
        ///</summary>
        [Authorize(Roles = "1,2")]
        [Route("{jobId}/scl/{sclId}/questions/{questionId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid jobId, Guid sclId, Guid questionId)
        {
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.GUID == sclId).Count() > 0)
            {
                SkillsChecklists.DeleteSkillsChecklistQuestions(sclId, questionId);
            }

            return Ok();
        }
        #endregion



    }

    #region Helpers

    ///<summary>
    /// Jobs
    ///</summary>
    public class Jobs
    {
        ///<summary>
        ///Get all jobs
        ///</summary>
        public static List<usp_GetJobsResult> GetAllJobs()
        {
            DataAccess da = new DataAccess();
            return da.GetJobs(null, null).ToList();
        }



        ///<summary>
        ///Get job specialties
        ///</summary>
        public static List<usp_GetSpecialtiesResult> GetJobSpecialties(Guid jobId)
        {
            DataAccess da = new DataAccess();
            var ids = da.GetJobSpecialties(jobId, null).ToList();

            List<usp_GetSpecialtiesResult> specialties = new List<usp_GetSpecialtiesResult>();
            for (int i = 0; i < ids.Count; ++i)
            {
                specialties.Add(Specialties.GetSpecialty(ids[i].SPECIALTY_ID));
            }
            return specialties;
        }

        ///<summary>
        ///Get job skillsChecklists
        ///</summary>
        public static List<usp_GetSkillsChecklistsResult> GetJobSkillsChecklists(Guid jobId)
        {
            DataAccess da = new DataAccess();
            var ids = da.GetJobSkillsChecklist(jobId, null).ToList();

            List<usp_GetSkillsChecklistsResult> scls = new List<usp_GetSkillsChecklistsResult>();
            for (int i = 0; i < ids.Count; ++i)
            {
                scls.Add(SkillsChecklists.GetSkillsChecklist(ids[i].SkillsChecklistGUID));
            }
            return scls;
        }

        /// <summary>
        /// Get specific job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static usp_GetJobsResult GetJob(Guid jobId)
        {
            DataAccess da = new DataAccess();
            return da.GetJobs(null, jobId).FirstOrDefault();
        }

        /// <summary>
        /// Adds JOb
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static Guid AddJob(Job job)
        {
            DataAccess da = new DataAccess();
            return da.AddJob(job.ClientGUID, job.Name, job.Description, job.StartDate, job.EndDate).Value;
        }

        /// <summary>
        /// Adds JOb specialty relationship
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
        /// Adds JOb skillschecklists relationship
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static void AddJobSkillsChecklist(Guid jobId, Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            da.AddJobSkillsChecklist(jobId, skillsChecklistId);
        }

        /// <summary>
        /// Update Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static void UpdateJob(Job job)
        {
            DataAccess da = new DataAccess();
            da.UpdateJob(job.ID, job.ClientGUID, job.Name, job.Description, job.StartDate, job.EndDate);
        }

        /// <summary>
        /// Delete JOb
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static void DeleteJob(Guid jobId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJob(jobId);
        }

        /// <summary>
        /// Delete JOb specialties
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        public static void DeleteJobSpecialties(Guid jobId, int? specialtyId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJobSpecialty(jobId, specialtyId);
        }
    }
    #endregion
}
