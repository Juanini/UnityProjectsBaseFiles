using System;
using System.Collections.Generic;
using UnityEngine;
using HannieEcho.UI.Data;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI
{
    public enum ViewStatus
    {
        ACTIVE, INACTIVE, ANIMATING
    }

    [System.Serializable]
    public struct ControlView
    {
        public Vector2 Position;
        public Vector2 Size;
    }

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIView : MonoBehaviour
    {
        [Header("Rect proportions")]
        public ControlView rectControl = new ControlView
        {
            Position = new Vector2(0.5f, 0.5f),
            Size = new Vector2(1f, 1f)
        };

        [Header("Anchor proportions")]
        public ControlView anchorControl = new ControlView
        {
            Position = new Vector2(0.5f, 0.5f),
            Size = new Vector2(1f, 1f)
        };

        [Header("Navigation constraints")]
        public bool allowHideNavigationBack = true;

        public virtual void OnViewCreated() { }
        public virtual void OnViewBeforeAppear() { }
        public virtual void OnViewAfterAppear() { }
        public virtual void OnViewBeforeDisappear() { }
        public virtual void OnViewAfterDisappear() { }
        public virtual void OnViewDismissedByBackKey() { }
        public virtual void OnViewLoseFocus() { }
        
        [SerializeField] private UIAnimation m_ShowAnim;
        [HideInInspector] public UIAnimation ShowAnim { get{ return m_ShowAnim; } }

        [SerializeField] private UIAnimation m_HideAnim;

        private ViewStatus m_Status;
        public ViewStatus Status => m_Status;
        [HideInInspector] public UINavigation navController;

        private Coroutine m_CoroutineOfAnimation;
        private CanvasGroup m_CanvasGroupComponent;
        public CanvasGroup CanvasGroupComponent => m_CanvasGroupComponent;

        public HashSet<UIView> Panels { get; private set; }

        protected virtual void Awake()
        {
            m_Status = ViewStatus.INACTIVE;
            m_CanvasGroupComponent = this.GetComponent<CanvasGroup>();
            
            m_ShowAnim?.Init();
            m_HideAnim?.Init();

            this.gameObject.SetActive(false);
        }

        protected UIView RegisterPanel<T>() where T : UIView
        {
            var panel = (T) navController.GetOrCreateView<T>(UINavigation.ViewParentMode.PANEL);
            if (Panels == null)
                Panels = new HashSet<UIView>();
            
            if (panel != null)
                Panels.Add(panel);
            
            return panel;
        }

        [Obsolete]
        protected async UniTask<UIView> RegisterPanel<T>(bool animated, Action<T> beforeShown = null) where T : UIView
        {
            var panel = (T)await navController.ShowPanel<T>(animated, beforeShown);
            if (Panels == null)
                Panels = new HashSet<UIView>();
            
            if (panel != null)
                Panels.Add(panel);
            
            return panel;
        }

        #region ShowCallbacks
        public virtual async UniTask Show(bool animated = true)
        {
            OnViewBeforeAppear();
            if (animated)
            {
                //var tcs = new TaskCompletionSource<bool>();
                m_Status = ViewStatus.ANIMATING;
                
                this.gameObject.SetActive(true);

                if (m_ShowAnim)
                {
                    await m_ShowAnim.Animate(this);
                }

                OnShowEndPromise();
            }
            else
            {
                this.gameObject.SetActive(true);
                OnShowEndPromise();
            }
        }
        protected void OnShowEndPromise()
        {
            m_Status = ViewStatus.ACTIVE;
            OnViewAfterAppear();
        }
        #endregion

        #region HideCallbacks
        public virtual async UniTask Hide(bool animated = true)
        {
            OnViewBeforeDisappear();
            if (animated)
            {
                m_Status = ViewStatus.ANIMATING;
                
                if (m_HideAnim)
                {
                    await m_HideAnim.Animate(this);
                }

                OnHideEndPromise();
            }
            else
            {
                OnHideEndPromise();
            }
        }
        protected void OnHideEndPromise()
        {
            m_Status = ViewStatus.INACTIVE;
            this.gameObject.SetActive(false);
            OnViewAfterDisappear();
        }
        #endregion
    }
}