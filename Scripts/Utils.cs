using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static bool IsInListRange(int val, int maxList)
    {
        if(val < 0 || val > maxList)
        { 
            return false;
        }

        return true;
    }

    public static bool IsInMainWorld()
    {
        
        return false;
    }

    public static void ShuffleIntList(ref List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++) 
        {
            int temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
    }
}
