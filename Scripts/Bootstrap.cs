using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameEventSystem;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bootstrap : MonoBehaviour
{
    [BoxGroup("Assets")] public List<AssetReference> assetsToLoad;
    
    void Start()
    {
        LoadAssets();
    }
    
    protected async UniTask LoadAssets()
    {
        foreach (var assetToLoad in assetsToLoad)
        {
            await AssetLoader.Ins.InstantiatePrefabAsync(assetToLoad);
        }
        
        GameEventManager.TriggerEvent(GameEvents.OnGameAssetsLoaded);
    }
}
