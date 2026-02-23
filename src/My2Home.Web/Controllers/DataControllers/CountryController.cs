using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using My2Home.Core.Interfaces;
using My2Home.Web.Controllers.ApiControllers;
using dto = My2Home.Web.ApiModels;
using entity = My2Home.Core.Entities;
using My2Home.Web.Helpers;
namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : BaseApiController<CountryController>
    {
        private readonly ICountryRepository _countryRepository;
        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        #region Get Methods
        /// <summary>
        /// Get country 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCountry")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                //_logger.LogInformation("From city controller");
                var retResult = await this._countryRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.CountryViewModel>(retResult);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet( Name = "GetCountryListAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountryListAsync()
        {
            var retResult = await this._countryRepository.GetPageListAsync(1, 5000, null);
            //var model = GetMapper.Map<IEnumerable<dto.CountryViewModel>>(retResult);           
            if (retResult != null && retResult.Count() > 0)
            {
                var model = retResult.Select(c => c.Map());
                var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 5000, (retResult.FirstOrDefault().TotalRows));
                return Ok(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Get, search country with page 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        [HttpGet("{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetCountryPageListAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {
            var retResult = await this._countryRepository.GetPageListAsync(pageNumber, rowsPerPage, searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.CountryViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.CountryViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return Ok(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new country 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.CountryViewModel model)
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
                var entityToInsert = GetMapper.Map<entity.CountryEntity>(model);
                var result = await this._countryRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.CountryId = result.Value;
                    return CreatedAtRoute("GetCountry", new { id = model.CountryId }, model);
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
        /// update country 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.CountryViewModel model)
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

                var entityToUpdate = GetMapper.Map<entity.CountryEntity>(model);
                var result = await this._countryRepository.UpdateAsync(entityToUpdate);
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
        /// partial update country 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchCountry")]
        public async Task<IActionResult> PatchCountry([FromRoute]int id, [FromBody]JsonPatchDocument<dto.CountryViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._countryRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.CountryViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.CountryEntity>(model);
                var result = await this._countryRepository.UpdateAsync(entityToUpdate);
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
        /// Delete country 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteCountry")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._countryRepository.DeleteByIdAsync(id);
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