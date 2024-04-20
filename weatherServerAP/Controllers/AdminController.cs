using CountryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using weatherServerAP.DTO;

namespace weatherServerAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldCitiesUser> userManager,
        JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            WorldCitiesUser? user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                return Unauthorized("Wrong username or password.");
            }
            bool success = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!success) 
            {
                return Unauthorized("Wrong username or password.");
            }
            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            string jwtTokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "YAY!",
                Token = jwtTokenString
            });
        }
    }
}
