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
            }

            return ins;
        }
    }

    public virtual void Awake()
    {
        if (ins != null && ins != this)
        {
            Destroy(gameObject);
            return;
        }

        ins = this as T;
    }
}