using UnityEngine;

namespace HannieEcho.UI
{
    public static class UIUtil
    {
        public static RectTransform CreateFullScreenRT(Transform parent, string name)
        {
            var ui_RT = new GameObject(name).AddComponent<RectTransform>();
            ui_RT.SetParent(parent, false);

            ui_RT.anchorMin = new Vector2(0f, 0f);
            ui_RT.anchorMax = new Vector2(1f, 1f);
            ui_RT.sizeDelta = new Vector2(0f, 0f);

            return ui_RT;
        }
    }
}
