using Azure.Identity;
using CountryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Weatherserver.DTO;

namespace Weatherserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldCitiesUser> userManager,
                                     JwtHandeler jwtHandeler) : ControllerBase
    {
        [HttpPost("Login")]
        
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {

            WorldCitiesUser? user = await userManager.FindByNameAsync(loginRequest.userName);
            if (user == null)
            {
                return Unauthorized("Wrong Username");

            }
            bool success = await userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!success)
            {
                return Unauthorized("Wrong Password");
            }
            
            JwtSecurityToken token = await jwtHandeler.GetTokenAsync(user);
            var jwtstring = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult { 
                Success = true,
                Message = "heh",
                Token = jwtstring
            });
        }
    }
}