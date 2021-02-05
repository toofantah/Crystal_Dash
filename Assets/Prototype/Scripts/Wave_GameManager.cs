using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using TMPro;

public class Wave_GameManager : MonoBehaviour
{

    int score = 0;
    public Text CurrentScoreText;
    public Text BestScoreText;
    public Text LastScoreLabelText;
    public Text LastScoreText;
    public Text LivesScoreLabelText;
    public Text LivesScoreText;

    public int MaxGlobalLives;
    public int GlobalLives; //newScripts/?.//// 
    public float secsToWaitAfterLoosingAlls;
    public float secsToWaitAfterLoosingToReplays;
    public GameObject ReviveButtonGO;

    public GameObject GameOverPanel;
    public GameObject GameOverEffectPanel;
    public GameObject GameOverEffectRestartPanel;

    public GameObject touchToMoveTextObj;
    public GameObject readyReplayRevivesObj;
    public GameObject readyReplayRestartObj;

    public GameObject StartFadeInObj;

    static int PlayCount;
    
    ///Added
    public GameObject playerGO;
    Transform lastPlayerPos;
    
    void Awake()
    {
        Application.targetFrameRate = 60;

        GlobalLives = MaxGlobalLives;
        Time.timeScale = 1.0f;


        CurrentScoreText.text = "0";
        BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        StartCoroutine(FadeIn());
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
            BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        }
    }


    public void Gameover()
    {
        
        StartCoroutine(GameoverCoroutine());
        checkGlobalLives();
    }


    IEnumerator GameoverCoroutine()
    {
        
        
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

        GameOverPanel.SetActive(false);
        ///ReviveScoreSetup();
        readyReplayRestartObj.SetActive(true);
        StopAllCoroutines();

        Time.timeScale = 0.3f;
        StartCoroutine(RestartAfterLosing(secsToWaitAfterLoosingAlls));
        //Some count down animations? 
        //Watch a video ads?
        //Get extra livess?
        ///stetdss
    }

    public void Revive()
    {

        
        //Actions on player to revice
        GameOverPanel.SetActive(false);
        lastPlayerPos = playerGO.transform;
        ///FindObjectOfType<Wave_Player>().isDead = false;
        Time.timeScale = 1f;
        readyReplayRevivesObj.SetActive(true);
        
        StopAllCoroutines();
        StartCoroutine(WaitToReplays(secsToWaitAfterLoosingToReplays));
        

    }

    public int GetScore()
    {
        return score;
    }

    void ReviveScoreSetup()
    {
        //Set Score text true nd all actions about score 
        LastScoreText.gameObject.SetActive(true);
        
        LastScoreLabelText.gameObject.SetActive(true);
        
        LastScoreText.text = score.ToString();
        
        //score = 0; //We uncomment this line if we want to revive with a score of 0 instead of contitnue the previous scores   
        CurrentScoreText.text = score.ToString();
       
    }    


    //Function check global lives
    public void checkGlobalLives()
    {
        if (GlobalLives <= 1)
        {
            GlobalLives = 0;
            ReviveButtonGO.SetActive(false);
            LivesScoreText.color = Color.yellow;
        }
        else
        {
            GlobalLives--;
            ReviveButtonGO.SetActive(true);
            LivesScoreText.color = Color.grey;
        }
        

    }

    //Function to wait x sec after you loose all your revuces
    IEnumerator RestartAfterLosing(float secsToWaitAfterLoosingAlls)
    {
        //Some count down animations? 
        //Watch a video ads?
        //Get extra livess?
        ///stetdss
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(1);
        FadeIn();
        Time.timeScale = 1f;
        
        GameOverEffectRestartPanel.SetActive(true);
        readyReplayRestartObj.SetActive(false);
        GlobalLives = MaxGlobalLives;
        LivesScoreText.gameObject.SetActive(false);
        LivesScoreLabelText.gameObject.SetActive(false);
        checkGlobalLives();
        LivesScoreText.text = GlobalLives.ToString();
        ///////////IMPORTANTSs Time.timeScale = 0.3f;
        ///yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1);
        readyReplayRestartObj.SetActive(true);
        yield return new WaitForSeconds(secsToWaitAfterLoosingAlls);
        Time.timeScale = 1f;
        ReviveScoreSetup();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   

    
    //Function to wait x sec after you restart *counts downs)s
    IEnumerator WaitToReplays(float secsToWaitAfterLoosingToReplays)
    {
        yield return new WaitForSeconds(secsToWaitAfterLoosingToReplays);
        FindObjectOfType<Wave_Player>().RestartPlayPlayer();
        
        Time.timeScale = 1f;
        ///TO REACTIVATE TO STOP MOUVMENT WHEN THERE IS A CONTINUES ss!!! IMPSPORTATSs Time.timeScale = 0f;
        LivesScoreText.gameObject.SetActive(false);
        LivesScoreLabelText.gameObject.SetActive(false);
        LivesScoreText.text = GlobalLives.ToString();
        readyReplayRevivesObj.SetActive(false);
        ReviveScoreSetup();
    }
}
