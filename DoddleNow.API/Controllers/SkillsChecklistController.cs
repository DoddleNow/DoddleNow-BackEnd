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

        #endregion

    }

    #region Helpers

    ///<summary>
    /// SkillsChecklists
    ///</summary>
    public class SkillsChecklists
    {
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
        public static usp_GetSkillsChecklistsResult GetSkillsChecklist(Guid skillsChecklistId)
        {
            DataAccess da = new DataAccess();
            return da.GetSkillsChecklists(skillsChecklistId).FirstOrDefault();
        }

        /// <summary>
        /// Adds SkillsChecklist
        /// </summary>
        /// <param name="skillsChecklist"></param>
        /// <returns></returns>
        public static Guid AddSkillsChecklist(SkillsChecklist skillsChecklist)
        {
            DataAccess da = new DataAccess();
            return da.AddSkillsChecklist(skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template);
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
            return da.AddQuestion(skillsChecklistId, question.Text, question.QuestionTypeID, question.Required).ID;
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
            da.UpdateQuestion(skillsChecklistId, skillsChecklistQuestionId, question.Text, question.QuestionTypeID, question.Required, question.Position);
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
            da.UpdateSkillsChecklist(skillsChecklist.Id, skillsChecklist.Title, skillsChecklist.Description, skillsChecklist.Template);
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
