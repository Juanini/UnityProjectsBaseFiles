using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI
{
    public class UINavigation : MonoBehaviour
    {
        private UIManager m_Manager;
        private Dictionary<Type, UIView> m_ViewCollection = new Dictionary<Type, UIView>();

        private Dictionary<Type, UIView> m_PanelViewCollection = new Dictionary<Type, UIView>();
        private Stack<UIView> m_NavigationViewStack = new Stack<UIView>();
        private Stack<UIView> m_NavigationPopUpStack = new Stack<UIView>();

        public enum ViewParentMode
        {
            DIALOG, PANEL, POPUP
        }

        [Header("Back Key Settings")]
        [SerializeField]
        private bool backKeyControlsNavigation = true;
        [SerializeField]
        private bool backKeyAnimatedAnimation = true;

        private UIView m_OnBackView = null;

        private void Awake()
        {
            m_OnBackView = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && backKeyControlsNavigation)
            {
                if (!m_OnBackView)
                {
                    m_OnBackView = GetTopView();
                    if (!m_OnBackView) return;

                    if (m_OnBackView.allowHideNavigationBack)
                    {
                        Trace.Log($"<color=green>Start back key process with view: {m_OnBackView.name}</color>");
                        m_OnBackView.OnViewDismissedByBackKey();
                        HideNavLastView(animated: backKeyAnimatedAnimation);
                    }
                    else
                        m_OnBackView = null;
                }
            }
        }

        public void Init(UIManager manager)
        {
            m_Manager = manager;
        }

        public async UniTask<UIView> ResetRoot<T>() where T : UIView
        {
            while (m_NavigationPopUpStack.Count > 0)
               m_NavigationPopUpStack.Pop().Hide(animated: false).Forget();

            while (m_NavigationViewStack.Count > 0)
               m_NavigationViewStack.Pop().Hide(animated: false).Forget();

            await HideAllPanels(animated: false);

            return await ShowDialog<T>(animated: false);
        }

        public bool IsViewOnFront(UIView view)
        {
            if (m_NavigationPopUpStack.Count > 0)
                return view == m_NavigationPopUpStack.Peek();
            else if (m_NavigationViewStack.Count > 0)
                return view == m_NavigationViewStack.Peek();
            else return false;
        }

        public bool GetView<T>(out T view) where T : UIView
        {
            var type = typeof(T);
            var result = m_ViewCollection.TryGetValue(type, out UIView v);
            view = v as T;
            return result;
        }

        public UIView GetTopView()
        {
            if (m_NavigationPopUpStack.Count > 0)
                return m_NavigationPopUpStack.Peek();
            else if (m_NavigationViewStack.Count > 0)
                return m_NavigationViewStack.Peek();
            else
                return null;
        }

        public void BringPanelTop<T>()
        {
            var type = typeof(T);
            if (m_PanelViewCollection.TryGetValue(type, out UIView view))
                view.transform.SetAsLastSibling();
        }

        public Type IsPanelOnView<T>(UIView view) where T : UIView
        {
            foreach (var item in view.Panels)
            {
                if (item.GetType() is T)
                    return item.GetType();
            }
            return null;
        }

        public UIView GetOrCreateView<T>(in ViewParentMode parentMode) where T : UIView
        {
            try
            {
                var type = typeof(T);
                RectTransform parent = null;
                switch (parentMode)
                {
                    case ViewParentMode.DIALOG: parent = m_Manager.DialogLayer; break;
                    case ViewParentMode.PANEL: parent = m_Manager.PanelLayer; break;
                    case ViewParentMode.POPUP: parent = m_Manager.PopUpLayer; break;
                }

                if (!m_ViewCollection.TryGetValue(type, out UIView view))
                {
                    Trace.Log($"No view of type {type} exists, creating view...");
                    view = UIViewCreator.CreateView(type, parent);
                    view.navController = this;
                    view.OnViewCreated();
                    m_ViewCollection.Add(type, view);
                }
                else
                    view.transform.SetAsLastSibling();
                return view;
            }
            catch (System.Exception ex) //when (!(ex is OperationCanceledException))
            {
                Trace.Log($"Critical error at loading: {ex}");
                return null;
            }
        }

        #region ShowCallbacks       
        public async UniTask<UIView> ShowDialog<T>(bool animated = true, bool hideLast = true, Action<T> beforeShown = null) where T : UIView
        {
            UIView view = GetOrCreateView<T>(ViewParentMode.DIALOG);

            if (!view) return null;

            beforeShown?.Invoke((T)view);
            if (view.Status == ViewStatus.ACTIVE) return null;

            UIView lastView = null;
            if (m_NavigationViewStack.Count > 0)
                lastView = m_NavigationViewStack.Peek();

            m_NavigationViewStack.Push(view);

            if (hideLast)
            {
                UpdatePanels(lastView, view, animated);
                if (lastView) lastView.Hide();
            }
            await view.Show(animated);

            return view;
        }
        public async UniTask<UIView> ShowPanel<T>(bool animated = true, Action<T> beforeShown = null) where T : UIView
        {
            var type = typeof(T);
            UIView view = GetOrCreateView<T>(ViewParentMode.PANEL);

            if (!view) return null;

            beforeShown?.Invoke((T)view);
            if (view.Status == ViewStatus.ACTIVE) return view;

            await view.Show(animated);

            if (!m_PanelViewCollection.ContainsKey(type))
                m_PanelViewCollection.Add(type, view);

            return view;
        }
        private async UniTask ShowPanel(UIView panel, bool animated, Action<UIView> beforeShown = null)
        {
            var type = panel.GetType();

            beforeShown?.Invoke(panel);
            // if (panel.Status == ViewStatus.ACTIVE) return;

            await panel.Show(animated);

            if (!m_PanelViewCollection.ContainsKey(type))
                m_PanelViewCollection.Add(type, panel);
        }
        public async UniTask<UIView> ShowPopup<T>(bool animated = true, Action<T> beforeShown = null) where T : UIView
        {
            UIView view = GetOrCreateView<T>(ViewParentMode.POPUP);

            if (!view) return null;

            if (m_NavigationViewStack.Count > 0)
            {
                var topView = m_NavigationViewStack.Peek();
                topView.OnViewLoseFocus();
            }

            beforeShown?.Invoke((T)view);
            if (view.Status == ViewStatus.ACTIVE) return view;

            // UIView lastView = null;
            // if (m_NavigationPopUpStack.Count > 0)
            //     lastView = m_NavigationPopUpStack.Peek();

            m_NavigationPopUpStack.Push(view);

            await view.Show(animated);
            // if (lastView) lastView.Hide();

            return view;
        }
        
        public async UniTask<UIView> ShowPopupStacked<T>(bool animated = true, Action<T> beforeShown = null) where T : UIView
        {
            UIView view = GetOrCreateView<T>(ViewParentMode.POPUP);

            if (!view) return null;

            if (m_NavigationViewStack.Count > 0)
            {
                var topView = m_NavigationViewStack.Peek();
                topView.OnViewLoseFocus();
            }

            beforeShown?.Invoke((T)view);
            if (view.Status == ViewStatus.ACTIVE) return view;

            m_NavigationPopUpStack.Push(view);
            await view.Show(animated);

            return view;
        }
        #endregion

        #region HideCallbacks
        public async UniTask HideNavLastViewTask(bool animated = true)
        {
            Trace.Log($"<color=blue>Start hide view</color>");

            if (m_NavigationPopUpStack.Count > 0)
            {
                m_OnBackView = m_NavigationPopUpStack.Pop();
                await m_OnBackView.Hide(animated);
                if (m_NavigationPopUpStack.Count > 0)
                {
                    var currentView = m_NavigationPopUpStack.Peek();
                    UpdatePanels(m_OnBackView, currentView, animated);
                    currentView.Show(animated).Forget();
                }

                Trace.Log($"<color=red>End hide view</color>");
                m_OnBackView = null;                
                return;
            }

            if (m_NavigationViewStack.Count > 0)
            {
                m_OnBackView = m_NavigationViewStack.Pop();
                await m_OnBackView.Hide(animated);
                if (m_NavigationViewStack.Count > 0)
                {
                    var currentView = m_NavigationViewStack.Peek();
                    UpdatePanels(m_OnBackView, currentView, animated);
                    currentView.Show(animated).Forget();
                }

                Trace.Log($"<color=red>End hide view</color>");
                m_OnBackView = null;
                return;
            }
        }
        public void HideNavLastView(bool animated = true)
        {
            Trace.Log($"<color=blue>Start hide view</color>");

            if (m_NavigationPopUpStack.Count > 0)
            {
                m_OnBackView = m_NavigationPopUpStack.Peek();
                m_NavigationPopUpStack.Pop();
                m_OnBackView.Hide(animated).Forget();

                if (m_NavigationPopUpStack.Count > 0)
                {
                    var currentView = m_NavigationPopUpStack.Peek();
                    UpdatePanels(m_OnBackView, currentView, animated);
                    currentView.Show(animated).Forget();
                }

                Trace.Log($"<color=red>End hide view</color>");
                m_OnBackView = null;
                return;
            }

            if (m_NavigationViewStack.Count > 0)
            {
                m_OnBackView = m_NavigationViewStack.Peek();
                m_NavigationViewStack.Pop();
                m_OnBackView.Hide(animated).Forget();

                if (m_NavigationViewStack.Count > 0)
                {
                    var currentView = m_NavigationViewStack.Peek();
                    UpdatePanels(m_OnBackView, currentView, animated);
                    currentView.Show(animated).Forget();
                }

                Trace.Log($"<color=red>End hide view</color>");
                m_OnBackView = null;
                return;
            }
        }
        public async UniTask HidePanel<T>(bool animated = true)
        {
            var type = typeof(T);
            if (m_PanelViewCollection.TryGetValue(type, out UIView view))
            {
                if (!view) return;
                if (view.Status == ViewStatus.INACTIVE) return;

                m_PanelViewCollection.Remove(type);
                await view.Hide(animated);
            }
        }       
        private async UniTask HidePanel(UIView panel, bool animated = true)
        {
            if (!panel) return;
            if (panel.Status == ViewStatus.INACTIVE) return;

            var type = panel.GetType();
            m_PanelViewCollection.Remove(type);
            await panel.Hide(animated);
        }
        public UniTask HideAllPanels(bool animated = true)
        {
            List<UniTask> m_Tasks = new List<UniTask>();
            foreach (var item in m_PanelViewCollection)
                m_Tasks.Add(item.Value.Hide(animated));

            m_PanelViewCollection.Clear();

            if (m_Tasks.Count > 0)
                return UniTask.WhenAll(m_Tasks);
            else
                return UniTask.CompletedTask;
        }
        #endregion

        private void UpdatePanels(UIView lastView, UIView currentView, bool animated)
        {
            if (!lastView || lastView.Panels == null)
            {
                if (currentView.Panels != null)
                {
                    foreach (var panel in currentView.Panels)
                        ShowPanel(panel, animated).Forget();
                }
            }
            else
            {
                if (currentView.Panels == null)
                {
                    foreach (var panel in lastView.Panels)
                        HidePanel(panel, animated).Forget();
                }
                else
                {
                    foreach (var panel in currentView.Panels)
                    {
                        if (!lastView.Panels.Contains(panel))
                            ShowPanel(panel, animated).Forget();
                    }
                    foreach (var panel in lastView.Panels)
                    {
                        if (!currentView.Panels.Contains(panel))
                            HidePanel(panel, animated).Forget();
                    }
                }
            }
        }
        
        public bool IsPopupActive()
        {
            return m_NavigationPopUpStack.Count > 0;
        }
    }
}
