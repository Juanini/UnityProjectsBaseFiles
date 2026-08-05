using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI.Data
{
    [CreateAssetMenu(fileName = "FadeInAnim", menuName = "UI/Animations/Fade In", order = 1)]
    public class FadeInUIAnimation : UIAnimation
    {
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        public override void Init() { }

        public override UniTask AfterScreenInit() => UniTask.CompletedTask;

        public override async UniTask Animate(UIView view)
        {
            CanvasGroup canvasGroup = view.CanvasGroupComponent;
            if (canvasGroup == null)
                return;

            canvasGroup.DOKill();
            canvasGroup.alpha = 0f;

            await canvasGroup.DOFade(1f, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }
    }
}
