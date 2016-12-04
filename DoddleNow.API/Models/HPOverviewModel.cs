using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    public class HPOverview
    {

        ///<summary>
        ///ImageUrl
        ///</summary>
        public string ImageUrl { get; set; }

        ///<summary>
        ///VideoUrl
        ///</summary>
        public string VideoUrl { get; set; }

        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid UserId { get; set; }

        ///<summary>
        ///User first name
        ///</summary>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        ///<summary>
        ///User last name
        ///</summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        ///<summary>
        ///User title
        ///</summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        ///<summary>
        ///User department
        ///</summary>
        [Display(Name = "Department")]
        public string Department { get; set; }

       
        ///<summary>
        ///User phone
        ///</summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        ///<summary>
        ///User primary email
        ///</summary>
        [Required]
        [Display(Name = "EMail")]
        public string EMail { get; set; }

        ///<summary>
        ///ClientID if user is associated to a client. Required if RoleID is 3, 4, or 5
        ///</summary>
        [Display(Name = "ClientID")]
        public Guid ClientID { get; set; }

        ///<summary>
        ///Secondary Email
        ///</summary>
        [Display(Name = "Secondary Email")]
        public string SecondaryEmail { get; set; }

        ///<summary>
        ///Cell phone
        ///</summary>
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        ///<summary>
        ///Personal Summary
        ///</summary>
        [Display(Name = "Personal Summary")]
        public string PersonalSummary { get; set; }


        ///<summary>
        ///Personal Interests
        ///</summary>
        [Display(Name = "Personal Interests")]
        public string PersonalInterests { get; set; }


        ///<summary>
        ///Languages
        ///</summary>
        [Display(Name = "Languages")]
        public List<Language> Languages { get; set; }


        /////<summary>
        /////User password
        /////</summary>
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        /////<summary>
        /////Confirmation of user password
        /////</summary>
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }



    }
    
}
