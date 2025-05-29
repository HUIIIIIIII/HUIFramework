using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HUIFramework.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HUIFramework.UI
{
    public class UISystem : SingletonMonoBase<UISystem>
    {
        private Dictionary<UIFormId, BaseForm> form_dic = new();
        
        public override async UniTask InitAsync()
        {
            foreach (UIFormId id in CommonExtension.GetEnumValues<UIFormId>())
            {
                string form_path = string.Format(AssetPath.ui_form_path, id.ToString());
                var form = await Addressables.LoadAssetAsync<GameObject>(form_path);
                var form_obj = Instantiate(form,this.transform);
                var base_form = form_obj.GetComponent<BaseForm>();
                base_form.OnInit();
                base_form.gameObject.SetActive(false);
                form_dic.Add(id,base_form);
            }
        }
        public T GetForm<T>(UIFormId id) where T : BaseForm
        {
            if (form_dic.TryGetValue(id, out var panel))
            {
                return panel as T;
            }
            return null;
        }
        
        public void ShowForm(UIFormId id, IFormData formData = null)
        {
            var form = GetForm<BaseForm>(id);
            if (form != null)
            {
                form.gameObject.SetActive(true);
                form.OnShow(formData);
            }
        }
    }
}