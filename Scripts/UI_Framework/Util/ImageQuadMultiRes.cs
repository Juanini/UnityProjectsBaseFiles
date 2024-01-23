using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace HannieEcho.UI
{
    [ExecuteInEditMode]
    public class ImageQuadMultiRes : Image
    {
        [Header("Custom Graphics")]
        [SerializeField] private int m_QuadResAtInit = 2;

        private int m_QuadRes;

        public void ResetQuadRes()
        {
            this.enabled = false;
            m_QuadRes = m_QuadResAtInit;
            this.enabled = true;
        }

        protected override void Awake()
        {
            base.Awake();
            m_QuadRes = m_QuadResAtInit;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_QuadResAtInit <= 1) m_QuadResAtInit = 2;
        }
#endif
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var r = GetPixelAdjustedRect();
            var size = m_QuadRes + 1;
            var vCount = size * size;
            var step = new Vector2(r.width / m_QuadRes, r.height / m_QuadRes);
            //var rect = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

            Color32 color32 = color;
            vh.Clear();
            for (int i = 0; i < vCount; i++)
            {
                var index2 = Index2OffsetCoods(i);
                float3 vert = new float3
                {
                    x = r.x + step.x * index2.x,
                    y = r.y + step.y * index2.y
                };
                float4 uv = new float4
                {
                    x = (index2.x / (float)size),
                    y = (index2.y / (float)size)
                };

                vh.AddVert(vert, color32, uv);
            }

            for (int x = 0; x < size - 1; x++)
            {
                for (int y = 0; y < size - 1; y++)
                {
                    var indexBL = OffsetCoords2Index(new int2(x, y));
                    var indexBR = OffsetCoords2Index(new int2(x + 1, y));
                    var indexTR = OffsetCoords2Index(new int2(x + 1, y + 1));
                    var indexTL = OffsetCoords2Index(new int2(x, y + 1));


                    vh.AddTriangle(indexBL, indexTL, indexTR);
                    vh.AddTriangle(indexTR, indexBR, indexBL);
                }
            }

            int2 Index2OffsetCoods(in int index)
            {
                return new int2
                {
                    x = index % size,
                    y = index / size
                };
            }
            int OffsetCoords2Index(in int2 offset)
            {
                return offset.x + (offset.y * size);
            }
        }
    }
}