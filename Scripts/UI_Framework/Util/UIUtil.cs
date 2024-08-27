using UnityEngine;

namespace HannieEcho.UI
{
    public static class UIUtil
    {
        public static RectTransform CreateFullScreenRT(Transform parent, string name)
        {
            var ui_RT = new GameObject(name).AddComponent<RectTransform>();
            ui_RT.SetParent(parent, false);

            ui_RT.anchorMin = new Vector2(0f, 0f);
            ui_RT.anchorMax = new Vector2(1f, 1f);
            ui_RT.sizeDelta = new Vector2(0f, 0f);

            return ui_RT;
        }
        
        public static void AdjustRectTransformToScreen(RectTransform targetRectTransform, float padding = 10f)
            {
                if (targetRectTransform == null) return;
        
                // Convertir los bordes del RectTransform a coordenadas de pantalla
                Vector3[] corners = new Vector3[4];
                targetRectTransform.GetWorldCorners(corners);
        
                // Obtener el ancho y alto de la pantalla
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
        
                // Calcular el desplazamiento necesario en cada dirección
                float offsetX = 0f;
                float offsetY = 0f;
        
                // Chequear si el rectángulo está fuera de los bordes de la pantalla y calcular el desplazamiento
                if (corners[0].x < padding)  // Izquierda
                {
                    offsetX = padding - corners[0].x;
                }
                if (corners[2].x > screenWidth - padding)  // Derecha
                {
                    offsetX = (screenWidth - padding) - corners[2].x;
                }
                if (corners[0].y < padding)  // Abajo
                {
                    offsetY = padding - corners[0].y;
                }
                if (corners[1].y > screenHeight - padding)  // Arriba
                {
                    offsetY = (screenHeight - padding) - corners[1].y;
                }
        
                // Aplicar el desplazamiento
                targetRectTransform.anchoredPosition += new Vector2(offsetX, offsetY);
            }
    }
}
