using HelpDesk.api.Entities;
using HelpDesk.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelpDesk.api.Controllers
{
    [Route("v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;

        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthenticationRepository repository,IConfiguration configuration)
        {
            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));
            _configuration = configuration ?? 
                throw new ArgumentNullException(nameof(configuration));
        }
        public class UserInfoDTO
        {
            public string userName { get; set; }
            public string password { get; set; }

            public UserInfoDTO(string userName, string password)
            {
                this.userName = userName;
                this.password = password;
            }
        }

        [HttpPost]
        public ActionResult<string> Authenticate(UserInfoDTO userInfo)
        {
            User? user = _repository.Authenticate(userInfo.userName, userInfo.password);

            if (user != null)
            {
                //Generate Token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[] {
                                    new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName)
                                };

                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(2),
                    credentials);

                var tokenToReturn = new JwtSecurityTokenHandler()
                    .WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);
            }
            else
                return NotFound();


        }
    }
}
