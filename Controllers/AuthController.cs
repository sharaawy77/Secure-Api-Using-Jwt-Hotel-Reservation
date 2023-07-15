using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using Secure_Api_Jwt.Models;
using Secure_Api_Jwt.Services;
using SecureApi.Models;

namespace Secure_Api_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await authService.RegisterAsync(model);
            if (!Result.IsAuth)
            {
                return BadRequest(Result.Message);
            }
            return Ok(Result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await authService.LoginAsync(model);
            if (!Result.IsAuth)
            {
                return BadRequest(Result.Message);
            }
            return Ok(Result);
        }
        [HttpPost("AddToRole")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddToRole([FromBody] AddToRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Result = await authService.AddToRoleAsync(model);
            if (!string.IsNullOrEmpty(Result))
            {
                return BadRequest(Result);
            }
            return Ok();
        }
    }
}
