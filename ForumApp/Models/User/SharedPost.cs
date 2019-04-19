using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApp.Models.User
{
    public class SharedPost
    {
        public SharedPost()
        {
            this.SharedPostComments = new List<SharedPostComment>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Shared Date")]
        public DateTime SahredDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? UserPostId { get; set; }
        public Post Post { get; set; }

        public List<SharedPostComment> SharedPostComments { get; set; }

        [NotMapped] public int SharedPostCommentCount => this.SharedPostComments.Count;

    }
}
