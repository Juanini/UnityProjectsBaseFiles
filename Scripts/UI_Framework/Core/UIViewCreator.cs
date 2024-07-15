using System;
using HannieEcho.UI.Data;
using UnityEngine;

namespace HannieEcho.UI
{
    public class UIViewCreator
    {
        private static UIData m_Data;

        public static void Init(UIData data)
        {
            m_Data = data;
        }

        public static void Purge()
        {
            m_Data?.Clear();
            m_Data = null;
        }

        private static UIView GetViewRef(in Type viewType)
        {
            if (!m_Data) return null;
            if (!m_Data.ViewReferences.TryGetValue(viewType, out UIView view))
            {
                Trace.Log($"The requested view could not be found: " + typeof(UIViewCreator));
                return null;
            }
            return view;
        }
        public static UIView CreateView(in Type viewType, in RectTransform parent)
        {
            //Validate if the requested view is available at config
            var viewCtrl = GetViewRef(viewType);
            if (viewCtrl == null)
            {
                Trace.Log("Failed to get UIView " + typeof(UIViewCreator));
                return null;
            }

            //Instatiate view and setup 
            var view = GameObject.Instantiate<UIView>(viewCtrl, parent, false);
            RectTransform viewRT = (RectTransform)view.transform;

            view.gameObject.SetActive(false);
            // viewRT.SetAnchorsSize(view.anchorControl.Size, AnchorsCoordinateSystem.AsChildOfCanvas, true);
            // viewRT.SetAnchorsPosition(view.anchorControl.Position, AnchorsCoordinateSystem.AsChildOfCanvas, true);
            //
            // viewRT.SetSize(view.rectControl.Size, CoordinateSystem.AsChildOfCanvasNormalized, true);
            // viewRT.SetPosition(view.rectControl.Position, CoordinateSystem.AsChildOfCanvasNormalized, true);

            return view;
        }
    }
}