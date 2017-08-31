using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Commons
{
    public static class GameObjectExtensions
    {
        /**
        * http://answers.unity3d.com/questions/890636/find-an-inactive-game-object.html
        */
        public static GameObject FindChild(this GameObject parent, string name, bool includeInactive = false)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(includeInactive);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
    }
}