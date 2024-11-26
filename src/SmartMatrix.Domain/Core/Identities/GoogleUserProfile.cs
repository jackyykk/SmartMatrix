namespace SmartMatrix.Domain.Core.Identities
{
    public class GoogleUserProfile
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string PictureUrl { get; set; }
    }
}