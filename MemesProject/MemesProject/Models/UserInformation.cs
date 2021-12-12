namespace MemesProject.Models
{
    public class UserInformation
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] AvatarImage { get; set; }
        public string AccountRegisterDate { get; set; }
        public int IloscMemow { get; set; }

        public int IloscKomentarzy { get; set; }

    }
}
