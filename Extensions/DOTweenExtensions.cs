using DG.Tweening;
using DG.Tweening.Core;
using Shapes;
using Spine.Unity;
using UnityEngine;

public static class DOTweenExtensions
{
    public static Tweener DOColor(this SpriteRenderer target, Color endValue, float duration)
    {
        // Usar getters y setters para manejar la propiedad de color del SpriteRenderer
        var getter = new DOGetter<Color>(() => target.color);
        var setter = new DOSetter<Color>(x => target.color = x);

        return DOTween.To(getter, setter, endValue, duration);
    }
    
    public static Sequence DoMoveVerticallyAndBack(this Transform transform, float moveDistance, float duration)
    {
        Vector3 originalPosition = transform.position;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveY(originalPosition.y + moveDistance, duration / 2))
            .Append(transform.DOMoveY(originalPosition.y, duration / 2));

        return sequence;
    }
    
    public static Tweener DOPunchScale(this Transform target, float punch, float duration)
    {
        Vector3 punchVector = new Vector3(punch, punch, punch);
        return target.DOPunchScale(punchVector, duration);
    }
    
    public static Sequence DoFadeAndMoveFromDirection(this CanvasGroup canvasGroup, Direction direction, float amount, float time, float fadeTo)
    {
        RectTransform rectTransform = canvasGroup.transform as RectTransform;
        Vector2 targetPosition = rectTransform.anchoredPosition;

        Vector2 offset = direction switch
        {
            Direction.UP    => new Vector2(0f, amount),
            Direction.DOWN  => new Vector2(0f, -amount),
            Direction.RIGHT => new Vector2(amount, 0f),
            Direction.LEFT  => new Vector2(-amount, 0f),
            _               => Vector2.zero
        };

        rectTransform.anchoredPosition = targetPosition + offset;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(rectTransform.DOAnchorPos(targetPosition, time))
            .Join(canvasGroup.DOFade(fadeTo, time));

        return sequence;
    }

    // * =====================================================================================================================================
    // * SPINE
    
    public static Tweener DoFade(this SkeletonAnimation skeletonAnimation, float endAlpha, float duration)
    {
        Color startColor = skeletonAnimation.skeleton.GetColor();

        return DOTween.To(() => startColor.a, x =>
        {
            startColor.a = x;
            skeletonAnimation.skeleton.SetColor(startColor);
        }, endAlpha, duration);
    }
    
    // * =====================================================================================================================================
    // * SHAPES
    
    public static Tweener DoFade(this Rectangle rectangle, float endAlpha, float duration)
    {
        var getter = new DOGetter<float>(() => rectangle.Color.a);
        var setter = new DOSetter<float>(alpha =>
        {
            Color color = rectangle.Color;
            color.a = alpha;
            rectangle.Color = color;
        });

        return DOTween.To(getter, setter, endAlpha, duration);
    }
    
    public static Tweener DoColor(this Polygon polygon, Color endValue, float duration)
    {
        return DOTween.To(() => polygon.Color, x => polygon.Color = x, endValue, duration);
    }
    
    public static Tweener DoColor(this Disc polygon, Color endValue, float duration)
    {
        return DOTween.To(() => polygon.Color, x => polygon.Color = x, endValue, duration);
    }
    
    public static Tweener DoRadius(this Disc disc, float endValue, float duration)
    {
        return DOTween.To(() => disc.Radius, x => disc.Radius = x, endValue, duration);
    }
    
    public static Tweener DoFade(this Disc disc, float endAlpha, float duration)
    {
        var getter = new DOGetter<float>(() => disc.Color.a);
        var setter = new DOSetter<float>(alpha =>
        {
            Color color = disc.Color;
            color.a = alpha;
            disc.Color = color;
        });

        return DOTween.To(getter, setter, endAlpha, duration);
    }
    
    /// <summary>
    /// Quickly enlarges the disc’s radius by <paramref name="_punch"/> and springs it back.
    /// Works just like DOTween’s built-in Punch helpers for transforms.
    /// </summary>
    public static Sequence DOPunchRadius(this Disc disc, float _punch, float _duration)
    {
        float originalRadius = disc.Radius;

        // Split the time: fast out, slower elastic return
        Sequence sequence = DOTween.Sequence();
        sequence.Append(
                DOTween.To(() => disc.Radius,
                        x => disc.Radius = x,
                        originalRadius + _punch,
                        _duration * 0.3f)
                    .SetEase(Ease.OutQuad))
            .Append(
                DOTween.To(() => disc.Radius,
                        x => disc.Radius = x,
                        originalRadius,
                        _duration * 0.7f)
                    .SetEase(Ease.OutElastic));

        return sequence;
    }
    
    public static Tweener DoFade(this Polyline polyline, float endAlpha, float duration)
    {
        var getter = new DOGetter<float>(() => polyline.Color.a);
        var setter = new DOSetter<float>(alpha =>
        {
            Color color = polyline.Color;
            color.a = alpha;
            polyline.Color = color;
        });

        return DOTween.To(getter, setter, endAlpha, duration);
    }
    
    public static Tweener DoFade(this Polygon polygon, float endAlpha, float duration)
    {
        var getter = new DOGetter<float>(() => polygon.Color.a);
        var setter = new DOSetter<float>(alpha =>
        {
            Color color = polygon.Color;
            color.a = alpha;
            polygon.Color = color;
        });

        return DOTween.To(getter, setter, endAlpha, duration);
    }
}