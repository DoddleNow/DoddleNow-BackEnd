using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Specialty
    {
        ///<summary>
        ///ID
        ///</summary>
        [Display(Name = "ID")]
        public Int32 ID { get; set; }

        ///<summary>
        ///Specialty Name
        ///</summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Specialty Description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

       
    }
}
