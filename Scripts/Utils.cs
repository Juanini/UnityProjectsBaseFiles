using System.Collections;
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
}
