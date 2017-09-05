/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Commons;

class MapScene : MonoBehaviour
{
    public MaSceneView View;

    void Start()
    {
        MapSceneDelegate.InjectDependencies(this);
        // TODO MainSceneDelegate.InitializeCore(this);
        Globals.LEVEL_CURRENT = 0;
        BuildMap();
        MapSceneDelegate.InitializeUI(this);
        UpdateScreen();
    }

    // ********************************************************************

    internal void SelectAction(bool arg)
    {
        GetLevelSelection();
        UpdateScreen();
    }

    internal void StartAction()
    {
        GetLevelSelection();
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }

    internal void SettingsCloseAction()
    {
        View.SettingsDialog.SetActive(false);
    }

    internal void SettingsOpenAction()
    {
        View.SettingsDialog.SetActive(true);
    }

    // ********************************************************************

    private void UpdateScreen()
    {
        UpdateLevelDescription();
        int c = Int32.Parse(Globals.save["achivement_count"]);
        View.AchiementCountText.GetComponent<Text>().text = c.ToString();
    }

    private void UpdateLevelDescription()
    {
        int i = Globals.LEVEL_CURRENT;

        string currentLevel = (i + 1).ToString();
        View.LevelDescriptionIndexText.GetComponent<Text>().text = currentLevel;

        string maxTile = Globals.save["level_" + i + "_tile_max"];
        string path = i + "/" + maxTile;
        Sprite sprite = Resources.Load<Sprite>(path);
        View.LevelDescriptionImage.GetComponent<Image>().sprite = sprite;

        BuildLevelAchivs(View.LevelDescription, i);
    }

    private void BuildMap()
    {
        int level_max = Globals.LEVEL_MAX;
        int level_count = GetSaveInt("level_count");

        View.LevelTogglePrefab.SetActive(false);
        GameObject original = View.LevelTogglePrefab;
        Transform parent = original.transform.parent;

        View.LevelToggles = new GameObject[level_max];

        for (int i = 0; i < level_max; i++)
        {
            GameObject go = Instantiate(original, parent);

            go.name = go.name + " " + i;
            GameObject Label = go.FindChild("Label");
            Label.GetComponent<Text>().text = (i + 1).ToString();

            GameObject States = go.FindChild("States");
            GameObject Unlocked = States.FindChild("Unlocked", true);
            Unlocked.SetActive(false);
            GameObject Locked = States.FindChild("Locked", true);
            Locked.SetActive(false);
            GameObject Building = States.FindChild("Building", true);
            Building.SetActive(false);

            Toggle toggle = go.GetComponent<Toggle>();
            bool open = GetSaveBool("level_" + i + "_unlocked");
            if (open)
            {
                Unlocked.SetActive(true);

                string maxTile = Globals.save["level_" + i + "_tile_max"];

                string path = i + "/" + maxTile;
                Sprite sprite = Resources.Load<Sprite>(path);
                GameObject image = Unlocked.FindChild("Image");
                image.GetComponent<Image>().sprite = sprite;

                BuildLevelAchivs(Unlocked, i);
            }
            else if (i >= level_count)
            {
                Building.SetActive(true);
                toggle.interactable = false;
            }
            else
            {
                int needed = GetSaveInt("level_" + i + "_require");
                if (needed == -1) needed = 99;
                Locked.FindChild("LockedValueText").GetComponent<Text>().text = needed.ToString();
                Locked.SetActive(true);
                toggle.interactable = false;
            }

            go.SetActive(true);

            bool selected = (Globals.LEVEL_CURRENT == i);
            toggle.isOn = selected;
            toggle.onValueChanged.AddListener(SelectAction);

            View.LevelToggles[i] = go;
        }
    }

    private void BuildLevelAchivs(GameObject go, int i)
    {
        bool achiv0512 = GetSaveBool("level_" + i + "_achiv_0512");
        bool achiv1024 = GetSaveBool("level_" + i + "_achiv_1024");
        bool achiv2048 = GetSaveBool("level_" + i + "_achiv_2048");
        BuildLevelAchivs(go, "0512", achiv0512);
        BuildLevelAchivs(go, "1024", achiv1024);
        BuildLevelAchivs(go, "2048", achiv2048);
    }

    private void BuildLevelAchivs(GameObject go, string id, bool cond)
    {
        GameObject achiv = go.FindChild("Achiv" + id + "ImageFalse", true);
        achiv.SetActive(!cond);
    }

    private bool GetSaveBool(string key)
    {
        if (Globals.save.ContainsKey(key))
            return Boolean.Parse(Globals.save[key]);
        else
            return false;
    }

    private int GetSaveInt(string key)
    {
        if (Globals.save.ContainsKey(key))
            return Int32.Parse(Globals.save[key]);
        else
            return -1;
    }

    private void GetLevelSelection()
    {
        GameObject go;
        Toggle tog;
        for (int i = 0; i < View.LevelToggles.Length; i++)
        {
            go = View.LevelToggles[i];
            tog = go.GetComponent<Toggle>();
            if (tog.isOn)
                Globals.LEVEL_CURRENT = i;
        }
    }
}

