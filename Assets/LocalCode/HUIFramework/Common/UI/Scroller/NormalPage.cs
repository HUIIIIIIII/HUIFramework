using TMPro;
using UnityEngine;

namespace HUIFramework.UI.Scroller
{
    public class NormalPage : MonoBehaviour, IPage
    {
        [SerializeField] private TextMeshProUGUI page_text;
        private int page_index = -1;
        public RectTransform page_rect => transform as RectTransform;

        public void OnPageLoad(int page_index)
        {
            this.page_index = page_index;
            page_text.text = page_index.ToString();
        }

        public void OnPageEnter()
        {
            Debug.Log($"Page {page_index} entered.");
        }

        public void OnPageExit()
        {
            Debug.Log($"Page {page_index} exited.");
        }

        public void OnPageUnload()
        {
            page_index = -1;
            page_text.text = "";
        }
    }
}