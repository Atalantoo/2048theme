using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons.Lang
{
    public class CommonsLang
    {
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Preconditions.html
    public class PreConditions
    {
        public static void checkArgument(bool condition)
        {
            checkArgument(condition, null);
        }

        public static void checkArgument(bool condition, string message)
        {
            if (!condition)
                throw new Exception("Illegal Argument!" + (message == null ? "" : ": " + message));
        }
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Strings.html)
    public class Strings
    {
        public static bool isNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        public static bool isNullOrBlank(string str)
        {
            return isNullOrEmpty(str == null ? null : replaceAll(str, " ", ""));
        }

        public static string replaceAll(string str, string oldChar, string newChar)
        {
            return Regex.Replace(str, @"\s+", "");
        }
    }

    public class Arrays
    {
        public static T[] add<T>(T[] array, T element)
        {
            T[] res = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                res[i] = array[i];
            res[array.Length] = element;
            return res;
        }
        public static bool inBound(string[] array, int i)
        {
            return i < array.Length && i > -1;
        }

        public static bool outOfBound(string[] array, int i)
        {
            return !inBound(array, i);
        }
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Joiner.html
    public class Joiner
    {
        string separator;

        public static Joiner on(string separator)
        {
            Joiner j = new Joiner();
            j.separator = separator;
            return j;
        }

        public string join(string[] array)
        {
            string line = "";
            bool first = true;
            foreach (string x in array)
            {
                if (!first)
                    line += separator;
                line += x;
                first = false;
            }
            return line;
        }
    }
}
