using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace HannieEcho.UI.Data
{
    public abstract class UIAnimation : ScriptableObject
    {
        public abstract UniTask Animate(UIView view);
        public abstract void Init();
        public abstract UniTask AfterScreenInit();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="animTime"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public Task SavePositionAndMove(Transform trans, float offsetX, float offsetY, float animTime, Ease ease = Ease.OutCirc)
        {
            Vector2 originPos = trans.localPosition;
            SendOffScreen(trans, offsetX, offsetY);
            return trans.DOLocalMove(originPos, animTime).SetEase(ease).AsyncWaitForCompletion();
        }

        public Vector2 SendOffScreen(Transform obj, float xOffset, float yOffset)
        {
            Vector2 originalPos = obj.localPosition;
            obj.localPosition = new Vector2(obj.localPosition.x + xOffset, obj.localPosition.y + yOffset);
            return originalPos;
        }
    }
}

public enum EAnimationDirection
{
    In,
    Out
}