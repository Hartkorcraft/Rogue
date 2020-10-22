using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    protected GameManager gameManager;

    bool showConsole = false;
    string input;

    public static DebugCommand KILL_ALL;
    public static DebugCommand HELO;

    public List<object> commandList;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        KILL_ALL = new DebugCommand("kill_all", "Removes all npcs from the scene", "kill_all", () =>
        {
            gameManager.KillAll();
        });

        HELO = new DebugCommand("helo", "writes helo", "helo", () =>
        {
            gameManager.Helo();
        });

        commandList = new List<object>
        {
            KILL_ALL,
            HELO
        };
    }


/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showConsole = !showConsole;
            //Debug.Log("konsola -____-");
        }
    }*/

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
        Debug.Log("Showed console");
    }
    private void OnReturn(InputValue value)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {

    }      
    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y=0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);  
    }
}
