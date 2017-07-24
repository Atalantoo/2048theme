using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons.Lang
{
    public class CommonsLang
    {
    }

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

    public class Strings
    {
        public static bool isEmpty(string str)
        {
            return true;
        }

        public static bool isNotEmpty(string str)
        {
            return !isEmpty(str);
        }

        public static bool isBlank(string str)
        {
            return true;
        }

        public static bool isNotBlank(string str)
        {
            return !isBlank(str);
        }
    }

    public class Arrays
    {
        public static bool inBound(string[] array, int i)
        {
            return i < array.Length && i > -1;
        }

        public static bool outOfBound(string[] array, int i)
        {
            return !inBound(array, i);
        }

        public static string join(string[] array, string separator)
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
        public static string join(int[] array, string separator)
        {
            string line = "";
            bool first = true;
            foreach (int x in array)
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
