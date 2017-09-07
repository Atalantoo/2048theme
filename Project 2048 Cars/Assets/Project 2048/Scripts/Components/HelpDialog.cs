using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;


class HelpDialog : MonoBehaviour
{
    public GameObject CloseButton;

    void Start()
    {
        CloseButton = gameObject.FindChild("HelpCloseButton");
        CloseButton.OnClick(CloseAction);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }
}

