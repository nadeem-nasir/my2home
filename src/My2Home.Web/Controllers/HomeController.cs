using Microsoft.AspNetCore.Mvc;
using My2Home.Core.Interfaces;
using My2Home.Web.ApiModels;
using entity = My2Home.Core.Entities;
using dto = My2Home.Web.ApiModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using My2Home.Web.Helpers;

namespace My2Home.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly IHostelRepository _hostelRepository;

       public  HomeController(ICityRepository cityRepository,
            IHostelRepository hostelRepository)
        {
            _cityRepository = cityRepository;
            _hostelRepository = hostelRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<List<SelectItemViewModel>> GetCityList()
        {
            List<SelectItemViewModel> cityList = new List<SelectItemViewModel>();
            //var vm = new HostelSearchViewModel();
            var result = await _cityRepository.GetAllAsync();
            foreach (var city in result)
            {
                cityList.Add(new SelectItemViewModel
                {
                    Label = city.CityName,
                    Value = city.CityId.ToString()
                });
            }
            return cityList;
        }
        [HttpGet]
        public async  Task<IActionResult> Search()
        {
            var vm = new HostelSearchViewModel
            {
                CityList = await GetCityList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Search(HostelSearchViewModel model)
        {
            var vm = new HostelSearchViewModel
            {
                CityList = await GetCityList()
            };

            var searchResult = await _hostelRepository.GetByCityIdAsync(model.SelectedCity);
            if (searchResult != null)
            {
                vm.HostelList = searchResult.Select(c => c.MapSearch()).ToList();
            }
                

            return View(vm);
        }

    }
}
