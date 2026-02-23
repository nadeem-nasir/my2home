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
    public class BedController : BaseApiController<BedController>
    {
        private readonly IBedRepository _bedRepository;
        public BedController(IBedRepository bedRepository)
        {
            this._bedRepository = bedRepository;
        }

        #region Get Methods
        [HttpGet("{id}", Name = "GetBed")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            

            try
            {
                var retResult = await this._bedRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.BedViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("dropdown/{hostelId}", Name = "GetUnOccupiedBedListAsync")]
        public async Task<IActionResult> GetUnOccupiedBedListAsync([FromRoute]int hostelId)
        {            
            var retResult = await this._bedRepository.GetUnOccupiedBedAsync(hostelId);
            if (retResult != null && retResult.Count() > 0)
            {
                //var model = GetMapper.Map<IEnumerable<dto.BedViewModel>>(retResult);
                var model = retResult.Select(c => c.Map());
                //var pagedResult = new dto.PagedResult<dto.BedViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(model);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpGet("{hostelId}/{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetBedPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int hostelId,  [FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {

            //var countryId = HttpContext.User.GetUserCountryId();
            var retResult = await this._bedRepository.GetPageListAsync(hostelId , pageNumber, rowsPerPage,  searchCondition);
           
            if (retResult != null && retResult.Count() > 0)
            {
                var model = GetMapper.Map<IEnumerable<dto.BedViewModel>>(retResult);
                var pagedResult = new dto.PagedResult<dto.BedViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new Bed 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.BedViewModel model)
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

                var countryId = HttpContext.User.GetUserCountryId();
                
                var entityToInsert = GetMapper.Map<entity.BedEntity>(model);
                var result = await this._bedRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.BedNumber = result.Value;
                    return CreatedAtRoute("GetBed", new { id = model.BedNumber }, model);
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
        /// full update Bed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.BedViewModel model)
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

                var entityToUpdate = GetMapper.Map<entity.BedEntity>(model);
                var result = await this._bedRepository.UpdateAsync(entityToUpdate);
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
        /// partial update Bed 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchBed")]
        public async Task<IActionResult> PatchBed([FromRoute]int id, [FromBody]JsonPatchDocument<dto.BedViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._bedRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.BedViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.BedEntity>(model);
                var result = await this._bedRepository.UpdateAsync(entityToUpdate);
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
        /// Delete Bed 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteBed")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._bedRepository.DeleteByIdAsync(id);
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