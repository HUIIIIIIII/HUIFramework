using UnityEngine;

namespace HUIFramework.UI.Scroller
{
    public interface IDot
    {
        public RectTransform dot_rect { get; }
        public void Select(bool enable ,int page_index);
    }
}