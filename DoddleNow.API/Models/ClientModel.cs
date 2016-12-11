using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    public class Client
    {

        ///<summary>
        ///Client GUID - Only used if updating object
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///Client Name
        ///</summary>
        [Display(Name = "Client Name")]
        public string Name { get; set; }

        ///<summary>
        ///Description of client.  Used for marketing
        ///</summary>
        [Display(Name = "Client Description")]
        public string Description { get; set; }

        /// <summary>
        /// If part of a network, may have a parent hospital/client
        /// </summary>
        [Display(Name = "Parent Id")]
        public Guid? ParentId { get; set; }

        ///<summary>
        ///Primary Address
        ///</summary>
        [Display(Name = "Primary Address 1")]
        public string Address1 { get; set; }

        ///<summary>
        ///Primary Address 2
        ///</summary>
        [Display(Name = "Primary Address 2")]
        public string Address2 { get; set; }

        ///<summary>
        ///City
        ///</summary>
        [Display(Name = "City")]
        public string City { get; set; }

        ///<summary>
        ///State
        ///</summary>
        [Display(Name = "State")]
        public string State { get; set; }

        ///<summary>
        ///User primary email
        ///</summary>
        [Display(Name = "ZIP Code")]
        public string ZIP { get; set; }

        ///<summary>
        ///Supplemental description
        ///</summary>
        [Display(Name = "Supplemental Description")]
        public string SupplementalDescription { get; set; }

        ///<summary>
        ///Custom URL Route
        ///</summary>
        [Display(Name = "URL Route")]
        public string URLRoute { get; set; }

        ///<summary>
        ///Profile Template ID
        ///</summary>
        [Display(Name = "Profile Template ID")]
        public int ProfileTemplateId { get; set; }

        /// <summary>
        /// One or many 140 character marketing bullets about the client to be displayed on client profile
        /// </summary>
        [Display(Name = "Marketing Bullets")]
        public string[] MarketingBullets { get; set; }

        ///<summary>
        ///# of Active Jobs
        ///</summary>
        [Display(Name = "Number of Active Jobs")]
        public int NumOfActiveJobs { get; set; }

        ///<summary>
        ///# of Active Jobs
        ///</summary>
        [Display(Name = "Number of Applicants in Active Jobs")]
        public int NumOfApplicants { get; set; }

        ///<summary>
        ///# of Past Jobs
        ///</summary>
        [Display(Name = "Number of Past Jobs")]
        public int NumOfPastJobs { get; set; }



    }
}