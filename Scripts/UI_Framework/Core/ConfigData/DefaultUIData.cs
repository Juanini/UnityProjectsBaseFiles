using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HannieEcho.UI.Data
{
    [CreateAssetMenu(fileName = "UIConfig",  menuName = "UI/Data/UIConfig", order = 1)]
    public class DefaultUIData : UIData
    {
        [SerializeField] private AssetLabelReference m_PriorityLabel;
        [SerializeField] private AssetLabelReference m_RestLabel;
        private readonly List<AsyncOperationHandle<GameObject>> m_AssetResultReferences = new List<AsyncOperationHandle<GameObject>>();

        public override Task<bool> LoadPriorityData()
        {
            return LoadData(m_PriorityLabel);
        }

        public override Task<bool> LoadRestData()
        {
            return LoadData(m_RestLabel);
        }

        protected override async Task<bool> LoadData(AssetLabelReference label)
        {
            if (string.IsNullOrEmpty(label.labelString))
            {
                Trace.Log("<color=white>Failed to load ui data, label is empty</color>");
                return false;
            }

            try
            {
                var dataResourcesLocHandleOp = Addressables.LoadResourceLocationsAsync(label.labelString, null);
                await dataResourcesLocHandleOp.Task;

                if (dataResourcesLocHandleOp.Status != AsyncOperationStatus.Succeeded) return false;
                if (dataResourcesLocHandleOp.Result == null) return false;

                var resLocList = dataResourcesLocHandleOp.Result;

                List<Task> taskList = new List<Task>();

                foreach(var item in resLocList)
                {
                    var asyncOpHandle = Addressables.LoadAssetAsync<GameObject>(item);
                    m_AssetResultReferences.Add(asyncOpHandle);
                    taskList.Add(asyncOpHandle.Task);
                }

                await Task.WhenAll(taskList);

                foreach(var item in m_AssetResultReferences)
                {
                    if (item.Status != AsyncOperationStatus.Succeeded)
                    {
                        continue;
                    }

                    if (!item.Result)
                    {
                        continue;
                    }

                    var viewComponent = item.Result.GetComponent<UIView>();

                    if (!viewComponent)
                    {
                        continue;
                    }

                    var type = viewComponent.GetType();

                    if (base.ViewReferences.ContainsKey(type))
                    {
                        // Trace.Log($"View asset type {type} already in collection... skipping");
                        continue;
                    }

                    // Trace.Log($"Asset resource location loaded\n" + $"Asset info type: {type} , name: {viewComponent.gameObject.name}");
                    ViewReferences.Add(type, viewComponent);
                }

                taskList.Clear();

                return true;
            }
            catch (System.Exception ex)
            {
                Trace.Log($"Msg: {ex.Message}");
                return false;
            }
        }

        public override void Clear()
        {
            foreach (var item in m_AssetResultReferences)
                Addressables.Release(item);

            m_AssetResultReferences.Clear();
            base.Clear();
            Trace.Log("<color=red>Destroying UI data</color>");
        }
    }
}