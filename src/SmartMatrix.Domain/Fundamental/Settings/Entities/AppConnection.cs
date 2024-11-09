using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Fundamental.Settings.Entities
{
    public class AppConnection : AuditableEntity<int>
    {
        public string Name { get; set; }        
        public string Type { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string AdditionalSettings { get; set; }
    }
}