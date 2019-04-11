using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models.User
{
    public class Category
    {
        public Category()
        {
            this.UserPosts = new List<Post>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<Post> UserPosts { get; set; }
    }
}