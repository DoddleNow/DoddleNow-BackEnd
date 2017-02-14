using DataAccessLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    public class Profile
    {

        /// <summary>
        /// If the client connects, they can see the full profile.  This contains extra candidate details not visible to the HP 
        /// </summary>
        [Display(Name = "References")]
        public Candidate CandidateDetails { get; set; }


        /// <summary>
        /// Returns double for a percent of profile completed
        /// </summary>
        public double Progress {
            get {
                //this is the number of objects in profile
                double denominator = 6;
                double numerator = 0;

                if (Overview != null)
                    numerator += 1;
                if (Preferences != null)
                    numerator += 1;
                if (Educations != null && Educations.Count > 0)
                    numerator += 1;
                if (Certifications != null && Certifications.Count > 0)
                    numerator += 1;
                if (WorkHistories != null && WorkHistories.Count > 0)
                    numerator += 1;
                if (References != null && References.Count > 0)
                    numerator += 1;

                return (numerator / denominator) * 100;
            }

        }

        /// <summary>
        /// HPOverview
        /// </summary>
        [Display(Name = "Overview")]
        public HPOverview Overview { get; set; }

        /// <summary>
        /// HPPreferences
        /// </summary>
        [Display(Name = "Preferences")]
        public HPPreferences Preferences { get; set; }

        /// <summary>
        /// Specialties
        /// </summary>
        [Display(Name = "Specialties")]
        public List<HPSpecialty> Specialties { get; set; }


        /// <summary>
        /// SCL
        /// </summary>
        [Display(Name = "SkillsChecklists")]
        public List<HPSkillsChecklist> SCLS { get; set; }


        /// <summary>
        /// Educations
        /// </summary>
        [Display(Name = "Educations")]
        public List<Education> Educations { get; set; }

        /// <summary>
        /// Certifications
        /// </summary>
        [Display(Name = "Certifications")]
        public List<Certification> Certifications { get; set; }

        /// <summary>
        /// Work Histories
        /// </summary>
        [Display(Name = "Work Histories")]
        public List<WorkHistory> WorkHistories { get; set; }

        /// <summary>
        /// References
        /// </summary>
        [Display(Name = "References")]
        public List<Reference> References { get; set; }

        
    }

    public class SCLWithQuestions
    {
        ///<summary>
        ///Id
        ///</summary>
        [JsonProperty(Order = -2)]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///Skills Checklist Name
        ///</summary>
        [JsonProperty(Order = -2)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Questions
        /// </summary>
        [Display(Name = "Questions")]
        public List<usp_GetQuestionsWithAnswersResult> Questions;
    }
    
}
