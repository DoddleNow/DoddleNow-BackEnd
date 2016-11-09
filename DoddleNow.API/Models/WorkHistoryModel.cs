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
   
    public class WorkHistory
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
        ///Company Name
        ///</summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        ///<summary>
        ///Company City
        ///</summary>
        [Display(Name = "Company City")]
        public string CompanyCity { get; set; }

        ///<summary>
        ///Company State
        ///</summary>
        [Display(Name = "Company State")]
        public string CompanyState { get; set; }

        ///<summary>
        ///Job Title
        ///</summary>
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        ///<summary>
        ///Job Responsibilities
        ///</summary>
        [Display(Name = "Job Responsibilities")]
        public string JobResponsibilities { get; set; }

        ///<summary>
        ///Start Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        ///<summary>
        ///End Date
        ///</summary>
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }


    }
}
