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
    public bool SoundOn = true;
    public bool MusicOn = true;

    GameObject CloseButton;
    GameObject LanguageButton;
    GameObject HelpButton;
    GameObject SoundButton;
    GameObject SoundButtonOn;
    GameObject SoundButtonOff;
    GameObject MusicButton;
    GameObject MusicButtonOn;
    GameObject MusicButtonOff;

    void Start()
    {
        CloseButton = gameObject.FindChild("CloseButton");
        LanguageButton = gameObject.FindChild("LanguageButton");
        HelpButton = gameObject.FindChild("HelpButton");
        SoundButton = gameObject.FindChild("SoundButton");
        SoundButtonOn = SoundButton.FindChild("TextEnabled i18n=enabled", true);
        SoundButtonOff = SoundButton.FindChild("TextDisabled i18n=disabled", true);
        MusicButton = gameObject.FindChild("MusicButton");
        MusicButtonOn = MusicButton.FindChild("TextEnabled i18n=enabled", true);
        MusicButtonOff = MusicButton.FindChild("TextDisabled i18n=disabled", true);
        CloseButton.OnClick(CloseAction);
        LanguageButton.OnClick(LanguageOpenAction);
        HelpButton.OnClick(HelpOpenAction);
        SoundButton.OnClick(SoundAction);
        MusicButton.OnClick(MusicAction);
        InitializeSound();
        InitializeMusic();
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

    internal void SoundAction()
    {
        SoundOn = !SoundOn;
        InitializeSound();
    }

    internal void MusicAction()
    {
        MusicOn = !MusicOn;
        InitializeMusic();
    }

    private void InitializeSound()
    {
        SoundButtonOn.SetActive(SoundOn);
        SoundButtonOff.SetActive(!SoundOn);
    }
    private void InitializeMusic()
    {
        MusicButtonOn.SetActive(MusicOn);
        MusicButtonOff.SetActive(!MusicOn);
    }
}


