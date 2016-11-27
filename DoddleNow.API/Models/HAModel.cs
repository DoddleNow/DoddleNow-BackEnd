using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoddleNow.API.Models
{
    /// <summary>
    /// Healthcare Admin Model
    /// </summary>
    public class HA
    {

        /// <summary>
        /// User Information
        /// </summary>
        [Display(Name = "Overview")]
        public usp_GetUserResult Overview { get; set; }

        
        /// <summary>
        /// Base client the account was created under
        /// </summary>
        [Display(Name = "ClientList")]
        public List<ClientList> ClientList { get; set; }


    }

    public class ClientList
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
    }
    
}
