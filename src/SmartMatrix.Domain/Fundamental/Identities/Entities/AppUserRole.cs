namespace SmartMatrix.Domain.Fundamental.Identities.Entities
{
    public class AppUserRole
    {
        public int AppUserId { get; set; }
        public int AppRoleId { get; set; }
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}