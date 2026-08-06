using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI.Data
{
    [CreateAssetMenu(fileName = "FadeOutAnim", menuName = "UI/Animations/Fade Out", order = 1)]
    public class FadeOutUIAnimation : UIAnimation
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
            canvasGroup.alpha = 1f;

            await canvasGroup.DOFade(0f, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }
    }
}