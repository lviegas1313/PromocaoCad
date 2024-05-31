using CadastroAPI.Models;
using CadastroAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadastroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalRepository _repository;

        public NotaFiscalController(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotaFiscal([FromForm] NotaFiscal notaFiscal)//, [FromForm] IFormFile imagem
        {
            if (notaFiscal == null)// || imagem == null || imagem.Length == 0
            {
                return BadRequest();
            }

            try
            {
                //var novaImagem = new Imagem(notaFiscal.NotaCupom, imagem);

               // await _repository.AddNotaFiscalAndImagemAsync(notaFiscal, novaImagem);

                return CreatedAtAction(nameof(GetNotaFiscal), new { id = notaFiscal.NotaCupom }, notaFiscal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{usuarioId}/{notaCupom}")]
        public async Task<ActionResult<NotaFiscal>> GetNotaFiscal(string usuarioId, string notaCupom)
        {
            var notaFiscal = await _repository.GetNotaFiscalAsync(usuarioId, notaCupom);

            if (notaFiscal == null)
            {
                return NotFound();
            }

            return Ok(notaFiscal);
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotasFiscais(string usuarioId)
        {
            var notasFiscais = await _repository.GetNotasFiscaisByUsuarioIdAsync(usuarioId);

            if (notasFiscais == null || !notasFiscais.Any())
            {
                return NotFound();
            }

            return Ok(notasFiscais);
        }
    }

}
