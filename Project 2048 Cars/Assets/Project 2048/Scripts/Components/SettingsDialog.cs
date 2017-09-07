using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;
using Commons.UI;

class SettingsDialog : MonoBehaviour
{
    public GameObject LanguageDialog;
    public GameObject HelpDialog;

    GameObject CloseButton;
    GameObject LanguageButton;
    GameObject HelpButton;

    void Start()
    {
        CloseButton = gameObject.FindChild("CloseButton");
        LanguageButton = gameObject.FindChild("LanguageButton");
        HelpButton = gameObject.FindChild("HelpButton");
        CloseButton.OnClick(CloseAction);
        LanguageButton.OnClick(LanguageOpenAction);
        HelpButton.OnClick(HelpOpenAction);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    internal void LanguageOpenAction()
    {
        gameObject.SetActive(false);
        LanguageDialog.SetActive(true);
    }

    internal void HelpOpenAction()
    {
        gameObject.SetActive(false);
        HelpDialog.SetActive(true);
    }

}


