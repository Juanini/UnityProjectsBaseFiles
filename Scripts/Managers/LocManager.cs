using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using I2.Loc;
using UnityEngine;

public class LocManager : MonoBehaviour
{
    public static LocManager Ins;

    public const int LANG_EN = 0;
    public const int LANG_ES = 1;
    public const int LANG_IT = 2;
    public const int LANG_FR = 3;

    void Awake()
    {
        Ins = this;
        GameEventManager.StartListening(GameEvents.ON_LANG_CHANGE, OnLangChanged);
    }
    
    // * =====================================================================================================================================
    // * 

    public static string GetText(string _key)
    {
        return I2.Loc.LocalizationManager.GetTranslation(_key);
    }

    // * =====================================================================================================================================
    // * 

    public void OnLangChanged(Hashtable _ht)
    {
        string langName = "";
        int langId = 0;
        
        if(_ht.ContainsKey(GameEventParam.LANG_ID))
        {
            langId = (int)_ht[GameEventParam.LANG_ID];
        }
        else
        {
            langId = LANG_EN;
        }

        switch (langId)
        {
            case LocManager.LANG_EN:
                langName = "English";
                break;
            
            case LocManager.LANG_ES:
                langName = "Spanish";
                break;
            
            case LocManager.LANG_IT:
                langName = "Italian";
                break;
            
            case LocManager.LANG_FR:
                langName = "French";
                break;
        }

        Trace.Log("LangManager - ChangeLang to: " + langName);

        if (LocalizationManager.HasLanguage(langName))
        {
            LocalizationManager.CurrentLanguage = langName;
        }
    }
}
