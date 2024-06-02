using CadastroAPI.Models;
using CadastroAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CadastroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalRepository _repository;
        private readonly INumerosSorteRepository _repositoryNumerosSorte;

        public NotaFiscalController(INotaFiscalRepository repository, INumerosSorteRepository numeroSorte)
        {
            _repository = repository;
            _repositoryNumerosSorte = numeroSorte;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNotaFiscal([FromBody] NotaFiscalDTO notaFiscalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notaFiscal = NotaFiscal.FromDto(notaFiscalDto);

            //try
            //{
            //await _repository.AddNotaFiscalAsync(notaFiscal);
            var nun = notaFiscal.CalcularNumerosSorte();
                await _repositoryNumerosSorte.GerarNumerosSorteAsync(notaFiscal.UsuarioId, notaFiscal.NotaCupom, nun);
                var numeros = _repositoryNumerosSorte.GetNumerosPorNotaFiscal(notaFiscal.NotaCupom).Result;
                return Ok(notaFiscal.ToDto(numeros));
            //}
            //catch (Exception)
            //{
            //    return BadRequest("Ocorreu um erro ao adicionar a nota fiscal.");
            //}
        }

        [HttpPost("add-nota-fiscal-with-image")]
        public async Task<IActionResult> AddNotaFiscalWithImage([FromForm] NotaFiscalWithImageDTO notaFiscalWithImageDto)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var notaFiscal = NotaFiscal.FromDto(notaFiscalWithImageDto.NotaFiscal, usuarioId);
                var imagem = new Imagem(notaFiscal.NotaCupom, notaFiscalWithImageDto.Imagem);

                await _repository.AddNotaFiscalAndImagemAsync(notaFiscal, imagem);

                return Ok("Nota fiscal adicionada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao adicionar a nota fiscal.");
            }
        }

        [HttpGet("{usuarioId}/{notaCupom}")]
        public async Task<IActionResult> GetNotaFiscal(string usuarioId, string notaCupom)
        {
            var notaFiscal = await _repository.GetNotaFiscalAsync(usuarioId, notaCupom);

            if (notaFiscal == null)
            {
                return NotFound("Nota fiscal não encontrada");
            }

            return Ok(notaFiscal);
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetNotasFiscais(string usuarioId)
        {
            var notasFiscais = await _repository.GetNotasFiscaisByUsuarioIdAsync(usuarioId);

            if (notasFiscais == null || !notasFiscais.Any())
            {
                return NotFound("Nenhuma nota fiscal encontrada para o usuário");
            }

            return Ok(notasFiscais);
        }
    }
}
