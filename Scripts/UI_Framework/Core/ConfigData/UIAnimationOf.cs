using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI.Data
{
    public abstract class UIAnimationOf<T> : UIAnimation where T : UIView
    {
        public override void Init() { }

        public override UniTask AfterScreenInit() => UniTask.CompletedTask;

        public sealed override void BeforeAnimate(UIView _view)
        {
            if (_view is T typedView)
                BeforeAnimate(typedView);
        }

        public sealed override UniTask Animate(UIView _view)
        {
            if (_view is T typedView)
                return Animate(typedView);

            Debug.LogError($"{name}: expected a {typeof(T).Name} but got {_view.GetType().Name}.", _view);
            return UniTask.CompletedTask;
        }

        protected virtual void BeforeAnimate(T _view) { }

        protected abstract UniTask Animate(T _view);
    }
}
