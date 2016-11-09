using DoddleNow.API.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    

    public class Education
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
        ///Institution Name
        ///</summary>
        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        ///<summary>
        ///Major
        ///</summary>
        [Display(Name = "Major")]
        public string Major { get; set; }

        ///<summary>
        ///Start Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        ///<summary>
        ///End Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        ///<summary>
        ///Highest Degree Earned ID
        ///</summary>
        [Display(Name = "Highest Degree Earned ID")]
        public int HighestDegreeEarnedID { get; set; }

        ///<summary>
        ///Highest Degree Earned 
        ///</summary>
        [Display(Name = "Highest Degree Earned")]
        public string HighestDegreeEarned { get; set; }

        /// <summary>
        /// Other Degree
        /// </summary>
        [Display(Name ="Other Degree")]
        public string OtherDegree { get; set; }

        /// <summary>
        /// Graduated
        /// </summary>
        [Display(Name = "Graduated")]
        public bool? Graduated { get; set; }

        /// <summary>
        /// Graduation Date
        /// </summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "Graduation Date")]
        public DateTime? GraduationDate { get; set; }

        



    }
}
