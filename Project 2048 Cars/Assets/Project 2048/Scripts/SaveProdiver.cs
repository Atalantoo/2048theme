using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    class SaveProdiver
    {
        public static bool GetSaveBool(string key)
        {
            if (Main.save.ContainsKey(key))
                return Boolean.Parse(Main.save[key]);
            else
                return false;
        }

        public static int GetSaveInt(string key)
        {
            if (Main.save.ContainsKey(key))
                return Int32.Parse(Main.save[key]);
            else
                return -1;
        }
    }
}
