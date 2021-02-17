using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

public class UnityAdsHandler : MonoBehaviour, IUnityAdsListener
{
    /// <summary>
    /// ?WAAAAARNINGSSSS: NOT SURESS IF SHOW ADS() ONLY OR SHOW ADS WITH REWARD ID=myPlacementId ,WHEN I KEEP PLACEMENT ID IN SADVERTISMENT.SHOW("rewardVideo") and ;
    ///ReviveButton.interactable = Advertisement.IsReady("rewaredVideo"); 
    ////BOTH ARE EMPTIED PARANMETER TO SHOW the SKIP BUTTON INA ADS
    //QUESTIONS: DOS IT INFLUENCE THE ALGORITHM OF GETTING MORE MONEY AS IT IS ID FOR REWARD VIDEO? 

    /// </summary>
/*#if UNITY_IOS
        private string gameId = "1486551";
#elif UNITY_ANDROID
      [SerializeField]  private string gameId = "4001611";
#endif*/

    [SerializeField] private string gameId = "4001611";
    bool testMode = false;

    public bool showAds=true;

    public Button ReviveButton;
    public GameObject GameOverPanel;
    public string myPlacementId = "rewardedVideo";
    public void Awake()
    {
        
        
        //check if ads are purchased
        CheckShowAds();
    }
    //IAP
    private string removeAdsIAPIDs= "com.lynxgamez.crystaldash.removeadsbutton";

    void Start()
    {
       // GameOverPanel.SetActive(false);
        ReviveButton = GameObject.Find("ReviveButton"). GetComponent<Button>();
        GameOverPanel.SetActive(false);
        ReviveButton.interactable = Advertisement.IsReady();
        if (ReviveButton) ReviveButton.onClick.AddListener(DisplayInteratialAds);
        Advertisement.AddListener(this);


       
        Advertisement.Initialize(gameId, testMode);
        
    }
     

    public void DisplayInteratialAds()
    {
        Debug.Log(showAds);
        if (showAds)
        {
            if(PlayerPrefs.GetInt("LastScore", 0)>1)
            {

                Advertisement.Show();
            }    
        }    
            
        else
        {
            FindObjectOfType<Wave_GameManager>().Revive();
            ///return;
        }    
             
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
            Debug.Log("IAP purchased"+"+ 15 lives offer! :D, thank you for your purchases!");
            GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives = GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives + 15;
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
    


    public void OnClickRefreshLivesOnPurchase()
    {
        PlayerPrefs.SetInt("Lives", GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives);
        GameObject.FindObjectOfType<Wave_GameManager>().GlobalLives = PlayerPrefs.GetInt("Lives", 0);
        GameObject.FindObjectOfType<Wave_GameManager>().CheckGlobalLives();
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

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == myPlacementId)
        {
            ReviveButton.interactable = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {

       /// throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        ///throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            FindObjectOfType<Wave_GameManager>().Revive();
        }
        else if (showResult == ShowResult.Skipped)
        {
            FindObjectOfType<Wave_GameManager>().CheckAdsRevive();
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error");
        }
        
    }
}
