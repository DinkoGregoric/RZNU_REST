using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Data.Entity;
using Newtonsoft.Json;

namespace RZNU.Models {
    /// <summary>
    /// Book class. 
    /// </summary>
    public class Book {
        /// <summary>
        /// Default book constructor.
        /// </summary>
        public Book() {

        }
        /// <summary>
        /// Book Id. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Book Name.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Book Name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Book Name.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// The year when the book was created.
        /// </summary>
        public int YearCreated { get; set; }


        /// <summary>
        /// List of users.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

    }
}