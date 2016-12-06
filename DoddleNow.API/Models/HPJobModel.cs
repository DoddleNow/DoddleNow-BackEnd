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
    public class HPJob
    {

        /// <summary>
        /// JobId
        /// </summary>
        [Display(Name = "JobId")]
        public Guid Id { get; set; }

        /// <summary>
        /// JobName
        /// </summary>
        [Display(Name = "JobName")]
        public string Name { get; set; }

        /// <summary>
        /// JobDescription
        /// </summary>
        [Display(Name = "JobDescription")]
        public string Description { get; set; }

        /// <summary>
        /// ClientName
        /// </summary>
        [Display(Name = "ClientName")]
        public string ClientName { get; set; }

        /// <summary>
        /// ClientDescription
        /// </summary>
        [Display(Name = "ClientDescription")]
        public string ClientDescription { get; set; }


        /// <summary>
        /// ClientAddress
        /// </summary>
        [Display(Name = "ClientAddress")]
        public string ClientAddress { get; set; }

        /// <summary>
        ///ClientAddress2
        /// </summary>
        [Display(Name = "ClientAddress2")]
        public string ClientAddress2 { get; set; }

        /// <summary>
        /// ClientCity
        /// </summary>
        [Display(Name = "ClientCity")]
        public string ClientCity { get; set; }

        /// <summary>
        /// ClientState
        /// </summary>
        [Display(Name = "ClientState")]
        public string  ClientState { get; set; }

        /// <summary>
        /// ClientZip
        /// </summary>
        [Display(Name = "ClientZip")]
        public string ClientZip { get; set; }

        /// <summary>
        /// Comma delimited list of specialties
        /// </summary>
        [Display(Name = "Specialities")]
        public string Specialities { get; set; }

        /// <summary>
        /// HP Starred the job with interest
        /// </summary>
        [Display(Name = "Starred")]
        public bool? Starred { get; set; }

        /// <summary>
        /// Client Interested in HP
        /// </summary>
        [Display(Name = "ClientInterested")]
        public bool ClientInterested { get; set; }

        ///<summary>
        ///ClientID for job to client association
        ///</summary>
        [Display(Name = "ClientID")]
        public Guid ClientId { get; set; }

        ///<summary>
        ///Job Start Date
        ///</summary
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        ///<summary>
        ///Job End Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// HP applied to job
        /// </summary>
        [Display(Name = "Applied")]
        public bool? Applied { get; set; }

        /// <summary>
        /// Percentage SCL Match as a whole number
        /// </summary>
        [Display(Name ="SCLMatch")]
        public int SCLMatch { get; set; }

        ///<summary>
        ///Job Shift(s) - comma delimited
        ///</summary>        
        [Display(Name = "Shifts")]
        public List<string> Shifts { get; set; }

        ///<summary>
        ///New applicants count 
        ///</summary>        
        [Display(Name = "NewApplicants")]
        public int NewApplicants { get; set; }

        ///<summary>
        ///Number of applicants for job
        ///</summary>        
        [Display(Name = "ApplicantCount")]
        public int ApplicantCount { get; set; }

        ///<summary>
        ///Active (bool)
        ///</summary>        
        [Display(Name = "Active")]
        public bool Active { get; set; }

        ///<summary>
        ///SCL Match Preference, set by HA.  HP must meet or exceed this percentage
        ///</summary>        
        [Display(Name = "SCLMatchPreference")]
        public int SCLMatchPreference { get; set; }

    }
    
}
