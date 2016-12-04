using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class HPSkillsChecklist
    {
       
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid UserId { get; set; }


        ///<summary>
        ///Skills Checklist Name
        ///</summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

    


    }
}
