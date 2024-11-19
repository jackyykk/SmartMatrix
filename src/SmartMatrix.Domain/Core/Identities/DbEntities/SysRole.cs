using System.Collections.Generic;
using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysRole : AuditableEntity<int>
    {
        public string PartitionKey { get; set; }
        public string Type { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<SysUser> Users { get; set; } = new List<SysUser>();

        public static SysRole Copy(SysRole x)
        {
            return new SysRole
            {
                Id = x.Id,
                Status = x.Status,
                IsDeleted = x.IsDeleted,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt,
                ModifiedBy = x.ModifiedBy,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy,
                PartitionKey = x.PartitionKey,
                Type = x.Type,
                RoleCode = x.RoleCode,
                RoleName = x.RoleName,                
                Description = x.Description
            };
        }
    }
}