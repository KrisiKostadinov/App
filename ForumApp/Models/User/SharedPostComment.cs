using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models.User
{
    public class SharedPostComment
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int SharedPostId { get; set; }
        public SharedPost SharedPost { get; set; }
    }
}
