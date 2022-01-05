namespace MemesProject.Models.ViewModels
{
    public class MemeViewModel
    {
        public IList<Meme> Memes { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
