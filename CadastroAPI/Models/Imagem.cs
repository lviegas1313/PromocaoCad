using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroAPI.Models
{
    public class Imagem
    {
        [Key]
        public string NotaFiscalId { get; set; } // Chave estrangeira para a nota fiscal
        //[Required]
        public byte[] Dados { get; set; } // Dados da imagem para armazenamento

        [NotMapped]
        public MemoryStream ImagemStream
        {
            get => new MemoryStream(Dados);
            set
            {
                using (var memoryStream = new MemoryStream())
                {
                    value.CopyTo(memoryStream);
                    Dados = memoryStream.ToArray();
                }
            }
        }
        public Imagem() { }
        public Imagem(string notaFiscalId, IFormFile imagem)
        {
            NotaFiscalId = notaFiscalId;
            SetImagemFromIFormFile(imagem);
        }

        public void SetImagemFromIFormFile(IFormFile imagem)
        {
            using (var stream = new MemoryStream())
            {
                imagem.CopyTo(stream);
                ImagemStream = stream;
            }
        }
    }
}