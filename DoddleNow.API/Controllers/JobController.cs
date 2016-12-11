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
    ///Shift controller.  
    ///</summary>
    [RoutePrefix("api/v1/shifts")]
    public class ShiftController : ApiController
    {
        ///<summary>
        ///Get all shifts
        ///</summary>
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllShifts()
        {
            return Ok(Jobs.GetAllShifts());
        }
    }

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
            job.Id = jobId;

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
        ///Get job specialties for job id 
        ///</summary>
        [Route("{jobId}/shifts")]
        [HttpGet]
        public IHttpActionResult GetJobShifts(Guid jobId)
        {
            return Ok(Jobs.GetJobShifts(jobId));
        }


        ///<summary>
        ///Create new specialty for job with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{jobId}/shifts/{shiftId}")]
        [HttpPost]
        public IHttpActionResult AddShift(Guid jobId, int shiftId)
        {
            Jobs.AddJobShift(jobId, shiftId);

            return Ok();
        }

        ///<summary>
        ///Delete Job Shift
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{jobId}/shifts/{shiftId}")]
        [HttpDelete]
        public IHttpActionResult DeleteShift(Guid jobId, int shiftId)
        {
            if (Jobs.GetJobShifts(jobId).Where(v => v.ID == shiftId).Count() > 0)
            {
                Jobs.DeleteJobShift(jobId, shiftId);
            }

            return Ok();
        }

        ///<summary>
        ///Delete Job Shifts
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{jobId}/shifts")]
        [HttpDelete]
        public IHttpActionResult DeleteShifts(Guid jobId)
        {
            Jobs.DeleteJobShifts(jobId);
            
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
                specialty.Id = specialtyId;
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
            {
                scl.Id = sclId;
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if(Jobs.GetJobSkillsChecklists(jobId).Where(v=>v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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
            if (Jobs.GetJobSkillsChecklists(jobId).Where(v => v.ID == sclId).Count() > 0)
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

        /// <summary>
        /// Get job shifts
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static List<usp_GetJobShiftsResult> GetJobShifts(Guid jobId)
        {
            DataAccess da = new DataAccess();
            return da.GetJobShifts(jobId).ToList();
        }

        /// <summary>
        /// Add an individual job shift
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="shiftId"></param>
        public static void AddJobShift(Guid jobId, int shiftId)
        {
            DataAccess da = new DataAccess();
            da.AddJobShift(jobId, shiftId);
        }


        /// <summary>
        /// Delete a single job shift
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="shiftId"></param>
        public static void DeleteJobShift(Guid jobId, int shiftId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJobShift(jobId, shiftId);
        }

        /// <summary>
        /// Delete all job shifts
        /// </summary>
        /// <param name="jobId"></param>
        public static void DeleteJobShifts(Guid jobId)
        {
            DataAccess da = new DataAccess();
            da.DeleteJobShifts(jobId);
        }

        ///<summary>
        ///Get all shifts
        ///</summary>
        public static List<usp_GetShiftsResult> GetAllShifts()
        {
            DataAccess da = new DataAccess();
            return da.GetShifts().ToList();
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
            return da.AddJob(job.ClientId, job.Name, job.Description, job.StartDate, job.EndDate).Value;
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
            usp_GetJobsResult original = da.GetJobs(job.ClientId, job.Id).FirstOrDefault();
            if(original != null)
            {
                job.Name = job.Name == null ? original.NAME : job.Name;
                job.Description = job.Description == null ? original.DESCRIPTION : job.Description;
                job.StartDate = job.StartDate == null ? original.StartDate : job.StartDate.Value;
                job.EndDate = job.EndDate == null ? original.EndDate : job.EndDate.Value;
            }

            da.UpdateJob(job.Id, job.ClientId, job.Name, job.Description, job.StartDate, job.EndDate);
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



        public static List<Candidate> GetJobCandidates(Guid jobId)
        {
            DataAccess da = new DataAccess();
            List<Candidate> candidates = new List<Models.Candidate>();

            List<usp_GetJobCandidatesResult> p = da.GetJobCandidates(jobId);

            for(int i=0;i< p.Count; ++ i)
            {
                candidates.Add(new Candidate
                {
                    ApplicantApplied = p[i].applicantApplied,
                    ClientInterest = p[i].clientInterest,
                    ClientStarred = p[i].clientStarred,
                    CoffeeConnect = p[i].coffeeConnect,
                    EMail = p[i].Email,
                    Exclude = p[i].exclude,
                    FirstName = p[i].FirstName,
                    LastName = p[i].LastName,
                    Location = p[i].location,
                    LocationDistance = p[i].locationDistance,
                    UserId = Guid.Parse(p[i].UserId),
                    CandidateGuid = p[i].CANDIDATE_ALIAS_GUID,
                    SCLMatch = p[i].SCLMatch.HasValue ? p[i].SCLMatch.Value : 0,
                    YearsOfExperienceStr = p[i].YearsOfExperienceStr
                });
            }

            return candidates;
        }

        public static List<Candidate> GetClientCandidates(Guid clientId)
        {
            DataAccess da = new DataAccess();
            List<Candidate> candidates = new List<Models.Candidate>();

            List<usp_GetClientCandidatesResult> p = da.GetClientCandidates(clientId);

            for (int i = 0; i < p.Count; ++i)
            {
                candidates.Add(new Candidate
                {
                    ApplicantApplied = p[i].applicantApplied,
                    ClientInterest = p[i].clientInterest,
                    ClientStarred = p[i].clientStarred,
                    CoffeeConnect = p[i].coffeeConnect,
                    EMail = p[i].Email,
                    Exclude = p[i].exclude,
                    FirstName = p[i].FirstName,
                    LastName = p[i].LastName,
                    Location = p[i].location,
                    LocationDistance = p[i].locationDistance,
                    UserId = Guid.Parse(p[i].UserId),
                    CandidateGuid = p[i].CANDIDATE_ALIAS_GUID,
                    SCLMatch = p[i].SCLMatch.HasValue ? p[i].SCLMatch.Value : 0,
                    YearsOfExperienceStr = p[i].YearsOfExperienceStr
                });
            }
            

            return candidates;
        }

        public static Candidate GetJobCandidate(Guid jobId, string candidateId)
        {
            DataAccess da = new DataAccess();
            Candidate candidate = new Candidate();

            usp_GetJobCandidatesResult p = da.GetJobCandidate(jobId, candidateId);


            candidate = new Candidate
            {
                ApplicantApplied = p.applicantApplied,
                ClientInterest = p.clientInterest,
                ClientStarred = p.clientStarred,
                CoffeeConnect = p.coffeeConnect,
                EMail = p.Email,
                Exclude = p.exclude,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Location = p.location,
                LocationDistance = p.locationDistance,
                UserId = Guid.Parse(p.UserId),
                CandidateGuid = p.CANDIDATE_ALIAS_GUID,
                SCLMatch = p.SCLMatch.HasValue ? p.SCLMatch.Value : 0,
                YearsOfExperienceStr = p.YearsOfExperienceStr
            };

            return candidate;
        }

        public static void UpdateJobCandidate(Guid jobId, Candidate candidate)
        {
            DataAccess da = new DataAccess();
            Candidate c = GetJobCandidate(jobId, candidate.UserId.ToString());
            c.ClientInterest = candidate.ClientInterest == null ? c.ClientInterest.Value : candidate.ClientInterest.Value;
            c.ClientStarred = candidate.ClientStarred == null ? c.ClientStarred.Value : candidate.ClientStarred.Value;
            c.CoffeeConnect = candidate.CoffeeConnect == null ? c.CoffeeConnect.Value : candidate.CoffeeConnect.Value;
            c.Exclude = candidate.Exclude == null ? c.Exclude.Value : candidate.Exclude.Value;
            c.ApplicantApplied = candidate.ApplicantApplied == null ? c.ApplicantApplied.Value : candidate.ApplicantApplied.Value;

            da.UpdateJobCandidate(jobId, c.UserId.ToString(), c.ClientInterest.Value, c.ClientStarred.Value, c.CoffeeConnect.Value, c.ApplicantApplied.Value, c.Exclude.Value);
        }

    }
    #endregion
}
