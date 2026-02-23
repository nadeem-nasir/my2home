using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("MessSubscription")]
    public class MessSubscriptionEntity: BaseEntity
    {
        public int MessSubscriptionId { get; set; }
    }
}
