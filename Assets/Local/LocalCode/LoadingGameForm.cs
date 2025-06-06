using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UniFramework.Event;
using UnityEngine;

namespace LocalCode
{
    public class LoadingGameForm : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text_progress;
        [SerializeField] private TextMeshProUGUI text_tips;
        [SerializeField] private RectTransform loading_area;
        [SerializeField] private RectTransform loading_bar;
        private const string loading_text = "DOWNLOADING...   <color=#FFCC03>{0}% / 100%</color>";

        private EventGroup event_group = new EventGroup();

        private void Init()
        {
            event_group.AddListener<PatchEvent.InitializeFailed>(OnHandleEventMessage);
            event_group.AddListener<PatchEvent.FoundUpdateFiles>(OnHandleEventMessage);
            event_group.AddListener<PatchEvent.DownloadUpdate>(OnHandleEventMessage);
            event_group.AddListener<PatchEvent.PackageVersionRequestFailed>(OnHandleEventMessage);
            event_group.AddListener<PatchEvent.PackageManifestUpdateFailed>(OnHandleEventMessage);
            event_group.AddListener<PatchEvent.WebFileDownloadFailed>(OnHandleEventMessage);
            event_group.AddListener<GameEvent.HideLoadingEvent>(OnHandleEventMessage);
            RefreshProgress(0f);
        }


        public void RefreshProgress(float progress)
        {
            var total_width = Mathf.Abs(loading_area.rect.width);
            var offset_max = loading_bar.offsetMax;
            offset_max.x = total_width - progress * total_width;
            loading_bar.offsetMax = -offset_max;
            text_progress.text = string.Format(loading_text, Mathf.RoundToInt(progress * 100f));
        }

        private void OnHandleEventMessage(IEventMessage message)
        {
            if (message is PatchEvent.InitializeFailed)
            {
                text_tips.text = $"Failed to initialize package ";
            }
            else if (message is PatchEvent.FoundUpdateFiles)
            {
                new UserEvent.UserBeginDownloadWebFiles().SendMsg();
                text_tips.text = string.Empty;
            }
            else if (message is PatchEvent.DownloadUpdate)
            {
                var msg = message as PatchEvent.DownloadUpdate;
                RefreshProgress((float)msg.current_download_count / msg.total_download_count);
                text_tips.text = string.Empty;
            }
            else if (message is PatchEvent.PackageVersionRequestFailed)
            {
                text_tips.text = $"Please check the network status";
            }
            else if (message is PatchEvent.PackageManifestUpdateFailed)
            {
                text_tips.text = $"Please check the network status";
            }
            else if (message is PatchEvent.WebFileDownloadFailed)
            {
                text_tips.text = $"Failed to download file";
            }
            else if (message is GameEvent.HideLoadingEvent)
            {
                text_tips.text = $"Game Loading ...";
                Show(false);
            }
            else
            {
                throw new System.NotImplementedException($"{message.GetType()}");
            }
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);
            if (show)
            {
                Init();
            }
            else
            {
                event_group.RemoveAllListener();
            }
        }
    }
}