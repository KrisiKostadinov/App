using System.ComponentModel.DataAnnotations;
using ShopProject.Models;

namespace ForumApp.Models
{
    public class PostLater
    {
        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

    }
}
