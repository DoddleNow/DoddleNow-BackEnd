using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    
    public class Candidate
    {
        ///<summary>
        ///UserId
        ///</summary>
        [Display(Name = "UserId")]
        public Guid UserId { get; set; }

        ///<summary>
        ///User first name
        ///</summary>
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        ///<summary>
        ///User last name
        ///</summary>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        ///<summary>
        ///User primary email
        ///</summary>
        [Display(Name = "EMail")]
        public string EMail { get; set; }

        /// <summary>
        /// HP Applied for this job vs. found via search
        /// </summary>
        [Display(Name = "ApplicantApplied")]
        public bool ApplicantApplied { get; set; }

        /// <summary>
        /// Set true if client interest
        /// </summary>
        [Display(Name = "ClientInterest")]
        public bool ClientInterest { get; set; }

        /// <summary>
        /// Set true if client starred
        /// </summary>
        [Display(Name = "ClientStarred")]
        public bool ClientStarred { get; set; }

        /// <summary>
        /// Connected/paid for the applicant's full information
        /// </summary>
        [Display(Name = "CoffeeConnect")]
        public bool CoffeeConnect { get; set; }

        /// <summary>
        /// Exclude from results for HA
        /// </summary>
        [Display(Name = "Exclude")]
        public bool Exclude { get; set; }

    }
}
