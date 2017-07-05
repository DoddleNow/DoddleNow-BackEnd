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
    ///Client controller.  Used to get client related information across the whole system or an individual
    ///</summary>
    [RoutePrefix("api/v1/assessments")]
    public class AssessmentsController : ApiController
    {
        ///<summary>
        ///Get all Assessments
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllAssessments()
        {
            return Ok(Assessments.GetAllAssessments());
        }

        ///<summary>
        ///Get question types
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("questiontypes")]
        [HttpGet]
        public IHttpActionResult GetQuestionTypes()
        {
            return Ok(Assessments.GetQuestionTypes());
        }

        ///<summary>
        ///Get Assessment with id = id 
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}")]
        [HttpGet]
        public IHttpActionResult GetAssessment(Guid sclId)
        {
            return Ok(Assessments.GetAssessment(sclId));
        }

        ///<summary>
        ///Get Assessment with id = id 
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("user/{userId}")]
        [HttpGet]
        public IHttpActionResult GetUserAssessments(string userId)
        {
            return Ok(Assessments.GetUserAssessments(userId));
        }

        /////<summary>
        /////Add SkillsChecklist
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("")]
        //[HttpPost]
        //public async Task<IHttpActionResult> AddSkillsChecklist(SkillsChecklist scl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Guid gid = SkillsChecklists.AddSkillsChecklist(scl);
        //    return Ok(gid);
        //}

        /////<summary>
        /////Update SkillsChecklist with id = id
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("{sclId}")]
        //[HttpPost]
        //public async Task<IHttpActionResult> UpdateSkillsChecklist(Guid sclId, SkillsChecklist scl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    scl.Id = sclId;

        //    SkillsChecklist orig = SkillsChecklists.GetSkillsChecklist(sclId);
            
        //    if(orig != null)
        //    {
        //        scl.Description = scl.Description == null ? orig.Description : scl.Description;
        //        scl.Template = scl.Template == null ? orig.Template : scl.Template;
        //        scl.Title = scl.Title == null ? orig.Title : scl.Title;
        //    }

        //    SkillsChecklists.UpdateSkillsChecklist(scl);

        //    return Ok();
        //}

        /////<summary>
        /////Delete SkillsChecklist
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("{sclId}")]
        //[HttpDelete]
        //public IHttpActionResult DeleteSkillsChecklist(Guid sclId)
        //{
        //    SkillsChecklists.DeleteSkillsChecklist(sclId);
        //    return Ok();
        //}

        #region Questions

        ///<summary>
        ///Get all questions for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}/questions")]
        [HttpGet]
        public IHttpActionResult GetAssessmentQuestions(Guid sclId)
        {
            return Ok(Assessments.GetAssessmentQuestions(sclId));
        }


        ///<summary>
        ///Get all questions with answers for an assessment with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5")]
        [Route("{sclId}/questions/{userId}/answers")]
        [HttpGet]
        public IHttpActionResult GetAssessmentQuestionsWithAnswers(Guid sclId, string userId)
        {
            SkillsChecklist s = SkillsChecklists.GetSkillsChecklist(sclId);
            SCLWithQuestions scl = new Models.SCLWithQuestions();
            scl.Id = s.Id;
            scl.Title = s.Title;
            List<QuestionWithAnswer> questions = SkillsChecklists.GetSkillsChecklistQuestionsAnswers(sclId, userId);
            scl.Questions = questions;
            return Ok(SkillsChecklists.GetSkillsChecklistQuestionsAnswers(sclId, userId));
        }

        ///<summary>
        ///Get question for assessment with id = id, question id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}/questions/{questionId}")]
        [HttpGet]
        public IHttpActionResult GetAssessmentQuestion(Guid sclId, Guid questionId)
        {
            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
            return Ok(SkillsChecklists.GetSkillsChecklistQuestion(sclId, questionId));
        }

        /////<summary>
        /////Create new questions for assessment with id = id
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("{sclId}/questions")]
        //[HttpPost]
        //public async Task<IHttpActionResult> AddAssessmentQuestion(Guid sclId, Question question)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //   SkillsChecklists.AddQuestion(sclId, question);
            

        //    return Ok();
        //}

        
        /////<summary>
        /////Update SkillsChecklist with id = id
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("{sclId}/questions/{questionId}")]
        //[HttpPost]
        //public async Task<IHttpActionResult> UpdateSkillsChecklistQuestion(Guid sclId, Guid questionId, Question question)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    usp_GetQuestionsResult orig = SkillsChecklists.GetSkillsChecklistQuestion(sclId, questionId);

        //    if(orig != null)
        //    {
        //        question.Position = question.Position == null ? orig.POSITION : question.Position;
        //        question.QuestionTypeID = question.QuestionTypeID == null ? orig.QuestionTypeID : question.QuestionTypeID;
        //        question.Required = question.Required == null ? orig.REQUIRED : question.Required;
        //        question.Text = question.Text == null ? orig.Text : question.Text;
        //    }

        //    //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
        //    SkillsChecklists.UpdateQuestion(sclId, questionId, question);

        //    return Ok();
        //}

        /////<summary>
        /////Delete SkillsChecklist Question
        /////</summary>
        //[Authorize(Roles = "1")]
        //[Route("{sclId}/questions/{questionId}")]
        //[HttpDelete]
        //public IHttpActionResult DeleteSkillsChecklistQuestion(Guid sclId, Guid questionId)
        //{
        //    //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
        //    SkillsChecklists.DeleteSkillsChecklistQuestions(sclId, questionId);
        //    return Ok();
        //}


        ///<summary>
        ///Add answers to question
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("{sclId}/answers")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAssessmentQuestionAnswer(Guid sclId, List<QuestionAnswer> answers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;

            Assessments.AddAnswers(sclId, userId, answers);

            return Ok();
        }

        



        #endregion

    }

    #region Helpers

    ///<summary>
    /// Assessments
    ///</summary>
    public class Assessments
    {
        /// <summary>
        /// Add individual answers from object
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="userId"></param>
        /// <param name="answers"></param>
        public static void AddAnswers(Guid skillsChecklistId, string userId, List<QuestionAnswer> answers)
        {
            DataAccess da = new DataAccess();

            for(int i=0;i<answers.Count;++i)
            {
                da.AddAnswer(skillsChecklistId, answers[i].SkillsChecklistQuestionId, userId, answers[i].AnswerValue);
            }
        }
        

        /// <summary>
        /// Get Assessment questions
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static List<usp_GetQuestionsResult> GetAssessmentQuestions(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestions(skillsChecklistId);
        }

        public static List<UserAssessment> GetUserAssessments(string userId)
        {
            DataAccess da = new DataAccess();
            List<UserAssessment> ua = new List<Models.UserAssessment>();
            List<usp_GetUserAssessmentsResult> assess = da.GetUserAssessments(userId).ToList();
            for(int i=0;i<assess.Count;++i)
            {
                ua.Add(new UserAssessment { AnswerCount = assess[i].ANSWER_COUNT.HasValue ? assess[i].ANSWER_COUNT.Value : 0,
                    Description = assess[i].DESCRIPTION, EFFDT = assess[i].EFFDT, Id = assess[i].ID, QuestionCount = assess[i].QUESTION_COUNT.HasValue ? assess[i].QUESTION_COUNT.Value : 0,
                    Template = assess[i].TEMPLATE, Title = assess[i].TITLE });

            }
            return ua;
        }

        /// <summary>
        /// Get assessment questions with answers
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static List<usp_GetQuestionsWithAnswersResult> GetAssessmentQuestionsAnswers(Guid skillsChecklistId, string userId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestionsWithAnswers(skillsChecklistId, userId);
        }

        /// <summary>
        /// Get question types
        /// </summary>
        /// <returns></returns>
        public static List<usp_GetQuestionTypesResult> GetQuestionTypes()
        {
            DataAccess da = new DataAccess();
            return da.GetQuestionTypes();
        }

        /// <summary>
        /// Get assessment question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="skillsChecklistQuestionId"></param>
        /// <returns></returns>
        public static usp_GetQuestionsResult GetAssessmentQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestions(skillsChecklistId).Where(v=>v.Id == skillsChecklistQuestionId).FirstOrDefault();
        }

        ///<summary>
        ///Get all assessments
        ///</summary>
        public static List<usp_GetAssessmentsResult> GetAllAssessments()
        {
            DataAccess da = new DataAccess();
            return da.GetAssessments(null).ToList();
        }

        /// <summary>
        /// Get specific Assessment
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static usp_GetAssessmentsResult GetAssessment(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            usp_GetAssessmentsResult item = da.GetAssessments(skillsChecklistId).FirstOrDefault();
            return item;
        }

        ///// <summary>
        ///// Adds SkillsChecklist
        ///// </summary>
        ///// <param name="skillsChecklist"></param>
        ///// <returns></returns>
        //public static Guid AddSkillsChecklist(SkillsChecklist skillsChecklist)
        //{
        //    DataAccess da = new DataAccess();
        //    return da.AddSkillsChecklist(skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template.HasValue ? skillsChecklist.Template.Value : false);
        //}

        ///// <summary>
        ///// Adds Question
        ///// </summary>
        ///// <param name="skillsChecklistId"></param>
        ///// <param name="question"></param>
        ///// <returns></returns>
        //public static int AddQuestion(Guid skillsChecklistId, Question question)
        //{
        //    DataAccess da = new DataAccess();
        //    return da.AddQuestion(skillsChecklistId, question.Text, question.QuestionTypeID.Value, question.Required.HasValue ? question.Required.Value : false).ID;
        //}

        ///// <summary>
        ///// Update Question
        ///// </summary>
        ///// <param name="skillsChecklistId"></param>
        ///// <param name="skillsChecklistQuestionId"></param>
        ///// <param name="question"></param>
        ///// <returns></returns>
        //public static void UpdateQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId, Question question)
        //{
        //    DataAccess da = new DataAccess();
        //    da.UpdateQuestion(skillsChecklistId, skillsChecklistQuestionId, question.Text, question.QuestionTypeID.Value, question.Required.HasValue ? question.Required.Value : false, question.Position);
        //}

        ///// <summary>
        ///// Delete all questions for id = id
        ///// </summary>
        ///// <param name="skillsChecklistId"></param>
        ///// <param name="skillsChecklistQuestionId"></param>
        ///// <returns></returns>
        //public static void DeleteSkillsChecklistQuestions(Guid skillsChecklistId, Guid? skillsChecklistQuestionId)
        //{
        //    DataAccess da = new DataAccess();
        //    da.DeleteQuestions(skillsChecklistId, skillsChecklistQuestionId);
        //}

        ///// <summary>
        ///// Update SkillsChecklist
        ///// </summary>
        ///// <param name="skillsChecklist"></param>
        ///// <returns></returns>
        //public static void UpdateSkillsChecklist(SkillsChecklist skillsChecklist)
        //{
        //    DataAccess da = new DataAccess();
        //    da.UpdateSkillsChecklist(skillsChecklist.Id, skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template.HasValue ? skillsChecklist.Template.Value : false);
        //}

        ///// <summary>
        ///// Delete SkillsChecklist
        ///// </summary>
        ///// <param name="skillsChecklistId"></param>
        ///// <returns></returns>
        //public static void DeleteSkillsChecklist(Guid skillsChecklistId)
        //{
        //    DataAccess da = new DataAccess();
        //    da.DeleteSkillsChecklist(skillsChecklistId);
        //}
    }
    #endregion
}
