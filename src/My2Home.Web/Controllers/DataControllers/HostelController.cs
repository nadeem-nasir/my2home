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
using Microsoft.AspNetCore.JsonPatch;
using My2Home.Web.Extensions;
using My2Home.Web.Helpers;

namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostelController : BaseApiController<HostelController>
    {
        private readonly IHostelRepository _hostelRepository;

        public HostelController(IHostelRepository hostelRepository)
        {
            _hostelRepository = hostelRepository;
        }

        [HttpGet( Name = "GetByOrganizationIdAsync")]
        public async Task<IActionResult> GetByOrganizationId()
        {
            var organizationId = HttpContext.User.GetUserOrganizationId();
            var retResult = await this._hostelRepository.GetByOrganizationIdAsync(organizationId);
            var model = GetMapper.Map<IEnumerable<dto.HostelViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.HostelViewModel>(model, 1, 50000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpGet("dropdown", Name = "GetDropDownByOrganizationId")]
        public async Task<IActionResult> GetDropDownByOrganizationId()
        {
            var organizationId = HttpContext.User.GetUserOrganizationId();
            var retResult = await this._hostelRepository.GetByOrganizationIdAsync(organizationId);
          //  var model = GetMapper.Map<IEnumerable<dto.HostelViewModel>>(retResult);
            if (retResult != null && retResult.Count() > 0)
            {
                var model = retResult.Select(c => c.Map());
                var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 50000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }


        #region Get Methods
        [HttpGet("{id}", Name = "GetHostel")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var retResult = await this._hostelRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.HostelViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetHostelPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {
            //var countryId = HttpContext.User.GetUserCountryId();
            var retResult = await this._hostelRepository.GetPageListAsync(pageNumber, rowsPerPage,  searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.HostelViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.HostelViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new Hostel 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.HostelViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "model state is null");
                }
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var organizationId = HttpContext.User.GetUserOrganizationId();
                model.HostelOrganizationId = organizationId;
                var entityToInsert = GetMapper.Map<entity.HostelEntity>(model);
                var result = await this._hostelRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.HostelId = result.Value;
                    return CreatedAtRoute("GetHostel", new { id = model.HostelId }, model);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
        #region Put methods
        /// <summary>
        /// full update Hostel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.HostelViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var entityToUpdate = GetMapper.Map<entity.HostelEntity>(model);
                var result = await this._hostelRepository.UpdateAsync(entityToUpdate);
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
        #region patch
        /// <summary>
        /// partial update Hostel 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchHostel")]
        public async Task<IActionResult> PatchHostel([FromRoute]int id, [FromBody]JsonPatchDocument<dto.HostelViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._hostelRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.HostelViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.HostelEntity>(model);
                var result = await this._hostelRepository.UpdateAsync(entityToUpdate);
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
        #region Delete methods
        /// <summary>
        /// Delete Hostel 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteHostel")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._hostelRepository.DeleteByIdAsync(id);
                if (result)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}