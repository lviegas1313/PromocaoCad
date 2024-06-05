using CadastroAPI.Models;
using CadastroAPI.Repositories;
using CadastroAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace CadastroAPI
{

    namespace CadastroAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CadastroController : ControllerBase
        {
            private readonly IUserRepository _userRepository;
            private readonly IEmailService _emailService;
            private readonly IConfiguration _configuration;
            private readonly IMemoryCache _memoryCache;

            public CadastroController(IUserRepository userRepository, IEmailService emailService, IConfiguration configuration, IMemoryCache memoryCache)
            {
                _userRepository = userRepository;
                _emailService = emailService;
                _configuration = configuration;
                _memoryCache = memoryCache;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] UserDTO userDto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                try
                {
                    var user = Usuario.FromDto(userDto);
                    user.CriptografarSenha(new PasswordHashService());
                    await _userRepository.AddAsync(user);

                    return Ok();

                }
                catch (Exception)
                {
                    return BadRequest("Ocorreu um erro ao adicionar Usuario.");
                }

            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
            {
                try
                {
                    var user = await _userRepository.GetByIdAsync(loginRequest.Cpf);
                    if (user == null || !user.ValidarSenha(loginRequest.Senha, new PasswordHashService()))
                        return Unauthorized();

                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
                catch (Exception)
                {

                    return BadRequest("Ocorreu um erro ao logar o Usuario.");
                }

            }

            [HttpPost("recover-password")]
            public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
            {
                try
                {
                    var user = await _userRepository.GetByEmailAsync(request.Email);

                    var resetToken = GeneratePasswordResetToken();
                    var resetLink = Url.Action(nameof(ResetPassword), "Cadastro", new { token = resetToken }, Request.Scheme);

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Expira em 1 hora
                    };
                    _memoryCache.Set(resetToken, user.CPF, cacheOptions);


                    var emailContent = $"Por favor, redefina sua senha clicando <a href='{resetLink}'>aqui</a>.";

                    //await _emailService.SendEmailAsync(user.Email, "Solicitação de Redefinição de Senha", emailContent);


                    return Ok(new { message = "E-mail de redefinição de senha enviado." });
                }
                catch (Exception)
                {
                    return NotFound("Ocorreu um erro ao tentar recuperar a senha");
                }
            }

            [HttpPost("reset-password")]
            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
            {
                try
                {
                    var user = await _userRepository.GetByEmailAsync(request.Email);
                    if (user == null)
                        return NotFound();

                    user.AlterarSenha(request.NewPassword, new PasswordHashService());

                    await _userRepository.UpdateAsync(user);

                    return Ok();
                }
                catch (Exception)
                {

                    return BadRequest("Ocorreu um erro ao Resetar Password do Usuario.");
                }

            }
            private string GenerateJwtToken(Usuario user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.CPF),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtSettings:Audience"]),
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtSettings:Issuer"])
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            private string GeneratePasswordResetToken()
            {
                return Guid.NewGuid().ToString();
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
