using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Language
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
        ///Description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }
        

    }
}
