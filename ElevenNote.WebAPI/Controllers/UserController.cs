using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ElevenNote.Services.Token;
using ElevenNote.Models.Token;

namespace ElevenNote.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService service, ITokenService tokenService)
            {
                _userService = service;
                _tokenService = tokenService;
            }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var registerResult = await _userService.RegisterUserAsync(model);
                if (registerResult)
                {
                    return Ok("User was registered.");
                }
                return BadRequest("User could not be registered.");
            }
        [Authorize]
        [HttpGet("{userId:int}")]    
        public async Task<IActionResult> GetById([FromRoute] int userId)
            {
                var userDetail = await _userService.GetUserByIdAsync(userId);
                if(userDetail is null)
                {
                    return NotFound();
                }
                return Ok(userDetail);
            }
        [HttpPost("~/api/Token")]
        public async Task<IActionResult> Token([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tokenResponse = await _tokenService.GetTokenAsync(request);
            if (tokenResponse is null)
                return BadRequest("Invalid username and Password.");
            return Ok(tokenResponse);
        }
    }
}
