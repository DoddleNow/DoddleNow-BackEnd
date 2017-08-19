using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connections.Amazon;
namespace DoddleNow.API.Models
{
    public class Document
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///UserId
        ///</summary>
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        ///<summary>
        ///Document Name
        ///</summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        ///<summary>
        ///Description
        ///</summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Filename - to get extension
        /// </summary>
        [Display(Name="Filename")]
        public string FileName { get; set; }

        ///<summary>
        ///URL
        ///</summary>
        [Display(Name = "URL")]
        public string URL { get; set; }

        ///<summary>
        ///S3 File information
        ///</summary>
        [Display(Name = "AWSFile")]
        public S3File AWSFile { get; set; }

        /// <summary>
        /// Base64 File data
        /// </summary>
        [Display(Name="File Data")]
        public string FileData { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        [Display(Name="Created")]
        public DateTime Created { get; set; }
        
        /// <summary>
        /// Sort Index if bundled
        /// </summary>
        [Display(Name="Sort Index")]
        public int SortIdx { get; set; }
        
    }

    public class DocumentBundle
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///Bundle Name
        ///</summary>
        [Display(Name = "Bundle Name")]
        public string Name { get; set; }

        ///<summary>
        ///Bundle
        ///</summary>
        [Display(Name = "Bundle")]
        public List<Document> Bundle { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        ///<summary>
        ///URL
        ///</summary>
        [Display(Name = "URL")]
        public string URL { get; set; }
    }

    public class Transmission
    {
        ///<summary>
        ///Id
        ///</summary>
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        ///<summary>
        ///UserId
        ///</summary>
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        ///<summary>
        ///BundleID
        ///</summary>
        [Display(Name = "BundleID")]
        public Guid BundleId { get; set; }

        ///<summary>
        ///To
        ///</summary>
        [Display(Name = "To")]
        public string To { get; set; }

        ///<summary>
        ///Subject
        ///</summary>
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        ///<summary>
        ///Body
        ///</summary>
        [Display(Name = "Body")]
        public string Body { get; set; }

        ///<summary>
        ///URL
        ///</summary>
        [Display(Name = "URL")]
        public string URL { get; set; }

        ///<summary>
        ///URL/File Expiration Date
        ///</summary>
        [Display(Name = "Expiration Date")]
        public DateTime Expiration { get; set; }

        ///<summary>
        ///Created
        ///</summary>
        [Display(Name = "Created")]
        public DateTime Created { get; set; }
        
    }
}
