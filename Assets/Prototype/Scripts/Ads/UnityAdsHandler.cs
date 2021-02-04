using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsHandler : MonoBehaviour
{

    [SerializeField] string googlePlay_ID="4001611";
    bool testMode = true;

    void Start()
    {
        Advertisement.Initialize(googlePlay_ID, testMode);
    }
     

    public void DisplayInteratialAds()
    {
        Advertisement.Show();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
