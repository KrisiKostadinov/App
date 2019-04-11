namespace ShopProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LikePost
    {
        public LikePost(int postId, string authorId, string dis)
        {
            this.PostId = postId;
            this.AuthorId = authorId;

            this.Dis = dis;
        }

        [Key]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public int PostId { get; set; }

        public string Dis { get; set; }
    }
}