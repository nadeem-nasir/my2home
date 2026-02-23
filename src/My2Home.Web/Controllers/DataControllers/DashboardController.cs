using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My2Home.Core.Interfaces;
using My2Home.Web.Controllers.ApiControllers;
using entity = My2Home.Core.Entities;
using dto = My2Home.Web.ApiModels;
using My2Home.Web.Extensions;

namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseApiController<DashboardController>
    {
        private readonly IRentRepository _rentRepository;
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IRentRepository rentRepository, IDashboardRepository dashboardRepository)
        {
            _rentRepository = rentRepository;
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("{hostelId}")]
        public async Task<IActionResult> GetDashboardAsync([FromRoute] int hostelId)
        {
            if(hostelId<= 0)
            {
                return new BadRequestObjectResult("hostelId");
            }                           
            var retResult = await this._dashboardRepository.GetByHostelIdAsync(hostelId);
            if (retResult != null)
            {
                return new OkObjectResult(retResult);
            }            
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("{hostelId}/{pageNumber}/{rowsPerPage}/{searchCondition:datetime}", Name = "GetAccountPageListAsync")]
        public async Task<IActionResult> GetAccountPageListAsync([FromRoute] int hostelId, [FromRoute] int pageNumber, [FromRoute] int rowsPerPage, [FromRoute] DateTime? searchCondition = null)
        {
            if (!searchCondition.HasValue)
            {
                searchCondition = System.DateTime.UtcNow;
            }
            var retResult = await this._rentRepository.GetAccountPageListAsync(hostelId, searchCondition.Value.Year, pageNumber, rowsPerPage);
            if (retResult != null && retResult.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<entity.AccountViewModel>(retResult, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("{hostelId}/{searchCondition:datetime}", Name = "GetMonthlyAccountPageListAsync")]
        public async Task<IActionResult> GetMonthlyAccountPageListAsync([FromRoute] int hostelId, [FromRoute] DateTime? searchCondition = null)
        {
            if (!searchCondition.HasValue)
            {
                searchCondition = System.DateTime.UtcNow;
            }
            var retResult = await this._rentRepository.GetAccountPageListAsync(hostelId, searchCondition.Value.Year, 1, 5000, searchCondition.Value.ToMonthName());
            if (retResult != null && retResult.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<entity.AccountViewModel>(retResult, 1, 5000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult.Results?.FirstOrDefault());
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}