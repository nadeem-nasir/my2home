using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("DocumentType")]
    public class DocumentTypeEntity: BaseEntity
    {
        public int DocumentTypeId { get; set; }
    }
}
