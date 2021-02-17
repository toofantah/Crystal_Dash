using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using TMPro;

public class Wave_GameManager : MonoBehaviour
{

    public static int isAdsPurchased=0;
    int score = 0;
    public Text CurrentScoreText;
    public Text BestScoreText;
    public Text LastScoreLabelText;
    public Text LastScoreText;
    public Text LivesScoreLabelText;
    public Text LivesScoreText;

    ///public bool isRemoveAdsPurchased = false;

    public int MaxGlobalLives;
    public int GlobalLives; //newScripts/?.//// 
    public float secsToWaitAfterLoosingAlls;
    public float secsToWaitAfterLoosingToReplays;
    public GameObject ReviveButtonGO;
    public GameObject ReviveButtonSubTitleTextGO;
    public GameObject GameOverPanel;
    public GameObject GameOverEffectPanel;
    public GameObject GameOverEffectRestartPanel;
    public GameObject RemoveAdsButtonsGO;
    public GameObject ThanksForPurchasingRemoveAdsPanelGO;

    public GameObject touchToMoveTextObj;
    public GameObject readyReplayRevivesObj;
    public GameObject readyReplayRestartObj;
    public int tempFirstScoreForAds;
    public int tempScoreSession;
    public GameObject StartFadeInObj;


    public static Wave_GameManager GameManagerInstances;
    static int PlayCount;
    
    ///Added
    public GameObject playerGO;
    Transform lastPlayerPos;
    
    void Awake()
    {

        /*if (GameManagerInstances)
        {
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        */

        
        ///tempScoreSession = 0;
        ///PlayerPrefs.SetInt("TempScoreSession", tempScoreSession);

        
        ReviveButtonSubTitleTextGO.SetActive(true);

        #region SETUP
        ReviveButtonSubTitleTextGO.SetActive(false);
        
        /*PlayerPrefs.DeleteAll();*/
        ThanksForPurchasingRemoveAdsPanelGO.SetActive(false);
        Application.targetFrameRate = 60;
        PlayerPrefs.GetInt("Lives", GlobalLives);
        
        LastScoreText.text = PlayerPrefs.GetInt("LastScore", score).ToString();

        //ExtraControlForFirstSkipAdsUnderZeroews();
        //ExtraControlForFirstSkipAdsUnderZeroews();
        CheckAdsRemovePurchases();




        GlobalLives = MaxGlobalLives;
        Time.timeScale = 1.0f;
        CurrentScoreText.text = "0";
        
        isAdsPurchased = PlayerPrefs.GetInt("isAdsPurchased", 0);
        CheckAdsRemovePurchases();

        BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        StartCoroutine(FadeIn());
        #endregion
        ReviveScoreSetup();
        

    }

    void Update()
    {
        if (touchToMoveTextObj.activeSelf == false) return;
        if (Input.GetMouseButton(0))        {
            touchToMoveTextObj.SetActive(false);
        }
    }

    IEnumerator FadeIn()
    {
        StartFadeInObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StartFadeInObj.SetActive(false);
        yield break;
    }




    public void addScore()
    {
        score++;
        CurrentScoreText.text = score.ToString();

        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            
            PlayerPrefs.SetInt("LastScore", score);
            BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();

            

        }
    }


    public void Gameover()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.GetInt("LastScore", score);
        ///ExtraControlForFirstSkipAdsUnderZeroews();
        CheckAdsRemovePurchases();
        Debug.Log(score);
        // ExtraControlForFirstSkipAdsUnderZeroews();



        tempScoreSession = score;
        PlayerPrefs.SetInt("TempScoreSession", tempScoreSession);

        StartCoroutine(GameoverCoroutine());
        CheckGlobalLives();


        
    }


    IEnumerator GameoverCoroutine()
    {
        PlayerPrefs.SetInt("LastScore", score);

        //ExtraControlForFirstSkipAdsUnderZeroews();

        CheckAdsRemovePurchases();
        GameOverEffectPanel.SetActive(true);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.5f);
        GameOverPanel.SetActive(true);
        
        ///checkGlobalLives();
        LivesScoreText.gameObject.SetActive(true);
        LivesScoreLabelText.gameObject.SetActive(true);
        LivesScoreText.text = GlobalLives.ToString();

        yield break;
    }


    public void Restart()
    {
        Debug.Log(PlayerPrefs.GetInt("GlobalLives", 0));
        /*if (PlayerPrefs.GetInt("GlobalLives", 0) > 5)
        {
            GlobalLives = PlayerPrefs.GetInt("GlobalLives", 0);

            
            GlobalLives--;
            PlayerPrefs.SetInt("Lives", GlobalLives);
            ReviveButtonGO.SetActive(true);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.grey;
            return;
        }
        else
        {
            GlobalLives = PlayerPrefs.GetInt("GlobalLives", 0);
            GlobalLives--;
            PlayerPrefs.SetInt("Lives", MaxGlobalLives);
            ReviveButtonGO.SetActive(true);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.grey;
        }
        */

        //CheckGlobalLives();
        GameOverPanel.SetActive(false);
        ///ReviveScoreSetup();
        readyReplayRestartObj.SetActive(true);
        StopAllCoroutines();
        
        PlayerPrefs.SetInt("TempScoreSession", score);
        Time.timeScale = 0.3f;
        StartCoroutine(RestartAfterLosing(secsToWaitAfterLoosingAlls));
        //Some count down animations? 
        //Watch a video ads?
        //Get extra livess?
        ///stetdss
    }


    public void CheckAdsRevive()
    {


        //Actions on player to revice
        GameOverPanel.SetActive(true);
        


    }
    public void Revive()
    {

        GameOverPanel.SetActive(false);
        //Actions on player to revice
        //////GameOverPanel.SetActive(false);
        lastPlayerPos = playerGO.transform;
        ///FindObjectOfType<Wave_Player>().isDead = false;
        Time.timeScale = 1f;
        readyReplayRevivesObj.SetActive(true);
        
        StopAllCoroutines();
        ///..CheckGlobalLives();
        StartCoroutine(WaitToReplays(secsToWaitAfterLoosingToReplays));

        
    }

    public int GetScore()
    {
        return score;
    }

    void ReviveScoreSetup()
    {
        if(PlayerPrefs.GetInt("LastScore",score)>0)
        {
            
            PlayerPrefs.GetInt("LastScore", score);
            
            //Set Score text true nd all actions about score 
            LastScoreText.gameObject.SetActive(true);

            LastScoreLabelText.gameObject.SetActive(true);

            LastScoreText.text = PlayerPrefs.GetInt("LastScore", score).ToString();

            //score = 0; //We uncomment this line if we want to revive with a score of 0 instead of contitnue the previous scores   
            CurrentScoreText.text = score.ToString();
        }
        
       
    }    


    //Function check global lives
    public void CheckGlobalLives()
    {
        /*if (PlayerPrefs.GetInt("GlobalLives",0) >5)
        {
            GlobalLives = PlayerPrefs.GetInt("GlobalLives", 0);


            GlobalLives--;
            PlayerPrefs.SetInt("Lives", GlobalLives);
            ReviveButtonGO.SetActive(true);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.grey;
            return;
        }*/
        if (GlobalLives <= 1)
        {
            GlobalLives = PlayerPrefs.GetInt("Lives", 0);
            GlobalLives--;
            GlobalLives = 0;
            PlayerPrefs.SetInt("Lives", GlobalLives);
            ReviveButtonGO.SetActive(false);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.yellow;
        }
        /*if (PlayerPrefs.GetInt("GlobalLives", 0) <= 0)
        {
            GlobalLives = PlayerPrefs.GetInt("GlobalLives", 0);
            GlobalLives--;
            GlobalLives = 0;
            PlayerPrefs.SetInt("Lives", MaxGlobalLives);
            ReviveButtonGO.SetActive(false);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.yellow;
        }*/
        else
        {
             PlayerPrefs.SetInt("Lives", GlobalLives);
            GlobalLives--;
            PlayerPrefs.SetInt("Lives", GlobalLives);
            ReviveButtonGO.SetActive(true);

            LivesScoreText.text = PlayerPrefs.GetInt("Lives", GlobalLives).ToString();

            LivesScoreText.color = Color.grey;
        }
        

    }

    //Function to wait x sec after you loose all your revuces
    IEnumerator RestartAfterLosing(float secsToWaitAfterLoosingAlls)
    {
        CheckGlobalLives();

        //Some count down animations? 
        //Watch a video ads?
        //Get extra livess?
        ///stetdss
        Time.timeScale = 0.3f;
        //yield return new WaitForSeconds(1);
        FadeIn();
        Time.timeScale = 1f;
        
        GameOverEffectRestartPanel.SetActive(true);
        readyReplayRestartObj.SetActive(false);
        GlobalLives = MaxGlobalLives;
        LivesScoreText.gameObject.SetActive(false);
        LivesScoreLabelText.gameObject.SetActive(false);
        
        LivesScoreText.text = GlobalLives.ToString();
        ///////////IMPORTANTSs Time.timeScale = 0.3f;
        ///yield return new WaitForSeconds(1);
        //yield return new WaitForSeconds(1);
        readyReplayRestartObj.SetActive(true);
        yield return new WaitForSeconds(secsToWaitAfterLoosingAlls);
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   

    
    //Function to wait x sec after you restart *counts downs)s
    IEnumerator WaitToReplays(float secsToWaitAfterLoosingToReplays)
    {
        ReviveScoreSetup();
        yield return new WaitForSeconds(secsToWaitAfterLoosingToReplays);
        FindObjectOfType<Wave_Player>().RestartPlayPlayer();
        
        Time.timeScale = 1f;
        ///TO REACTIVATE TO STOP MOUVMENT WHEN THERE IS A CONTINUES ss!!! IMPSPORTATSs Time.timeScale = 0f;
        LivesScoreText.gameObject.SetActive(false);
        LivesScoreLabelText.gameObject.SetActive(false);
        LivesScoreText.text = GlobalLives.ToString();
        readyReplayRevivesObj.SetActive(false);
       
    }

    public void CheckAdsRemovePurchases()
    {
        
        if (isAdsPurchased==1)
        {
            ReviveButtonSubTitleTextGO.SetActive(false);
            ReviveButtonSubTitleTextGO.GetComponent<Text>().alignment = (TextAnchor)TextAlignment.Center;
            RemoveAdsButtonsGO.SetActive(false);
            FindObjectOfType<UnityAdsHandler>().showAds = false;
        }
        else 
            if(isAdsPurchased==0)
        {
            if(PlayerPrefs.GetInt("BestScore",0)<=5)
            {

                FindObjectOfType<UnityAdsHandler>().showAds = false; ;
                //FindObjectOfType<UnityAdsHandler>().CheckShowAds();

                ReviveButtonSubTitleTextGO.SetActive(false);

                //REMOVES??? (LOOKS BEAUTIFUKK)S)set back to false to show also when loosing at first attempt?s>
                RemoveAdsButtonsGO.SetActive(false);


            }
            else
            if (PlayerPrefs.GetInt("BestScore", 0) <= 5    ||PlayerPrefs.GetInt("TempScoreSession", tempScoreSession) <= 1)
            {

                FindObjectOfType<UnityAdsHandler>().showAds = false; ;
                //FindObjectOfType<UnityAdsHandler>().CheckShowAds();
               
                ReviveButtonSubTitleTextGO.SetActive(false);

                //REMOVES??? (LOOKS BEAUTIFUKK)S)set back to false to show also when loosing at first attempt?s>
                RemoveAdsButtonsGO.SetActive(false);


            }
            else
            {
                FindObjectOfType<UnityAdsHandler>().showAds = true; ;
                ReviveButtonSubTitleTextGO.SetActive(true);
                
               
                RemoveAdsButtonsGO.SetActive(true);
            }



            

           
        }
        
    }

    public void CheckThankYouForPurchasesRemovesAds()
    {

        ThanksForPurchasingRemoveAdsPanelGO.SetActive(true);
    }

//Extra function for ads:

    public void ExtraControlForFirstSkipAdsUnderZeroews()
    {
        if (PlayerPrefs.GetInt("isAdsPurchased", 0) == 0)
        {
            if (PlayerPrefs.GetInt("LastScore", score) <= 4)
            {

                FindObjectOfType<UnityAdsHandler>().showAds = false; ;
                //FindObjectOfType<UnityAdsHandler>().CheckShowAds();
                ReviveButtonGO.transform.GetChild(1).gameObject.GetComponent<Text>().enabled=false;
                Debug.Log(ReviveButtonSubTitleTextGO.activeSelf);
            }
            else
            {
                FindObjectOfType<UnityAdsHandler>().showAds = true;
                //FindObjectOfType<UnityAdsHandler>().CheckShowAds();
                ReviveButtonSubTitleTextGO.SetActive(true);
            }
        }


    }
}
