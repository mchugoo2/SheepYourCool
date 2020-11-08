using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mMarkerEasy;
    [SerializeField] private GameObject mMarkerMedium;
    [SerializeField] private GameObject mMarkerHard;

    [SerializeField] private GameObject mButtonStart;

    public int mSheepAmount = 10;
    public int mAreaSize = 20;

    public void SetDifficulty(string diff)
    {
        if (diff == "easy")
        {
            mSheepAmount = 10;
            mAreaSize = 20;
            mMarkerEasy.SetActive(true);
            mMarkerMedium.SetActive(false);
            mMarkerHard.SetActive(false);
        }
        else if (diff == "medium")
        {
            mSheepAmount = 15;
            mAreaSize = 40;
            mMarkerEasy.SetActive(false);
            mMarkerMedium.SetActive(true);
            mMarkerHard.SetActive(false);
        }
        else if (diff == "hard")
        {
            mSheepAmount = 20;
            mAreaSize = 60;
            mMarkerEasy.SetActive(false);
            mMarkerMedium.SetActive(false);
            mMarkerHard.SetActive(true);
        }
    }

    public void StartGame()
    {

    }
}
