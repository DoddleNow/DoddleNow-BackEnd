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
        [Display(Name = "ID")]
        public Guid ID { get; set; }

        ///<summary>
        ///Client Name
        ///</summary>
        [Required]
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
        [Display(Name = "Parent GUID")]
        public Guid ParentGUID { get; set; }

        ///<summary>
        ///Primary Address
        ///</summary>
        [Required]
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
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        ///<summary>
        ///State
        ///</summary>
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        ///<summary>
        ///User primary email
        ///</summary>
        [Required]
        [Display(Name = "ZIP Code")]
        public string ZIP { get; set; }
        
    }
}