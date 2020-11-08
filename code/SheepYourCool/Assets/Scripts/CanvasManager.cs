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
        UpdateNumbers(numNormalSheep, 0, 0);
    }

    public void UpdateNumbers(int normalNumber, int goldenNumber, int redNumber)
    {
        mNormalSheepText.text = normalNumber.ToString();
        mGoldenSheepText.text = goldenNumber.ToString();
        mRedSheepText.text = redNumber.ToString();
    }
}
