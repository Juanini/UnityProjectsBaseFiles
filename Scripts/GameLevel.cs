using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameLevel : MonoBehaviour
{
    [BoxGroup("Assets")] public Transform assetsParent;
    [BoxGroup("Assets")] public List<AssetReference> assetsToLoad;
    
    void Start()
    {
        OnSceneLoaded();
        // GameManager.Ins.OnLevelLoaded(this);
    }

    public virtual void OnSceneLoaded() { }
    public virtual void OnSceneBeforeBeingUnloaded() { }

    protected async UniTask LoadAssets()
    {
        foreach (var assetToLoad in assetsToLoad)
        {
            var obj = await AssetLoader.Ins.InstantiatePrefabAsync(assetToLoad, assetsParent);

            if (obj != null)
            {
                Trace.Log(this.name + " - " + "LOAD ASSET: " + obj.name);
            }
        }
    }
}
