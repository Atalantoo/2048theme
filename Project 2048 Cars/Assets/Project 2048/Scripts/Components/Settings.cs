using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;

class Settings : MonoBehaviour
{
    public GameObject TranslateTarget;
    public bool DisplayHelpAtStartup = false;

    GameObject SettingsDialog;
    GameObject LanguageDialog;
    GameObject HelpDialog;

    void Start()
    {
        LanguageDialog = gameObject.FindChild("LanguageDialog", true);
        HelpDialog = gameObject.FindChild("HelpDialog", true);
        SettingsDialog = gameObject.FindChild("SettingsDialog", true);

        LanguageDialog.SetActive(false);
        LanguageDialog.AddComponent<BringToFront>();
        LanguageDialog.AddComponent<LanguageDialog>();

        HelpDialog.SetActive(false);
        HelpDialog.AddComponent<BringToFront>();
        HelpDialog.AddComponent<HelpDialog>();

        SettingsDialog.SetActive(false);
        SettingsDialog.AddComponent<BringToFront>();
        SettingsDialog.AddComponent<SettingsDialog>();

        SettingsDialog script = SettingsDialog.GetComponent<SettingsDialog>();
        script.LanguageDialog = LanguageDialog;
        script.HelpDialog = HelpDialog;
        script.SoundOn = true; // TODO saved
        script.MusicOn = true; // TODO saved

        LanguageDialog script2 = LanguageDialog.GetComponent<LanguageDialog>();
        script2.Translate = Main.Lang;
        script2.Target = TranslateTarget;

        if (DisplayHelpAtStartup)
            HelpDialog.SetActive(true);
    }

    internal void OpenAction()
    {
        SettingsDialog.SetActive(true);
    }

}

