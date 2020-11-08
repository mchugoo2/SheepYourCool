using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI normalSheepText;
    public TextMeshProUGUI goldenSheepText;
    public TextMeshProUGUI redSheepText;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        normalSheepText.text = SheepManager.mNormalSheepAmount.ToString();
        goldenSheepText.text = SheepManager.mCaughtSheepAmount.ToString();
        redSheepText.text = SheepManager.mBorderedSheepAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackToMenu()
    {
        SheepManager.Reset();
        SceneManager.LoadScene(0);
    }
}
