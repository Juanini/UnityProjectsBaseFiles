using DG.Tweening;
using DG.Tweening.Core;
using Shapes;
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
    
    public static Tweener DoColor(this Polygon polygon, Color endValue, float duration)
    {
        return DOTween.To(() => polygon.Color, x => polygon.Color = x, endValue, duration);
    }
    
    public static Sequence DoMoveVerticallyAndBack(this Transform transform, float moveDistance, float duration)
    {
        Vector3 originalPosition = transform.position;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveY(originalPosition.y + moveDistance, duration / 2))
            .Append(transform.DOMoveY(originalPosition.y, duration / 2));

        return sequence;
    }
}