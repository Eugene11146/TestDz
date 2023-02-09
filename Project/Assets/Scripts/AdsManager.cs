using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public float addTimer = 200f;
    InterstitialAd interstitialAd;
    string key = "ca-app-pub-3940256099942544/1033173712"; // Debug ads
    float ticker = 0;
    public bool paused = false;
    bool showAd = false;

    private void Awake()
    {
        showAd = false;
        MobileAds.Initialize(initStatus => { });
        ticker = addTimer;

        InitializeAd();
    }

    private void Update()
    {
        if (ticker > 0 && !showAd)
            ticker -= Time.unscaledDeltaTime;
    }

    public void InitializeAd()
    {
        interstitialAd = new InterstitialAd(key);
        interstitialAd.OnAdOpening += InterstitialAd_OnAdOpening;
        interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;
        interstitialAd.OnAdFailedToLoad += InterstitialAd_OnAdFailedToLoad;
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    public void ShowAd()
    {
        if (ticker <= 0)
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
                Debug.Log("show");

                InitializeAd();
            }
            else
            {
                Debug.LogWarning("No Ads");

                InitializeAd();
            }
            ticker = addTimer;
            interstitialAd.Destroy();
        }
    }

    private void InterstitialAd_OnAdClosed(object sender, System.EventArgs e)
    {
        showAd = false;
        paused = false;
        Debug.Log("Closedd");
    }

    private void InterstitialAd_OnAdOpening(object sender, System.EventArgs e)
    {
        paused = true;
        showAd = true;
        Debug.Log("Opennedd");
    }

    private void InterstitialAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        LoadAdError loadAdError = e.LoadAdError;

        string message = loadAdError.GetMessage();

        Debug.Log(message);
    }
}
