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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using My2Home.Web.Helpers;

namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : BaseApiController<ExpenseCategoryController>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        public ExpenseCategoryController(IExpenseCategoryRepository expenseCategoryRepository)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
        }

        #region Get Methods
        /// <summary>
        /// Get country 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetExpenseCategory")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                //_logger.LogInformation("From city controller");
                var retResult = await this._expenseCategoryRepository.GetByIdAsync(id);
                if (retResult == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.ExpenseCategoryViewModel>(retResult);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("dropdown", Name = "GetDropDownByExpenseCategoryId")]
        public async Task<IActionResult> GetDropDownByExpenseCategoryId()
        {
            //var organizationId = HttpContext.User.GetUserOrganizationId();
            var retResult = await this._expenseCategoryRepository.GetPageListAsync(1, 500000, null);           
            if (retResult != null && retResult.Count() > 0)
            {
                var model = retResult.Select(c => c.Map());
               // model = model.OrderBy(n => n.Label).ToList();
                var pagedResult = new dto.PagedResult<dto.SelectItemViewModel>(model, 1, 500000, (retResult.FirstOrDefault().TotalRows));
                return new OkObjectResult(pagedResult);
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
        [HttpGet("{pageNumber}/{rowsPerPage}/{searchCondition?}", Name = "GetExpenseCategoryPageListAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPageListAsync([FromRoute] int pageNumber, int rowsPerPage, string searchCondition = null)
        {
            var retResult = await this._expenseCategoryRepository.GetPageListAsync(pageNumber, rowsPerPage, searchCondition);
            var model = GetMapper.Map<IEnumerable<dto.ExpenseCategoryViewModel>>(retResult);
            if (model != null && model.Count() > 0)
            {
                var pagedResult = new dto.PagedResult<dto.ExpenseCategoryViewModel>(model, pageNumber, rowsPerPage, (retResult.FirstOrDefault().TotalRows));
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
        public async Task<IActionResult> Create([FromBody] dto.ExpenseCategoryViewModel model)
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

                var entityToInsert = GetMapper.Map<entity.ExpenseCategoryEntity>(model);
                var result = await this._expenseCategoryRepository.InsertAsync(entityToInsert);
                if (result.HasValue && result.Value > 0)
                {
                    model.ExpenseCategoryId = result.Value;
                    return CreatedAtRoute("GetExpenseCategory", new { id = model.ExpenseCategoryId }, model);
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
        public async Task<IActionResult> Put([FromBody] dto.ExpenseCategoryViewModel model)
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

                var entityToUpdate = GetMapper.Map<entity.ExpenseCategoryEntity>(model);
                var result = await this._expenseCategoryRepository.UpdateAsync(entityToUpdate);
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
        [HttpPatch("{id}", Name = "PatchExpenseCategory")]
        public async Task<IActionResult> PatchCountry([FromRoute]int id, [FromBody]JsonPatchDocument<dto.ExpenseCategoryViewModel> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var infoFromDb = await this._expenseCategoryRepository.GetByIdAsync(id);
                if (infoFromDb == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var model = GetMapper.Map<dto.ExpenseCategoryViewModel>(infoFromDb);
                patchDocument.ApplyTo(model);
                var entityToUpdate = GetMapper.Map<entity.ExpenseCategoryEntity>(model);
                var result = await this._expenseCategoryRepository.UpdateAsync(entityToUpdate);
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
        [HttpDelete("{id}", Name = "DeleteExpenseCategory")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                if (id <= 0)
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must be greater then zero");
                }
                var result = await this._expenseCategoryRepository.DeleteByIdAsync(id);
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

/* 
 [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult> Get(int id)
        {
            return NotFound("");
        }

        [HttpPost]
        public async Task<IActionResult>  Post([FromBody] string value)
        {
            return NoContent();
            //return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult>  Delete(int id)
        {
            return NoContent();
        }
     
     
     */
