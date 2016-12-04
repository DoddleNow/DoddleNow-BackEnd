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
    //public class Job
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
    public class Job
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///ClientID for job to client association
        ///</summary>
        [Display(Name = "ClientID")]
        public Guid ClientId { get; set; }

        ///<summary>
        ///Job Name
        ///</summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Job Description
        ///</summary>        
        [Display(Name = "Description")]
        public string Description { get; set; }


        ///<summary>
        ///Job Start Date
        ///</summary
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "StartDate")]
        public DateTime? StartDate{ get; set; }

        ///<summary>
        ///Job End Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "EndDate")]
        public DateTime? EndDate { get; set; }


        ///<summary>
        ///Job Shift(s) 
        ///</summary>        
        [Display(Name = "Shifts")]
        public List<string> Shifts { get; set; }


      }

    public class Shift
    {
        ///<summary>
        ///Shift Id
        ///</summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        ///<summary>
        ///Shift Description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

    }

}
