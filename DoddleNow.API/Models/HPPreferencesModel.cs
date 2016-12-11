using DoddleNow.API.Controllers;
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
    public class HPPreferences
    {

              
        ///<summary>
        ///Availability: Zero is available now.  365 is available in one year
        ///</summary>
        [Display(Name = "Availability")]
        public int? AvailabilityInDays { get; set; }

        ///<summary>
        ///Available On Date
        ///</summary>
        [Display(Name = "AvailableOn")]
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? AvailableOn { get; set; }

        ///<summary>
        ///Willing to travel in miles
        ///</summary>
        [Display(Name = "WillingToTravelMiles")]
        public int? WillingToTravelMiles { get; set; }

        ///<summary>
        ///Shift Preference
        ///</summary>
        [Display(Name = "Shift Preference")]
        public string ShiftPreference { get; set; }

        ///<summary>
        ///HP Primary Address
        ///</summary>
        [Display(Name = "Address")]
        public Address Address{ get; set; }

        ///<summary>
        ///Notfication Preferences
        ///</summary>
        [Display(Name = "Notifications")]
        public HPNotification Notifications { get; set; }

        ///<summary>
        ///Experience
        ///</summary>
        [Display(Name = "Experience")]
        public HPExperience Experience { get; set; }

        

    }

    public class HPNotification
    {
        /// <summary>
        /// Send when new match comes up
        /// </summary>
        [Display(Name = "OnNewMatches")]
        public string OnNewMatches { get; set; }

        /// <summary>
        /// Allow contact by phone
        /// </summary>
        [Display(Name = "ContactViaPhone")]
        public bool? ContactViaPhone { get; set; }

        /// <summary>
        /// Allow contact by email
        /// </summary>
        [Display(Name = "ContactViaEmail")]
        public bool? ContactViaEmail { get; set; }

        /// <summary>
        /// Allow contact by SMS
        /// </summary>
        [Display(Name = "ContactViaSMS")]
        public bool? ContactViaSMS { get; set; }

    }

    public class HPExperience
    {
        ///<summary>
        ///YearsOfExperience
        ///</summary>
        [Display(Name = "YearsOfExperience")]
        public int? YearsOfExperience { get; set; }

        ///<summary>
        ///MaxEducation
        ///</summary>
        [Display(Name = "MaxEducation")]
        public string MaxEducation { get; set; }

        
    }
    
}
