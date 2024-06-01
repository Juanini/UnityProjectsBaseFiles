using UnityEngine;
using System.Collections.Generic;

public class PolygonColliderFromSprites : MonoBehaviour
{
    private List<SpriteRenderer> spriteRenderers;

    public void CreateCollider(List<SpriteRenderer> _spriteRenderers)
    {
        spriteRenderers = _spriteRenderers;
        CombineSpriteColliders();
    }
    
    private void CombineSpriteColliders()
    {
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.isTrigger = true;
        
        List<Vector2> combinedPoints = new List<Vector2>();

        // Calculate the centroid of the selected sprite renderers
        Vector2 centroid = Vector2.zero;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            centroid += (Vector2)spriteRenderer.transform.position;
        }
        centroid /= spriteRenderers.Count;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Sprite sprite = spriteRenderer.sprite;
            Vector2[] spritePoints = sprite.vertices;
            Vector2 spritePosition = (Vector2)spriteRenderer.transform.position - centroid;

            foreach (Vector2 point in spritePoints)
            {
                combinedPoints.Add(spritePosition + point);
            }
        }

        polygonCollider.SetPath(0, combinedPoints.ToArray());

        // Position the game object at the centroid
        transform.position = centroid;
    }
}