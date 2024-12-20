using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Tools.SimpleNoteTool.Payloads
{
    public class SimpleNote_OutputPayload : AuditablePayload<int>
    {
        
        #region Properties

        public string PartitionKey { get; set; } = PartitionKeyOptions.SmartMatrix;
        public string Classification { get; set; } = ClassificationOptions.Normal_Note;
        public string Type { get; set; } = TypeOptions.Standard;
        public string Category { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }        

        public new string Status { get; set; } = StatusOptions.Active;

        #endregion

        #region Options

        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.Sys_SmartMatrix;
        }

        public class ClassificationOptions
        {            
            public const string Normal_Note = "normal_note";
        }

        public class TypeOptions
        {
            public const string Standard = "standard";
        }

        public class StatusOptions
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Disabled = CommonConstants.DbEntityStatus.Disabled;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public class OwnerOptions
        {
            public const string System = CommonConstants.DbEntityOwner.System;            
        }

        #endregion
        
    }
}