using System.ComponentModel.DataAnnotations;

namespace ForumApp.Models.User
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public string AuthorIdFromCreated { get; set; }
    }
}