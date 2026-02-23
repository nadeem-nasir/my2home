using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace My2Home.Core.Entities
{
    [Table("Expense")]
    public class ExpenseEntity: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }
        public string ExpenseName { get;set;}
        public int ExpenseYear { get;set;}
        public string ExpenseMonth { get;set;}
        public float ExpenseAmount { get;set;}
        public string ExpenseDescription { get;set;}
        public DateTime ExpenseCreatedOn { get;set;}
        public DateTime ExpenseUpdatedON { get;set;}
        public string ExpenseUpdatedBy { get;set;}
        public string ExpenseCreatedBy { get;set;}
        public string ExpenseCategoryId {get;set;}
        public int ExpenseHostelId { get; set; }

        [NotMapped]
        public int TotalRows { get; set; }
    }
}
