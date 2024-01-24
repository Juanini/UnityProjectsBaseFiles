using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T ins;
    public static T Ins
    {
        get
        {
            if (ins == null)
            {
                ins = (T)FindObjectOfType(typeof(T));
                if (ins == null)
                {
                    SetupInstance();
                }
            }

            return ins;
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }
    
    private static void SetupInstance()
    {
        ins = (T)FindObjectOfType(typeof(T));
        if (ins == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            ins = gameObj.AddComponent<T>();
        } 
    }

    private void RemoveDuplicates()
    {
        if (ins == null)
        {
            ins = this as T;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}