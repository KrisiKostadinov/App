﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumApp.Models.User
{
    public class Post
    {
        public Post()
        {
            this.Comments = new List<Comment>();
        }

        public int? Id { get; set; }
        
        public string Title { get; set; }

        [Display(Name = "Public / Private post")]
        public bool IsPublic { get; set; } = true;

        [Required]
        public string Description { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<Comment> Comments { get; set; }

        [NotMapped] public int CommentCount => this.Comments.Count;
    }
}