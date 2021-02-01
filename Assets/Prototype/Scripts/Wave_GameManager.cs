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
    

    public GameObject GameOverPanel;
    public GameObject GameOverEffectPanel;

    public GameObject touchToMoveTextObj;

    public GameObject StartFadeInObj;

    static int PlayCount;
    
    ///Added
    public GameObject playerGO;
    Transform lastPlayerPos;
    
    void Awake()
    {
        Application.targetFrameRate = 60;


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
    }


    IEnumerator GameoverCoroutine()
    {
        GameOverEffectPanel.SetActive(true);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.5f);
        GameOverPanel.SetActive(true);
        yield break;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Revive()
    {
        

        //Actions on player to revice
        GameOverPanel.SetActive(false);
        lastPlayerPos = playerGO.transform;
        ///FindObjectOfType<Wave_Player>().isDead = false;
        Time.timeScale = 1f;
        FindObjectOfType<Wave_Player>().RestartPlayPlayer();
        ReviveScoreSetup();


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
}
