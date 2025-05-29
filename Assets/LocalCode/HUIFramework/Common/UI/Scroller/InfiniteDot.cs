using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HUIFramework.UI.Scroller
{
    public class InfiniteDot : MonoBehaviour
    {
        [SerializeField] private Button prev_button;
        [SerializeField] private Button next_button;
        
        private List<IDot> dots;
        public void Init(InfinitePage page)
        {
            dots = new List<IDot>();
            var max_page_count = page.MaxPageCount;
            var index = 0;
            foreach (var dot in GetComponentsInChildren<IDot>())
            {
                dots.Add(dot);
                index++;
                if (index <= max_page_count)
                {
                    dot.dot_rect.gameObject.SetActive(true);
                    dots.Add(dot);
                }
                else
                {
                    dot.dot_rect.gameObject.SetActive(false);
                }
            }
            prev_button.onClick.AddListener(() => page.PrevPage());
            next_button.onClick.AddListener(() => page.NextPage());
            page.AddPageChangeEvent(OnPageChanged);
        }

        public void OnPageChanged(int page_id)
        {
            if(page_id > dots.Count - 1)
            {
                for (int i = 0; i < dots.Count -1; i++)
                {
                    var dot = dots[i];
                    dot.Select(false, page_id);
                }
                dots.Last().Select(true,page_id);
            }
            else
            {
                for (int i = 0; i < dots.Count; i++)
                {
                    dots[i].Select(i == page_id, page_id);
                }
            }
        }
    }
}