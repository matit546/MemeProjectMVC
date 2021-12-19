namespace MemesProject.Models.ViewModels
{
    public class CommentViewModel
    {
        public long IdMeme { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IfBlocked { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string IdUser { get; set; }
    }
}
