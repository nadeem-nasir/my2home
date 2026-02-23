using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("HostelsDocument")]
    public class HostelsDocumentEntity: BaseEntity
    {
        public int HostelsDocumentId { get; set; }
    }
}
