using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI mNormalSheepText;
    public TextMeshProUGUI mGoldenSheepText;
    public TextMeshProUGUI mRedSheepText;

    public void Initialize(int numNormalSheep)
    {
        mNormalSheepText.text = numNormalSheep.ToString();
        mGoldenSheepText.text = "0";
        mRedSheepText.text = "0";
    }

    public void SetNumber(string whichSheep, int number)
    {
        if(whichSheep == "Normal")
        {
            mNormalSheepText.text = number.ToString();
        }

        else if (whichSheep == "Golden")
        {
            mGoldenSheepText.text = number.ToString();
        }

        else if(whichSheep == "Red")
        {
            mRedSheepText.text = number.ToString();
        }
    }
}
