using DoddleNow.API.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class HPJobSearchModel
    {
        /// <summary>
        /// Global Search Param
        /// </summary>
        [Display(Name ="SearchParam")]
        public string SearchParam { get; set; }

        ///<summary>
        ///Distance
        ///</summary>
        [Display(Name = "Distance")]
        public int Distance { get; set; }

        ///<summary>
        ///SpecialtyIDs
        ///</summary>
        [Display(Name = "SpecialtyIDs")]
        public List<int> SpecialtyIDs{ get; set; }

        
    }

}
