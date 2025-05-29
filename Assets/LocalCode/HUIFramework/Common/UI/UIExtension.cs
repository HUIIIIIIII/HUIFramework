using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HUIFramework.UI
{
    public static class UIExtension
    {
        public static void AutoResize(this RectTransform rectTransform)
        {
            var safeArea = Screen.safeArea;        
            var orientation = Screen.orientation;   

            rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width , safeArea.yMin / Screen.height);
            rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width , safeArea.yMax / Screen.height);
            if (orientation == ScreenOrientation.LandscapeLeft)
            {
                rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
            }
            else if(orientation == ScreenOrientation.LandscapeRight)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, 1);
            }
            else if(orientation == ScreenOrientation.Portrait)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, safeArea.yMax / Screen.height);
            }
            else if(orientation == ScreenOrientation.PortraitUpsideDown)
            {
                rectTransform.anchorMin = new Vector2(0, safeArea.yMin / Screen.height);
                rectTransform.anchorMax = new Vector2(1, 1);
            }
        }
        [MenuItem("GameObject/UI/Image", true)]
        public static void IgnoreImage()
        {
        }
        
        [MenuItem("GameObject/UI/Image.")]
        public static void CreateImage()
        {
            var image = Create<Image>();
            image.raycastTarget = false;
            image.maskable = image.GetComponentInParent<RectMask2D>() != null || image.GetComponentInParent<Mask>() != null;
        }
        // [MenuItem("GameObject/UI/Text - TextMeshPro", true)]
        // public static void IgnoreTextMeshPro()
        // {
        // }
        // [MenuItem("GameObject/UI/Text - TextMeshPro.")]
        // public static void CreateTextMeshPro()
        // {
        //     var text = Create<TMPro.TextMeshProUGUI>();
        //     text.raycastTarget = false;
        //     text.maskable = text.GetComponentInParent<RectMask2D>() != null || text.GetComponentInParent<Mask>() != null;
        // }
        public static T Create<T>(string name = null) where T : Component
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            GameObject go = new GameObject(name);
            go.transform.SetParent(Selection.activeTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            Selection.activeGameObject = go;
            return go.AddComponent<T>();
        }
    }
}