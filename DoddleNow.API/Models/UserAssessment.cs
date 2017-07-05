using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    
    public class UserAssessment
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///Title
        ///</summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        ///<summary>
        ///Survey description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        ///<summary>
        ///Effective Date
        ///</summary>
        [Display(Name = "EFFDT")]
        public DateTime EFFDT { get; set; }

        
        ///<summary>
        ///Template?
        ///</summary>
        [Display(Name = "Template")]
        public bool Template { get; set; }

        ///<summary>
        ///Question Count
        ///</summary>
        [Display(Name = "QuestionCount")]
        public int QuestionCount { get; set; }

        ///<summary>
        ///Answer Count
        ///</summary>
        [Display(Name = "Answer Count")]
        public int AnswerCount { get; set; }

        ///<summary>
        ///Complete
        ///</summary>
        [Display(Name = "Complete")]
        public bool Complete { get {
                return (QuestionCount > 0 && (QuestionCount == AnswerCount));
            } }
        
    }
}
