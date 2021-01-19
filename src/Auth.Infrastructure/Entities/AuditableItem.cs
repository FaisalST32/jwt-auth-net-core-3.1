using System;

namespace Auth.Infrastructure.Entities
{
    public class AuditableItem
    {
        public DateTime? DateCreatedUTC { get; set; }
        public DateTime? DateModifiedUTC { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
