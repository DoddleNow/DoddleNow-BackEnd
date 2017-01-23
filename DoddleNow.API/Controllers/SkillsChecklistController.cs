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
    [RoutePrefix("api/v1/scl")]
    public class SkillsChecklistController : ApiController
    {
        ///<summary>
        ///Get all SkillsChecklists
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllSkillsChecklists()
        {
            return Ok(SkillsChecklists.GetAllSkillsChecklists());
        }

        ///<summary>
        ///Get question types
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("questiontypes")]
        [HttpGet]
        public IHttpActionResult GetQuestionTypes()
        {
            return Ok(SkillsChecklists.GetQuestionTypes());
        }

        ///<summary>
        ///Get SkillsChecklist with id = id 
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklist(Guid sclId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklist(sclId));
        }

        ///<summary>
        ///Add SkillsChecklist
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSkillsChecklist(SkillsChecklist scl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid gid = SkillsChecklists.AddSkillsChecklist(scl);
            return Ok(gid);
        }

        ///<summary>
        ///Update SkillsChecklist with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{sclId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSkillsChecklist(Guid sclId, SkillsChecklist scl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            scl.Id = sclId;

            SkillsChecklist orig = SkillsChecklists.GetSkillsChecklist(sclId);
            
            if(orig != null)
            {
                scl.Description = scl.Description == null ? orig.Description : scl.Description;
                scl.Template = scl.Template == null ? orig.Template : scl.Template;
                scl.Title = scl.Title == null ? orig.Title : scl.Title;
            }

            SkillsChecklists.UpdateSkillsChecklist(scl);

            return Ok();
        }

        ///<summary>
        ///Delete SkillsChecklist
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{sclId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklist(Guid sclId)
        {
            SkillsChecklists.DeleteSkillsChecklist(sclId);
            return Ok();
        }

        #region Questions

        ///<summary>
        ///Get all questions for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}/questions")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklistQuestions(Guid sclId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklistQuestions(sclId));
        }


        ///<summary>
        ///Get all questions with answers for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}/questions/{userId}/answers")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklistQuestionsWithAnswers(Guid sclId, string userId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklistQuestionsAnswers(sclId, userId));
        }

        ///<summary>
        ///Get question for Skill check list with id = id, question id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{sclId}/questions/{questionId}")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklistQuestion(Guid sclId, Guid questionId)
        {
            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
            return Ok(SkillsChecklists.GetSkillsChecklistQuestion(sclId, questionId));
        }

        ///<summary>
        ///Create new questions for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{sclId}/questions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSkillsChecklistQuestion(Guid sclId, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           SkillsChecklists.AddQuestion(sclId, question);
            

            return Ok();
        }

        
        ///<summary>
        ///Update SkillsChecklist with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{sclId}/questions/{questionId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSkillsChecklistQuestion(Guid sclId, Guid questionId, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usp_GetQuestionsResult orig = SkillsChecklists.GetSkillsChecklistQuestion(sclId, questionId);

            if(orig != null)
            {
                question.Position = question.Position == null ? orig.POSITION : question.Position;
                question.QuestionTypeID = question.QuestionTypeID == null ? orig.QuestionTypeID : question.QuestionTypeID;
                question.Required = question.Required == null ? orig.REQUIRED : question.Required;
                question.Text = question.Text == null ? orig.Text : question.Text;
            }

            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
            SkillsChecklists.UpdateQuestion(sclId, questionId, question);

            return Ok();
        }

        ///<summary>
        ///Delete SkillsChecklist Question
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{sclId}/questions/{questionId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid sclId, Guid questionId)
        {
            //exposing the SkillsChecklistQuestionId as a questionId to the front end.  They are not concerned with sclQID vs qid
            SkillsChecklists.DeleteSkillsChecklistQuestions(sclId, questionId);
            return Ok();
        }


        ///<summary>
        ///Add answers to question
        ///</summary>
        [Authorize(Roles = "6")]
        [Route("{sclId}/answers")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSkillsChecklistQuestionAnswer(Guid sclId, List<QuestionAnswer> answers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            
            SkillsChecklists.AddAnswers(sclId, userId, answers);

            return Ok();
        }

        



        #endregion

    }

    #region Helpers

    ///<summary>
    /// SkillsChecklists
    ///</summary>
    public class SkillsChecklists
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

            da.DeleteSkillsChecklistAnswers(skillsChecklistId);

            for(int i=0;i<answers.Count;++i)
            {
                da.AddAnswer(skillsChecklistId, answers[i].SkillsChecklistQuestionId, userId, answers[i].AnswerValue);
            }
        }

        /// <summary>
        /// Add all rankings for a client's SCL
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="clientId"></param>
        /// <param name="rankings"></param>
        public static void AddRankings(Guid skillsChecklistId, Guid clientId, List<QuestionRank> rankings, Guid? jobId)
        {
            DataAccess da = new DataAccess();

            da.DeleteSkillsChecklistClientRankings(skillsChecklistId, clientId, jobId);

            for (int i = 0; i < rankings.Count; ++i)
            {
                da.AddRanking(skillsChecklistId, rankings[i].SkillsChecklistQuestionId, clientId, rankings[i].Rank.HasValue ? rankings[i].Rank.Value : 0, jobId);
            }
        }


        /// <summary>
        /// Get skillschecklist rankings for a client
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static List<usp_GetQuestionsWithRankingsResult> GetSkillsChecklistQuestionsRankings(Guid skillsChecklistId, Guid clientId, Guid? jobId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestionsWithRankings(skillsChecklistId, clientId, jobId).ToList();
        }

        /// <summary>
        /// Get skillschecklist questions
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static List<usp_GetQuestionsResult> GetSkillsChecklistQuestions(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestions(skillsChecklistId);
        }

        /// <summary>
        /// Get skillschecklist questions with answers
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static List<usp_GetQuestionsWithAnswersResult> GetSkillsChecklistQuestionsAnswers(Guid skillsChecklistId, string userId)
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
        /// Get skillschecklist question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="skillsChecklistQuestionId"></param>
        /// <returns></returns>
        public static usp_GetQuestionsResult GetSkillsChecklistQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklistQuestions(skillsChecklistId).Where(v=>v.Id == skillsChecklistQuestionId).FirstOrDefault();
        }

        ///<summary>
        ///Get all SkillsChecklists
        ///</summary>
        public static List<usp_GetSkillsChecklistsResult> GetAllSkillsChecklists()
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklists(null).ToList();
        }

        /// <summary>
        /// Get specific SkillsChecklist
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static SkillsChecklist GetSkillsChecklist(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            SkillsChecklist scl = null;
            usp_GetSkillsChecklistsResult item = da.GetSkillsChecklists(skillsChecklistId).FirstOrDefault();
            if(item != null)
            {
                scl = new SkillsChecklist();
                scl.Description = item.DESCRIPTION;
                scl.Template = item.TEMPLATE;
                scl.Title = item.TITLE;
                scl.Id = item.ID;
            }

            return scl;
        }

        /// <summary>
        /// Adds SkillsChecklist
        /// </summary>
        /// <param name="skillsChecklist"></param>
        /// <returns></returns>
        public static Guid AddSkillsChecklist(SkillsChecklist skillsChecklist)
        {
            DataAccess da = new DataAccess();
            return da.AddSkillsChecklist(skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template.HasValue ? skillsChecklist.Template.Value : false);
        }

        /// <summary>
        /// Adds Question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int AddQuestion(Guid skillsChecklistId, Question question)
        {
            DataAccess da = new DataAccess();
            return da.AddQuestion(skillsChecklistId, question.Text, question.QuestionTypeID.Value, question.Required.HasValue ? question.Required.Value : false).ID;
        }

        /// <summary>
        /// Update Question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="skillsChecklistQuestionId"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static void UpdateQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId, Question question)
        {
            DataAccess da = new DataAccess();
            da.UpdateQuestion(skillsChecklistId, skillsChecklistQuestionId, question.Text, question.QuestionTypeID.Value, question.Required.HasValue ? question.Required.Value : false, question.Position);
        }

        /// <summary>
        /// Delete all questions for id = id
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="skillsChecklistQuestionId"></param>
        /// <returns></returns>
        public static void DeleteSkillsChecklistQuestions(Guid skillsChecklistId, Guid? skillsChecklistQuestionId)
        {
            DataAccess da = new DataAccess();
            da.DeleteQuestions(skillsChecklistId, skillsChecklistQuestionId);
        }

        /// <summary>
        /// Update SkillsChecklist
        /// </summary>
        /// <param name="skillsChecklist"></param>
        /// <returns></returns>
        public static void UpdateSkillsChecklist(SkillsChecklist skillsChecklist)
        {
            DataAccess da = new DataAccess();
            da.UpdateSkillsChecklist(skillsChecklist.Id, skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template.HasValue ? skillsChecklist.Template.Value : false);
        }

        /// <summary>
        /// Delete SkillsChecklist
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <returns></returns>
        public static void DeleteSkillsChecklist(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            da.DeleteSkillsChecklist(skillsChecklistId);
        }
    }
    #endregion
}
