using UnityEngine;

namespace HUIFramework.UI.Scroller
{
    public interface  IPage
    { 
        public RectTransform page_rect { get; }
        public void OnPageLoad(int page_index);
        public void OnPageEnter();
        public void OnPageExit();
        public void OnPageUnload();
    }
}