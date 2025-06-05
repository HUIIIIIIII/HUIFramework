using System;

namespace HUIFramework.Common.Ad
{
    public class MaxAdItem : IAdItem
    {
        public void InitializeAds()
        {
            
        }

        public bool IsRewardVideoAvailable()
        {
            return false;
        }

        public void ShowRewardVideo(Action<bool> OnComplete)
        {
            
        }

        public bool IsInterstitialAvailable()
        {
            return false;
        }

        public void ShowInterstitial(Action OnComplete)
        {
           
        }

        public bool IsBannerAvailable()
        {
            return false;
        }

        public void ShowBanner()
        {
           
        }

        public void HideBanner()
        {
            
        }

        public bool BannerAlreadyUsed()
        {
            return false;
        }

        public void ResetBannerUsage()
        {
            
        }
    }
}