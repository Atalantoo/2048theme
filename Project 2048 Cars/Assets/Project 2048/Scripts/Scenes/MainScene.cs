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

class MainScene : MonoBehaviour
{
    public MainSceneView View;

    void Start()
    {
        MainSceneDelegate.InjectDependencies(this);
        // TODO MainSceneDelegate.InitializeCore(this);
        Globals.LEVEL_CURRENT = 0;
        BuildMap();
        MainSceneDelegate.InitializeUI(this);
        UpdateScreen();
    }

    // ********************************************************************

    internal void SelectLevelAction(bool arg)
    {
        GetToggle();
        UpdateScreen();
    }

    internal void StartAction()
    {
        FindLevels();
        GetToggle();
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }


    // ********************************************************************

    private void FindLevels()
    {

        Globals.LEVELS_LENGTH = 2;
    }

    private void UpdateScreen()
    {
        // UpdateButtons();
    }

    private void BuildMap()
    {
        // TODO save
        string maxTile = "2048";
        int levels = 9;

        View.LevelTogglePrefab.SetActive(false);
        GameObject original = View.LevelTogglePrefab;
        Transform parent = original.transform.parent;


        View.LevelToggles = new GameObject[levels];

        for (int i = 0; i < levels; i++)
        {
            GameObject toggle = Instantiate(original, parent);

            toggle.name = toggle.name + " " + i;
            GameObject Label = toggle.FindChild("Label");
            Label.GetComponent<Text>().text = (i + 1).ToString();

            GameObject States = toggle.FindChild("States");
            GameObject Unlocked = States.FindChild("Unlocked", true);
            Unlocked.SetActive(false);
            GameObject Locked = States.FindChild("Locked", true);
            Locked.SetActive(false);
            GameObject Building = States.FindChild("Building", true);
            Building.SetActive(false);

            if (i == 0 || i == 1)
            {
                Unlocked.SetActive(true);

                string path = i + "/" + maxTile;
                Sprite sprite = Resources.Load<Sprite>(path);
                GameObject image = Unlocked.FindChild("Image");
                image.GetComponent<Image>().sprite = sprite;
            }
            if (i == 0)
            {
                GameObject achiv1 = Unlocked.FindChild("Achiv1ImageFalse");
                achiv1.SetActive(false);
            }
            if (i == 2)
            {
                Locked.SetActive(true);
                toggle.GetComponent<Toggle>().interactable = false;
            }
            if (i >= 3)
            {
                Building.SetActive(true);
                toggle.GetComponent<Toggle>().interactable = false;

            }

            toggle.SetActive(true);

            bool selected = (Globals.LEVEL_CURRENT == i);
            toggle.GetComponent<Toggle>().isOn = selected;

            View.LevelToggles[i] = toggle;
        }
    }

    private void GetToggle()
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

