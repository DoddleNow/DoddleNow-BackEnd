using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class SkillsChecklist
    {
       
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid ID { get; set; }

        ///<summary>
        ///Skills Checklist Name
        ///</summary>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        ///<summary>
        ///Description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        ///<summary>
        ///Template
        ///</summary>
        [Display(Name = "Template")]
        public bool Template { get; set; }


    }
}
