using System;
using System.Text.RegularExpressions;

namespace CadastroAPI.Validators
{
    public class CNPJValidator
    {
        private string cnpj;

        public CNPJValidator(string cnpj)
        {
            this.cnpj = Regex.Replace(cnpj, "[^0-9]", ""); // Remove caracteres não numéricos
        }

        public bool IsValid()
        {
            if (cnpj.Length != 14)
                return false;

            if (new string(cnpj[0], 14) == cnpj)
                return false;

            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += (cnpj[i] - '0') * multiplicadores1[i];
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += (cnpj[i] - '0') * multiplicadores2[i];
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cnpj[12] - '0' == digito1 && cnpj[13] - '0' == digito2;
        }

        public override string ToString()
        {
            return cnpj;
        }
    }

}
