using System;
using PrimeTween;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

namespace HUIFramework.UI.Scroller
{
    public class InfinitePage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private int max_page_id;
        [SerializeField] private float drag_damping = 3f;
        [SerializeField] private float move_one_page_duration = 1f;
        [SerializeField] private InfiniteDot dot;
        
        private Action<int> OnPageChanged;
        private int current_page_id = 0;
        public int MaxPageCount => max_page_id + 1;

        private List<IPage> pages;
        private float page_width => content?.rect.width ?? 0;
        private int page_count => pages?.Count ?? 0;
        

        private Tween move_tween;
        private float drag_begin_pos;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            pages = new List<IPage>();
            foreach (var page in GetComponentsInChildren<IPage>())
            {
                pages.Add(page);
            }
            for (int i = 0; i < pages.Count; i++)
            {
                var page_rect = pages[i].page_rect;
                page_rect.anchoredPosition = new Vector2(i * page_width, 0);
            }

            MoveToPage(current_page_id, false);
            if (dot != null)
            {
                dot.Init(this);
            }
        }

        public void SetMaxPageId(int max_page_id)
        {
            this.max_page_id = max_page_id;
        }
        public void AddPageChangeEvent(Action<int> callback)
        {
            OnPageChanged += callback;
        }

        public void PrevPage()
        {
            if (current_page_id > 0)
            {
                MoveToPage(current_page_id - 1);
            }
        }

        public void NextPage()
        {
            if (current_page_id < max_page_id)
            {
                MoveToPage(current_page_id + 1);
            }
        }

        private void MoveToPage(int target_page_id, bool animate = true)
        {
            LoadPage(target_page_id);
            if (current_page_id >= 0 && current_page_id != target_page_id)
            {
                pages[current_page_id % page_count].OnPageExit();
            }

            pages[target_page_id % page_count].OnPageEnter();
            var target_x = -page_width * target_page_id;

            if (animate)
            {
                move_tween.Complete();
                float distance = Mathf.Abs(content.anchoredPosition.x - target_x);
                float duration = Mathf.InverseLerp(0, page_width, distance) * move_one_page_duration;
                move_tween = Tween.UIAnchoredPositionX(content, target_x, duration, Ease.OutQuad).OnComplete(() =>
                {
                    var last_page = content.GetChild(content.childCount - 1) as RectTransform;
                    var first_page = content.GetChild(0) as RectTransform;
                    var target_page = pages[target_page_id % page_count].page_rect;
                    var target_page_index = target_page.GetSiblingIndex();
                    
                    if (current_page_id > target_page_id && target_page_index < page_count / 2 + 1)
                    {
                        last_page.SetAsFirstSibling();
                        last_page.anchoredPosition = first_page.anchoredPosition - new Vector2(page_width, 0);
                    }
                    else if (current_page_id < target_page_id && target_page_index >= page_count / 2 + 1)
                    {
                        first_page.SetAsLastSibling();
                        first_page.anchoredPosition = last_page.anchoredPosition + new Vector2(page_width, 0);
                    }

                    current_page_id = target_page_id;
                    OnPageChanged?.Invoke(current_page_id);
                });
            }
            else
            {
                content.anchoredPosition = new Vector2(target_x, 0);
                current_page_id = target_page_id;
                OnPageChanged?.Invoke(current_page_id);
            }
        }

        private void LoadPage(int page_id)
        {
            var page_index = page_id % page_count;
            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                if (i == page_index)
                {
                    page.OnPageLoad(page_id);
                }
                else if(i == (page_index - 1 + page_count) % page_count && page_id - 1 >= 0)
                {
                    page.OnPageLoad(page_id - 1);
                }
                else if(i == (page_index + 1) % page_count && page_id + 1 <= max_page_id)
                {
                    pages[i].OnPageLoad(page_id + 1);
                }
                else
                {
                    pages[i].OnPageUnload();
                }
            }
        }

        #region  drag & move
        public void OnBeginDrag(PointerEventData eventData)
        {
            drag_begin_pos = content.anchoredPosition.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.delta.x > 0 && current_page_id <= 0 ||
                eventData.delta.x < 0 && current_page_id >= max_page_id)
            {
                return;
            }

            var drag_distance = eventData.delta.x * drag_damping;
            var content_pos = content.anchoredPosition;
            content_pos.x += drag_distance;
            content_pos.x = Mathf.Clamp(content_pos.x, drag_begin_pos - page_width, drag_begin_pos + page_width);
            content.anchoredPosition = content_pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var drag_distance = content.anchoredPosition.x - drag_begin_pos;
            if (Mathf.Abs(drag_distance) > page_width * 0.3f)
            {
                if (drag_distance > 0)
                {
                    PrevPage();
                }
                else
                {
                    NextPage();
                }
            }
            else if (Mathf.Approximately(drag_distance, 0))
            {
                return;
            }
            else
            {
                MoveToPage(current_page_id);
            }
        }
        #endregion
    }
}