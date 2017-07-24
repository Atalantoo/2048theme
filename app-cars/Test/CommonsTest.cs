using System;
using System.IO;

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
                string[] words = line.Split(' ');
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
