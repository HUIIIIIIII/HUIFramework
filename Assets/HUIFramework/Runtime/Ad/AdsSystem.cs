using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;

namespace HUIFramework.Common.Ad
{
    public class AdsSystem : SingletonMonoBase<AdsSystem>
    {
        private List<IAdItem> ad_items = new List<IAdItem>();
        public override async UniTask InitAsync()
        {
            await base.InitAsync();
            if (ad_items.IsNullOrEmpty())
            {
                ad_items.Add(new MaxAdItem());
            }
            
            foreach (var adItem in ad_items)
            {
                adItem.InitializeAds();
            }
        }

        public bool IsRewardVideoAvailable()
        {
            foreach (var adItem in ad_items)
            {
                if (adItem.IsRewardVideoAvailable())
                {
                    return true;
                }
            }
            return false;
        }
        public void ShowRewardVideo(Action<bool> OnComplete)
        {
            foreach (var adItem in ad_items)
            {
                if (adItem.IsRewardVideoAvailable())
                {
                    adItem.ShowRewardVideo(OnComplete);
                }
            }
        }
        public bool IsInterAvailable()
        {
            foreach (var adItem in ad_items)
            {
                if (adItem.IsInterstitialAvailable())
                {
                    if (adItem.IsInterstitialAvailable())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public void ShowInterstitial(Action OnComplete)
        {
            foreach (var adItem in ad_items)
            {
                if (adItem.IsInterstitialAvailable())
                {
                    adItem.ShowInterstitial(OnComplete);
                    return;
                }
            }
        }
        public bool IsBannerAvailable()
        {
            foreach (var adItem in ad_items)
            {
                if (adItem.IsBannerAvailable())
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowBanner()
        {
            foreach (var ad_item in ad_items)
            {
                if (ad_item.IsBannerAvailable())
                {
                    ad_item.ShowBanner();
                }
            }
        }
        public void HideBanner()
        {
            foreach (var ad_item in ad_items)
            {
                ad_item.HideBanner();
            }
        }
    }
   
}