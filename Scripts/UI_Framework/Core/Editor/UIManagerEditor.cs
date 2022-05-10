using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Assertions;

namespace HannieEcho.UI
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        UIManager Target
        {
            get { return (UIManager)target; }
        }

        protected virtual void Awake()
        {
            if (Target.ContextParent)
                return;

            Trace.Log($"Init FaterUICanvas");

            InitMainCanvas();
            InitSafeZone();
            InitBuiltinLayers();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //if (GUILayout.Button("Update values"))
            //{
            //    //Target.GetScenesInManager();
            //    //Target.SetSceneDependencies();
            //}
        }

        #region callbacks
        private void InitMainCanvas()
        {
            Target.gameObject.name = "UI_Root";

            var rootCanvas = Target.GetComponent<Canvas>();
            rootCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            rootCanvas.planeDistance = 1.0f;

            var rootCanvasScaler = Target.GetComponent<CanvasScaler>();
            rootCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }
        private void InitSafeZone()
        {
            var rootTransfom = (RectTransform)Target.transform;
            rootTransfom.anchorMin = new Vector2(0f, 0f);
            rootTransfom.anchorMax = new Vector2(1f, 1f);
            rootTransfom.sizeDelta = new Vector2(0f, 0f);

            Target.ContextParent = UIUtil.CreateFullScreenRT(rootTransfom, "Context");
        }
        private void InitBuiltinLayers()
        {
            Assert.IsNotNull(Target.ContextParent, "Context must not be null");
            var dialogLayerTransform = UIUtil.CreateFullScreenRT(Target.ContextParent, "DialogLayer");
            var panelLayerTransform = UIUtil.CreateFullScreenRT(Target.ContextParent, "PanelLayer");
            var popupLayerTransform = UIUtil.CreateFullScreenRT(Target.ContextParent, "PopUpLayer");

            Target.SetBuiltinLayers(dialogLayerTransform, panelLayerTransform, popupLayerTransform);
        }
        #endregion
    }
}
