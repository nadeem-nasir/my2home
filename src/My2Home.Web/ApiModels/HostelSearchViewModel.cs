using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.ApiModels
{
    public class HostelSearchViewModel
    {
        public HostelSearchViewModel()
        {
            CityList = new List<SelectItemViewModel>();
            HostelList = new List<HostelViewModel>();
        }
        public int SelectedCity { get; set; }
        public List<SelectItemViewModel> CityList { get; set; }
        public List<HostelViewModel> HostelList { get; set; }
    }
}
