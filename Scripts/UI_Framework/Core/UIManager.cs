using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Assertions;
using HannieEcho.UI.Data;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace HannieEcho.UI
{
    [System.Serializable]
    public class UnityEventUI : UnityEvent<UINavigation>
    { }

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(UINavigation))]
    public class UIManager : MonoBehaviour
    {
        [Header("Built in Layers")]
        [SerializeField, UIReadOnly]
        private RectTransform m_DialogLayer;
        public RectTransform DialogLayer => m_DialogLayer;

        [SerializeField, UIReadOnly]
        private RectTransform m_PanelLayer;
        public RectTransform PanelLayer => m_PanelLayer;

        [SerializeField, UIReadOnly]
        private RectTransform m_PopUpLayer;
        public RectTransform PopUpLayer => m_PopUpLayer;

        [HideInInspector] public RectTransform ContextParent;

        public bool autoInit = true;

        [Header("Views Data")]
        [SerializeField] private UIData m_ConfigData;

        [Header("Events")]
        public UnityEventUI onLoadedData;
        public UnityEvent onFailedData;

        private Canvas m_CanvasComponent;
        private UINavigation m_Navigation;

        private EventSystem eventSystem;

        #region MonoCallbacks
        protected virtual void Awake()
        {
            m_CanvasComponent = GetComponent<Canvas>();

            Assert.IsNotNull(m_DialogLayer, "DialogLayer is null.");
            Assert.IsNotNull(m_PanelLayer, "PanelLayer is null");
            Assert.IsNotNull(m_PopUpLayer, "PanelLayer is null");
            Assert.IsNotNull(m_ConfigData, "ViewConfig can't be null");

            m_Navigation = GetComponent<UINavigation>();
            m_Navigation.Init(this);
        }

        private void Start()
        {
            if (!autoInit) return;

            Init();
        }
        private void OnDestroy()
        {
            UIViewCreator.Purge();
        }
        #endregion

        public async Task Init()
        {
            var result = await m_ConfigData.LoadPriorityData();
            m_ConfigData.LoadRestData();

            if (result)
            {
                UIViewCreator.Init(m_ConfigData);
                onLoadedData?.Invoke(m_Navigation);
            }
            else
            {
                Trace.Log("UIConfig data couldn't be load the data");
                onFailedData?.Invoke();
            }

            CheckEventSystem();
            CheckSafeAreaHelper();

            // EventBuffer.Add<BlockInputEvent>(new BlockInputEvent());
		    // EventBuffer.Register<BlockInputEvent>(OnBlockInputEvent);

            // EventBuffer.Add<ReleaseInputEvent>(new ReleaseInputEvent());
		    // EventBuffer.Register<ReleaseInputEvent>(OnReleaseInputEvent);
        }

        public void SetTargetCamera(Camera cam)
        {
            m_CanvasComponent.worldCamera = cam;
        }

        private void CheckEventSystem()
        {
            if(eventSystem == null)
            {
                eventSystem = FindObjectOfType<EventSystem>(true);
            }
        }
        
        private void CheckSafeAreaHelper()
        {
            // if(safeAreaHelper == null)
            // {
            //     safeAreaHelper = FindObjectOfType<SafeAreaHelper>(true);
            // }
        }

        // private void OnBlockInputEvent(IEvent evnt)
        // {
        //     this.Log("MenuAnimation - Block Input");

        //     CheckEventSystem();
        //     eventSystem.enabled = false;
        // }

        // private void OnReleaseInputEvent(IEvent evnt)
        // {
        //     this.Log("MenuAnimation - Release Input");
        //     CheckEventSystem();
        //     eventSystem.enabled = true;
        // }

#if UNITY_EDITOR
        public void SetBuiltinLayers(RectTransform dialog, RectTransform panel, RectTransform popup)
        {
            m_DialogLayer = dialog;
            m_PanelLayer = panel;
            m_PopUpLayer = popup;
        }
#endif
    }
}

// public struct BlockInputEvent : IEvent { }
// public struct ReleaseInputEvent : IEvent { }