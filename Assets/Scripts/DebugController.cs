using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugController : MonoBehaviour
{
    protected GameManager gameManager;

    [SerializeField] bool showConsole = false;
    string input;

    public List<object> commandList;

    public static DebugCommand KILL_ALL;
    public static DebugCommand HELO;
    public static DebugCommand<int> PRINT_INT;
    public static DebugCommand<int,int> PRINT_INT2;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        KILL_ALL = new DebugCommand("kill_all", "Removes all npcs from the scene", "kill_all", () =>
        {
            gameManager.KillAllNpcs();
        });

        HELO = new DebugCommand("helo", "writes helo", "helo", () =>
        {
            gameManager.PrintHelo();
        });

        PRINT_INT = new DebugCommand<int>("print_int", "writes int", "print_int <int>", (x) =>
        {
            gameManager.PrintInt(x);
        });

        PRINT_INT2 = new DebugCommand<int,int>("print_int2", "writes int2", "print_int <int> <int>", (x,y) =>
        {
            gameManager.PrintInt2(x,y);
        });
        commandList = new List<object>
        {
            KILL_ALL,
            HELO,
            PRINT_INT,
            PRINT_INT2
        };
    }


    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            
            if(input.Contains(commandBase.commandId))
            {

                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<int, int> != null)
                {
                    (commandList[i] as DebugCommand<int,int>).Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                }

            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showConsole = !showConsole;
            //Debug.Log("konsola -____-");
        }

        if(showConsole)
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                HandleInput();
                input = "";
            }
        }
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
