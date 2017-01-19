using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class QuestionRank
    {
        ///<summary>
        ///SkillsChecklistQuestionId
        ///</summary>
        [Required]
        [Display(Name = "SkillsChecklistQuestionId")]
        public Guid SkillsChecklistQuestionId { get; set; }

        ///<summary>
        ///QuestionTypeId
        ///</summary>
        [Display(Name = "QuestionTypeId")]
        public int QuestionTypeID { get; set; }

        ///<summary>
        ///QuestionText
        ///</summary>
        [Display(Name = "QuestionText")]
        public string QuestionText { get; set; }

        ///<summary>
        ///Position
        ///</summary>
        [Display(Name = "Position")]
        public int Position { get; set; }

        ///<summary>
        ///Required
        ///</summary>
        [Display(Name = "Required")]
        public bool Required { get; set; }

        ///<summary>
        ///Least acceptable value on a ranked question (int)
        ///</summary>
        [Display(Name = "Rank")]
        public int? Rank { get; set; }

        
    }
}
