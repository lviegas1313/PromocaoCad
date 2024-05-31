using CadastroAPI.Context;
using CadastroAPI.Models;
using CadastroAPI.Repositories;
using CadastroAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CadastroAPI
{

    namespace CadastroAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CadastroController : ControllerBase
        {
            private readonly IUserRepository _userRepository;
            
            public CadastroController(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            
            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] User user)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //user
                await _userRepository.AddAsync(user);

                return Ok();
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
            {
                var user = await _userRepository.GetByIdAsync(loginRequest.Cpf);
                if (user == null || !user.ValidarSenha(loginRequest.Senha))
                    return Unauthorized();

                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }            

            [HttpPost("recover-password")]
            public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                    return NotFound();

                // Implement email sending logic here

                return Ok();
            }

            [HttpPost("reset-password")]
            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                    return NotFound();

               // user.
               // await _context.SaveChangesAsync();

                return Ok();
            }

            private string GenerateJwtToken(User user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("your_secret_key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, user.CPF)
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }

        public class LoginRequest
        {
            public string Cpf { get; set; }
            public string Senha { get; set; }
        }

        public class RecoverPasswordRequest
        {
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }
    }


}
