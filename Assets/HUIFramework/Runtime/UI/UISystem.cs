using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using HUIFramework.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;

namespace HUIFramework.UI
{
    public class UISystem : SingletonMonoBase<UISystem>
    {
        private const string ui_form_path = "Assets/Game/UI/Form/{0}.prefab";
        private Camera ui_camera;
        public Camera UICamera => ui_camera;
        private Dictionary<UILayer, Transform> ui_groups = new Dictionary<UILayer, Transform>();
        private Dictionary<UIFormId, BaseForm> form_dic = new();

        public override async UniTask InitAsync()
        {
            var camera_list = Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack;
            ui_camera = camera_list.Find(x => x
                .gameObject.name == "ui_camera");

            var layer_list = Enum.GetValues(typeof(UILayer)).OfType<UILayer>().ToList();
            layer_list.Sort((x, y) => (int)x - (int)y);
            foreach (var layer in layer_list)
            {
                var group = new GameObject(layer.ToString()).transform;
                group.SetParent(transform);
                group.localPosition = Vector3.zero;
                group.localRotation = Quaternion.identity;
                group.localScale = Vector3.one;
                ui_groups.Add(layer, group);
            }
        }

        private async UniTask<T> CreateFormAsync<T>(UIFormId id) where T : BaseForm
        {
            if (form_dic.TryGetValue(id, out var panel))
            {
                return panel as T;
            }

            var form_path = string.Format(ui_form_path, id.ToString());
            var form_obj = await Addressables.LoadAssetAsync<GameObject>(form_path);
            var form = Instantiate(form_obj, transform).GetComponent<BaseForm>();
            var parent = ui_groups[form.FormLayer];
            form.transform.SetParent(parent);
            form.OnCreate();
            form_dic.Add(id, form);
            return form as T;
        }

        public T GetForm<T>(UIFormId id) where T : BaseForm
        {
            if (form_dic.TryGetValue(id, out var form))
            {
                return form as T;
            }

            return null;
        }

        public async UniTask ShowFormAsync(UIFormId id, IFormData formData = null)
        {
            var form = await CreateFormAsync<BaseForm>(id);
            if (form != null)
            {
                if (form.Status == FormStaus.Show || form.Status == FormStaus.Showing)
                {
                    return;
                }

                form.SetFormStaus(FormStaus.Showing);
                await form.OnShow(formData);
                form.gameObject.SetActive(true);
                form.SetFormStaus(FormStaus.Show);
            }
        }

        public async UniTask CloseFormAsync(UIFormId id)
        {
            if (form_dic.TryGetValue(id, out var form))
            {
                if (form.Status == FormStaus.Close || form.Status == FormStaus.Closing)
                {
                    return;
                }

                form.SetFormStaus(FormStaus.Closing);
                await form.OnHide();
                form.gameObject.SetActive(false);
                form.SetFormStaus(FormStaus.Close);
            }
        }
    }
}