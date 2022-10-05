using Constants.IdentityServer;
using Identity.API.Domain;
using Identity.API.Domain.DTO;
using Identity.Domain.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Authorize(Policy = PolicyConstants.Admin)]
    [Route("api/v1/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public IdentityController(
            ILogger<IdentityController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _Logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDTO userDTO)
        {
            User user = await _UserManager.FindByNameAsync(userDTO.Username);

            if (user != null)
            {
                return BadRequest("User already registered");
            }

            var result = await _UserManager.CreateAsync(new User()
            {
                UserName = userDTO.Username,
                Email = userDTO.Email,
                LockoutEnabled = false
            }, userDTO.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        //{
        //    bool logged = false;

        //    var existing = await _UserManager.FindByNameAsync(loginUserDTO.Username);

        //    if (existing == null)
        //    {
        //        return BadRequest("Invalid username");
        //    }

        //    var signInResult = await _SignInManager.CheckPasswordSignInAsync(existing, loginUserDTO.Password, false);
            
        //    if (signInResult.Succeeded)
        //    {
        //        logged = true;
        //    }

        //    if (logged)
        //    {
        //        return Ok("Logged");
        //    }
        //    else
        //    {
        //        return BadRequest("Invalid password");
        //    }
        //}
    }
}
