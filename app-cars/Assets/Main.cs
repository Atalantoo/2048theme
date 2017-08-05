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
        // TODO load resources

        gameManager = new GameManager();
        GameStartInput startInput = new GameStartInput()
        {
            Height = 4,
            Width = 4
        };
        game = gameManager.Start(startInput);

        // TODO assign sprites from game
        // TODO assign moves from game
    }

    void Update()
    {

    }
}
