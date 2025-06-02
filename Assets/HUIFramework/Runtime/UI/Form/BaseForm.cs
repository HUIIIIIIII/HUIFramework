using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HUIFramework.UI
{
    using UnityEngine;

    public abstract class BaseForm : MonoBehaviour
    {
        [SerializeField] protected UIFormId form_id;
        [SerializeField] private UILayer form_layer;
        [SerializeField] protected List<UIComponent> ui_components = new();
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private FormStaus status;

        public FormStaus Status => status;
        public UILayer FormLayer => form_layer;

        public void SetFormStaus(FormStaus staus)
        {
            this.status = staus;
            raycaster.enabled = staus == FormStaus.Show;
        }

        public void SetCamera(Camera camera)
        {
            canvas.worldCamera = camera;
        }

        public void SetLayer(UILayer layer)
        {
            canvas.sortingOrder = (int)layer;
        }
        
        public void AddUIComponentInGame(UIComponent component)
        {
            component.SetOwnerForm(this);
            component.OnCreate();
            ui_components.Add(component);
        }

        public virtual void OnCreate()
        {
            foreach (var ui_component in ui_components)
            {
                ui_component.OnCreate();
            }
        }

        public virtual async UniTask OnShow(IFormData formData = null)
        {
            foreach (var ui_component in ui_components)
            {
                ui_component.OnShow();
            }
        }

        public virtual async UniTask OnHide()
        {
            foreach (var ui_component in ui_components)
            {
                ui_component.OnHide();
            }
        }

        # region  in editor func
        [Button(ButtonSizes.Large, Name = "SetForm")]
        private void SetForm()
        {
            ui_components.Clear();
            foreach (var ui_component in transform.GetComponentsInChildren<UIComponent>(true))
            {
                ui_component.SetOwnerForm(this);
                if (ui_component.ContollByForm)
                {
                    ui_components.Add(ui_component);
                }
            }

            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            
            var canvas_scaler = gameObject.GetOrAddComponent<CanvasScaler>();
            canvas_scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas_scaler.referenceResolution = new Vector2(1080,1920);
            canvas_scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            var image = gameObject.GetOrAddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
            image.raycastTarget = true;
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.offsetMin = Vector2.zero;
            image.rectTransform.offsetMax = Vector2.zero;
        }
        
        [Button(ButtonSizes.Large, Name = "RemoveUselessTarget")]
        public void RemoveUselessTarget()
        {
            var graphics = GetComponentsInChildren<Graphic>(true);
            foreach (var graphic in graphics)
            {
                if (graphic.TryGetComponent<IEventSystemHandler>(out var handler))
                {
                    graphic.raycastTarget = true;
                }
                else
                {
                    graphic.raycastTarget = false;
                }
            }
        }
        #endregion
    }

    public enum FormStaus
    {
        None,
        Show,
        Showing,
        Close,
        Closing,
    }

    public interface IFormData
    {
    }
}

