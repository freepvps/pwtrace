using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net.Plugins.Sub
{
    public class RandomStringGenerator
    {

        public Random Random { get; set; }
        public string Alphabet { get; set; }

        public string Pattern { get; set; }
        public int RandomMinLength { get; set; }
        public int RandomMaxLength { get; set; }

        public RandomStringGenerator()
        {
            var alphabet = new char['z' - 'a' + 'Z' - 'A' + '9' - '0' + 3];
            var p = 0;
            for (var i = 'a'; i <= 'z'; i++)
            {
                alphabet[p++] = (char)i;
                alphabet[p++] = char.ToUpper((char)i);
            }
            for (var i = '0'; i <= '9'; i++)
            {
                alphabet[p++] = (char)i;
            }
            
            Pattern = "{0}";
            Alphabet = new string(alphabet);

            Random = new Random();
            RandomMinLength = 1;
            RandomMaxLength = 9;
        }

        public string GetRandomString()
        {
            return GetRandomString(Pattern, RandomMinLength, RandomMaxLength);
        }
        public string GetRandomString(string pattern, int minLen, int maxLen)
        {
            return GetRandomString(pattern, minLen, maxLen, Random, Alphabet);
        }
        public string GetRandomString(string pattern, int minLen, int maxLen, Random random, string alphabet)
        {
            var length = random.Next(minLen, maxLen + 1);
            var res = new char[length];

            for (var i = 0; i < length; i++)
            {
                res[i] = alphabet[random.Next(alphabet.Length)];
            }
            return string.Format(pattern, new string(res));
        }
    }
}
