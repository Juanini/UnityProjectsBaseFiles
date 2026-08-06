using Cysharp.Threading.Tasks;
using DG.Tweening;
using HannieEcho.UI;
using HannieEcho.UI.Data;
using UnityEngine;

public class FadeOutUIAnimation : MonoBehaviour
{
    [CreateAssetMenu(fileName = "FadeOutAnim", menuName = "UI/Animations/Fade Out", order = 1)]
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
            canvasGroup.alpha = 1f;

            await canvasGroup.DOFade(0f, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }
    }
}
