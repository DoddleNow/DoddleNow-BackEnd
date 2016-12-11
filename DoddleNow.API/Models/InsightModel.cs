using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Insight
    {

        ///<summary>
        ///SpecialtyID
        ///</summary>
        [Display(Name = "SpecialtyID")]
        public int SpecialtyID { get; set; }
        
        ///<summary>
        ///Name
        ///</summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Short Name
        ///</summary>
        [Display(Name = "ShortName")]
        public string ShortName { get; set; }

        ///<summary>
        ///Matches
        ///</summary>
        [Display(Name = "Matches")]
        public int Matches { get; set; }

        ///<summary>
        ///Total users in system with that specialty
        ///</summary>
        [Display(Name = "Total")]
        public int Total { get; set; }

        ///<summary>
        ///Scale
        ///</summary>
        [Display(Name = "Scale")]
        public List<Scale> Scale { get; set; }

        ///<summary>
        ///Potential Candidates
        ///</summary>
        [Display(Name = "PotentialCandidates")]
        public List<PotentialCandidate> PotentialCandidates { get; set; }


    }


    public class Scale
    {
        ///<summary>
        ///Data percent
        ///</summary>
        [Display(Name = "Data")]
        public int Data { get; set; }

        ///<summary>
        ///Matches for this percent
        ///</summary>
        [Display(Name = "Matches")]
        public int Matches { get; set; }

    }


    public class PotentialCandidate 
    {
        ///<summary>
        ///UserID
        ///</summary>
        [Display(Name = "UserID")]
        public string UserID { get; set; }


        ///<summary>
        ///Location
        ///</summary>
        [Display(Name = "Location")]
        public string Location { get; set; }



        ///<summary>
        ///Availabile On Date
        ///</summary>
        [Display(Name = "Availabile On")]
        public DateTime AvailableOn { get; set; }


        ///<summary>
        ///Experience
        ///</summary>
        [Display(Name = "Experience")]
        public int Experience { get; set; }

        ///<summary>
        ///SCL Match
        ///</summary>
        [Display(Name = "SCLMatch")]
        public int SCLMatch { get; set; }

        ///<summary>
        ///Education
        ///</summary>
        [Display(Name = "Education")]
        public int Education { get; set; }

        ///<summary>
        ///Shift
        ///</summary>
        [Display(Name = "Shift")]
        public int Shift { get; set; }


    }

    public class Setting
    {
        ///<summary>
        ///Client ID
        ///</summary>
        [Display(Name = "ClientID")]
        public Guid ClientID { get; set; }

        ///<summary>
        ///SpecialtyID if specialty-specific setting
        ///</summary>
        [Display(Name = "SpecialtyID")]
        public int? SpecialtyID { get; set; }

        ///<summary>
        ///Availability
        ///</summary>
        [Display(Name = "Availability")]
        public int Availability { get; set; }

        

        ///<summary>
        ///Experience
        ///</summary>
        [Display(Name = "Experience")]
        public int Experience { get; set; }

        ///<summary>
        ///SCL Match
        ///</summary>
        [Display(Name = "SCLMatch")]
        public int SCLMatch { get; set; }
        
        ///<summary>
        ///Education
        ///</summary>
        [Display(Name = "Education")]
        public int Education { get; set; }

        ///<summary>
        ///Shift
        ///</summary>
        [Display(Name = "Shift")]
        public int Shift { get; set; }
    }
}
