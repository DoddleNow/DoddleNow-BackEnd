using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class Address
    {
        ///<summary>
        ///ID
        ///</summary>
        [Display(Name = "ID")]
        public Guid ID { get; set; }

        ///<summary>
        ///UserId
        ///</summary>
        [Display(Name = "UserId")]
        public Guid UserId { get; set; }

        ///<summary>
        ///Address
        ///</summary>
        [Required]
        [Display(Name = "AddressTypeId")]
        public int AddressTypeId { get; set; }

        ///<summary>
        ///Address Type
        ///</summary>
        [Display(Name = "Address Type")]
        public string AddressType { get; set; }

        ///<summary>
        ///Address
        ///</summary>
        [Display(Name = "Address")]
        public string Address_1 { get; set; }

        ///<summary>
        ///Address 2
        ///</summary>
        [Display(Name = "Address 2")]
        public string Address_2 { get; set; }

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
        ///ZIP
        ///</summary>
        [Display(Name = "ZIP")]
        public string ZIP { get; set; }
        
    }
}
