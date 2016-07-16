using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class QuestionModel
    {
        ///<summary>
        ///ID
        ///</summary>
        [Display(Name = "ID")]
        public int ID { get; set; }

        ///<summary>
        ///QuestionTypeID
        ///</summary>
        [Display(Name = "QuestionTypeID")]
        public int QuestionTypeID { get; set; }

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
        public int Position { get; set; }

        ///<summary>
        ///Question Required
        ///</summary>
        [Display(Name = "Required")]
        public bool Required { get; set; }




    }
}
