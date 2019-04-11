using ForumApp.Models;

namespace ShopProject.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}