using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Reference
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name ="ID")]
        public Guid ID { get; set; }

        ///<summary>
        ///UserId
        ///</summary>
        [Display(Name = "UserId")]
        public Guid UserId { get; set; }

        ///<summary>
        ///Name
        ///</summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Title
        ///</summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        ///<summary>
        ///Direct Supervisor
        ///</summary>
        [Display(Name = "Direct Supervisor")]
        public bool DirectSupervisor { get; set; }

        ///<summary>
        ///Contact Phone
        ///</summary>
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }



    }
}
