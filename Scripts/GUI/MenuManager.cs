using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameEventSystem;

public class MenuManager : MonoBehaviour {

	public static MenuManager Ins;

	// Menus
    public static int M_MAIN_MENU 	    = 0;
	public static int M_PAUSE 		    = 1;
	public static int M_SETTINGS 	    = 2;
    public static int M_GAME_OVER       = 4;
    public static int M_INTRO           = 5;
    
    private Dictionary<int, string> menuPrefabs;

    private int currentScreenEntry;
    private GameObject currentScreen;

    private string menusPath = "GUI/MenuPrefabs/";

    public void CreateMenuDic()
    {
        menuPrefabs = new Dictionary<int,string>()
		{
			{ M_MAIN_MENU,          menusPath + "MainMenu"          },
            { M_PAUSE,              menusPath + "PauseMenu"         },
            { M_SETTINGS,           menusPath + "SettingsMenu"      },
            { M_GAME_OVER,          menusPath + "GameOverMenu"      },
            { M_INTRO,              menusPath + "IntroMenu"         },
		};	
    }

    private void Awake()
    {
        Ins = this;
        GameEventManager.StartListening(GameEvents.E_OPEN_SCREEN, OnOpenScreenRequest);
        MenuManager.Ins.CreateMenuDic();
    }

	void OnOpenScreenRequest(Hashtable param)
    {
        if(param.ContainsKey(GameEventParam.SCREEN_TO_OPEN))
        { 
            int screen = (int)param[GameEventParam.SCREEN_TO_OPEN];
            ShowMenu(screen);
        }
    }

	public void ShowMenu(int menu, bool hideMenuFlag = true)
    {        
        GameObject desiredScreen;
		string prefabLocation = menuPrefabs[menu];
		desiredScreen = (GameObject)Instantiate(Resources.Load(prefabLocation, typeof(GameObject)));
		showMenu(ref desiredScreen, hideMenuFlag);
		currentScreenEntry = menu;
        
    }

	private void showMenu(ref GameObject menu, bool hideMenuFlag = true)
    {
        MenuBase aCurrentBase = currentScreen != null ? currentScreen.GetComponent<MenuBase>() : null;
        
		if (aCurrentBase)
        {
            aCurrentBase.onExitScreen();

            if (menuPrefabs.ContainsKey(currentScreenEntry))
			{
				Object.DestroyImmediate(currentScreen, true);
			}
        }

        if (currentScreen && hideMenuFlag)
		{
			hideMenu(currentScreen);
		}
            
        Trace.Log("MainMenu - Show Menu:" + menu.name);
        
		currentScreen = menu;
        MenuBase aBase = menu.GetComponent<MenuBase>();
        if (aBase)
		{
			aBase.onEnterScreen();
		}
        else
        {
            Trace.Log("Error, component not found, proceeding the old way...");
            menu.SetActive(true);
        }
    }

    private void hideMenu(GameObject menu)
    {
        Trace.Log("MainMenu - Hide Menu: " + menu.name);
        menu.SetActive(false);
    }
}
