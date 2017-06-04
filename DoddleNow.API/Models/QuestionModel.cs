using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Question
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///QuestionTypeID
        ///</summary>
        [Required]
        [Display(Name = "QuestionTypeID")]
        public int? QuestionTypeID { get; set; }

        ///<summary>
        ///Question Text
        ///</summary>
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        ///<summary>
        ///Question position
        ///</summary>
        [Display(Name = "Position")]
        public int? Position { get; set; }

        ///<summary>
        ///Question Required
        ///</summary>
        [Display(Name = "Required")]
        public bool? Required { get; set; }

        /// <summary>
        /// Options for multiple choice
        /// </summary>
        [Display(Name ="Options")]
        public string[] Options { get; set; }


    }

    public class QuestionWithAnswer: Question 
    {
        [Display(Name = "SkillsChecklistQuestionId")]
        public Guid SkillsChecklistQuestionId { get; set; }


        [Display(Name ="Answer")]
        public string Answer { get; set; }

    }

    public class QuestionOption
    {
        
        [Display(Name ="OptionText")]
        public string OptionText { get; set; }
    }
}
