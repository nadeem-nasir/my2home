using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace My2Home.Web.ApiModels
{
    
    public class ExpenseCategoryViewModel: BaseViewModel
    {
        [MaxLength(200)]
        [Required]
        public string ExpenseCategoryName { get; set; }
        public int ExpenseCategoryId { get; set; }

    }
}
