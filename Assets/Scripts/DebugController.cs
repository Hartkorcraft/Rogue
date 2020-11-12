using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugController : MonoBehaviour
{
    protected GameManager gameManager;

    [SerializeField] bool showConsole = false;
    string input;

    public List<DebugCommandBase> commandList;

    public static DebugCommand KILL_ALL;
    public static DebugCommand HELO;
    public static DebugCommand<int> PRINT_INT;
    public static DebugCommand<int,int> PRINT_INT2;
    public static DebugCommand HELP;
    public static DebugCommand<string> XD; 
    public static DebugCommand HELPXD;


    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


        KILL_ALL = new DebugCommand("kill_all", "Removes all npcs from the scene", "kill_all", () =>
        {
            gameManager.KillAllNpcs();
        });

        HELO = new DebugCommand("helo", "writes helo", "helo", () =>
        {
            Debug.Log("Helo!");
        });

        PRINT_INT = new DebugCommand<int>("print_int", "writes int", "print_int <int>", (x) =>
        {
            Debug.Log(x);
        });

        PRINT_INT2 = new DebugCommand<int,int>("print_int2", "writes int2", "print_int <int> <int>", (x,y) =>
        {
            Debug.Log(x + " " + y);
        });

        XD = new DebugCommand<string>("XD", "prints commmand description", "XD <string>", (x) =>
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                if ((commandList[i] as DebugCommandBase).commandId == x)
                {
                    Debug.Log((commandList[i] as DebugCommandBase).commandDescription);
                    break;
                }
            }

        });

        HELP = new DebugCommand("help", "prints commmand list", "help", () =>
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                Debug.Log((commandList[i] as DebugCommandBase).commandId);
            }
        });

        HELPXD = new DebugCommand("helpxd", "prints commmand list", "helpxd", () =>
        {
            Debug.Log("XD");
        });

        commandList = new List<DebugCommandBase>
        {
            KILL_ALL,
            HELO,
            PRINT_INT,
            PRINT_INT2,
            XD,
            HELP,
            HELPXD
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
                    if (UtilsHart.isNumeric(properties[1]))
                        (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                    else
                        Debug.Log("Invalid command parametr");
                }
                else if(commandList[i] as DebugCommand<string> !=null)
                {
                    Debug.Log(commandList[i]);
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
                else if (commandList[i] as DebugCommand<int, int> != null)
                {
                    if (UtilsHart.isNumeric(properties[1]) && UtilsHart.isNumeric(properties[2]))
                        (commandList[i] as DebugCommand<int, int>).Invoke(int.Parse(properties[1]), int.Parse(properties[2]));
                    else
                        Debug.Log("Invalid command parametr");
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

    /////////////////////Commends//////////////////


}
