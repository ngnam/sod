using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Logs
{
    public class AuditLog
    {
        public AuditLog()
        {

        }
        public int Id { get; set; }
        public string Event { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string SubjectIdentifier { get; set; }
        public string SubjectName { get; set; }
        public string SubjectType { get; set; }
        public string SubjectAdditionalData { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }
}
