using LocalCode.Common;
using UnityEngine;
using YooAsset;

namespace LocalCode
{
    [CreateAssetMenu(fileName = "GameLocalSetting",menuName = "HUIFramework/Setting/GameLocalSetting", order = 0)]
    public class GameLocalSetting : ScriptableObject
    {
        [SerializeField] private EPlayMode game_play_mode;
        public EPlayMode GamePlayMode => game_play_mode;
        
        [SerializeField] private string default_host_server = "http://127.0.0.1/CDN/Android/v1.0"; 
        public string DefaultHostServer => default_host_server;
        
        [SerializeField] private string fall_back_host_server = "http://127.0.0.1/CDN/Android/v1.0";
        public string FallBackHostServer => fall_back_host_server;
        
        [SerializeField] private int max_download_retry_count = 3;
        public int MaxDownloadRetryCount => max_download_retry_count;
    }
    
}