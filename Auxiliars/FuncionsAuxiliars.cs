using System;
using System.Text;

namespace KarmaWebAPI
{
    public class FuncionsAuxiliars
    {
        public string GenerarPasswordAleatori()
        {
            const int longitudMinima = 8;
            const string majuscules = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string minuscules = "abcdefghijklmnopqrstuvwxyz";
            const string numeros = "0123456789";
            const string caractersEspecials = "!@#$%^&*()_+[]{}|;:,.<>?";

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Assegurar que la contrasenya conté almenys un de cada tipus de caràcter
            password.Append(majuscules[random.Next(majuscules.Length)]);
            password.Append(minuscules[random.Next(minuscules.Length)]);
            password.Append(numeros[random.Next(numeros.Length)]);
            password.Append(caractersEspecials[random.Next(caractersEspecials.Length)]);

            // Omplir la resta de la contrasenya fins a la longitud mínima
            string totsElsCaracters = majuscules + minuscules + numeros + caractersEspecials;
            for (int i = password.Length; i < longitudMinima; i++)
            {
                password.Append(totsElsCaracters[random.Next(totsElsCaracters.Length)]);
            }

            // Barrejar els caràcters per evitar patrons previsibles
            return Barrejar(password.ToString());
        }

        private string Barrejar(string input)
        {
            char[] array = input.ToCharArray();
            Random random = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                char value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
    }
}
