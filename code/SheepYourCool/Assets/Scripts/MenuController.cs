using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mMarkerEasy;
    [SerializeField] private GameObject mMarkerMedium;
    [SerializeField] private GameObject mMarkerHard;

    [SerializeField] private GameObject mButtonStart;

    public int mEasySheepAmount = 10;
    public int mEasyAreaSize = 50;

    public int mMiddleSheepAmount = 15;
    public int mMiddleAreaSize = 100;

    public int mHardSheepAmount = 20;
    public int mHardAreaSize = 200;

    private void Start()
    {
        GlobalVariables.mInitializedPerMenu = true;
        GlobalVariables.mSheepAmount = mEasySheepAmount;
        GlobalVariables.mAreaSize = mEasyAreaSize;
        
    }

    public void SetDifficulty(string diff)
    {
        if (diff == "easy")
        {
            GlobalVariables.mSheepAmount = mEasySheepAmount;
            GlobalVariables.mAreaSize = mEasyAreaSize;
            mMarkerEasy.SetActive(true);
            mMarkerMedium.SetActive(false);
            mMarkerHard.SetActive(false);
        }
        else if (diff == "medium")
        {
            GlobalVariables.mSheepAmount = mMiddleSheepAmount;
            GlobalVariables.mAreaSize = mMiddleAreaSize;
            mMarkerEasy.SetActive(false);
            mMarkerMedium.SetActive(true);
            mMarkerHard.SetActive(false);
        }
        else if (diff == "hard")
        {
            GlobalVariables.mSheepAmount = mHardSheepAmount;
            GlobalVariables.mAreaSize = mHardAreaSize;
            mMarkerEasy.SetActive(false);
            mMarkerMedium.SetActive(false);
            mMarkerHard.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
