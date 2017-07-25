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
        public static void CheckArgument(bool condition)
        {
            CheckArgument(condition, null);
        }

        public static void CheckArgument(bool condition, string message)
        {
            if (!condition)
                throw new Exception("Illegal Argument!" + (message == null ? "" : ": " + message));
        }
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Strings.html)
    public class Strings
    {
        public static bool NullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        public static bool NullOrBlank(string str)
        {
            return NullOrEmpty(str == null ? null : ReplaceAll(str, " ", ""));
        }

        public static string ReplaceAll(string str, string oldChar, string newChar)
        {
            return Regex.Replace(str, @"\s+", "");
        }
    }

    public class Arrays
    {
        public static T[] Add<T>(T[] array, T element)
        {
            T[] res = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                res[i] = array[i];
            res[array.Length] = element;
            return res;
        }
        public static bool InBound(string[] array, int i)
        {
            return i < array.Length && i > -1;
        }

        public static bool OutOfBound(string[] array, int i)
        {
            return !InBound(array, i);
        }
    }

    // https://google.github.io/guava/releases/16.0/api/docs/com/google/common/base/Joiner.html
    public class Joiner
    {
        string separator;

        public static Joiner On(string separator)
        {
            Joiner j = new Joiner();
            j.separator = separator;
            return j;
        }

        public string Join(string[] array)
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
