using System.Collections.Generic;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager Ins;

    // * =====================================================================================================================================
    // * Main

    void Awake()
    {
        Ins = this;
    }

    public bool DataExists(string key)
    {
        return ES3.KeyExists(key);
    }

    // * =====================================================================================================================================
    // * INT

    public void SaveInt(string key, int value)
    {
        ES3.Save<int>(key, value);
    }

    public int LoadInt(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<int>(key);
        }

        return 0;
    }

    public void IncreaseInt(string key, int _amount)
    {
        int value = LoadInt(key);
        value += _amount;
        SaveInt(key, value);
    }

    public void DecreaseInt(string key, int _amount)
    {
        int value = LoadInt(key);
        value -= _amount;
        SaveInt(key, value);
    }

    // * =====================================================================================================================================
    // * FLOAT

    public void SaveFloat(string key, float value)
    {
        ES3.Save<float>(key, value);
    }

    public float LoadFloat(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<float>(key);
        }

        return 0;
    }

    // * =====================================================================================================================================
    // * STRING

    public void SaveString(string key, string value)
    {
        ES3.Save<string>(key, value);
    }

    public string LoadString(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<string>(key);
        }

        return "";
    }

    // * =====================================================================================================================================
    // * BOOL

    public void SaveBool(string key, bool value)
    {
        ES3.Save<bool>(key, value);
    }

    public bool LoadBool(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<bool>(key);
        }

        return false;
    }

    // * =====================================================================================================================================
    // * LIST

    public void SaveList(string key, List<int> list)
    {
        ES3.Save<List<int>>(key, list);
    }

    public List<int> LoadList(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<List<int>>(key);
        }

        return new List<int>();
    }

    // * =====================================================================================================================================
    // * List Item String

    public void SaveItemList_String(string key, List<string> list)
    {
        ES3.Save<List<string>>(key + "_SL", list);
    }

    public List<string> LoadItemList_String(string key)
    {
        if(ES3.KeyExists(key + "_SL"))
        {
            return ES3.Load<List<string>>(key + "_SL");
        }

        return new List<string>();
    }

    // * =====================================================================================================================================
    // * List Int

    public void SaveIntList(string key, List<int> list)
    {
        ES3.Save<List<int>>(key, list);
    }

    public List<int> LoadIntList(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<List<int>>(key);
        }

        return new List<int>();
    }
    
    // * =====================================================================================================================================
    // * 
    
    public void SaveDouble(string key, double value)
    {
        ES3.Save(key, value);
    }

    public double LoadDouble(string key)
    {
        if(ES3.KeyExists(key))
        {
            return ES3.Load<double>(key);
        }

        return 0;
    }
}
