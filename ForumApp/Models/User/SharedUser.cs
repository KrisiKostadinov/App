using System;

namespace ForumApp.Models.User
{
    public class SharedUser
    {
        public SharedUser(int? postId, string userId)
        {
            this.PostId = postId;
            this.UserId = userId;
        }

        public SharedUser()
        {
            
        }

        public DateTime Shared { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }

        public int? PostId { get; set; }
    }
}
