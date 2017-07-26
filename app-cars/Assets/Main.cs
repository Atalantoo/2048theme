using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    GameManager2048 game;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Game Initialize", gameObject);
        game = new GameManager2048();
        game.Initialize();

        Debug.Log("Game Start", gameObject);
        string[][] startInput = new string[1][];
        startInput[0] = new string[2];
        startInput[0][0] = "4";
        startInput[0][1] = "4";
        string[][] startOutput = game.Start(startInput);

        Debug.Log("Game Start Result", gameObject);
        Debug.Log(startOutput, gameObject);
        Debug.Log(startOutput[0][0] + startOutput[0][1] + startOutput[0][2] + startOutput[0][3], gameObject);
        Debug.Log(startOutput[1][0] + startOutput[1][1] + startOutput[1][2] + startOutput[1][3], gameObject);
        Debug.Log(startOutput[2][0] + startOutput[2][1] + startOutput[2][2] + startOutput[2][3], gameObject);
        Debug.Log(startOutput[3][0] + startOutput[3][1] + startOutput[3][2] + startOutput[3][3], gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
