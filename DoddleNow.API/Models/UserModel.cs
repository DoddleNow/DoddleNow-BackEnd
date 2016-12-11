using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    //public class UserModel
    //{
    //    [Required]
    //    [Display(Name = "User name")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}
    public class User
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

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
        ///Role Id of user
        /// Id	Name
        /// 1	DoddleNow Super Admin
        /// 2	DoddleNow Support
        /// 3	Hospital Super Admin
        /// 4	HR Admin
        /// 5	Manager
        /// 6	Healthcare Professional
        ///</summary>
        [Display(Name = "RoleID")]
        public int RoleID { get; set; }

        
        ///<summary>
        ///ClientID if user is associated to a client. Required if RoleID is 3, 4, or 5
        ///</summary>
        [Display(Name = "ClientID")]
        public Guid ClientID { get; set; }

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
        [Display(Name = "EMail")]
        public string EMail { get; set; }

        ///<summary>
        ///User password
        ///</summary>
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        ///<summary>
        ///Confirmation of user password
        ///</summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
