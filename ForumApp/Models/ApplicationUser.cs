using System.Collections.Generic;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Identity;

namespace ForumApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.UserPosts = new List<Post>();

            this.UserComments = new List<Comment>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string Town { get; set; }

        public List<Post> UserPosts { get; set; }

        public List<Comment> UserComments { get; set; }
    }
}