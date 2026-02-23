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
namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : BaseApiController<OrganizationController>
    {
        private readonly IOrganizationRepository _organizationRepository;
        public OrganizationController(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        #region Get Methods
        [HttpGet(Name = "GetByIdentityUserId")]
        public async Task<IActionResult> GetByIdentityUserId()
        {
            try
            {
               var identityUserId = HttpContext.User.GetUserId();
                var retResult = await this._organizationRepository.GetByIdentityUserIdAsync(identityUserId);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.OrganizationViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetByIdentityUserIdPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {    
            var retResult = await this._organizationRepository.GetPageListAsync(pageNumber, rowsPerPage, searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.OrganizationViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.OrganizationViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new Organization 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.OrganizationViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "model state is null");
                }

                var entityToInsert = GetMapper.Map<entity.OrganizationEntity>(model);
                var result = await this._organizationRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.OrganizationId = result.Value;
                    return CreatedAtRoute("GetByIdentityUserId", new { id = model.OrganizationId }, model);
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
        /// full update Organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.OrganizationViewModel model)
        {
            try
            {
                if (model == null|| model.OrganizationId<= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var entityToUpdate = GetMapper.Map<entity.OrganizationEntity>(model);
                var result = await this._organizationRepository.UpdateAsync(entityToUpdate);
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
        /// partial update Organization 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchOrganization")]
        public async Task<IActionResult> PatchOrganization([FromRoute]int id, [FromBody]JsonPatchDocument<dto.OrganizationViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._organizationRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.OrganizationViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.OrganizationEntity>(model);
                var result = await this._organizationRepository.UpdateAsync(entityToUpdate);
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
        /// Delete Organization 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteOrganization")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._organizationRepository.DeleteByIdAsync(id);
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