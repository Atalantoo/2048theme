using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class GameObjectUtils
{
    /**
     * http://answers.unity3d.com/questions/890636/find-an-inactive-game-object.html
     */
    public static GameObject FindInactive(GameObject parent, String name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    /**
     * http://answers.unity3d.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
     */
    public static void ResizeSpriteToScreen(GameObject go)
    {
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        go.transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float x = (float)worldScreenWidth / width;
        float y = (float)worldScreenHeight / height;

        go.transform.localScale = new Vector3(x, y, 0);
    }

    public static void ResizeViewToScreen(int LandscapeSize, int PortraitSize)
    {
        if (Screen.width > Screen.height)
        {
            Camera.main.orthographicSize = LandscapeSize;
        }
        else
        {
            Camera.main.orthographicSize = PortraitSize;
        }
    }
}

