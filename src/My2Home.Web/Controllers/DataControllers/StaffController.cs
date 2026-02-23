using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My2Home.Core.CommonEnums;
using My2Home.Core.Interfaces;
using My2Home.Web.Controllers.ApiControllers;
using My2Home.Web.Extensions;
using My2Home.Web.Identity;
using My2Home.Web.Services;
using My2Home.Web.ViewModels.Account;
using entity = My2Home.Core.Entities;
namespace My2Home.Web.Controllers.DataControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : BaseApiController<StaffController>
    {
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public StaffController(SignInManager<ApplicationIdentityUser> signInManager,
            IEmailSender emailSender,
            //ISmsSender smsSender,
            RoleManager<IdentityRole> roleManager, IOrganizationUserRepository organizationUserRepository,
            IOrganizationRepository organizationRepository)
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
            //_smsSender = smsSender;
            _roleManager = roleManager;
            this._organizationUserRepository = organizationUserRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<IActionResult> GetAll()
        {
            var identityUserId = HttpContext.User.GetUserId();
            var organizationId = HttpContext.User.GetUserOrganizationId();
            if (string.IsNullOrEmpty(identityUserId) || organizationId <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var result = await _organizationRepository.GetByOrganizationIdAsync(organizationId);
            return new OkObjectResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterStaffViewModel model, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("register");
                }
                var identityUserId = HttpContext.User.GetUserId();
                var organizationId = HttpContext.User.GetUserOrganizationId();
                // var retResult = await this._organizationRepository.GetByIdentityUserIdAsync(identityUserId);
                if (string.IsNullOrEmpty(identityUserId) || organizationId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var identityUser = await _userManager.GetUserAsync(HttpContext.User);
                if (identityUser == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var user = new ApplicationIdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    CountryId = identityUser.CountryId,
                    EmailConfirmed = true,
                    ExpiryDateTime = identityUser.ExpiryDateTime,
                    CreatedDate = System.DateTime.UtcNow,
                    FirstName = model.FullName,
                    PlanTextPassword = model.Password,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.hosteladmin.ToString());
                    await _organizationUserRepository.InsertAsync(new entity.OrganizationUserEntity
                    {
                        OrganizationId = organizationId,
                        OrganizationIdentityUserId = user.Id
                    });
                    await _emailSender.SendWelcomeEmail(model.Email, model.FullName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(model);
                }
                //create identity user
                //Create organization user and add same orginization id 
                //send email 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("lockuser/{userId}")]
        public async Task<IActionResult> LockUserAccount([FromRoute]string userId)
        {
            var userToLock =  await _userManager.FindByIdAsync(userId);
            if(userToLock== null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var result = await _userManager.SetLockoutEnabledAsync(userToLock, true);           
            if (result.Succeeded)
            {
                result = await _userManager.SetLockoutEndDateAsync(userToLock, DateTimeOffset.MaxValue);           
            }            
            if(result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPut("unlockuser/{userId}")]
        public async Task<IActionResult> UnLockUserAccount([FromRoute]string userId)
        {
            
            var userToLock = await _userManager.FindByIdAsync(userId);
            if (userToLock == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
           // var result = await _userManager.SetLockoutEnabledAsync(userToLock, false);
            //if (result.Succeeded)
            //{
                var result = await _userManager.SetLockoutEndDateAsync(userToLock, null);
            //}
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpDelete("{id}", Name = "DeleteStaff")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    StatusCode(StatusCodes.Status400BadRequest, "id must have value");
                }
                var result = await this._organizationRepository.DeleteOrganizationUserAsync(id);
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

    }
}
