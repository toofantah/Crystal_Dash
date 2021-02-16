using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

public class UnityAdsHandler : MonoBehaviour
{

    [SerializeField] string googlePlay_ID="4001611";
    bool testMode = false;

    bool showAds=true;


    public void Awake()
    {
        //check if ads are purchased
        CheckShowAds();
    }
    //IAP
    private string removeAdsIAPIDs= "com.lynxgamez.crystaldash.removeadsbutton";

    void Start()
    {
        Advertisement.Initialize(googlePlay_ID, testMode);
        
    }
     

    public void DisplayInteratialAds()
    {
        if (showAds)
            Advertisement.Show();
        else
            return;    
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id==removeAdsIAPIDs)
        {
            PlayerPrefs.SetInt("isAdsPurchased", 1);
            Debug.Log("IAP purchased"+"+ 20 lives offer! :D, thank you for your purchases!");
            GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives = GameObject.FindObjectOfType<Wave_GameManager>().MaxGlobalLives + 20;
            PlayerPrefs.SetInt("Lives", GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives);
            PlayerPrefs.SetInt("isAdsPurchased", 1);
            ///GameObject.FindObjectOfType<Wave_GameManager>().CheckGlobalLives();
            GameObject.FindObjectOfType<Wave_GameManager>().CheckAdsRemovePurchases();
            GameObject.FindObjectOfType<Wave_GameManager>().CheckThankYouForPurchasesRemovesAds();
            CheckShowAds();
            //GameObject.FindObjectOfType<Wave_GameManager>().isAdsPurchased = true;
            //PlayerPrefs.SetInt("isRemoveAdsPurchased", 1);
            //GameObject.FindObjectOfType<Wave_GameManager>().isAdsPurchased();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("purchase of prodyuct:" + product + ":  failed because of: " + reason);
    }
    





    public void CheckShowAds()
    {
        Wave_GameManager.isAdsPurchased = PlayerPrefs.GetInt("isAdsPurchased", 0);

        if (Wave_GameManager.isAdsPurchased == 0)
        {
            showAds = true;
        }
        else if (Wave_GameManager.isAdsPurchased == 1)
        {
            showAds = false;
        }
    }

}
