using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Commons;
using Commons.UI;

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
        Globals.LEVEL_CURRENT = Int32.Parse(ToggleHelper.GetSelection(View.LevelToggles).name);
        UpdateScreen();
    }

    internal void StartAction()
    {
        Globals.LEVEL_CURRENT = Int32.Parse(ToggleHelper.GetSelection(View.LevelToggles).name);
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }

    internal void SettingsOpenAction()
    {
        View.Settings.GetComponent<Settings>().OpenAction();
    }

    // ********************************************************************

    private void UpdateScreen()
    {
        UpdateLevelDescription();
        int c = Int32.Parse(Main.save["achivement_count"]);
        View.AchiementCountText.GetComponent<Text>().text = c.ToString();
    }

    private void UpdateLevelDescription()
    {
        int i = Globals.LEVEL_CURRENT;

        string currentLevel = (i + 1).ToString();
        View.LevelDescriptionIndexText.GetComponent<Text>().text = currentLevel;

        string maxTile = Main.save["level_" + i + "_tile_max"];
        string path = i + "/" + maxTile;
        Sprite sprite = Resources.Load<Sprite>(path);
        View.LevelDescriptionImage.GetComponent<Image>().sprite = sprite;

        BuildLevelAchivs(View.LevelDescription, i);
    }

    private void BuildMap()
    {
        int level_max = Globals.LEVEL_MAX;
        int level_count = SaveProdiver.GetSaveInt("level_count");
        Globals.LEVEL_CURRENT = 0;

        View.LevelTogglePrefab.SetActive(false);
        GameObject original = View.LevelTogglePrefab;
        Transform parent = original.transform.parent;

        View.LevelToggles = new GameObject[level_max];
        for (int i = 0; i < level_max; i++)
        {
            GameObject go = Instantiate(original, parent);
            go.name = i.ToString();

            GameObject Label = go.FindChild("Label");
            Label.GetComponent<Text>().text = (i + 1).ToString();

            GameObject States = go.FindChild("States");
            GameObject Unlocked = States.FindChild("Unlocked", true);
            Unlocked.SetActive(false);
            GameObject Locked = States.FindChild("Locked", true);
            Locked.SetActive(false);
            GameObject WorkInProgress = States.FindChild("WorkInProgress", true);
            WorkInProgress.SetActive(false);

            Toggle toggle = go.GetComponent<Toggle>();
            bool open = SaveProdiver.GetSaveBool("level_" + i + "_unlocked");
            if (open)
            {
                Unlocked.SetActive(true);

                string maxTile = Main.save["level_" + i + "_tile_max"];

                string path = i + "/" + maxTile;
                Sprite sprite = Resources.Load<Sprite>(path);
                GameObject image = Unlocked.FindChild("Image");
                image.GetComponent<Image>().sprite = sprite;

                BuildLevelAchivs(Unlocked, i);

                Globals.LEVEL_CURRENT = i;
            }
            else if (i >= level_count)
            {
                WorkInProgress.SetActive(true);
                toggle.interactable = false;
            }
            else
            {
                int needed = SaveProdiver.GetSaveInt("level_" + i + "_require");
                if (needed == -1) needed = 99;
                Locked.FindChild("LockedValueText").GetComponent<Text>().text = needed.ToString();
                Locked.SetActive(true);
                toggle.interactable = false;
            }

            go.SetActive(true);
            toggle.onValueChanged.AddListener(SelectAction);
            toggle.isOn = false;

            View.LevelToggles[i] = go;
        }

        View.LevelToggles[Globals.LEVEL_CURRENT]
            .GetComponent<Toggle>()
                .isOn = true;
    }

    private void BuildLevelAchivs(GameObject go, int i)
    {
        bool achiv0512 = SaveProdiver.GetSaveBool("level_" + i + "_achiv_0512");
        bool achiv1024 = SaveProdiver.GetSaveBool("level_" + i + "_achiv_1024");
        bool achiv2048 = SaveProdiver.GetSaveBool("level_" + i + "_achiv_2048");
        BuildLevelAchivs(go, "0512", achiv0512);
        BuildLevelAchivs(go, "1024", achiv1024);
        BuildLevelAchivs(go, "2048", achiv2048);
    }

    private void BuildLevelAchivs(GameObject go, string id, bool cond)
    {
        GameObject achiv = go.FindChild("Achiv" + id + "ImageFalse", true);
        achiv.SetActive(!cond);
    }
    
}

