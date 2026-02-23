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
    public class RentController : BaseApiController<RentController>
    {
        private readonly IRentRepository _RentRepository;
        public RentController(IRentRepository RentRepository)
        {
            _RentRepository = RentRepository;
        }
        #region Get Methods
        [HttpGet("{id}", Name = "GetRent")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var retResult = await this._RentRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.RentListViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{hostelId}/{pageNumber}/{rowsPerPage}/{searchCondition:datetime}/{searchType}", Name = "GetRentPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int hostelId, [FromRoute] int pageNumber, [FromRoute] int rowsPerPage, [FromRoute] DateTime? searchCondition = null, bool searchType = true)
        {
            //var countryId = HttpContext.User.GetUserCountryId();
            if (!searchCondition.HasValue)
            {
                searchCondition = System.DateTime.UtcNow;
            }
            string month = "";
            if (searchType)
            {
                month = searchCondition.Value.ToMonthName();
            }
            var retResult = await this._RentRepository.GetPageListAsync(hostelId, pageNumber, rowsPerPage, month, searchCondition.Value.Year);
            var model = GetMapper.Map<IEnumerable<dto.RentListViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.RentListViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        //[HttpGet("dropdown", Name = "GetRentDropDownPageListAsync")]
        //public async Task<IActionResult> GetRentPageListAsync()
        //{
        //    var countryId = HttpContext.User.GetUserCountryId();
        //    var retResult = await this._RentRepository.GetPageListAsync(1, 50000, countryId);
        //    if (retResult != null && retResult.Count() > 0)
        //    {
        //        //var model = GetMapper.Map<IEnumerable<dto.RentViewModel>>(retResult);
        //        var model = retResult.Select(c => c.Map());
        //        var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 50000, (retResult.FirstOrDefault().TotalRows));
        //        return new OkObjectResult(pagedResult);
        //    }
        //    return StatusCode(StatusCodes.Status204NoContent);
        //}
        #endregion

        #region Post methods
        /// <summary>
        /// Create new Rent 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.RentViewModel model)
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

                model.RentCreationDate = System.DateTime.UtcNow;

                var entityToInsert = GetMapper.Map<entity.RentEntity>(model);

                entityToInsert.RentMonth = model.RentDateTime.ToMonthName();
                entityToInsert.RentYear = model.RentDateTime.Year;

                entityToInsert.RentCreationDate = System.DateTime.UtcNow;
                int? result = 0;
                if (entityToInsert.RentTenantId <= 0)
                {
                    result = await this._RentRepository.InsertRentListAsync(entityToInsert);
                }
                else
                {
                    result = await this._RentRepository.InsertAsync(entityToInsert);
                }

                if (result.HasValue && result.Value > 0)
                {
                    model.RentId = result.Value;
                    return CreatedAtRoute("GetRent", new { id = model.RentId }, model);
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
        [HttpPut("{rentId}/{rentStatus}")]
        public async Task<IActionResult> Put([FromRoute] int rentId, [FromRoute] string rentStatus)
        {
            try
            {
                if (rentId<= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var result = await this._RentRepository.UpdateAsync(rentId, rentStatus);
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
        /// <summary>
        /// full update Rent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.RentCreateViewModel model)
        {
            try
            {
                model.RentCreationDate = System.DateTime.UtcNow;

                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var entityToUpdate = GetMapper.Map<entity.RentEntity>(model);
                entityToUpdate.RentMonth = entityToUpdate.RentDateTime.ToMonthName();
                entityToUpdate.RentYear = entityToUpdate.RentDateTime.Year;
                var result = await this._RentRepository.UpdateAsync(entityToUpdate);
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
        /// partial update Rent 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchRent")]
        public async Task<IActionResult> PatchRent([FromRoute]int id, [FromBody]JsonPatchDocument<dto.RentViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._RentRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.RentViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.RentEntity>(model);
                var result = await this._RentRepository.UpdateAsync(entityToUpdate);
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
        /// Delete Rent 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteRent")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._RentRepository.DeleteByIdAsync(id);
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
//public async Task<IEnumerable<dto.RentViewModel>> Get()
//{
//   var restResult =  await _RentRepository.GetAllAsync();
//    var retDto = GetMapper.Map<IEnumerable<dto.RentViewModel>>(restResult);
//    return retDto;
//    //Pageing 
//}