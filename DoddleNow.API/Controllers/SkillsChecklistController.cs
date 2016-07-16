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
        [Route("{SkillsChecklistId}")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklist(Guid skillsChecklistId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklist(skillsChecklistId));
        }

        ///<summary>
        ///Add SkillsChecklist
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSkillsChecklist(SkillsChecklistModel skillsChecklistModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SkillsChecklists.AddSkillsChecklist(skillsChecklistModel);
            return Ok();
        }

        ///<summary>
        ///Update SkillsChecklist with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{skillsChecklistId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSkillsChecklist(Guid skillsChecklistId, SkillsChecklistModel skillsChecklistModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            skillsChecklistModel.SkillsChecklistGUID = skillsChecklistId;
            SkillsChecklists.UpdateSkillsChecklist(skillsChecklistModel);

            return Ok();
        }

        ///<summary>
        ///Delete SkillsChecklist
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{skillsChecklistId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklist(Guid skillsChecklistId)
        {
            SkillsChecklists.DeleteSkillsChecklist(skillsChecklistId);
            return Ok();
        }

        #region Questions

        ///<summary>
        ///Get all questions for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{skillsChecklistId}/questions")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklistQuestions(Guid skillsChecklistId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklistQuestions(skillsChecklistId));
        }

        ///<summary>
        ///Get question for Skill check list with id = id, question id
        ///</summary>
        [Authorize(Roles = "1,2,3,4,5,6")]
        [Route("{skillsChecklistId}/questions/{skillsChecklistQuestionId}")]
        [HttpGet]
        public IHttpActionResult GetSkillsChecklistQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId)
        {
            return Ok(SkillsChecklists.GetSkillsChecklistQuestion(skillsChecklistId, skillsChecklistQuestionId));
        }

        ///<summary>
        ///Create new questions for Skill check list with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{skillsChecklistId}/questions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSkillsChecklistQuestion(Guid skillsChecklistId, QuestionModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           SkillsChecklists.AddQuestion(skillsChecklistId, questionModel);
            

            return Ok();
        }

        
        ///<summary>
        ///Update SkillsChecklist with id = id
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{skillsChecklistId}/questions/{skillsChecklistQuestionId}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSkillsChecklistQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId, QuestionModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            SkillsChecklists.UpdateQuestion(skillsChecklistId, skillsChecklistQuestionId, questionModel);

            return Ok();
        }

        ///<summary>
        ///Delete SkillsChecklist
        ///</summary>
        [Authorize(Roles = "1")]
        [Route("{skillsChecklistId}/questions/{skillsChecklistQuestionId}")]
        [HttpDelete]
        public IHttpActionResult DeleteSkillsChecklistQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId)
        {
            SkillsChecklists.DeleteSkillsChecklistQuestions(skillsChecklistId, skillsChecklistQuestionId);
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
            return da.GetSkillsChecklistQuestions(skillsChecklistId).Where(v=>v.GUID == skillsChecklistQuestionId).FirstOrDefault();
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
        /// <param name="skillsChecklistModel"></param>
        /// <returns></returns>
        public static Guid AddSkillsChecklist(SkillsChecklistModel skillsChecklistModel)
        {
            DataAccess da = new DataAccess();
            return da.AddSkillsChecklist(skillsChecklistModel.Title, skillsChecklistModel.Description, skillsChecklistModel.Template);
        }

        /// <summary>
        /// Adds Question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public static int AddQuestion(Guid skillsChecklistId, QuestionModel questionModel)
        {
            DataAccess da = new DataAccess();
            return da.AddQuestion(skillsChecklistId, questionModel.Text, questionModel.QuestionTypeID, questionModel.Required).ID;
        }

        /// <summary>
        /// Update Question
        /// </summary>
        /// <param name="skillsChecklistId"></param>
        /// <param name="skillsChecklistQuestionId"></param>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public static void UpdateQuestion(Guid skillsChecklistId, Guid skillsChecklistQuestionId, QuestionModel questionModel)
        {
            DataAccess da = new DataAccess();
            da.UpdateQuestion(skillsChecklistId, skillsChecklistQuestionId, questionModel.Text, questionModel.QuestionTypeID, questionModel.Required, questionModel.Position);
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
        /// <param name="skillsChecklistModel"></param>
        /// <returns></returns>
        public static void UpdateSkillsChecklist(SkillsChecklistModel skillsChecklistModel)
        {
            DataAccess da = new DataAccess();
            da.UpdateSkillsChecklist(skillsChecklistModel.SkillsChecklistGUID, skillsChecklistModel.Title, skillsChecklistModel.Description, skillsChecklistModel.Template);
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
