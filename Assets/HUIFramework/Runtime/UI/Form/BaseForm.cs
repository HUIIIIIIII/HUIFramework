using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace HUIFramework.UI
{
    using UnityEngine;

    public abstract class BaseForm : MonoBehaviour
    {
        [SerializeField] protected UIFormId form_id;
        [SerializeField] private UILayer form_layer;
        [SerializeField] protected List<UINode> nodes = new();
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject block;


        [Button(ButtonSizes.Large, Name = "SetForm")]
        private void SetForm()
        {
            nodes.Clear();
            foreach (var ui_node in transform.GetComponentsInChildren<UINode>())
            {
                ui_node.SetForm(this);
                if (ui_node.ContollByForm)
                {
                    AddUINode(ui_node);
                }
            }

            canvas = transform.GetComponent<Canvas>();
            if (block == null)
            {
                block = new GameObject("block");
                block.transform.SetParent(transform);
                block.transform.localPosition = Vector3.zero;
                block.transform.localScale = Vector3.one;
                var block_rect = block.transform as RectTransform;
                block_rect.anchorMin = Vector2.zero;    
                block_rect.anchorMax = Vector2.one;
                block_rect.offsetMax = Vector2.zero;
                block_rect.offsetMin = Vector2.zero;
                block_rect.pivot = new Vector2(0.5f, 0.5f);
            }
        }
        protected void AddUINode(UINode node)
        {
            nodes.Add(node);
        }

        public abstract void OnInit();
        public abstract void OnShow(IFormData formData = null);
        public abstract void OnHide();
    }

    public interface IFormData
    {
    }
}