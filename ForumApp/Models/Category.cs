using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ForumApp.Models;

namespace ShopProject.Models
{
    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Post> Posts { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreatedDate { get; set; }

        public ApplicationUser Author { get; set; }
    }
}