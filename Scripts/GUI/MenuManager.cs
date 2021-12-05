using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameEventSystem;

public class MenuManager : MonoBehaviour {

	public static MenuManager Ins;

    public Canvas canvas;
    
    private Dictionary<int, string> menuPrefabs;

    private int currentScreenEntry;
    private GameObject currentScreen;

    /// <summary>
    /// Setup the menus used for this game.
    /// </summary>
    /// <param name="dictionary">Menus dictionary in this game.</param>
    public void CreateMenuDic(Dictionary<int,string> dictionary)
    {
        Trace.Log("CreateMenuDic");
        menuPrefabs = new Dictionary<int, string>();
        menuPrefabs = dictionary;
    }

    private void Awake()
    {
        Trace.Log("MenuManager - Awake");

        Ins = this;
        GameEventManager.StartListening(GameEvents.E_OPEN_SCREEN, OnOpenScreenRequest);
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
        if(menuPrefabs == null)
        {
            Trace.LogError("Menu Manager - Error with menus dictionary.");
            return;
        }

        if(!menuPrefabs.ContainsKey(menu))
        {
            Trace.LogError("Menu Manager - Menus dictionary does not contains key: " + menu);
            return;
        }

        GameObject desiredScreen;
		string prefabLocation = menuPrefabs[menu];
		desiredScreen = (GameObject)Instantiate(Resources.Load(prefabLocation, typeof(GameObject)), canvas.transform);
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
