using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    private UnityAction onEnterCallback;
    private string onEnterTag;
    private UnityAction onExitCallback;

    public void Setup(
        UnityAction _onEnterCallback,
        UnityAction _onExitCallback,
        string _tag)
    {
        onEnterTag = _tag;
        onEnterCallback = _onEnterCallback;
        onExitCallback = _onExitCallback;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(onEnterTag))
        {
            onEnterCallback?.Invoke();    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(onEnterTag))
        {
            onExitCallback?.Invoke();
        }
    }
}
