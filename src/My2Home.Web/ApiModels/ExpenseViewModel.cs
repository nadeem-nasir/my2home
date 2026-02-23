using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class ExpenseViewModel: BaseViewModel
    {
        public int ExpenseId { get; set; }
        [MaxLength(200)]
        public string ExpenseName { get; set; }
        public int ExpenseYear { get; set; }
        public string ExpenseMonth { get; set; }
        public float ExpenseAmount { get; set; }
        [MaxLength(1000)]
        public string ExpenseDescription { get; set; }
        public DateTime ExpenseCreatedOn { get; set; }
        public DateTime ExpenseUpdatedON { get; set; } = DateTime.UtcNow;
        public string ExpenseUpdatedBy { get; set; }
        public string ExpenseCreatedBy { get; set; }
        public string ExpenseCategoryId { get; set; }
        public int ExpenseHostelId { get; set; }

        

    }
}
