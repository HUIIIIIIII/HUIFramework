using UnityEngine;

namespace HUIFramework.UI
{
    public class UIComponent : MonoBehaviour
    {
        protected BaseForm owener_form;
        [SerializeField] private bool contoll_by_form = true;
        public bool ContollByForm => contoll_by_form;
        
        public void SetOwnerForm(BaseForm form)
        {
            owener_form = form;
        }

        public virtual void OnCreate()
        {
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnHide()
        {
        }
    }
}