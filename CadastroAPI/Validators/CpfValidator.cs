using System.Text.RegularExpressions;


namespace CadastroAPI.Validators
{

    public class CpfValidator
    {
        private string cpf;

        public CpfValidator(string cpf)
        {
            // Remove caracteres não numéricos
            this.cpf = Regex.Replace(cpf, "[^0-9]", "");
        }

        public bool IsValid()
        {
            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], 11) == cpf)
                return false;

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * multiplicadores1[i];
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * multiplicadores2[i];
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
        }

        public override string ToString()
        {
            return cpf;
        }
    }


}
