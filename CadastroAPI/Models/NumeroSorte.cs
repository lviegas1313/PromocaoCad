namespace CadastroAPI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NumeroSorte
    {
        [Required]
        public string NotaFiscalId { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Key]
        public string Numero { get; set; }

        public DateTime DataSorteio { get; set; }

        public DateTime DataCadastro { get; set; }

        // Método para converter NumeroSorte para NumeroSorteDto
        public NumeroSorteDTO ToDto()
        {
            return new NumeroSorteDTO
            {
                Numero = this.Numero,
                DataSorteio = this.DataSorteio
            };
        }

        // Método estático para converter NumeroSorteDto para NumeroSorte
        public static NumeroSorte FromDto(NumeroSorteDTO dto)
        {
            return new NumeroSorte
            {
                Numero = dto.Numero,
                DataSorteio = dto.DataSorteio
            };
        }
    }

    public class NumeroSorteDTO
    {
        [Key]
        public string Numero { get; set; }

        public DateTime DataSorteio { get; set; }
    }

}