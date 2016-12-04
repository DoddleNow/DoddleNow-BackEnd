using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class HPSpecialty
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Int32 Id { get; set; }

        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid UserId { get; set; }

        ///<summary>
        ///Specialty Name
        ///</summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        

       
    }
}
