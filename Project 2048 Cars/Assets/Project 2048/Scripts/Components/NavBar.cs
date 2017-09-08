using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using SimpleJSON;

class NavBar : MonoBehaviour
{
    public string Name ="";
    GameObject AppSubnameText;

    void Start()
    {
        string subname = Name.ToUpper() + "!";
        AppSubnameText = gameObject.FindChild("AppSubnameText");
        AppSubnameText.GetComponent<Text>().text = subname;
    }
}
