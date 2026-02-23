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
    public class CityController : BaseApiController<CityController>
    {
        private readonly ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        #region Get Methods
        [HttpGet("{id}", Name = "GetCity")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var retResult = await this._cityRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.CityViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetCityPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {

            var countryId = HttpContext.User.GetUserCountryId();
            var retResult = await this._cityRepository.GetPageListAsync(pageNumber, rowsPerPage, countryId, searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.CityViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.CityViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("dropdown", Name = "GetCityDropDownPageListAsync")]
        public async Task<IActionResult> GetCityPageListAsync()
        {
            var countryId = HttpContext.User.GetUserCountryId();
            var retResult = await this._cityRepository.GetPageListAsync(1, 50000, countryId);           
            if (retResult != null && retResult.Count() > 0)
            {
                //var model = GetMapper.Map<IEnumerable<dto.CityViewModel>>(retResult);
                var model = retResult.Select(c => c.Map());
                var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 50000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        #endregion

        #region Post methods
        /// <summary>
        /// Create new city 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.CityViewModel model)
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
                var isAlreadyExsit = await this._cityRepository.GetByNameAsync(model.CityName);
                if(isAlreadyExsit!= null )
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                var countryId = HttpContext.User.GetUserCountryId();
                if (model.CityCountryId <= 0)
                {
                    model.CityCountryId = countryId;
                }

                model.CityName = model.CityName.ToLower();
                var entityToInsert = GetMapper.Map<entity.CityEntity>(model);
                var result = await this._cityRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.CityId = result.Value;
                    return CreatedAtRoute("GetCity", new { id = model.CityId }, model);
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
        /// full update city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.CityViewModel model)
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

                var entityToUpdate = GetMapper.Map<entity.CityEntity>(model);
                var result = await this._cityRepository.UpdateAsync(entityToUpdate);
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
        /// partial update city 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchCity")]
        public async Task<IActionResult> PatchCity([FromRoute]int id, [FromBody]JsonPatchDocument<dto.CityViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._cityRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.CityViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.CityEntity>(model);
                var result = await this._cityRepository.UpdateAsync(entityToUpdate);
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
        /// Delete city 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteCity")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._cityRepository.DeleteByIdAsync(id);
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


//[HttpGet]
//public async Task<IEnumerable<dto.CityViewModel>> Get()
//{
//   var restResult =  await _cityRepository.GetAllAsync();
//    var retDto = GetMapper.Map<IEnumerable<dto.CityViewModel>>(restResult);
//    return retDto;
//    //Pageing 
//}