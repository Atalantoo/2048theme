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
            throw new NotImplementedException();
            return true;
        }

        public static bool isNotEmpty(string str)
        {
            return !isEmpty(str);
        }

        public static bool isBlank(string str)
        {
            throw new NotImplementedException();
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
    }

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
