using CadastroAPI.Models;
using CadastroAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace CadastroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalRepository _repository;
        private readonly INumerosSorteRepository _repositoryNumerosSorte;

        public NotaFiscalController(INotaFiscalRepository repository, INumerosSorteRepository numeroSorte)
        {
            _repository = repository;
            _repositoryNumerosSorte = numeroSorte;
        }

        //[HttpPost("add")]
        //public async Task<IActionResult> AddNotaFiscal([FromBody] NotaFiscalDTO notaFiscalDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (User.Identity?.IsAuthenticated ?? false)
        //    {
        //        notaFiscalDto.UsuarioId = User.FindFirst(ClaimTypes.Name)?.Value;
        //    }

        //    try
        //    {
        //        var notaFiscal = NotaFiscal.FromDto(notaFiscalDto);
        //        await _repository.AddNotaFiscalAsync(notaFiscal);
        //        var nun = notaFiscal.CalcularNumerosSorte();
        //        await _repositoryNumerosSorte.GerarNumerosSorteAsync(notaFiscal.UsuarioId, notaFiscal.NotaCupom, nun);
        //        var numeros = _repositoryNumerosSorte.GetNumerosPorNotaFiscal(notaFiscal.NotaCupom).Result;
        //        return Ok(notaFiscal.ToDto(numeros));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Ocorreu um erro ao adicionar a nota fiscal.");
        //    }
        //}

        [HttpPost("Adicionar-nota_fiscal_cupom")]
        public async Task<IActionResult> AddNotaFiscalWithImage([FromForm] NotaFiscalDTO notaFiscalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (User.Identity?.IsAuthenticated ?? false)
                {
                    notaFiscalDto.UsuarioId = User.FindFirst(ClaimTypes.Name)?.Value;
                }

                var usuarioId = User.FindFirst(ClaimTypes.Name).Value;
                var aux = JsonSerializer.Deserialize<List<ProdutoDTO>>(notaFiscalDto.Produtosstring);
                notaFiscalDto.Produtos = aux;
                var notaFiscal = NotaFiscal.FromDto(notaFiscalDto);

                if (notaFiscalDto.Imagem != null)
                {
                    var imagem = new Imagem(notaFiscal.NotaCupom, notaFiscalDto.Imagem);
                    await _repository.AddNotaFiscalAndImagemAsync(notaFiscal, imagem);
                    var nun = notaFiscal.CalcularNumerosSorte();
                    await _repositoryNumerosSorte.GerarNumerosSorteAsync(notaFiscal.UsuarioId, notaFiscal.NotaCupom, nun);
                    var numeros = _repositoryNumerosSorte.GetNumerosPorNotaFiscal(notaFiscal.NotaCupom).Result;
                    return Ok(notaFiscal.ToDto(numeros));
                }

                return BadRequest("imagem da nota precisa ser enviada");
            }
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro ao adicionar a nota fiscal.");
            }
        }

        [HttpGet("{notaCupom}")]
        public async Task<IActionResult> GetNotaFiscal(string notaCupom)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;

                var notaFiscal = await _repository.GetNotaFiscalAsync(usuarioId, notaCupom);

                if (notaFiscal == null)
                {
                    return NotFound("Nota fiscal não encontrada");
                }

                return Ok(notaFiscal);
            }
            catch (Exception)
            {
                return BadRequest("Nota fiscal não encontrada");
            }
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetNotasFiscais()
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.Name)?.Value;
                var notasFiscais = await _repository.GetNotasFiscaisByUsuarioIdAsync(usuarioId);

                if (notasFiscais == null || !notasFiscais.Any())
                {
                    return NotFound("Nenhuma nota fiscal encontrada para o usuário");
                }

                return Ok(notasFiscais);
            }
            catch (Exception)
            {

                return BadRequest("Notas fiscais não encontradas");
            }

        }
    }
}
