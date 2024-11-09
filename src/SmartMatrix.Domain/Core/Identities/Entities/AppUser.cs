using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.Entities
{
    public class AppUser : AuditableEntity<int>
    {
        // User Information
        public string UserName { get; set; }    // This is unique and used for login
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";        

        // Contact Information
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Address Information
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        // Status
        public string LoginType { get; set; } = "Default";
        public new string Status { get; set; } = StatusOption.Active;

        public class StatusOption
        {
            public const string Active = CommonConstants.EntityStatus.Active;
            public const string Inactive = CommonConstants.EntityStatus.Inactive;
            public const string Deleted = CommonConstants.EntityStatus.Deleted;
        }
    }
}