using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    GameManager game;

    void Start()
    {
        Debug.Log("Game Initialize", gameObject);
        game = new GameManager();

        Debug.Log("Game Start", gameObject);
        GameStartInput startInput = new GameStartInput()
        {
            Height = 4,
            Width = 4
        };
        Game startOutput = game.Start(startInput);

        Debug.Log("Game Start Result", gameObject);
        Debug.Log(startOutput, gameObject);
    }

    void Update()
    {

    }
}
