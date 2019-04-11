using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ForumApp.Models;

namespace ShopProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public Post()
        {
            this.Comments = new List<Comment>();

            this.PostLaters = new List<PostLater>();
        }

        [Key]
        public int Id { get; set; }

        public string Subtitle { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string AuthorId { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public ApplicationUser Author { get; set; }

        public Category Category { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastCreatedDate { get; set; }

        public List<Comment> Comments { get; set; }

        [NotMapped]
        public int NumberComments => Comments.Count;

        public bool IsReadToLater { get; set; }

        public List<PostLater> PostLaters { get; set; }
    }
}