using CadastroAPI.Models;
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
            private readonly CadastroContext _context;

            public CadastroController(CadastroContext context)
            {
                _context = context;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] User user)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
                _context.Users.Add(user);
                 await _context.SaveChangesAsync();

                return Ok();
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginRequest.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, user.Senha))
                    return Unauthorized();

                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            [HttpPost("recover-password")]
            public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    return NotFound();

                // Implement email sending logic here

                return Ok();
            }

            [HttpPost("reset-password")]
            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    return NotFound();

                user.Senha = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                await _context.SaveChangesAsync();

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
            public string Email { get; set; }
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
