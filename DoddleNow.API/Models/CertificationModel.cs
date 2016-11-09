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
   
    public class Certification
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
        ///Name
        ///</summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Issuing Body
        ///</summary>
        [Display(Name = "Issuing Body")]
        public string IssuingBody { get; set; }

        ///<summary>
        ///Issuance Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "Issuance Date")]
        public DateTime? IssuanceDate { get; set; }

        ///<summary>
        ///Expiration Date
        ///</summary>
        [JsonConverter(typeof(ShortDateConverter))]
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }



    }
}
