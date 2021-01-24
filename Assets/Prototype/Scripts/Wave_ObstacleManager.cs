

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_ObstacleManager : MonoBehaviour
{

    public GameObject PlyerObj;
    public GameObject[] ObstaclesEasy;
    public GameObject[] ObstaclesNormal;


    int ObstacleIndex = 0;
    int DistanceToNext = 50;

    GameObject FirstOne;
    GameObject SecondOne;

    int playerPositionIndex = -1;


    void Start()
    {
        InstantiateObstacle();
    }

    void Update()
    {
        if (playerPositionIndex != (int)PlyerObj.transform.position.y / 25)
        {
            InstantiateObstacle();
            playerPositionIndex = (int)PlyerObj.transform.position.y / 25;
        }
    }



    public void InstantiateObstacle()
    {
        int score = GameObject.Find("GameManager").GetComponent<Wave_GameManager>().GetScore();
        if (score < 20)
        {
            int FirstObstacleNumber = Random.Range(0, ObstaclesEasy.Length);
            GameObject NewObs = Instantiate(ObstaclesEasy[FirstObstacleNumber], new Vector3(0, ObstacleIndex * DistanceToNext), Quaternion.identity);
            NewObs.transform.SetParent(transform);
        }
        else
        {
            int FirstObstacleNumber = Random.Range(0, ObstaclesNormal.Length);
            GameObject NewObs = Instantiate(ObstaclesNormal[FirstObstacleNumber], new Vector3(0, ObstacleIndex * DistanceToNext), Quaternion.identity);
            NewObs.transform.SetParent(transform);
        }



        ObstacleIndex++;
    }










}