using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace My2Home.Web.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
