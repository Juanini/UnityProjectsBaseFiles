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
}