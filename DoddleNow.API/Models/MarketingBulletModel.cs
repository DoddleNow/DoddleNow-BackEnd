using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
   
    public class MarketingBullet
    {
       
        ///<summary>
        ///ClientId
        ///</summary>
        private Guid ClientId { get; set; }

        ///<summary>
        ///Marketing Bullet
        ///</summary>
        [Required]
        [Display(Name = "Marketing Bullet")]
        public string Bullet { get; set; }

        
    }
}
