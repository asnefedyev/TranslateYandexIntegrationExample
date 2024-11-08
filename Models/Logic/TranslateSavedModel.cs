using System.Security.Cryptography;
using System.Text;

namespace TranslateExample.Models.Logic
{
    public class TranslateSavedModel
    {
        public string SourceText { get; set; }
        public short? Count { get; set; }
        public SourceCacheEnum Source { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string? TargetText { get; set; }

        public string Hash => ComputeHash();

        private string ComputeHash()
        {
            if (string.IsNullOrEmpty(SourceText) || string.IsNullOrEmpty(SourceLang) || string.IsNullOrEmpty(TargetLang))
            {
                return null;
            }

            var input = $"{SourceText}{SourceLang}{TargetLang}";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}