using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Test
{
    class CommonsTest
    {
        public static string[][] readFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[][] res = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var regex = new Regex("  *");
                string[] words = regex.Split(line);
                res[i] = new string[words.Length];
                for (int j = 0; j < words.Length; j++)
                {
                    res[i][j] = words[j];
                }
            }
            return res;
        }
    }
}
