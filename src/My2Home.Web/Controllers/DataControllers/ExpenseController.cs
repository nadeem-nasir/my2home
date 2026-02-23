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
    public class ExpenseController : BaseApiController<ExpenseController>
    {
        private IExpenseRepository _expenseRepository;
        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        #region Get Methods
        [HttpGet("{id}", Name = "GetExpense")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var retResult = await this._expenseRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.ExpenseViewModel>(retResult);
                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{hostelId}/{pageNumber}/{rowsPerPage}/{searchCondition:datetime}", Name = "GetExpensePageListAsync")]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int hostelId, [FromRoute] int pageNumber, [FromRoute] int rowsPerPage, [FromRoute] DateTime? searchCondition = null)
        {
            if (!searchCondition.HasValue)
            {
                searchCondition = System.DateTime.UtcNow;
            }

            var retResult = await this._expenseRepository.GetPageListAsync(hostelId, pageNumber, rowsPerPage, searchCondition.Value.ToMonthName(), searchCondition.Value.Year);
            var model = GetMapper.Map<IEnumerable<dto.ExpenseViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.ExpenseViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        #endregion

        #region Post methods
        /// <summary>
        /// Create new Expense 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dto.ExpenseViewModel model)
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
                var entityToInsert = GetMapper.Map<entity.ExpenseEntity>(model);
                entityToInsert.ExpenseUpdatedON = DateTime.UtcNow;
                entityToInsert.ExpenseUpdatedBy = HttpContext.User.GetUserId();
                entityToInsert.ExpenseCreatedBy = HttpContext.User.GetUserId();
                entityToInsert.ExpenseYear = entityToInsert.ExpenseCreatedOn.Year;
                entityToInsert.ExpenseMonth = entityToInsert.ExpenseCreatedOn.ToMonthName();

                var result = await this._expenseRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.ExpenseId = result.Value;
                    return CreatedAtRoute("GetExpense", new { id = model.ExpenseId }, model);
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
        /// full update Expense
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] dto.ExpenseViewModel model)
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

                var entityToUpdate = GetMapper.Map<entity.ExpenseEntity>(model);
                entityToUpdate.ExpenseYear = entityToUpdate.ExpenseCreatedOn.Year;
                entityToUpdate.ExpenseMonth = entityToUpdate.ExpenseCreatedOn.ToMonthName();

                var result = await this._expenseRepository.UpdateAsync(entityToUpdate);

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
        /// partial update Expense 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PatchExpense")]
        public async Task<IActionResult> PatchExpense([FromRoute]int id, [FromBody]JsonPatchDocument<dto.ExpenseViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._expenseRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.ExpenseViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.ExpenseEntity>(model);
                var result = await this._expenseRepository.UpdateAsync(entityToUpdate);
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
        /// Delete Expense 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteExpense")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._expenseRepository.DeleteByIdAsync(id);
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