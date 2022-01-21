namespace MemesProject.Models
{
    public class UserList
    {
        public IList<ApplicationUser> ApplicationUser { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
