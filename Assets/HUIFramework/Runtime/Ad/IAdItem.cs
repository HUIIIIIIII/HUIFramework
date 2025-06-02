using System;

namespace HUIFramework.Common.Ad
{
    public interface IAdItem
    {
        public void InitializeAds();
        public bool IsRewardVideoAvailable();
        public void ShowRewardVideo(Action<bool> OnComplete);
        public bool IsInterstitialAvailable();
        public void ShowInterstitial(Action OnComplete);
        public bool IsBannerAvailable();
        public void ShowBanner();
        public void HideBanner();
        public bool BannerAlreadyUsed();
        public void ResetBannerUsage();
    }
}