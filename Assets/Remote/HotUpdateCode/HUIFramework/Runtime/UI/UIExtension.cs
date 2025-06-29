using System;
using HUIFramework.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HUIFramework.UI
{
    public static class UIExtension
    {
        public static void AutoResize(this RectTransform rectTransform)
        {
            var safeArea = Screen.safeArea;
            var orientation = Screen.orientation;

            rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, safeArea.yMin / Screen.height);
            rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, safeArea.yMax / Screen.height);
            if (orientation == ScreenOrientation.LandscapeLeft)
            {
                rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
            }
            else if (orientation == ScreenOrientation.LandscapeRight)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, 1);
            }
            else if (orientation == ScreenOrientation.Portrait)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, safeArea.yMax / Screen.height);
            }
            else if (orientation == ScreenOrientation.PortraitUpsideDown)
            {
                rectTransform.anchorMin = new Vector2(0, safeArea.yMin / Screen.height);
                rectTransform.anchorMax = new Vector2(1, 1);
            }
        }

        public static void AddButtonClickEvent(this Button button, Action action)
        {
            if (button == null || action == null) return;
            button.onClick.AddListener(() =>
            {
                action?.Invoke();
                button.interactable = false;
                TimerSystem.Instance.ScheduleOnce(0.5f, () => { button.interactable = true; });
            });
        }

        public static Vector2? GetLocalPointInRect(this RectTransform rect,
            Vector3 world_pos, Camera world_camera, Camera ui_camera)
        {
            var screen_pos = world_camera.WorldToScreenPoint(world_pos);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
                    screen_pos, ui_camera, out var local_point))
            {
                return local_point;
            }
            else
            {
                return null;
            }
        }
        public static void SetRectWidth(this RectTransform rect, float width)
        {
            if (rect == null) return;
            var size = rect.sizeDelta;
            size.x = width;
            rect.sizeDelta = size;
        }

        public static void SetRectHeight(this RectTransform rect, float height)
        {
            if (rect == null) return;
            var size = rect.sizeDelta;
            size.y = height;
            rect.sizeDelta = size;
        }
        public static void SetRectXY(this RectTransform rect, float width, float height)
        {
            if (rect == null) return;
            var size = rect.sizeDelta;
            size.x = width;
            size.y = height;
            rect.sizeDelta = size;
        }
        public static void SetAnchorPosX(this RectTransform rect, float x)
        {
            if(rect == null) return;
            var pos = rect.anchoredPosition;
            pos.x = x;
            rect.anchoredPosition = pos;
        }

        public static void SetAnchorPosY(this RectTransform rect, float y)
        {
            if(rect == null) return;
            var pos = rect.anchoredPosition;
            pos.y = y;
            rect.anchoredPosition = pos;
        }
    }
}