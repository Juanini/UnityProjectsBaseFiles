using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HannieEcho.UI.Data
{
    public abstract class UIData : ScriptableObject
    {
        public readonly Dictionary<System.Type, UIView> ViewReferences = new Dictionary<System.Type, UIView>();

        public virtual async Task<bool> LoadPriorityData()
        {
            return true;
        }
        public virtual async Task<bool> LoadRestData()
        {
            return true;
        }
        protected virtual async Task<bool> LoadData(AssetLabelReference label)
        {
            return true;
        }
        public virtual void Clear()
        {
            ViewReferences.Clear();
        }
    }
}
