using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;


class LanguageDialog : MonoBehaviour
{
    public TranslateUIProvider Translate;
    public GameObject Target;

    GameObject CloseButton;
    GameObject ValueText;
    GameObject LanguageTogglePrefab;
    GameObject[] LanguageToggles;

    void Start()
    {
        CloseButton = gameObject.FindChild("CloseButton");
        LanguageTogglePrefab = gameObject.FindChild("LanguageTogglePrefab", true);
        ValueText = gameObject.FindChild("LanguageValueText");
        BuildLanguageList();
        CloseButton.OnClick(CloseAction);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    internal void SelectedAction(bool arg)
    {
        string current = ToggleHelper.GetSelection(LanguageToggles).name;

        Translate.PreferredLanguage = current;
        string label = Translate.Language(current).label;
        ValueText.GetComponent<Text>().text = label;

        Translate.Initialize();
        Translate.Apply(Target);
    }

    // INTERNAL *************************************************

    private void BuildLanguageList()
    {
        string current = Translate.PreferredLanguage;
        Language[] langs = Translate.Languages;

        // current
        string current_label = Translate.Language(current).label;
        ValueText.GetComponent<Text>().text = current_label;

        // load prefab
        GameObject original = LanguageTogglePrefab;
        Transform parent = original.transform.parent;
        original.SetActive(false);

        // populate
        int count = langs.Length;
        LanguageToggles = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            Language lang = langs[i];
            string code = lang.code;
            string label = lang.label;
            bool status = current.ToLower().Equals(code.ToLower());

            GameObject go = GameObject.Instantiate(original, parent);
            go.name = code;
            go.GetComponentInChildren<Text>().text = label;
            go.GetComponent<Toggle>().isOn = status;
            go.GetComponent<Toggle>().onValueChanged.AddListener(SelectedAction);
            go.SetActive(true);

            LanguageToggles[i] = go;
        }
    }

}
