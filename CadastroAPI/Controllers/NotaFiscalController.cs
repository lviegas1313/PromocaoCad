using CadastroAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadastroAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly CadastroContext _context;

        public NotaFiscalController(CadastroContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotaFiscal([FromBody] NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
            {
                return BadRequest();
            }

            _context.NotasFiscais.Add(notaFiscal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotaFiscal), new { id = notaFiscal.NotaCupom }, notaFiscal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotaFiscal([FromBody] NotaFiscal notaFiscal, IFormFile imagem)
        {
            if (notaFiscal == null)
            {
                return BadRequest();
            }

            if (imagem != null && imagem.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await imagem.CopyToAsync(stream);
                    notaFiscal.Imagem = stream;
                }
            }

            _context.NotasFiscais.Add(notaFiscal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotaFiscal), new { id = notaFiscal.NotaCupom }, notaFiscal);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotaFiscal(string id)
        {
            var notaFiscal = await _context.NotasFiscais
                .Include(n => n.Produtos)
                .FirstOrDefaultAsync(n => n.NotaCupom == id);

            if (notaFiscal == null)
            {
                return NotFound();
            }

            return Ok(notaFiscal);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotasFiscais()
        {
            var notasFiscais = await _context.NotasFiscais
                .Include(n => n.Produtos)
                .ToListAsync();

            return Ok(notasFiscais);
        }
    }
}
