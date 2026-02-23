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
using My2Home.Web.Helpers;

namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseApiController<RoomController>
    {
        private readonly IRoomRepository _roomRepository;
        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        #region Get Methods
        [HttpGet("{id}", Name = "GetRoom")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var retResult = await this._roomRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.RoomViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("dropdown/{hostelId}", Name = "GetDropDownByHostelId")]
        public async Task<IActionResult> GetDropDownByOrganizationId([FromRoute]int hostelId)
        {
           var retResult = await this._roomRepository.GetByHostelIdAsync(hostelId);
            if (retResult != null && retResult.Count() > 0)
            {
                var model = retResult.Select(c => c.Map());
                var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 50000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("{hostelId}/{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetRoomPageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int hostelId , [FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {
            if(hostelId<= 0)
            {
                return BadRequest("hostelId required");
            }
            var retResult = await this._roomRepository.GetPageListAsync(hostelId, pageNumber, rowsPerPage, searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.RoomViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.RoomViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new Room 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.RoomViewModel model)
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

                var entityToInsert = GetMapper.Map<entity.RoomEntity>(model);
                var result = await this._roomRepository.InsertAsync(entityToInsert,model.NumberOfRoomesToCreate);
                if (result.HasValue && result.Value > 0)
                {
                    model.RoomId = result.Value;
                    return CreatedAtRoute("GetRoom", new { id = model.RoomId }, model);
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
        /// full update Room
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.RoomViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var entityToUpdate = GetMapper.Map<entity.RoomEntity>(model);
                var result = await this._roomRepository.UpdateAsync(entityToUpdate);
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
        /// partial update Room 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchRoom")]
        public async Task<IActionResult> PatchRoom([FromRoute]int id, [FromBody]JsonPatchDocument<dto.RoomViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._roomRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.RoomViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.RoomEntity>(model);
                var result = await this._roomRepository.UpdateAsync(entityToUpdate);
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
        /// Delete Room 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteRoom")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._roomRepository.DeleteByIdAsync(id);
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