using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Main : MonoBehaviour
{

    GameManager gameManager;
    Game game;

    void Start()
    {
        gameManager = new GameManager();
        GameStartInput startInput = new GameStartInput()
        {
            Height = 4,
            Width = 4
        };
        game = gameManager.Start(startInput);

        CreateItems(gameObject);
    }

    void Update()
    {

    }

    void CreateItems(GameObject parent)
    {
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // SCALE
        // https://docs.unity3d.com/ScriptReference/SystemInfo-deviceType.html
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        if (SystemInfo.deviceType == DeviceType.Handheld)
            scaler.referenceResolution = new Vector2(640, 960);
        else if (SystemInfo.deviceType == DeviceType.Desktop)
            scaler.referenceResolution = new Vector2(1280, 720);
        else if (SystemInfo.deviceType == DeviceType.Console)
            scaler.referenceResolution = new Vector2(1920, 1080);
        else
        {
            Debug.LogError("DeviceType is unknown.");
            scaler.referenceResolution = new Vector2(1024, 1024);
        }
        // REFRESH
        GraphicRaycaster ray = canvasGO.AddComponent<GraphicRaycaster>();

        canvasGO.transform.SetParent(parent.transform);

        for (int y = 0; y<4;y++)
            for (int x = 0; x < 4; x++)
            {
                CreateItem(canvasGO, x, y);
            }
    }

    void CreateItem(GameObject parent, int x, int y)
    {
        GameObject itemGO = new GameObject(y + "," +x);

        /*
        Text txt;
        txt = itemGO.AddComponent<Text>();
        txt.resizeTextForBestFit = true;
        txt.resizeTextMinSize = 12;
        txt.resizeTextMaxSize = 40;
        txt.alignment = TextAnchor.MiddleCenter;

        txt.text = "2";
        txt.color = Color.black;
        txt.font = Resources.Load<Font>("Fonts/Arial");
        txt.fontSize = 16;
        txt.fontStyle = FontStyle.Normal;
        */
        init(itemGO, "cars/Sprites/f40/0000.png");

        itemGO.transform.SetParent(parent.transform);
    }

    public static void init(GameObject go, string resource)
    {

        Image image = go.AddComponent<Image>();

        Texture2D tex = Resources.Load<Texture2D>(resource);

        // IF scliced
        // http://docs.unity3d.com/ScriptReference/Sprite.Create.html
        float pixelsPerUnit = 100.0f;
        uint extrude = 0;
        SpriteMeshType meshType = SpriteMeshType.Tight;
        // http://docs.unity3d.com/450/Documentation/ScriptReference/Sprite-border.html
        // Vector4 border = Vector4.zero;
        // http://docs.unity3d.com/ScriptReference/Vector4.html
        Vector4 border = new Vector4(10, 10, 10, 10);
        image.type = Image.Type.Sliced;

        // ELSE
        // image.type = Image.Type.Simple;

        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), pixelsPerUnit, extrude, meshType,
            border);
    }
}
