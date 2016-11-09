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
        /// Returns double for a percent of profile completed
        /// </summary>
        public double Progress {
            get {
                //this is the number of objects in profile
                double denominator = 6;
                double numerator = 0;

                if (Overview != null)
                    numerator += 1;
                if (Locations != null && Locations.Count > 0)
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
        /// Locations
        /// </summary>
        [Display(Name = "Locations")]
        public List<Location> Locations { get; set; }

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

        /// <summary>
        /// Languages
        /// </summary>
        [Display(Name = "Languages")]
        public List<Language> Languages { get; set; }


    }
    
}
